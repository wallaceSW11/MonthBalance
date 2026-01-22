# 🗺️ Roadmap - Backend .NET 10 + PostgreSQL

## 📋 Visão Geral

Backend API REST com .NET 10, Entity Framework Core, PostgreSQL e integração com frontend Vue 3.

**Ambiente DEV:** Frontend e Backend em portas separadas  
**Ambiente PROD:** Backend serve static files do frontend + API

---

## 🎯 Fases de Implementação

### ✅ Fase 1: Setup Inicial do Projeto ✔️ CONCLUÍDA
**Objetivo:** Criar estrutura base do projeto .NET 10

- [x] 1.1 Criar projeto Web API .NET 10
- [x] 1.2 Configurar estrutura de pastas (Controllers, Models, Services, Data)
- [x] 1.3 Adicionar pacotes NuGet essenciais
  - `Microsoft.EntityFrameworkCore` ✔️
  - `Npgsql.EntityFrameworkCore.PostgreSQL` ✔️
  - `Microsoft.EntityFrameworkCore.Design` ✔️
  - `Microsoft.EntityFrameworkCore.Tools` ✔️
- [x] 1.4 Configurar `appsettings.json` e `appsettings.Development.json`
- [x] 1.5 Configurar CORS para desenvolvimento

**Entregável:** ✅ Projeto .NET rodando com endpoint de health check (`http://localhost:5150/api/health`)

---

### ✅ Fase 2: Configuração do Entity Framework + PostgreSQL
**Objetivo:** Configurar banco de dados e migrations

- [ ] 2.1 Criar `ApplicationDbContext`
- [ ] 2.2 Configurar connection string do PostgreSQL
- [ ] 2.3 Criar models iniciais (User, Transaction, Category, etc)
- [ ] 2.4 Configurar relacionamentos entre entidades
- [ ] 2.5 Criar primeira migration
- [ ] 2.6 Aplicar migration no banco
- [ ] 2.7 Criar seed data (dados iniciais)

**Entregável:** Banco PostgreSQL configurado com migrations funcionando

---

### ✅ Fase 3: Implementação da API REST
**Objetivo:** Criar endpoints da API

- [ ] 3.1 Criar controllers base (BaseController)
- [ ] 3.2 Implementar padrão Repository
- [ ] 3.3 Implementar padrão Unit of Work
- [ ] 3.4 Criar DTOs (Data Transfer Objects)
- [ ] 3.5 Configurar AutoMapper
- [ ] 3.6 Implementar endpoints CRUD básicos
- [ ] 3.7 Adicionar validações (FluentValidation)
- [ ] 3.8 Implementar tratamento de erros global

**Entregável:** API REST funcional com CRUD completo

---

### ✅ Fase 4: Autenticação e Autorização
**Objetivo:** Implementar segurança da API

- [ ] 4.1 Configurar JWT Authentication
- [ ] 4.2 Criar endpoints de autenticação (login, register)
- [ ] 4.3 Implementar refresh token
- [ ] 4.4 Adicionar autorização baseada em roles
- [ ] 4.5 Proteger endpoints com `[Authorize]`
- [ ] 4.6 Configurar políticas de acesso

**Entregável:** Sistema de autenticação JWT funcional

---

### ✅ Fase 5: Integração Frontend + Backend (DEV)
**Objetivo:** Conectar frontend Vue com backend .NET em desenvolvimento

- [ ] 5.1 Configurar CORS para aceitar requisições do frontend
- [ ] 5.2 Criar serviço HTTP no frontend (axios)
- [ ] 5.3 Configurar variável de ambiente `VITE_API_BASE_URL`
- [ ] 5.4 Testar comunicação frontend → backend
- [ ] 5.5 Implementar interceptors para JWT
- [ ] 5.6 Adicionar tratamento de erros HTTP

**Entregável:** Frontend e Backend comunicando em DEV (portas separadas)

---

### ✅ Fase 6: Build e Deploy - Static Files (PROD)
**Objetivo:** Configurar backend para servir frontend em produção

- [ ] 6.1 Criar script de build no `package.json` do frontend
- [ ] 6.2 Configurar output do build para pasta `wwwroot` do backend
- [ ] 6.3 Adicionar middleware `UseStaticFiles` no backend
- [ ] 6.4 Configurar fallback para SPA (Single Page Application)
- [ ] 6.5 Ajustar `VITE_API_BASE_URL` para produção (rota relativa)
- [ ] 6.6 Criar script de deploy completo (build frontend + backend)
- [ ] 6.7 Testar build de produção localmente

