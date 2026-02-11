# 🏗️ Arquitetura Final - BFF com Docker Compose

## 📊 Diagrama de Arquitetura

```
┌─────────────────────────────────────────────────────────────────────┐
│                         INTERNET (Público)                          │
│                                                                     │
│                    http://SEU_IP_ELASTICO                          │
└────────────────────────────────┬────────────────────────────────────┘
                                 │
                                 │ Porta 80 (HTTP)
                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                      AWS EC2 INSTANCE                               │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │              Docker Network: month-balance-network            │ │
│  │                                                               │ │
│  │  ┌─────────────────────────────────────────────────────┐    │ │
│  │  │  Container: frontend                                │    │ │
│  │  │  ─────────────────────                              │    │ │
│  │  │  Image: month-balance-frontend:latest               │    │ │
│  │  │  Ports: 80:80 (EXPOSTO PUBLICAMENTE)               │    │ │
│  │  │                                                     │    │ │
│  │  │  ┌─────────────────────────────────────┐          │    │ │
│  │  │  │  Nginx                              │          │    │ │
│  │  │  │  ─────                              │          │    │ │
│  │  │  │  • Serve arquivos estáticos (Vue)  │          │    │ │
│  │  │  │  • Proxy reverso:                   │          │    │ │
│  │  │  │    /api/* → http://backend:5150/api/│          │    │ │
│  │  │  └─────────────────────────────────────┘          │    │ │
│  │  └──────────────────────┬──────────────────────────────┘    │ │
│  │                         │                                    │ │
│  │                         │ proxy_pass                         │ │
│  │                         │ (Docker network)                   │ │
│  │                         ▼                                    │ │
│  │  ┌─────────────────────────────────────────────────────┐    │ │
│  │  │  Container: backend                                 │    │ │
│  │  │  ────────────────────                               │    │ │
│  │  │  Image: month-balance-backend:latest                │    │ │
│  │  │  Expose: 5150 (APENAS INTERNO)                      │    │ │
│  │  │                                                     │    │ │
│  │  │  ┌─────────────────────────────────────┐          │    │ │
│  │  │  │  .NET API                           │          │    │ │
│  │  │  │  ────────                           │          │    │ │
│  │  │  │  • ASPNETCORE_URLS=http://+:5150   │          │    │ │
│  │  │  │  • JWT Authentication               │          │    │ │
│  │  │  │  • WebAuthn                         │          │    │ │
│  │  │  │  • CORS: Não necessário             │          │    │ │
│  │  │  └─────────────────────────────────────┘          │    │ │
│  │  └──────────────────────┬──────────────────────────────┘    │ │
│  │                         │                                    │ │
│  │                         │ Host=postgres;Port=5432            │ │
│  │                         │ (Docker network)                   │ │
│  │                         ▼                                    │ │
│  │  ┌─────────────────────────────────────────────────────┐    │ │
│  │  │  Container: postgres                                │    │ │
│  │  │  ─────────────────────                              │    │ │
│  │  │  Image: postgres:17-alpine                          │    │ │
│  │  │  Expose: 5432 (APENAS INTERNO)                      │    │ │
│  │  │                                                     │    │ │
│  │  │  ┌─────────────────────────────────────┐          │    │ │
│  │  │  │  PostgreSQL Database                │          │    │ │
│  │  │  │  ───────────────────                │          │    │ │
│  │  │  │  • Database: monthbalance           │          │    │ │
│  │  │  │  • Volume: postgres_data (persistente)│        │    │ │
│  │  │  └─────────────────────────────────────┘          │    │ │
│  │  └─────────────────────────────────────────────────────┘    │ │
│  │                                                               │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## 🔄 Fluxo de Requisição Detalhado

### Exemplo: Login de Usuário

```
1. Usuário acessa: http://54.144.175.38/login
   └─> Nginx serve: /usr/share/nginx/html/index.html

2. Vue.js carrega e usuário clica em "Login"
   └─> JavaScript faz: POST /api/auth/login
   
