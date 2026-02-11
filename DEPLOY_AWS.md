# Guia de Deploy AWS EC2 - Docker Compose com Proxy Reverso

## Arquitetura Implementada

```
Internet (porta 80)
    ↓
[Docker: Frontend - Nginx]
    ↓ (proxy /api → backend:5150 via Docker network)
[Docker: Backend .NET] (NÃO exposto publicamente)
    ↓
[Docker: PostgreSQL]
```

## Benefícios

1. **Segurança**: Backend não fica exposto diretamente na internet
2. **Sem CORS**: Tudo vem da mesma origem (porta 80)
3. **Simplicidade**: Um único ponto de entrada
4. **Isolamento**: Comunicação interna via Docker network
5. **Performance**: Nginx faz cache e compressão

## Pré-requisitos no EC2

```bash
# Instalar Docker
sudo yum update -y
sudo yum install docker -y
sudo service docker start
sudo usermod -a -G docker ec2-user

# Instalar Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Verificar instalação
docker --version
docker-compose --version
```

## Passos para Deploy

### 1. Preparar o Servidor EC2

```bash
# Conectar via SSH
ssh -i sua-chave.pem ec2-user@SEU_IP_ELASTICO

# Criar diretório do projeto
mkdir -p ~/month-balance
cd ~/month-balance
```

### 2. Copiar Arquivos para o Servidor

```bash
# No seu computador local, copie os arquivos necessários
scp -i sua-chave.pem docker-compose.yml ec2-user@SEU_IP_ELASTICO:~/month-balance/
scp -i sua-chave.pem .env.production ec2-user@SEU_IP_ELASTICO:~/month-balance/.env
```

### 3. Configurar Variáveis de Ambiente

```bash
# No servidor EC2
cd ~/month-balance
nano .env
```

Edite o arquivo `.env` com suas credenciais:

```env
DB_NAME=monthbalance
DB_USER=postgres
DB_PASSWORD=SuaSenhaSeguraAqui123!

JWT_SECRET=sua_chave_jwt_super_secreta_minimo_32_caracteres_aqui
```

### 4. Build e Deploy das Imagens

Você tem duas opções:

#### Opção A: Usar GitHub Container Registry (Recomendado)

```bash
# No seu computador local, faça login no GitHub Container Registry
echo $GITHUB_TOKEN | docker login ghcr.io -u SEU_USUARIO --password-stdin

# Build e push do backend
cd backend
docker build -t ghcr.io/SEU_USUARIO/month-balance-backend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-backend:latest

# Build e push do frontend (com VITE_API_BASE_URL=/api)
cd ../frontend
docker build --build-arg VITE_API_BASE_URL=/api -t ghcr.io/SEU_USUARIO/month-balance-frontend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-frontend:latest

# No servidor EC2, faça pull das imagens
docker login ghcr.io -u SEU_USUARIO
docker-compose pull
docker-compose up -d
```

#### Opção B: Build Direto no EC2

```bash
# Copie todo o código para o servidor
scp -i sua-chave.pem -r . ec2-user@SEU_IP_ELASTICO:~/month-balance/

# No servidor EC2
cd ~/month-balance

# Edite o docker-compose.yml para usar build local
nano docker-compose.yml
# Substitua "image:" por "build:" para cada serviço

# Build e inicie os containers
docker-compose up -d --build
```

### 5. Iniciar a Aplicação

```bash
# No servidor EC2
cd ~/month-balance

# Iniciar todos os serviços
docker-compose up -d

# Verificar status
docker-compose ps

# Ver logs
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f frontend
docker-compose logs -f backend
docker-compose logs -f postgres
```

### 6. Configurar Security Group AWS

No console AWS, configure o Security Group do EC2:

**Inbound Rules:**
- Porta 80 (HTTP): 0.0.0.0/0 ✅
- Porta 443 (HTTPS): 0.0.0.0/0 ✅ (se usar SSL)
- Porta 22 (SSH): Seu IP
- ~~Porta 5150~~: NÃO ADICIONAR ❌ (backend não deve estar exposto)

