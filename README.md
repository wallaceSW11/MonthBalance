# 💰 Month Balance

Sistema de controle financeiro pessoal com gestão mensal de receitas e despesas.

## Stack

- **Frontend:** Vue 3 + TypeScript + Vite + TailwindCSS
- **Backend:** .NET 10 + Entity Framework Core + PostgreSQL 17
- **Deploy:** Docker + Nginx

## Desenvolvimento Local

```bash
# Iniciar backend e banco
./scripts/dev.sh  # Linux/Mac
.\scripts\dev.ps1  # Windows

# Iniciar frontend (outro terminal)
cd frontend
npm install
npm run dev
```

**Portas:**
- Frontend: http://localhost:5173
- Backend: http://localhost:5000
- PostgreSQL: localhost:5432

## Produção (AWS)

```bash
# Build e push das imagens
./scripts/build-images.sh
./scripts/push-images.sh

# No EC2
docker compose pull
docker compose up -d
```

**Arquitetura:**
- Frontend: porta 80 (público)
- Backend: porta 8080 (interno, via proxy reverso)
- PostgreSQL: porta 5432 (interno)

## Comandos Úteis

```bash
# Ver logs
docker compose logs -f [backend|frontend|postgres]

# Parar tudo
docker compose down

# Rebuild
docker compose build --no-cache

# Testes
cd frontend && npm run test:unit
```

## Estrutura

```
month-balance/
├── backend/              # API .NET
├── frontend/             # App Vue 3
├── scripts/              # Scripts úteis
├── docker-compose.yml    # Produção
└── docker-compose.dev.yml # Desenvolvimento
```
