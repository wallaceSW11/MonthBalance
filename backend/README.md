# 🚀 Month Balance API

Backend da aplicação Month Balance - Controle financeiro pessoal mensal.

## 🛠️ Stack

- **.NET 10.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 10.0**
- **PostgreSQL 17**
- **JWT Authentication**
- **Docker & Docker Compose**

## 📦 Estrutura

```
backend/
├── Controllers/      # Endpoints REST
├── Models/          # Entidades do banco
├── DTOs/            # Data Transfer Objects
├── Services/        # Lógica de negócio
├── Repositories/    # Acesso a dados
├── Data/            # DbContext
├── Middleware/      # Autenticação, etc
├── Migrations/      # EF Core Migrations
└── Program.cs       # Entry point
```

## 🚀 Como Rodar

### Desenvolvimento Local (sem Docker)

1. **Instalar PostgreSQL 17** (porta 5432)

2. **Configurar banco**:
```bash
# Criar database
createdb -U postgres monthbalance
```

3. **Aplicar migrations**:
```bash
dotnet ef database update
```

4. **Rodar API**:
```bash
dotnet run
```

API disponível em: `http://localhost:5000`  
Swagger: `http://localhost:5000/swagger`

### Com Docker

1. **Criar arquivo .env**:
```bash
cp .env.example .env
```

2. **Editar .env**:
```env
DB_NAME=monthbalance
DB_USER=postgres
DB_PASSWORD=postgres123
JWT_SECRET=your-super-secret-key-min-32-chars-here
```

3. **Subir containers**:
```bash
docker-compose up -d
```

4. **Aplicar migrations** (primeira vez):
```bash
docker exec -it month-balance-api dotnet ef database update
```

API disponível em: `http://localhost:5150`

## 📝 Scripts Úteis

```bash
# Build
dotnet build

# Run (hot reload)
dotnet watch run

# Criar migration
dotnet ef migrations add MigrationName

# Aplicar migrations
dotnet ef database update

# Remover última migration
dotnet ef migrations remove

# Dropar banco
dotnet ef database drop
```

## 🔐 Autenticação

A API usa **JWT Bearer Token**. 

Para endpoints protegidos, envie o header:
```
Authorization: Bearer {seu-token-jwt}
```

## 📚 Documentação

Swagger disponível em: `http://localhost:5150/swagger`

## 🎯 Status

✅ Fase 1: Setup Inicial - **CONCLUÍDO**
- [x] Projeto .NET 10 criado
- [x] Entity Framework Core configurado
- [x] PostgreSQL configurado
- [x] DbContext criado
- [x] Docker configurado
- [x] JWT configurado
- [x] CORS configurado
- [x] Entidades criadas
- [x] Migration inicial criada

🔄 Próximas Fases:
- [ ] Fase 2: Auth Module
- [ ] Fase 3: MonthData Module
- [ ] Fase 4: IncomeTypes Module
- [ ] Fase 5: Incomes Module
- [ ] Fase 6: ExpenseTypes Module
- [ ] Fase 7: Expenses Module

---

**Versão:** 1.0  
**Data:** 06/02/2026
