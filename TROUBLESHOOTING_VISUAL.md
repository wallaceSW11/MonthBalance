# 🔧 Troubleshooting Visual - Guia de Diagnóstico

## 🚨 Problema 1: Backend ainda está exposto (ERRO CRÍTICO!)

### Sintoma
```bash
curl http://SEU_IP:5150/api/health
# Responde com 200 OK ❌ (NÃO DEVERIA!)
```

### Diagnóstico
```bash
docker-compose ps
```

**❌ Se ver isso (ERRADO):**
```
NAME                    PORTS
month-balance-api       0.0.0.0:5150->5150/tcp  ❌ EXPOSTO!
```

**✅ Deveria ser assim (CORRETO):**
```
NAME                    PORTS
month-balance-api       5150/tcp  ✅ APENAS INTERNO!
```

### Solução
```bash
# 1. Editar docker-compose.yml
nano docker-compose.yml

# 2. Mudar de:
backend:
  ports:
    - "5150:5150"  # ❌ REMOVER

# 3. Para:
backend:
  expose:
    - "5150"  # ✅ ADICIONAR

# 4. Recriar containers
docker-compose down
docker-compose up -d

# 5. Verificar
docker-compose ps
curl http://localhost:5150/api/health  # ❌ Deve falhar agora!
```

---

## 🚨 Problema 2: Erro 502 Bad Gateway

### Sintoma
```bash
curl http://SEU_IP/api/health
# 502 Bad Gateway ❌
```

### Diagnóstico Passo a Passo

#### Passo 1: Backend está rodando?
```bash
docker-compose ps backend
```

**❌ Se ver:**
```
NAME                    STATUS
month-balance-api       Exited (1)  ❌
```

**Solução:**
```bash
# Ver logs do backend
docker-compose logs backend

# Reiniciar
docker-compose restart backend
```

#### Passo 2: Nginx consegue alcançar o backend?
```bash
docker-compose exec frontend curl http://backend:5150/api/health
```

**❌ Se falhar:**
```
curl: (6) Could not resolve host: backend
```

**Solução:**
```bash
# Verificar network
docker network inspect month-balance_month-balance-network

# Verificar se backend está na network
docker-compose ps
docker-compose down
docker-compose up -d
```

#### Passo 3: Nginx configurado corretamente?
```bash
docker-compose exec frontend cat /etc/nginx/conf.d/default.conf | grep proxy_pass
```

**❌ Se ver:**
```nginx
proxy_pass http://localhost:5150/api/;  ❌ ERRADO!
```

**✅ Deveria ser:**
```nginx
proxy_pass http://backend:5150/api/;  ✅ CORRETO!
```

**Solução:**
```bash
# 1. Editar nginx.conf local
nano frontend/nginx.conf

# 2. Mudar localhost para backend
proxy_pass http://backend:5150/api/;

# 3. Rebuild do frontend
cd frontend
docker build -t ghcr.io/SEU_USUARIO/month-balance-frontend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-frontend:latest

# 4. No EC2
docker-compose pull frontend
docker-compose up -d frontend
```

---

## 🚨 Problema 3: Erro de CORS (não deveria acontecer!)

### Sintoma
```
Console do navegador:
Access to XMLHttpRequest at 'http://IP:5150/api/auth/login' 
from origin 'http://IP' has been blocked by CORS policy ❌
```

### Diagnóstico

#### Passo 1: Frontend está usando URL correta?
```bash
# Verificar arquivos JS do frontend
docker-compose exec frontend cat /usr/share/nginx/html/assets/*.js | grep -o "http://[^\"]*" | head -5
```

**❌ Se ver IPs ou portas hardcoded:**
```
http://54.144.175.38:5150  ❌ ERRADO!
http://localhost:5150      ❌ ERRADO!
```

**✅ NÃO deveria mostrar nada ou apenas:**
```
(nenhum resultado)  ✅ CORRETO!
```

