# 💰 Month Balance

Sistema completo de controle financeiro pessoal com frontend Vue.js e backend .NET.

## 🏗️ Arquitetura

- **Frontend**: Vue 3 + TypeScript + Vite + TailwindCSS
- **Backend**: .NET 10 + Entity Framework Core
- **Database**: PostgreSQL 17
- **Containerização**: Docker + Docker Compose

## 🚀 Quick Start com Docker

### 1. Configure o ambiente

```bash
# Copie o arquivo de exemplo
cp .env.example .env

# Edite o .env e configure:
# - DB_PASSWORD (senha do PostgreSQL)
# - JWT_SECRET (mínimo 32 caracteres)
```

### 2. Inicie todos os serviços

**No Windows (PowerShell):**
```powershell
.\docker-start.ps1
```

**No Linux/Mac ou WSL2:**
```bash
chmod +x docker-start.sh
./docker-start.sh
```

**Ou manualmente:**
```bash
docker compose up -d
```

### 3. Acesse a aplicação

- **Frontend**: http://localhost:8080
- **Backend API**: http://localhost:5150
- **Swagger**: http://localhost:5150/swagger

## 📁 Estrutura do Projeto

```
month-balance/
├── frontend/           # Aplicação Vue.js
│   ├── src/
│   ├── Dockerfile
│   └── docker-compose.yml
├── backend/            # API .NET
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Dockerfile
│   └── docker-compose.yml
├── docker-compose.yml  # Orquestração completa
├── .env.example        # Variáveis de ambiente
└── DOCKER_SETUP.md     # Documentação detalhada
```

## 🔧 Desenvolvimento

### Cenários de uso:

**1. Desenvolvimento local (sem Docker):**
- Frontend: `pnpm dev` → http://localhost:5173
- Backend: `dotnet run` → http://localhost:5000
- PostgreSQL: Instância local ou Docker isolado

**2. Teste local com Docker:**
- Frontend: http://localhost:8080
- Backend: http://localhost:5150
- PostgreSQL: http://localhost:5433

**3. Produção (Oracle Cloud):**
- Configuração via variáveis de ambiente
- Containers prontos para deploy

### Rodar o projeto completo (RECOMENDADO)

```bash
# Na raiz do projeto
docker compose up -d
```

Isso sobe: PostgreSQL + Backend + Frontend integrados.

### Rodar apenas o backend (desenvolvimento isolado)

Útil quando você está desenvolvendo o backend e quer rodar o frontend localmente com `pnpm dev`:

```bash
cd backend
docker compose up -d
```

Isso sube: PostgreSQL + Backend (sem frontend).

### Rodar apenas o frontend (desenvolvimento isolado)

Útil quando você já tem o backend rodando e quer apenas testar o build do frontend:

```bash
cd frontend
docker compose up -d
```

Isso sobe: Apenas o frontend (precisa do backend rodando em outro lugar).

## 📊 Comandos Úteis

```bash
# Ver logs de todos os serviços
docker compose logs -f

# Ver logs de um serviço específico
docker compose logs -f backend

# Parar todos os serviços
docker compose down

# Rebuild após mudanças no código
docker compose up -d --build

# Limpar tudo (CUIDADO: apaga o banco!)
docker compose down -v
```

## 🗄️ Database

O PostgreSQL roda na porta **5433** externamente para evitar conflitos.

**Conectar via psql:**
```bash
docker compose exec postgres psql -U postgres -d monthbalance
```

**String de conexão:**
```
Host=localhost;Port=5433;Database=monthbalance;Username=postgres;Password=sua_senha
```

## 🌐 Deploy para Oracle Cloud

Este projeto está preparado para deploy no **Oracle Cloud Free Tier**:
- Configuração via variáveis de ambiente
- Containers prontos para Oracle Container Instances
- Fácil migração para Oracle Database

## 📚 Documentação

- [DOCKER_SETUP.md](./DOCKER_SETUP.md) - Guia completo do Docker
- [backend/README.md](./backend/README.md) - Documentação do backend
- [frontend/README.md](./frontend/README.md) - Documentação do frontend
- [backend/API_DOCUMENTATION.md](./backend/API_DOCUMENTATION.md) - Documentação da API

## 🛠️ Tecnologias

### Frontend
- Vue 3 + Composition API
- TypeScript
- Vite
- TailwindCSS
- Pinia (State Management)
- Vue Router
- Axios

### Backend
- .NET 10
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- WebAuthn (Passkeys)
- Swagger/OpenAPI

## 📝 Portas

### Desenvolvimento Local (sem Docker)
- Frontend: 5173 (dev) / 4173 (preview)
- Backend: 5000

### Docker Local
- Frontend: 8080
- Backend: 5150
- PostgreSQL: 5433

## 📝 Licença

Este projeto é privado e de uso pessoal.
