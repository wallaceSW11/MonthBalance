# 📝 Changelog

## [1.0.0] - 2026-01-22

### ✅ Fase 1: Setup Inicial
- Projeto .NET 10 Web API criado
- Entity Framework Core 10.0.0 configurado
- Npgsql PostgreSQL provider instalado
- Estrutura de pastas completa
- CORS configurado para desenvolvimento
- Health check endpoint (`/api/health`)

### ✅ Fase 2: Entity Framework + PostgreSQL
- Models criados com **INT Identity** (performance otimizada)
- ApplicationDbContext configurado
- Relacionamentos entre entidades
- Migration `InitialCreateWithIntIds` criada
- Seed data implementado
- Scripts PowerShell para gerenciar PostgreSQL
- Docker Compose para desenvolvimento

### 🔄 Refatorações
- **GUID → INT Identity**: Mudança de `Guid` para `int` com auto-increment
  - **Motivo**: Performance (índices 4x menores), simplicidade, controle pelo backend
  - **Impacto**: Consultas mais rápidas, melhor para clustered index
  - **Breaking Change**: Frontend precisa ser atualizado para usar `number` ao invés de `string`

### 📦 Estrutura de Dados

**MonthData**
- `Id` (int, identity)
- `Year` (int)
- `Month` (int, 1-12)
- Índice único em (Year, Month)

**Income**
- `Id` (int, identity)
- `Name` (string, max 200)
- `Type` (string, "manual" ou "hourly")
- `GrossValue` (decimal, nullable)
- `NetValue` (decimal, nullable)
- `HourlyRate` (decimal, nullable)
- `Hours` (int, nullable)
- `Minutes` (int, nullable)
- `MonthDataId` (FK)

**Expense**
- `Id` (int, identity)
- `Name` (string, max 200)
- `Value` (decimal)
- `MonthDataId` (FK)

### 📚 Documentação
- README.md
- DATABASE_SETUP.md
- DOCKER_SETUP.md
- CHANGELOG.md (este arquivo)

### 🐳 Docker
- `docker-compose.dev.yml` - PostgreSQL para desenvolvimento
- Scripts PowerShell para gerenciar containers

### 🔜 Próximos Passos
- Fase 3: Implementação da API REST
- Fase 4: Autenticação e Autorização
- Fase 5: Integração Frontend + Backend