**Entregável:** Backend servindo frontend + API na mesma porta

---

### ✅ Fase 7: Logging e Monitoramento
**Objetivo:** Adicionar logs e monitoramento

- [ ] 7.1 Configurar Serilog
- [ ] 7.2 Adicionar logs estruturados
- [ ] 7.3 Configurar diferentes níveis de log (DEV vs PROD)
- [ ] 7.4 Implementar middleware de logging de requisições
- [ ] 7.5 Adicionar health checks
- [ ] 7.6 Configurar Application Insights (opcional)

**Entregável:** Sistema de logging robusto

---

### ✅ Fase 8: Testes
**Objetivo:** Garantir qualidade do código

- [ ] 8.1 Configurar projeto de testes (xUnit)
- [ ] 8.2 Criar testes unitários para services
- [ ] 8.3 Criar testes de integração para controllers
- [ ] 8.4 Mockar DbContext para testes
- [ ] 8.5 Configurar cobertura de testes
- [ ] 8.6 Adicionar testes de performance (opcional)

**Entregável:** Suite de testes com cobertura > 80%

---

### ✅ Fase 9: Performance e Otimização
**Objetivo:** Otimizar performance da API

- [ ] 9.1 Implementar caching (Redis ou In-Memory)
- [ ] 9.2 Adicionar paginação em endpoints de listagem
- [ ] 9.3 Otimizar queries do EF (Include, AsNoTracking)
- [ ] 9.4 Implementar compressão de resposta (Gzip)
- [ ] 9.5 Configurar rate limiting
- [ ] 9.6 Adicionar índices no banco de dados

**Entregável:** API otimizada e performática

---

### ✅ Fase 10: Docker e Containerização
**Objetivo:** Configurar Docker para desenvolvimento e produção

- [ ] 10.1 Criar Dockerfile para backend (.NET 10)
- [ ] 10.2 Criar Dockerfile multi-stage (build + runtime)
- [ ] 10.3 Criar docker-compose.yml (backend + postgres)
- [ ] 10.4 Configurar volumes para persistência de dados
- [ ] 10.5 Configurar networks entre containers
- [ ] 10.6 Criar .dockerignore
- [ ] 10.7 Testar build e execução local com Docker
- [ ] 10.8 Otimizar imagem (reduzir tamanho)
- [ ] 10.9 Configurar health checks nos containers
- [ ] 10.10 Criar docker-compose.prod.yml para produção

**Entregável:** Aplicação rodando 100% em Docker (local)

---

### ✅ Fase 11: Deploy Oracle Cloud Free Tier
**Objetivo:** Preparar e fazer deploy no Oracle Cloud

