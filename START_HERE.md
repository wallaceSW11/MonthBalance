# 🚀 COMECE AQUI - Deploy AWS EC2

## ⚡ TL;DR (Muito Rápido)

```bash
# 1. Build e push
cd backend && docker build -t ghcr.io/USER/backend:latest . && docker push ghcr.io/USER/backend:latest
cd ../frontend && docker build --build-arg VITE_API_BASE_URL=/api -t ghcr.io/USER/frontend:latest . && docker push ghcr.io/USER/frontend:latest

# 2. No EC2
scp docker-compose.yml .env.production ec2-user@IP:~/month-balance/
ssh ec2-user@IP
cd ~/month-balance && mv .env.production .env && nano .env
docker-compose pull && docker-compose up -d

# 3. Testar
curl http://IP/api/health  # ✅ Deve funcionar
curl http://IP:5150/api/health  # ❌ Deve falhar (correto!)
```

---

## 🎯 O que foi feito?

### Antes (Inseguro)
```
Browser → http://IP:5150/api ❌ Backend exposto
```

### Depois (Seguro - BFF)
```
Browser → http://IP/api → Nginx → backend:5150 ✅ Backend protegido
```

---

## 📁 Arquivos Modificados

| Arquivo | Mudança |
|---------|---------|
| `frontend/nginx.conf` | `localhost:5150` → `backend:5150` |
| `frontend/.env` | `http://IP:5150/api` → `/api` |
| `docker-compose.yml` | Backend: `ports` → `expose` |
| `docker-compose.yml` | Frontend: porta `8080` → `80` |
| `backend/appsettings.Production.json` | CORS vazio |

---

## 📚 Documentação

### Escolha seu caminho:

#### 🏃 Rápido (20 min)
1. **[README_DEPLOY.md](README_DEPLOY.md)** - Deploy em 3 passos
2. **[CHECKLIST_DEPLOY.md](CHECKLIST_DEPLOY.md)** - Validação

#### 📖 Completo (1 hora)
1. **[DEPLOY_AWS.md](DEPLOY_AWS.md)** - Guia detalhado
2. **[ARQUITETURA_FINAL.md](ARQUITETURA_FINAL.md)** - Como funciona
3. **[CHECKLIST_DEPLOY.md](CHECKLIST_DEPLOY.md)** - Validação

#### 🔍 Entender Mudanças (30 min)
1. **[RESUMO_ALTERACOES.md](RESUMO_ALTERACOES.md)** - Resumo executivo
2. **[DIFF_VISUAL.md](DIFF_VISUAL.md)** - Antes vs Depois
3. **[ARQUITETURA_FINAL.md](ARQUITETURA_FINAL.md)** - Arquitetura

#### 🐛 Resolver Problemas
1. **[TROUBLESHOOTING_VISUAL.md](TROUBLESHOOTING_VISUAL.md)** - Diagnóstico

#### 📋 Todos os Arquivos
**[INDICE_DOCUMENTACAO.md](INDICE_DOCUMENTACAO.md)** - Índice completo

---

## ✅ Validação Rápida

```bash
# No EC2
curl http://localhost/api/health  # ✅ 200 OK
curl http://localhost:5150/api/health  # ❌ Connection refused

# Do seu PC
curl http://SEU_IP/api/health  # ✅ 200 OK
curl http://SEU_IP:5150/api/health  # ❌ Connection refused

# No navegador
# http://SEU_IP → ✅ Carrega
# DevTools → Network → ✅ Sem CORS
# Requisições → ✅ Vão para /api (não :5150)
```

---

## 🔒 Security Group AWS

```
✅ Porta 80  → 0.0.0.0/0
✅ Porta 22  → Seu IP
❌ Porta 5150 → REMOVER!
```

---

## 🎯 Pontos Críticos

### 1. Build do Frontend
```bash
# ❌ ERRADO
docker build -t frontend .

# ✅ CORRETO
docker build --build-arg VITE_API_BASE_URL=/api -t frontend .
```

### 2. Nginx Proxy
```nginx
# ❌ ERRADO
proxy_pass http://localhost:5150/api/;

# ✅ CORRETO
proxy_pass http://backend:5150/api/;
```

### 3. Docker Compose
```yaml
# ❌ ERRADO
backend:
  ports: ["5150:5150"]

# ✅ CORRETO
backend:
  expose: ["5150"]
```

---

## 🚨 Se algo não funcionar

1. **Erro 502?** → [TROUBLESHOOTING_VISUAL.md](TROUBLESHOOTING_VISUAL.md#-problema-2-erro-502-bad-gateway)
2. **Backend exposto?** → [TROUBLESHOOTING_VISUAL.md](TROUBLESHOOTING_VISUAL.md#-problema-1-backend-ainda-está-exposto-erro-crítico)
3. **CORS?** → [TROUBLESHOOTING_VISUAL.md](TROUBLESHOOTING_VISUAL.md#-problema-3-erro-de-cors-não-deveria-acontecer)

---

## 📞 Próximos Passos

Após o deploy:

1. ✅ Validar com CHECKLIST_DEPLOY.md
2. ✅ Configurar SSL (opcional)
3. ✅ Configurar domínio (opcional)
4. ✅ Configurar backup automático (opcional)

---

## 🎉 Pronto!

Se os testes de validação passaram, seu deploy está completo e seguro!

**Dúvidas?** Consulte o [INDICE_DOCUMENTACAO.md](INDICE_DOCUMENTACAO.md)
