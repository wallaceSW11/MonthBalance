# 🗄️ Database Setup Guide

## 📋 Pré-requisitos

- Docker e Docker Compose instalados
- .NET 10 SDK
- dotnet-ef tool (já instalado globalmente)

## 🚀 Setup Rápido

### 1. Iniciar PostgreSQL

```powershell
cd backend/scripts
.\start-postgres.ps1
```

Ou manualmente:

```bash
cd backend
docker-compose -f docker-compose.dev.yml up -d
```

### 2. Aplicar Migrations

As migrations são aplicadas automaticamente ao rodar a API em modo Development.

Ou manualmente:

```powershell
cd backend/scripts
.\apply-migrations.ps1
```

Ou via comando:

```bash
cd backend
dotnet ef database update --project MonthBalance.API
```

### 3. Verificar Banco

```bash
docker exec -it mb-postgres-dev psql -U mbuser -d monthbalance
```

Comandos úteis no psql:

```sql
-- Listar tabelas
\dt

-- Ver estrutura de uma tabela
\d "MonthData"
\d "Incomes"
\d "Expenses"

-- Ver dados
SELECT * FROM "MonthData";
SELECT * FROM "Incomes";
SELECT * FROM "Expenses";

-- Sair
\q
```

## 🔧 Comandos Úteis

### Criar Nova Migration

```bash
cd backend
dotnet ef migrations add NomeDaMigration --project MonthBalance.API
```

### Reverter Migration

```bash
dotnet ef migrations remove --project MonthBalance.API
```

### Atualizar Banco para Migration Específica

```bash
dotnet ef database update NomeDaMigration --project MonthBalance.API
```

### Resetar Banco (CUIDADO!)

```bash
dotnet ef database drop --project MonthBalance.API --force
dotnet ef database update --project MonthBalance.API
```

## 🐳 Docker Commands

### Ver Logs do PostgreSQL

```bash
docker-compose -f docker-compose.dev.yml logs -f postgres
```

### Parar PostgreSQL

```powershell
cd backend/scripts
.\stop-postgres.ps1
```

Ou:

```bash
docker-compose -f docker-compose.dev.yml down
```

### Parar e Remover Volumes (APAGA DADOS!)

```bash
docker-compose -f docker-compose.dev.yml down -v
```

### Backup do Banco

```bash
docker exec mb-postgres-dev pg_dump -U mbuser monthbalance > backup.sql
```

### Restore do Banco

```bash
docker exec -i mb-postgres-dev psql -U mbuser monthbalance < backup.sql
```

## 📊 Estrutura do Banco

### Tabelas

- **MonthData**: Dados do mês (ano, mês)
- **Incomes**: Receitas do mês
- **Expenses**: Despesas do mês

### Relacionamentos

```
MonthData (1) ----< (N) Incomes
MonthData (1) ----< (N) Expenses
```

### Índices

- `MonthData`: Índice único em (Year, Month)

## 🔐 Credenciais (Development)

- **Host**: localhost
- **Port**: 5432
- **Database**: monthbalance
- **User**: mbuser
- **Password**: mbpass123

## ⚠️ Troubleshooting

### Erro: "Npgsql.PostgresException: 28P01: password authentication failed"

Verifique se o PostgreSQL está rodando e as credenciais estão corretas.

### Erro: "Cannot connect to database"

1. Verifique se o container está rodando: `docker ps`
2. Verifique os logs: `docker-compose -f docker-compose.dev.yml logs postgres`
3. Reinicie o container: `docker-compose -f docker-compose.dev.yml restart`

### Erro: "Port 5432 already in use"

Outro PostgreSQL está rodando. Pare-o ou mude a porta no `docker-compose.dev.yml`.

### Migrations não aplicam

1. Verifique se o banco está acessível
2. Delete a pasta `Migrations` e recrie: `dotnet ef migrations add InitialCreate`
3. Force update: `dotnet ef database update --force`

## 📝 Seed Data

A API cria automaticamente dados de exemplo ao iniciar em modo Development:

- 1 MonthData (mês atual)
- 1 Income de exemplo
- 1 Expense de exemplo

Para desabilitar, comente o código em `Program.cs`.