**Solução:**
```bash
# Frontend foi built com VITE_API_BASE_URL errado!
# Rebuild com a variável correta:

cd frontend
docker build --build-arg VITE_API_BASE_URL=/api -t frontend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-frontend:latest

# No EC2
docker-compose pull frontend
docker-compose up -d frontend
```

#### Passo 2: Verificar .env do frontend
```bash
cat frontend/.env
```

**❌ Se ver:**
```env
VITE_API_BASE_URL=http://54.144.175.38:5150/api  ❌ ERRADO!
```

**✅ Deveria ser:**
```env
VITE_API_BASE_URL=/api  ✅ CORRETO!
```

---

## 🚨 Problema 4: Backend não conecta ao banco

### Sintoma
```bash
docker-compose logs backend
# Error: Failed to connect to postgres:5432 ❌
```

### Diagnóstico

#### Passo 1: Postgres está rodando?
```bash
docker-compose ps postgres
```

**❌ Se ver:**
```
NAME                    STATUS
month-balance-db        Exited (1)  ❌
```

**Solução:**
```bash
docker-compose logs postgres
docker-compose restart postgres
```

#### Passo 2: Backend consegue pingar postgres?
```bash
docker-compose exec backend ping -c 3 postgres
```

**❌ Se falhar:**
```
ping: postgres: Name or service not known ❌
```

**Solução:**
```bash
# Recriar network
docker-compose down
docker-compose up -d
```

#### Passo 3: Credenciais corretas?
```bash
# Verificar .env
cat .env

# Testar conexão manualmente
docker-compose exec postgres psql -U postgres -d monthbalance -c "SELECT 1;"
```

---

## 🚨 Problema 5: Frontend não carrega (página em branco)

### Sintoma
```bash
curl http://SEU_IP/
# Retorna vazio ou erro 404 ❌
```

### Diagnóstico

#### Passo 1: Arquivos foram copiados?
```bash
docker-compose exec frontend ls -la /usr/share/nginx/html/
```

**❌ Se ver apenas:**
```
total 8
drwxr-xr-x 2 root root 4096 ...
```

**✅ Deveria ver:**
```
total 1234
-rw-r--r-- 1 root root  1234 ... index.html
drwxr-xr-x 2 root root  4096 ... assets/
```

**Solução:**
```bash
# Rebuild do frontend
cd frontend
docker build -t frontend:latest .
docker push ghcr.io/SEU_USUARIO/month-balance-frontend:latest

# No EC2
docker-compose pull frontend
docker-compose up -d frontend
```

#### Passo 2: Nginx está funcionando?
```bash
docker-compose exec frontend nginx -t
```

**❌ Se ver erros:**
```
nginx: configuration file /etc/nginx/nginx.conf test failed ❌
```

**Solução:**
```bash
docker-compose logs frontend
# Corrigir erros no nginx.conf
```

---

## 🚨 Problema 6: Containers não iniciam

### Sintoma
```bash
docker-compose up -d
# Error: ... ❌
```

### Diagnóstico

#### Passo 1: Ver logs detalhados
```bash
docker-compose up
# (sem -d para ver logs em tempo real)
```

#### Passo 2: Verificar imagens
```bash
docker images | grep month-balance
```

**❌ Se não ver as imagens:**
```
(nenhum resultado) ❌
```

**Solução:**
```bash
# Pull das imagens
docker-compose pull

# Ou build local
docker-compose build
```

#### Passo 3: Verificar .env
```bash
cat .env
```

**Verificar que tem:**
- ✅ DB_PASSWORD
- ✅ JWT_SECRET (mínimo 32 caracteres)

---

## 📊 Fluxograma de Diagnóstico

