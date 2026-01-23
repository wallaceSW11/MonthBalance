# 🚀 Deploy no Oracle Cloud Free Tier

## 📋 Pré-requisitos

- Conta Oracle Cloud (Free Tier)
- Instância ARM64 (Ampere A1) criada
- Docker e Docker Compose instalados na instância

---

## 🔧 Configuração da Instância

### 1. Criar Instância ARM64

No Oracle Cloud Console:
- **Shape**: VM.Standard.A1.Flex (ARM64)
- **CPU**: 4 OCPUs (Free Tier permite até 4)
- **RAM**: 24 GB (Free Tier permite até 24 GB)
- **OS**: Ubuntu 22.04 LTS (ARM64)
- **Boot Volume**: 200 GB (Free Tier permite até 200 GB)

### 2. Configurar Firewall (Security List)

Adicionar regras de entrada:
- **Porta 22**: SSH
- **Porta 80**: HTTP
- **Porta 443**: HTTPS
- **Porta 5000**: Backend API (temporário para testes)

### 3. Conectar via SSH

```bash
ssh -i sua-chave.pem ubuntu@seu-ip-publico
```

---

## 🐳 Instalação do Docker

```bash
# Atualizar sistema
sudo apt update && sudo apt upgrade -y

# Instalar Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Adicionar usuário ao grupo docker
sudo usermod -aG docker $USER

# Instalar Docker Compose
sudo apt install docker-compose -y

# Verificar instalação
docker --version
docker-compose --version
```

**Importante**: Fazer logout e login novamente para aplicar permissões do grupo docker.

---

## 📦 Deploy da Aplicação

### 1. Clonar Repositório

```bash
cd ~
git clone https://github.com/seu-usuario/month-balance.git
cd month-balance
```

### 2. Configurar Variáveis de Ambiente

```bash
# Copiar exemplo
cp .env.example .env

# Editar com senhas seguras
nano .env
```

**Gerar senhas seguras:**
```bash
# PostgreSQL Password
openssl rand -base64 32

# JWT Key
openssl rand -base64 64
```

### 3. Build e Start

```bash
# Build das imagens
docker-compose build

# Subir containers
docker-compose up -d

# Verificar logs
docker-compose logs -f
```

### 4. Verificar Status

```bash
# Ver containers rodando
docker ps

# Testar API
curl http://localhost:5000/api/auth/check-email/teste@teste.com
```

---

## 🔒 Configurar HTTPS com Nginx

### 1. Instalar Nginx

```bash
sudo apt install nginx certbot python3-certbot-nginx -y
```

### 2. Configurar Nginx

```bash
sudo nano /etc/nginx/sites-available/monthbalance
```

Adicionar:
```nginx
server {
    listen 80;
    server_name seu-dominio.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

```bash
# Ativar site
sudo ln -s /etc/nginx/sites-available/monthbalance /etc/nginx/sites-enabled/

# Testar configuração
sudo nginx -t

# Reiniciar Nginx
sudo systemctl restart nginx
```

### 3. Configurar SSL com Let's Encrypt

```bash
sudo certbot --nginx -d seu-dominio.com
```

---

## 🔄 Atualização da Aplicação

```bash
# Parar containers
docker-compose down

# Atualizar código
git pull

# Rebuild e restart
docker-compose build
docker-compose up -d

# Verificar logs
docker-compose logs -f backend
```

---

## 📊 Monitoramento

### Ver Logs

```bash
# Todos os containers
docker-compose logs -f

# Apenas backend
docker-compose logs -f backend

# Apenas postgres
docker-compose logs -f postgres
```

### Ver Uso de Recursos

```bash
# CPU e RAM
docker stats

# Espaço em disco
df -h
docker system df
```

### Backup do Banco

```bash
# Criar backup
docker exec monthbalance-postgres pg_dump -U postgres monthbalance > backup_$(date +%Y%m%d).sql

# Restaurar backup
cat backup_20260123.sql | docker exec -i monthbalance-postgres psql -U postgres monthbalance
```

---

## 🛠️ Troubleshooting

### Container não inicia

```bash
# Ver logs detalhados
docker-compose logs backend

# Verificar se porta está em uso
sudo netstat -tulpn | grep 5000

# Reiniciar container específico
docker-compose restart backend
```

### Erro de conexão com banco

```bash
# Verificar se postgres está rodando
docker ps | grep postgres

# Testar conexão
docker exec -it monthbalance-postgres psql -U postgres -d monthbalance
```

### Limpar tudo e recomeçar

```bash
# Parar e remover containers
docker-compose down

# Remover volumes (CUIDADO: apaga dados!)
docker-compose down -v

# Limpar imagens antigas
docker system prune -a

# Rebuild do zero
docker-compose build --no-cache
docker-compose up -d
```

---

## 💰 Custos (Free Tier)

- **Compute**: GRÁTIS (4 OCPUs ARM64)
- **RAM**: GRÁTIS (24 GB)
- **Storage**: GRÁTIS (200 GB)
- **Bandwidth**: GRÁTIS (10 TB/mês)
- **IP Público**: GRÁTIS (2 IPs)

**Total**: R$ 0,00/mês 🎉

---

## 🔐 Segurança

### Firewall do Sistema

```bash
# Instalar UFW
sudo apt install ufw -y

# Configurar regras
sudo ufw allow 22/tcp
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Ativar firewall
sudo ufw enable

# Verificar status
sudo ufw status
```

### Atualizar Sistema

```bash
# Configurar atualizações automáticas
sudo apt install unattended-upgrades -y
sudo dpkg-reconfigure -plow unattended-upgrades
```

### Trocar Senhas Regularmente

```bash
# Editar .env
nano .env

# Restart containers
docker-compose down
docker-compose up -d
```

---

## 📚 Comandos Úteis

```bash
# Ver todos os containers
docker ps -a

# Parar tudo
docker-compose down

# Subir tudo
docker-compose up -d

# Rebuild sem cache
docker-compose build --no-cache

# Ver logs em tempo real
docker-compose logs -f

# Entrar no container
docker exec -it monthbalance-backend bash

# Limpar espaço
docker system prune -a --volumes
```

---

**Versão:** 1.0  
**Data:** 23/01/2026  
**Compatível com**: Oracle Cloud Free Tier (ARM64)
