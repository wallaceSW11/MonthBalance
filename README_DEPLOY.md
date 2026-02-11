# 🚀 Deploy AWS EC2 - Guia Rápido

## ✅ O que foi feito

Implementamos arquitetura BFF (Backend for Frontend) com Docker Compose onde:

- ✅ **Frontend** exposto na porta 80
- ✅ **Backend** NÃO exposto (apenas interno)
- ✅ **Nginx** faz proxy reverso: `/api` → `backend:5150`
- ✅ **Sem CORS** necessário
- ✅ **Comunicação** via Docker network

## 📁 Arquivos Principais

| Arquivo | Descrição |
|---------|-----------|
| `docker-compose.yml` | Configuração dos containers (backend sem `ports`) |
| `frontend/nginx.conf` | Proxy reverso para `backend:5150` |
| `frontend/.env` | `VITE_API_BASE_URL=/api` |
| `backend/appsettings.Production.json` | CORS vazio |
| `.env.production` | Template de variáveis |

## 🎯 Deploy em 3 Passos

### 1️⃣ Build e Push (Local)

```bash
# Backend
cd backend
docker build -t ghcr.io/SEU_USUARIO/month-balance-backend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-backend:latest

# Frontend (IMPORTANTE: --build-arg)
cd ../frontend
docker build --build-arg VITE_API_BASE_URL=/api -t ghcr.io/SEU_USUARIO/month-balance-frontend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-frontend:latest
```

### 2️⃣ Copiar para EC2

```bash
scp -i sua-chave.pem docker-compose.yml ec2-user@SEU_IP:~/month-balance/
scp -i sua-chave.pem .env.production ec2-user@SEU_IP:~/month-balance/.env
```

### 3️⃣ Deploy no EC2

```bash
# Conectar
ssh -i sua-chave.pem ec2-user@SEU_IP

# Deploy
cd ~/month-balance
nano .env  # Configurar DB_PASSWORD e JWT_SECRET
docker-compose pull
docker-compose up -d

# Verificar
docker-compose ps
docker-compose logs -f
```

## 🔒 Security Group AWS

**IMPORTANTE:** Remova a porta 5150!

```
✅ Porta 80  → 0.0.0.0/0
✅ Porta 443 → 0.0.0.0/0 (se SSL)
✅ Porta 22  → Seu IP
❌ Porta 5150 → REMOVER
```

## 🧪 Testes

```bash
# No EC2
curl http://localhost/api/health  # ✅ Deve funcionar

# Do seu PC
curl http://SEU_IP/api/health  # ✅ Deve funcionar
curl http://SEU_IP:5150/api/health  # ❌ Deve falhar (correto!)
```

## 📊 Fluxo

```
Browser → http://IP/api/auth/login
    ↓
Nginx (frontend:80)
    ↓ proxy_pass
Backend (backend:5150) - NÃO exposto
    ↓
PostgreSQL (postgres:5432) - NÃO exposto
```

## 🐛 Troubleshooting

### Erro 502 Bad Gateway

```bash
docker-compose logs backend
docker-compose exec frontend curl http://backend:5150/api/health
```

### Backend exposto (ERRO!)

```bash
# Verificar docker-compose.yml
# Backend deve ter "expose: 5150" e NÃO "ports: 5150:5150"
docker-compose ps
```

### CORS Error

```bash
# Verificar build do frontend
docker-compose exec frontend cat /usr/share/nginx/html/assets/*.js | grep -o "http://[^\"]*"
# NÃO deve mostrar IPs hardcoded
```

## 📚 Documentação Completa

- `ARQUITETURA_FINAL.md` - Diagrama detalhado
- `DEPLOY_AWS.md` - Guia completo
- `CHECKLIST_DEPLOY.md` - Checklist passo a passo
- `RESUMO_ALTERACOES.md` - O que mudou
- `COMANDOS_DEPLOY.sh` - Todos os comandos

## ⚡ Comandos Úteis

```bash
# Status
docker-compose ps

# Logs
docker-compose logs -f

# Reiniciar
docker-compose restart

# Atualizar
docker-compose pull && docker-compose up -d

# Backup DB
docker-compose exec postgres pg_dump -U postgres monthbalance > backup.sql
```

## ✅ Checklist Final

- [ ] Backend NÃO responde em `http://IP:5150` (deve falhar)
- [ ] API responde em `http://IP/api/health` (deve funcionar)
- [ ] Frontend carrega em `http://IP` (deve funcionar)
- [ ] Login funciona sem erros de CORS
- [ ] Security Group sem porta 5150

## 🎉 Pronto!

Se todos os itens acima estão OK, seu deploy está completo e seguro!

---

**Dúvidas?** Consulte os arquivos de documentação detalhada.
