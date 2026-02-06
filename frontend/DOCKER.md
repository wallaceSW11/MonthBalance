# 🐳 Docker Setup - Month Balance

## 📋 Pré-requisitos

- Docker
- Docker Compose
- WSL2 (Windows) ou Linux/Mac

## 🚀 Desenvolvimento Local

### 1. Criar arquivo .env

```bash
cp .env.example .env
```

Edite o `.env` e ajuste a porta se necessário:

```env
VITE_API_BASE_URL=http://localhost:3000/api
PORT=8080
```

### 2. Build e Run

**No Windows (WSL2):**
```bash
# Entrar no WSL2
wsl

# Navegar até o projeto
cd /mnt/c/git/MB3_do_zero/MonthBalance/frontend

# Build da imagem
docker compose build

# Subir container
docker compose up -d

# Ver logs
docker compose logs -f

# Parar container
docker compose down
```

**No Linux/Mac:**
```bash
docker compose build
docker compose up -d
```

### 3. Acessar

- Local: `http://localhost:8080` (ou porta definida no .env)

## ☁️ Oracle Cloud (Produção)

### 1. Configurar Variáveis de Ambiente

No Oracle Cloud, configure as variáveis:

```
VITE_API_BASE_URL=https://api.seudominio.com/api
PORT=80
```

### 2. Deploy

```bash
# Build
docker compose build

# Run (porta 80)
docker compose up -d
```

### 3. Verificar

```bash
# Status
docker compose ps

# Logs
docker compose logs -f month-balance
```

## 🔧 Comandos Úteis

```bash
# Rebuild forçado
docker compose build --no-cache

# Restart
docker compose restart

# Remover tudo
docker compose down -v

# Entrar no container
docker exec -it month-balance sh
```

## 📦 Estrutura

- `Dockerfile`: Multi-stage build (Node 20 + Nginx Alpine)
- `docker-compose.yml`: Orquestração
- `nginx.conf`: Configuração Nginx (SPA, gzip, cache)
- `.dockerignore`: Arquivos ignorados no build

## 🔒 Segurança

- `.env` não é commitado (está no .gitignore)
- Headers de segurança configurados no Nginx
- Gzip habilitado
- Cache de assets estáticos

## 🐛 Troubleshooting

### Container não sobe

```bash
docker compose logs month-balance
```

### Porta em uso

Altere `PORT` no `.env`

### Build falha

```bash
docker compose build --no-cache
```