### 7. Testar a Aplicação

```bash
# Teste interno (dentro do EC2)
curl http://localhost/api/health

# Teste externo (do seu computador)
curl http://SEU_IP_ELASTICO/api/health

# Acesse no navegador
http://SEU_IP_ELASTICO
```

## Fluxo de Requisições

```
1. Browser → http://SEU_IP_ELASTICO/api/auth/login
2. Nginx (frontend:80) → http://backend:5150/api/auth/login
3. Backend → postgres:5432
4. Backend → Nginx → Browser
```

## Comandos Úteis

```bash
# Ver status dos containers
docker-compose ps

# Ver logs em tempo real
docker-compose logs -f

# Reiniciar um serviço
docker-compose restart backend

# Parar todos os serviços
docker-compose down

# Parar e remover volumes (CUIDADO: apaga o banco!)
docker-compose down -v

# Atualizar imagens e reiniciar
docker-compose pull
docker-compose up -d

# Executar comando dentro de um container
docker-compose exec backend bash
docker-compose exec postgres psql -U postgres -d monthbalance

# Ver uso de recursos
docker stats
```

## Troubleshooting

### Backend não responde

```bash
# Verifique se o container está rodando
docker-compose ps

# Veja os logs do backend
docker-compose logs backend

# Teste a conexão interna
docker-compose exec frontend curl http://backend:5150/api/health

# Verifique a network
docker network ls
docker network inspect month-balance_month-balance-network
```

### Erro de CORS

Se ainda ver erros de CORS, significa que o proxy não está funcionando:

```bash
# Verifique o nginx.conf
docker-compose exec frontend cat /etc/nginx/conf.d/default.conf

# Teste o proxy manualmente
docker-compose exec frontend curl http://backend:5150/api/health

# Verifique se os containers estão na mesma network
docker-compose exec frontend ping backend
```

### Banco de dados não conecta

```bash
# Verifique se o postgres está rodando
docker-compose ps postgres

# Veja os logs do postgres
docker-compose logs postgres

# Teste a conexão
docker-compose exec backend ping postgres
docker-compose exec postgres psql -U postgres -d monthbalance -c "SELECT 1;"
```

### Frontend não carrega

```bash
# Verifique os logs do nginx
docker-compose logs frontend

# Verifique se os arquivos foram copiados
docker-compose exec frontend ls -la /usr/share/nginx/html

# Teste o nginx
docker-compose exec frontend nginx -t
```

## Atualização da Aplicação

```bash
# 1. Fazer pull das novas imagens
docker-compose pull

# 2. Recriar os containers
docker-compose up -d

# 3. Verificar se tudo está funcionando
docker-compose ps
docker-compose logs -f
```

## Backup do Banco de Dados

```bash
# Criar backup
docker-compose exec postgres pg_dump -U postgres monthbalance > backup_$(date +%Y%m%d_%H%M%S).sql

# Restaurar backup
cat backup_20260211_120000.sql | docker-compose exec -T postgres psql -U postgres monthbalance
```

## Monitoramento

```bash
# Ver uso de recursos em tempo real
docker stats

# Ver logs de todos os serviços
docker-compose logs -f --tail=100

# Verificar saúde dos containers
docker-compose ps
```

## SSL/HTTPS (Opcional - Recomendado)

Para adicionar SSL com Let's Encrypt:

```bash
# Instalar certbot
sudo yum install certbot python3-certbot-nginx -y

# Obter certificado (substitua seu-dominio.com)
sudo certbot --nginx -d seu-dominio.com

# Renovação automática
sudo certbot renew --dry-run
```

## Próximos Passos (Opcional)

1. **Domínio Personalizado**: Configure um domínio no Route 53
2. **SSL/HTTPS**: Configure Let's Encrypt
3. **CI/CD**: Configure GitHub Actions para deploy automático
4. **Monitoramento**: Configure CloudWatch ou Prometheus
5. **Backup Automático**: Configure backup automático do banco
6. **Load Balancer**: Use ELB para alta disponibilidade