- [ ] 11.1 Criar script de build da imagem Docker
- [ ] 11.2 Configurar variáveis de ambiente para produção
- [ ] 11.3 Criar documentação de deploy Oracle Cloud
- [ ] 11.4 Configurar backup automático do PostgreSQL
- [ ] 11.5 Configurar SSL/HTTPS (Let's Encrypt)
- [ ] 11.6 Configurar domínio e DNS
- [ ] 11.7 Testar deploy completo no Oracle Cloud
- [ ] 11.8 Configurar monitoramento e alertas
- [ ] 11.9 Criar script de rollback

**Entregável:** Aplicação rodando em produção no Oracle Cloud

---

### ✅ Fase 12: Documentação e CI/CD
**Objetivo:** Documentar e automatizar deploy

- [ ] 12.1 Configurar Swagger/OpenAPI
- [ ] 12.2 Documentar endpoints da API
- [ ] 12.3 Criar README.md completo do backend
- [ ] 12.4 Documentar processo de deploy
- [ ] 12.5 Configurar CI/CD (GitHub Actions)
- [ ] 12.6 Criar pipeline de testes automatizados
- [ ] 12.7 Configurar deploy automático para Oracle Cloud

**Entregável:** Projeto documentado e com deploy automatizado

---

## 📂 Estrutura de Pastas Proposta

```
backend/
├── MonthBalance.API/              # Projeto principal
│   ├── Controllers/               # Controllers da API
│   ├── Models/                    # Entidades do banco
│   ├── DTOs/                      # Data Transfer Objects
│   ├── Services/                  # Lógica de negócio
│   ├── Repositories/              # Acesso a dados
│   ├── Data/                      # DbContext e Migrations
│   ├── Middleware/                # Middlewares customizados
│   ├── Extensions/                # Extension methods
│   ├── Validators/                # Validações (FluentValidation)
│   ├── Mappings/                  # AutoMapper profiles
│   ├── wwwroot/                   # Static files (frontend build)
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs
│   └── MonthBalance.API.csproj
├── MonthBalance.Tests/            # Projeto de testes
│   ├── Unit/
│   ├── Integration/
│   └── MonthBalance.Tests.csproj
├── scripts/                       # Scripts de build e deploy
│   ├── build-frontend.ps1
│   ├── deploy-prod.ps1
│   ├── setup-database.ps1
│   └── docker-build.ps1
├── docker/                        # Configurações Docker
│   ├── Dockerfile
│   ├── Dockerfile.dev
│   ├── docker-compose.yml
│   ├── docker-compose.prod.yml
│   └── .dockerignore
├── .gitignore
├── ROADMAP.md                     # Este arquivo
└── README.md
```

---

## 🔧 Tecnologias e Pacotes

### Core
- **.NET 10** (Web API)
- **Entity Framework Core 10**
- **PostgreSQL** (Npgsql)

### Autenticação
- **JWT Bearer Authentication**
- **Microsoft.AspNetCore.Authentication.JwtBearer**

### Validação e Mapeamento
- **FluentValidation**
- **AutoMapper**

### Logging
- **Serilog**
- **Serilog.Sinks.Console**
- **Serilog.Sinks.File**

### Testes
- **xUnit**
- **Moq**
- **FluentAssertions**

### Documentação
- **Swashbuckle.AspNetCore** (Swagger)

### Performance
- **Microsoft.Extensions.Caching.Memory**
- **StackExchange.Redis** (opcional)

### Docker
- **Docker** (containerização)
- **Docker Compose** (orquestração local)
- **PostgreSQL** (imagem oficial)

---

## 🚀 Scripts de Build

### Frontend → Backend (Produção)

**package.json (frontend):**
```json
{
  "scripts": {
    "build:prod": "vue-tsc && vite build --mode production --outDir ../backend/MonthBalance.API/wwwroot"
  }
}
```

**PowerShell Script (backend/scripts/build-frontend.ps1):**
```powershell
# Navegar para frontend
Set-Location ../frontend

# Instalar dependências
pnpm install

# Build para produção (output direto no wwwroot)
pnpm run build:prod

# Voltar para backend
Set-Location ../backend

Write-Host "✅ Frontend build concluído e copiado para wwwroot" -ForegroundColor Green
```

---

## 🌍 Configuração de Ambientes

### Desenvolvimento (DEV)
- **Frontend:** `http://localhost:5173` (Vite)
- **Backend:** `http://localhost:5000` (API)
- **CORS:** Habilitado para `http://localhost:5173`
- **VITE_API_BASE_URL:** `http://localhost:5000/api`

### Produção (PROD)
- **Backend:** `http://localhost:5000` (API + Static Files)
- **Frontend:** Servido pelo backend via `wwwroot`
- **CORS:** Desabilitado (mesma origem)
- **VITE_API_BASE_URL:** `/api` (rota relativa)

---

## 📝 Próximos Passos

1. **Revisar roadmap** e ajustar conforme necessário
2. **Iniciar Fase 1** - Setup inicial do projeto
3. **Implementar incrementalmente** seguindo as fases
4. **Testar cada fase** antes de avançar

---

## 🎯 Metas de Qualidade

- ✅ Cobertura de testes > 80%
- ✅ Tempo de resposta API < 200ms
- ✅ Zero warnings no build
- ✅ Documentação completa (Swagger)
- ✅ Logs estruturados
- ✅ SOLID principles
- ✅ Clean Architecture

---

## 🐳 Configuração Docker

### Dockerfile (Multi-stage)

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY ["MonthBalance.API/MonthBalance.API.csproj", "MonthBalance.API/"]
RUN dotnet restore "MonthBalance.API/MonthBalance.API.csproj"

# Copiar código e buildar
COPY . .
WORKDIR "/src/MonthBalance.API"
RUN dotnet build "MonthBalance.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "MonthBalance.API.csproj" -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonthBalance.API.dll"]
```

### docker-compose.yml (Desenvolvimento)

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:16-alpine
    container_name: mb-postgres
    environment:
      POSTGRES_DB: monthbalance
      POSTGRES_USER: mbuser
      POSTGRES_PASSWORD: mbpass123
    ports:
      - "5432:5432"
    volumes:
      - postgres-data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U mbuser"]
      interval: 10s
      timeout: 5s
      retries: 5
    networks:
      - mb-network

  backend:
    build:
      context: .
      dockerfile: docker/Dockerfile
    container_name: mb-backend
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=monthbalance;Username=mbuser;Password=mbpass123
      - ASPNETCORE_URLS=http://+:80
    ports:
      - "5000:80"
    depends_on:
      postgres:
        condition: service_healthy
    volumes:
      - ./MonthBalance.API/wwwroot:/app/wwwroot
    networks:
      - mb-network

volumes:
  postgres-data:

networks:
  mb-network:
    driver: bridge
```