3. Nginx intercepta /api/auth/login
   └─> nginx.conf: location /api/ { proxy_pass http://backend:5150/api/; }
   └─> Encaminha para: http://backend:5150/api/auth/login
   
4. Container backend recebe a requisição
   └─> AuthController.Login() processa
   └─> Consulta: postgres:5432
   
5. Backend retorna JWT token
   └─> Nginx encaminha resposta
   └─> Browser recebe token
   
6. Próximas requisições incluem: Authorization: Bearer <token>
```

## 🔒 Camadas de Segurança

### 1. Network Isolation

```
┌─────────────────────────────────────────┐
│  Docker Network (Isolada)               │
│  ─────────────────────────              │
│                                         │
│  ✅ frontend ←→ backend (permitido)    │
│  ✅ backend ←→ postgres (permitido)    │
│  ❌ Internet ←→ backend (bloqueado)    │
│  ❌ Internet ←→ postgres (bloqueado)   │
└─────────────────────────────────────────┘
```

### 2. Port Exposure

| Container | Porta Interna | Porta Externa | Status |
|-----------|---------------|---------------|--------|
| frontend | 80 | 80 | ✅ Exposta |
| backend | 5150 | - | ❌ Não exposta |
| postgres | 5432 | - | ❌ Não exposta |

### 3. AWS Security Group

```
┌─────────────────────────────────────────┐
│  EC2 Security Group                     │
│  ──────────────────                     │
│                                         │
│  Inbound Rules:                         │
│  ✅ 80/tcp   from 0.0.0.0/0            │
│  ✅ 443/tcp  from 0.0.0.0/0 (SSL)      │
│  ✅ 22/tcp   from SEU_IP (SSH)         │
│  ❌ 5150/tcp REMOVIDO                   │
│  ❌ 5432/tcp REMOVIDO                   │
└─────────────────────────────────────────┘
```

## 📦 Configuração dos Containers

### Frontend Container

```yaml
frontend:
  image: ghcr.io/SEU_USUARIO/month-balance-frontend:latest
  ports:
    - "80:80"  # Exposto publicamente
  networks:
    - month-balance-network
  depends_on:
    - backend
```

**Configuração Nginx:**
```nginx
location /api/ {
    proxy_pass http://backend:5150/api/;
    # backend = nome do serviço Docker
}
```

**Build Args:**
```bash
docker build --build-arg VITE_API_BASE_URL=/api
```

### Backend Container

```yaml
backend:
  image: ghcr.io/SEU_USUARIO/month-balance-backend:latest
  expose:
    - "5150"  # Apenas interno
  networks:
    - month-balance-network
  environment:
    ASPNETCORE_URLS: http://+:5150
    ConnectionStrings__DefaultConnection: Host=postgres;Port=5432;...
```

**Sem CORS necessário:**
```json
{
  "Cors": {
    "AllowedOrigins": []
  }
}
```

### PostgreSQL Container

```yaml
postgres:
  image: postgres:17-alpine
  expose:
    - "5432"  # Apenas interno
  networks:
    - month-balance-network
  volumes:
    - postgres_data:/var/lib/postgresql/data
```

## 🌐 DNS Resolution no Docker

```
┌─────────────────────────────────────────┐
│  Docker Internal DNS                    │
│  ──────────────────                     │
│                                         │
│  backend    → 172.18.0.3:5150          │
│  postgres   → 172.18.0.2:5432          │
│  frontend   → 172.18.0.4:80            │
│                                         │
│  Containers se comunicam por nome!      │
└─────────────────────────────────────────┘
```

## 📊 Comparação: Antes vs Depois

### Antes (Inseguro)

```
Internet → http://IP:5150/api/auth/login → Backend
         ↓
    Problemas:
    ❌ Backend exposto
    ❌ CORS necessário
    ❌ IP hardcoded no frontend
    ❌ Múltiplos pontos de entrada
```

### Depois (Seguro - BFF)

```
Internet → http://IP/api/auth/login → Nginx → Backend
         ↓
    Benefícios:
    ✅ Backend protegido
    ✅ Sem CORS
    ✅ URL relativa no frontend
    ✅ Único ponto de entrada
```

## 🎯 Pontos-Chave da Arquitetura

### 1. Backend for Frontend (BFF)

O Nginx atua como BFF, agregando e protegendo o backend:

```
Frontend (Vue.js)
    ↓ /api/*
Nginx (BFF)
    ↓ http://backend:5150/api/*
Backend (.NET)
```

### 2. Zero Trust Network

Apenas o que precisa estar exposto está exposto:

- ✅ Frontend: Porta 80 (necessário)
- ❌ Backend: Não exposto (protegido)
- ❌ Database: Não exposto (protegido)

### 3. Service Discovery

Docker Compose fornece DNS automático:

```bash
# Dentro do container frontend
ping backend  # Funciona!
curl http://backend:5150/api/health  # Funciona!

# Fora do Docker
ping backend  # Não funciona (correto!)
curl http://backend:5150/api/health  # Não funciona (correto!)
```

## 🔍 Verificação da Arquitetura

### Comandos de Verificação

```bash
# 1. Verificar que containers estão na mesma network
docker network inspect month-balance_month-balance-network

# 2. Verificar conectividade interna
docker-compose exec frontend ping backend
docker-compose exec backend ping postgres

# 3. Verificar portas expostas
docker-compose ps
# Apenas frontend deve ter 0.0.0.0:80->80/tcp

# 4. Verificar proxy reverso
docker-compose exec frontend curl http://backend:5150/api/health

# 5. Verificar que backend NÃO está exposto externamente
curl http://SEU_IP:5150/api/health  # Deve falhar!
```

## 📈 Escalabilidade Futura

Esta arquitetura permite fácil escalabilidade:

```yaml
# Adicionar mais backends
backend:
  deploy:
    replicas: 3

# Nginx faz load balancing automaticamente
upstream backend_cluster {
    server backend:5150;
}
```

## 🎉 Resultado Final

✅ Arquitetura BFF implementada  
✅ Backend completamente protegido  
✅ Comunicação interna via Docker network  
✅ Sem necessidade de CORS  
✅ Único ponto de entrada (porta 80)  
✅ Pronto para produção!  
