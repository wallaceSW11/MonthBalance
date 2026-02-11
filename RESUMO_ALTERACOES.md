# 📦 Resumo das Alterações - BFF com Docker Compose

## 🎯 Objetivo Alcançado

✅ Backend **NÃO** exposto publicamente  
✅ Frontend exposto apenas na porta 80  
✅ Comunicação via proxy reverso (Nginx → backend:5150)  
✅ Sem necessidade de CORS permissivo  
✅ Arquitetura BFF implementada  

---

## 📁 Arquivos Modificados

### 1. `frontend/nginx.conf`

**Mudança Principal:** Proxy reverso para nome do serviço Docker

```nginx
# ANTES (localhost - não funciona no Docker)
location /api/ {
    proxy_pass http://localhost:5150/api/;
    ...
}

# DEPOIS (nome do serviço Docker)
location /api/ {
    proxy_pass http://backend:5150/api/;
    ...
}
```

**Por quê?** No Docker Compose, os containers se comunicam pelo nome do serviço, não por localhost.

---

### 2. `frontend/.env`

```env
# ANTES
VITE_API_BASE_URL=http://54.144.175.38:5150/api

# DEPOIS
VITE_API_BASE_URL=/api
```

**Por quê?** O frontend agora usa URL relativa. O Nginx cuida do proxy para o backend.

---

### 3. `docker-compose.yml`

**Backend - Removida exposição pública:**

```yaml
# ANTES
backend:
  ports:
    - "5150:5150"  # ❌ Exposto publicamente

# DEPOIS
backend:
  expose:
    - "5150"  # ✅ Apenas interno (Docker network)
```

**Frontend - Porta 80:**

```yaml
# ANTES
frontend:
  ports:
    - "8080:80"

# DEPOIS
frontend:
  ports:
    - "80:80"  # ✅ Porta padrão HTTP
```

---

### 4. `backend/appsettings.Production.json`

```json
{
  "Cors": {
    "AllowedOrigins": []  // ✅ Vazio - não precisa de CORS
  }
}
```

**Por quê?** Com proxy reverso, todas as requisições vêm da mesma origem (porta 80).

---

### 5. `backend/Program.cs`

```csharp
// Comentários atualizados explicando que em produção
// com proxy reverso não é necessário CORS restritivo
```

---

## 🔄 Fluxo de Requisição

```
┌─────────────┐
│   Browser   │
│ (Internet)  │
└──────┬──────┘
       │ http://IP_ELASTICO/api/auth/login
       ▼
┌─────────────────────┐
│  Frontend (Nginx)   │
│  Porta 80 (pública) │
└──────┬──────────────┘
       │ proxy_pass http://backend:5150/api/auth/login
       ▼ (Docker network)
┌─────────────────────┐
│  Backend (.NET)     │
│  Porta 5150 (interna)│
└──────┬──────────────┘
       │ Host=postgres;Port=5432
       ▼
┌─────────────────────┐
│  PostgreSQL         │
│  Porta 5432 (interna)│
└─────────────────────┘
```

---

## 🔒 Segurança

### Security Group AWS

| Porta | Protocolo | Origem | Status |
|-------|-----------|--------|--------|
| 80 | HTTP | 0.0.0.0/0 | ✅ Aberta |
| 443 | HTTPS | 0.0.0.0/0 | ✅ Aberta (se SSL) |
| 22 | SSH | Seu IP | ✅ Restrita |
| 5150 | HTTP | - | ❌ **NÃO DEVE EXISTIR** |

### Verificação

```bash
# Backend NÃO deve responder externamente
curl http://SEU_IP:5150/api/health
# Esperado: Connection refused ✅

# API deve responder via proxy
curl http://SEU_IP/api/health
# Esperado: 200 OK ✅
```

---

## 📦 Build das Imagens

### Frontend

```bash
cd frontend
docker build \
  --build-arg VITE_API_BASE_URL=/api \
  -t ghcr.io/SEU_USUARIO/month-balance-frontend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-frontend:latest
```

**IMPORTANTE:** O `--build-arg VITE_API_BASE_URL=/api` é crucial!

### Backend

```bash
cd backend
docker build -t ghcr.io/SEU_USUARIO/month-balance-backend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-backend:latest
```

---

## 🚀 Deploy no EC2

```bash
# 1. Copiar arquivos
scp docker-compose.yml ec2-user@IP:~/month-balance/
scp .env.production ec2-user@IP:~/month-balance/.env

# 2. No EC2
cd ~/month-balance
docker-compose pull
docker-compose up -d

# 3. Verificar
docker-compose ps
docker-compose logs -f
```

---

## ✅ Checklist de Validação

### Testes Internos (no EC2)

```bash
# Backend direto
curl http://localhost:5150/api/health  # ✅ Deve funcionar

# Via proxy
curl http://localhost/api/health  # ✅ Deve funcionar
```

### Testes Externos (do seu PC)

```bash
# Frontend
curl http://SEU_IP/  # ✅ Deve retornar HTML

# API via proxy
curl http://SEU_IP/api/health  # ✅ Deve funcionar

# Backend direto (deve falhar)
curl http://SEU_IP:5150/api/health  # ❌ Connection refused (correto!)
```

### No Navegador

1. Acesse `http://SEU_IP`
2. Abra DevTools → Network
3. Faça login
4. Verifique que as requisições vão para `/api/*` (não para `:5150`)
5. Verifique que **NÃO** há erros de CORS

---

## 🐛 Troubleshooting Rápido

### Erro 502 Bad Gateway

```bash
# Verificar se backend está rodando
docker-compose ps backend

# Testar conectividade interna
docker-compose exec frontend curl http://backend:5150/api/health
```

### Erro de CORS

```bash
# Verificar build do frontend
docker-compose exec frontend cat /usr/share/nginx/html/assets/*.js | grep -o "http://[^\"]*"
# NÃO deve mostrar IPs ou portas hardcoded

# Rebuild se necessário
docker build --build-arg VITE_API_BASE_URL=/api -t frontend:latest frontend/
```

### Backend não conecta ao banco

```bash
# Verificar network
docker network inspect month-balance_month-balance-network

# Testar ping
docker-compose exec backend ping postgres
```

---

## 📊 Comparação: Antes vs Depois

| Aspecto | Antes | Depois |
|---------|-------|--------|
| Backend exposto | ✅ Porta 5150 pública | ❌ Apenas interno |
| CORS necessário | ✅ Sim, permissivo | ❌ Não necessário |
| URL da API | IP:5150/api | /api (relativa) |
| Segurança | ⚠️ Backend exposto | ✅ Backend protegido |
| Complexidade | ⚠️ CORS + IPs | ✅ Proxy simples |

---

## 📚 Arquivos de Referência

- `DEPLOY_AWS.md` - Guia completo de deploy
- `CHECKLIST_DEPLOY.md` - Checklist passo a passo
- `.env.production` - Template de variáveis
- `docker-compose.yml` - Configuração final

---

## 🎉 Resultado Final

✅ Arquitetura BFF implementada  
✅ Backend protegido (não exposto)  
✅ Frontend na porta 80  
✅ Proxy reverso funcionando  
✅ Sem problemas de CORS  
✅ Pronto para produção!  