### docker-compose.prod.yml (Produção)

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:16-alpine
    container_name: mb-postgres-prod
    environment:
      POSTGRES_DB: ${POSTGRES_DB}
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    volumes:
      - postgres-data-prod:/var/lib/postgresql/data
      - ./backups:/backups
    restart: unless-stopped
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${POSTGRES_USER}"]
      interval: 30s
      timeout: 10s
      retries: 5
    networks:
      - mb-network-prod

  backend:
    build:
      context: .
      dockerfile: docker/Dockerfile
    container_name: mb-backend-prod
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}
      - ASPNETCORE_URLS=http://+:80
      - JWT_SECRET=${JWT_SECRET}
      - JWT_ISSUER=${JWT_ISSUER}
      - JWT_AUDIENCE=${JWT_AUDIENCE}
    ports:
      - "80:80"
      - "443:443"
    depends_on:
      postgres:
        condition: service_healthy
    restart: unless-stopped
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost/health"]
      interval: 30s
      timeout: 10s
      retries: 3
    networks:
      - mb-network-prod

volumes:
  postgres-data-prod:

networks:
  mb-network-prod:
    driver: bridge
```

### .dockerignore

```
**/bin/
**/obj/
**/out/
**/.vs/
**/.vscode/
**/.git/
**/.gitignore
**/node_modules/
**/dist/
**/*.md
**/Dockerfile*
**/docker-compose*
```

### Comandos Docker Úteis

```bash
# Build da imagem
docker-compose build

# Subir containers (DEV)
docker-compose up -d

# Ver logs
docker-compose logs -f backend

# Parar containers
docker-compose down

# Parar e remover volumes
docker-compose down -v

# Produção
docker-compose -f docker-compose.prod.yml up -d

# Executar migrations
docker-compose exec backend dotnet ef database update

# Backup do banco
docker-compose exec postgres pg_dump -U mbuser monthbalance > backup.sql

# Restore do banco
docker-compose exec -T postgres psql -U mbuser monthbalance < backup.sql
```

---

## ☁️ Oracle Cloud Free Tier - Especificações

### Recursos Disponíveis (Always Free)
- **Compute:** 2 VMs AMD (1/8 OCPU, 1GB RAM cada) OU 4 VMs ARM (1 OCPU, 6GB RAM cada)
- **Storage:** 200GB Block Volume
- **Database:** 2 Autonomous Databases (20GB cada)
- **Network:** 10TB de tráfego/mês

### Configuração Recomendada
- **VM ARM:** 1 OCPU, 6GB RAM (melhor custo-benefício)
- **OS:** Ubuntu 22.04 LTS
- **Docker + Docker Compose** instalados
- **PostgreSQL:** Container Docker (não usar Autonomous DB)
- **Backup:** Cron job diário para backup do PostgreSQL

### Portas a Abrir
- **80** (HTTP)
- **443** (HTTPS)
- **5432** (PostgreSQL - apenas interno)

---

**Versão:** 1.2  
**Última atualização:** Janeiro 2026  
**Status:** 🟢 Fase 1 Concluída | 🟡 Fase 2 em Andamento
