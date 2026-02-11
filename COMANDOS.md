# Comandos Essenciais

## Desenvolvimento Local

```bash
# Iniciar backend + banco
./scripts/dev.sh

# Iniciar frontend
cd frontend && npm run dev

# Parar
docker compose -f docker-compose.dev.yml down
```

## Build e Deploy

```bash
# Build das imagens
docker build -t ghcr.io/wallacesw11/month-balance-backend:latest ./backend
docker build -t ghcr.io/wallacesw11/month-balance-frontend:latest ./frontend

# Push
docker push ghcr.io/wallacesw11/month-balance-backend:latest
docker push ghcr.io/wallacesw11/month-balance-frontend:latest

# No EC2
cd ~/month-balance
git pull
docker compose pull
docker compose down
docker compose up -d
```

## Logs e Debug

```bash
# Ver logs
docker compose logs -f
docker compose logs backend --tail=100
docker compose logs frontend --tail=100

# Testar backend internamente
docker compose exec frontend curl http://backend:8080/api/auth/login

# Verificar variáveis
docker compose exec backend printenv | grep ASPNETCORE
docker compose exec backend printenv | grep JWT
```

## Banco de Dados

```bash
# Acessar PostgreSQL
docker compose exec postgres psql -U postgres -d monthbalance

# Backup
docker compose exec postgres pg_dump -U postgres monthbalance > backup.sql

# Restaurar
cat backup.sql | docker compose exec -T postgres psql -U postgres monthbalance
```

## Limpeza

```bash
# Parar tudo
docker compose down

# Limpar volumes (APAGA DADOS!)
docker compose down -v

# Limpar imagens antigas
docker system prune -a
```
