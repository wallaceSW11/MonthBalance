# ✅ Checklist de Deploy - AWS EC2 Docker Compose

## Arquitetura Final

```
┌─────────────────────────────────────────────────┐
│  Internet (Porta 80)                            │
└────────────────┬────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────┐
│  Frontend Container (Nginx)                     │
│  - Porta 80 exposta publicamente                │
│  - Serve arquivos estáticos (Vue.js)            │
│  - Proxy reverso: /api → backend:5150           │
└────────────────┬────────────────────────────────┘
                 │ Docker Network
                 │ (month-balance-network)
                 ▼
┌─────────────────────────────────────────────────┐
│  Backend Container (.NET)                       │
│  - Porta 5150 (apenas interna)                  │
│  - NÃO exposta publicamente                     │
│  - Sem CORS necessário                          │
└────────────────┬────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────┐
│  PostgreSQL Container                           │
│  - Porta 5432 (apenas interna)                  │
│  - Volume persistente                           │
└─────────────────────────────────────────────────┘
```

## 📋 Pré-Deploy (Local)

### Arquivos Modificados

- [ ] `frontend/nginx.conf` - Proxy para `backend:5150`
- [ ] `frontend/.env` - `VITE_API_BASE_URL=/api`
- [ ] `docker-compose.yml` - Backend sem `ports`, apenas `expose`
- [ ] `backend/appsettings.Production.json` - CORS vazio
- [ ] `backend/Program.cs` - CORS configurado para produção

### Build das Imagens

```bash
# Backend
cd backend
docker build -t ghcr.io/SEU_USUARIO/month-balance-backend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-backend:latest

# Frontend (IMPORTANTE: usar /api)
cd ../frontend
docker build --build-arg VITE_API_BASE_URL=/api -t ghcr.io/SEU_USUARIO/month-balance-frontend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-frontend:latest
```

- [ ] Backend image built e pushed
- [ ] Frontend image built e pushed (com VITE_API_BASE_URL=/api)

## 🖥️ No Servidor EC2

### 1. Preparação

```bash
# Conectar ao EC2
ssh -i sua-chave.pem ec2-user@SEU_IP_ELASTICO

# Criar diretório
mkdir -p ~/month-balance
cd ~/month-balance
```

- [ ] Conectado ao EC2
- [ ] Diretório criado

### 2. Copiar Arquivos

```bash
# Do seu computador local
scp -i sua-chave.pem docker-compose.yml ec2-user@SEU_IP_ELASTICO:~/month-balance/
scp -i sua-chave.pem .env.production ec2-user@SEU_IP_ELASTICO:~/month-balance/.env
```

- [ ] docker-compose.yml copiado
- [ ] .env configurado com credenciais reais

### 3. Configurar Variáveis de Ambiente

```bash
# No EC2
nano .env
```

Verificar:
- [ ] `DB_PASSWORD` configurado
- [ ] `JWT_SECRET` configurado (mínimo 32 caracteres)

### 4. Deploy

```bash
# Login no registry (se usar GitHub Container Registry)
docker login ghcr.io -u SEU_USUARIO

# Pull e start
docker-compose pull
docker-compose up -d

# Verificar status
docker-compose ps
```

- [ ] Containers rodando (3/3)
- [ ] Postgres healthy
- [ ] Backend rodando
- [ ] Frontend rodando

### 5. Verificar Logs

```bash
# Ver logs de todos os serviços
docker-compose logs -f

# Verificar erros específicos
docker-compose logs backend | grep -i error
docker-compose logs frontend | grep -i error
```

- [ ] Sem erros críticos nos logs
- [ ] Backend conectou ao banco
- [ ] Migrations executadas

## 🔒 Security Group AWS

### Inbound Rules

- [ ] Porta 80 (HTTP): `0.0.0.0/0` ✅
- [ ] Porta 443 (HTTPS): `0.0.0.0/0` ✅ (se usar SSL)
- [ ] Porta 22 (SSH): `SEU_IP` ✅
- [ ] Porta 5150: **NÃO DEVE EXISTIR** ❌

### Verificar

```bash
# No EC2, verificar portas abertas
sudo netstat -tlnp | grep LISTEN
```

Deve mostrar:
- [ ] Porta 80 (docker-proxy) ✅
- [ ] Porta 5150 **NÃO** deve estar acessível externamente ❌

## 🧪 Testes

### 1. Teste Interno (dentro do EC2)

```bash
# Teste o backend diretamente
curl http://localhost:5150/api/health

# Teste via proxy do frontend
curl http://localhost/api/health
```

- [ ] Backend responde diretamente
- [ ] Proxy funciona

### 2. Teste Externo (do seu computador)

```bash
# Teste o frontend
curl http://SEU_IP_ELASTICO/

# Teste a API via proxy
curl http://SEU_IP_ELASTICO/api/health

# Teste que o backend NÃO está exposto
curl http://SEU_IP_ELASTICO:5150/api/health
# Deve falhar: Connection refused ✅
```

- [ ] Frontend acessível
- [ ] API acessível via `/api`
- [ ] Backend **NÃO** acessível diretamente na porta 5150 ✅

### 3. Teste no Navegador

```
http://SEU_IP_ELASTICO
```

- [ ] Página carrega
- [ ] Login funciona
- [ ] Sem erros de CORS no console
- [ ] Requisições vão para `/api/*`

### 4. Verificar Network Docker

```bash
# Verificar que todos estão na mesma network
docker network inspect month-balance_month-balance-network

# Testar conectividade interna
docker-compose exec frontend ping backend
docker-compose exec backend ping postgres
```

- [ ] Todos os containers na mesma network
- [ ] Frontend consegue pingar backend
- [ ] Backend consegue pingar postgres

## 🐛 Troubleshooting

### Backend não responde

```bash
docker-compose logs backend
docker-compose exec backend curl http://localhost:5150/api/health
```

### Erro 502 Bad Gateway

```bash
# Verificar se backend está rodando
docker-compose ps backend

# Verificar logs do nginx
docker-compose logs frontend

# Testar conectividade
docker-compose exec frontend curl http://backend:5150/api/health
```

### Erro de CORS

Se ainda aparecer erro de CORS:
- [ ] Verificar que `VITE_API_BASE_URL=/api` no build
- [ ] Verificar que nginx.conf tem `proxy_pass http://backend:5150/api/`
- [ ] Rebuild do frontend com a variável correta

### Banco não conecta

```bash
docker-compose logs postgres
docker-compose exec postgres psql -U postgres -d monthbalance -c "SELECT 1;"
```

## 📊 Monitoramento

```bash
# Status dos containers
docker-compose ps

# Uso de recursos
docker stats

# Logs em tempo real
docker-compose logs -f --tail=50
```

## ✅ Checklist Final

- [ ] Frontend acessível na porta 80
- [ ] Backend **NÃO** acessível na porta 5150 externamente
- [ ] API funciona via `/api/*`
- [ ] Sem erros de CORS
- [ ] Login funciona
- [ ] Dados persistem (testar criar/editar/deletar)
- [ ] Logs sem erros críticos
- [ ] Security Group configurado corretamente

## 🎉 Deploy Completo!

Se todos os itens acima estão marcados, seu deploy está completo e funcionando corretamente!

## 📝 Comandos Úteis

```bash
# Reiniciar tudo
docker-compose restart

# Atualizar para nova versão
docker-compose pull && docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar tudo
docker-compose down

# Backup do banco
docker-compose exec postgres pg_dump -U postgres monthbalance > backup.sql
```