```
┌─────────────────────────────────────┐
│ Problema?                           │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│ Backend responde em :5150?          │
│ curl http://IP:5150/api/health      │
└──────────────┬──────────────────────┘
               │
        ┌──────┴──────┐
        │             │
       SIM           NÃO
        │             │
        ▼             ▼
    ❌ ERRO!      ✅ OK!
    Ver Problema 1
        │
        ▼
┌─────────────────────────────────────┐
│ API responde em /api?               │
│ curl http://IP/api/health           │
└──────────────┬──────────────────────┘
               │
        ┌──────┴──────┐
        │             │
       NÃO           SIM
        │             │
        ▼             ▼
    Ver Problema 2  ✅ OK!
        │
        ▼
┌─────────────────────────────────────┐
│ Erro de CORS no navegador?          │
└──────────────┬──────────────────────┘
               │
        ┌──────┴──────┐
        │             │
       SIM           NÃO
        │             │
        ▼             ▼
    Ver Problema 3  ✅ TUDO OK!
```

---

## 🔍 Comandos de Verificação Rápida

### Checklist Completo

```bash
# 1. Containers rodando?
docker-compose ps
# Todos devem estar "Up"

# 2. Backend NÃO exposto?
curl http://localhost:5150/api/health
# Deve falhar (Connection refused)

# 3. API funciona via proxy?
curl http://localhost/api/health
# Deve retornar 200 OK

# 4. Conectividade interna?
docker-compose exec frontend ping -c 3 backend
docker-compose exec backend ping -c 3 postgres
# Ambos devem funcionar

# 5. Portas corretas?
docker-compose ps
# Frontend: 0.0.0.0:80->80/tcp
# Backend:  5150/tcp (sem 0.0.0.0)

# 6. Logs sem erros?
docker-compose logs --tail=50
# Verificar se há erros críticos

# 7. Network configurada?
docker network inspect month-balance_month-balance-network
# Deve mostrar os 3 containers
```

---

## 🎯 Teste Final de Validação

Execute este script completo:

```bash
#!/bin/bash

echo "=== TESTE DE VALIDAÇÃO COMPLETO ==="
echo ""

# Teste 1
echo "1. Backend NÃO deve estar exposto..."
if curl -s -o /dev/null -w "%{http_code}" http://localhost:5150/api/health | grep -q "000"; then
    echo "   ✅ PASS: Backend não está exposto"
else
    echo "   ❌ FAIL: Backend está exposto! (Ver Problema 1)"
fi

# Teste 2
echo "2. API deve funcionar via proxy..."
if curl -s -o /dev/null -w "%{http_code}" http://localhost/api/health | grep -q "200"; then
    echo "   ✅ PASS: API funciona via proxy"
else
    echo "   ❌ FAIL: API não funciona via proxy (Ver Problema 2)"
fi

# Teste 3
echo "3. Containers devem estar rodando..."
if [ $(docker-compose ps -q | wc -l) -eq 3 ]; then
    echo "   ✅ PASS: Todos os containers rodando"
else
    echo "   ❌ FAIL: Containers não estão rodando (Ver Problema 6)"
fi

# Teste 4
echo "4. Frontend deve ter porta 80 exposta..."
if docker-compose ps | grep -q "0.0.0.0:80->80"; then
    echo "   ✅ PASS: Frontend na porta 80"
else
    echo "   ❌ FAIL: Frontend não está na porta 80"
fi

# Teste 5
echo "5. Backend NÃO deve ter porta exposta publicamente..."
if ! docker-compose ps | grep -q "0.0.0.0:5150"; then
    echo "   ✅ PASS: Backend não está exposto"
else
    echo "   ❌ FAIL: Backend está exposto! (Ver Problema 1)"
fi

echo ""
echo "=== FIM DOS TESTES ==="
```

---

## 📞 Suporte

Se após seguir todos os passos ainda houver problemas:

1. Verifique os logs: `docker-compose logs -f`
2. Verifique a documentação: `DEPLOY_AWS.md`
3. Verifique o checklist: `CHECKLIST_DEPLOY.md`
4. Verifique a arquitetura: `ARQUITETURA_FINAL.md`
