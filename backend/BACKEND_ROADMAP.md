# 🚀 Backend Roadmap - Month Balance API

## 📋 Stack Tecnológica

- **.NET 10** (ASP.NET Core Web API)
- **Entity Framework Core** (ORM + Migrations)
- **PostgreSQL** (Database)
- **Docker** (Backend + Database)
- **JWT** (Autenticação)
- **FluentValidation** (Validação de dados)
- **AutoMapper** (Mapeamento de DTOs)

---

## 🗂️ Estrutura do Projeto

```
MonthBalance.API/
├── Controllers/          # Endpoints da API
├── Models/              # Entidades do banco
├── DTOs/                # Data Transfer Objects
├── Services/            # Lógica de negócio
├── Repositories/        # Acesso a dados
├── Validators/          # Validações FluentValidation
├── Middleware/          # Autenticação, Logging, etc
├── Data/                # DbContext e Migrations
├── Configurations/      # Configurações EF
├── Dockerfile
└── docker-compose.yml
```

---

## 📊 Modelo de Dados (Entidades)

### 1. User
```csharp
public class User
{
    public int Id { get; set; } // Auto-increment
    public string Name { get; set; }
    public string Email { get; set; } // Unique
    public string PasswordHash { get; set; }
    public string? Avatar { get; set; }
    public bool NotificationsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public ICollection<MonthData> MonthData { get; set; }
    public ICollection<IncomeTypeModel> IncomeTypes { get; set; }
    public ICollection<ExpenseTypeModel> ExpenseTypes { get; set; }
}
```

### 2. MonthData
```csharp
public class MonthData
{
    public int Id { get; set; } // Auto-increment
    public int UserId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime LastAccessed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public User User { get; set; }
    public ICollection<Income> Incomes { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}
```
**Índice Único:** `UserId + Year + Month` (cada usuário só pode ter 1 registro por mês/ano)

### 3. IncomeTypeModel
```csharp
public class IncomeTypeModel
{
    public int Id { get; set; } // Auto-increment
    public int UserId { get; set; }
    public string Name { get; set; }
    public IncomeType Type { get; set; } // Enum: Paycheck, Hourly, Extra
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public User User { get; set; }
    public ICollection<Income> Incomes { get; set; }
}

public enum IncomeType
{
    Paycheck,
    Hourly,
    Extra
}
```

### 4. Income
```csharp
public class Income
{
    public int Id { get; set; } // Auto-increment
    public int MonthDataId { get; set; }
    public int IncomeTypeId { get; set; }
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    public decimal CalculatedValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public MonthData MonthData { get; set; }
    public IncomeTypeModel IncomeType { get; set; }
}
```

### 5. ExpenseTypeModel
```csharp
public class ExpenseTypeModel
{
    public int Id { get; set; } // Auto-increment
    public int UserId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public User User { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}
```

### 6. Expense
```csharp
public class Expense
{
    public int Id { get; set; } // Auto-increment
    public int MonthDataId { get; set; }
    public int ExpenseTypeId { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public MonthData MonthData { get; set; }
    public ExpenseTypeModel ExpenseType { get; set; }
}
```

---

## 🔌 Endpoints da API

### 🔐 Auth Controller (`/api/auth`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/register` | Criar nova conta | ❌ |
| POST | `/login` | Login (retorna JWT) | ❌ |
| POST | `/logout` | Logout | ✅ |
| POST | `/change-password` | Alterar senha | ✅ |
| GET | `/me` | Dados do usuário logado | ✅ |
| PUT | `/me` | Atualizar perfil | ✅ |

**DTOs:**
```csharp
// Request
public class RegisterRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}

public class UpdateUserRequest
{
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public bool NotificationsEnabled { get; set; }
}

// Response
public class LoginResponse
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Avatar { get; set; }
    public bool NotificationsEnabled { get; set; }
}
```

---

### 📅 MonthData Controller (`/api/month-data`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/` | Listar todos os meses do usuário | ✅ |
| GET | `/{year}/{month}` | Buscar mês específico | ✅ |
| POST | `/` | Criar novo mês | ✅ |
| PUT | `/{id}/last-accessed` | Atualizar último acesso | ✅ |
| DELETE | `/{id}` | Deletar mês | ✅ |

**DTOs:**
```csharp
public class MonthDataDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime LastAccessed { get; set; }
}

public class CreateMonthDataRequest
{
    public int Year { get; set; }
    public int Month { get; set; }
}
```

---

### 💰 IncomeTypes Controller (`/api/income-types`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/` | Listar tipos de receita do usuário | ✅ |
| GET | `/{id}` | Buscar tipo específico | ✅ |
| POST | `/` | Criar novo tipo | ✅ |
| PUT | `/{id}` | Atualizar tipo | ✅ |
| DELETE | `/{id}` | Deletar tipo | ✅ |

**DTOs:**
```csharp
public class IncomeTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // "paycheck", "hourly", "extra"
}

public class CreateIncomeTypeRequest
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public class UpdateIncomeTypeRequest
{
    public string Name { get; set; }
}
```

---

### 💵 Incomes Controller (`/api/incomes`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/month/{monthDataId}` | Listar receitas do mês | ✅ |
| GET | `/{id}` | Buscar receita específica | ✅ |
| POST | `/` | Criar nova receita | ✅ |
| PUT | `/{id}` | Atualizar receita | ✅ |
| DELETE | `/{id}` | Deletar receita | ✅ |

**DTOs:**
```csharp
public class IncomeDto
{
    public int Id { get; set; }
    public int MonthDataId { get; set; }
    public int IncomeTypeId { get; set; }
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    public decimal CalculatedValue { get; set; }
}

public class CreateIncomeRequest
{
    public int MonthDataId { get; set; }
    public int IncomeTypeId { get; set; }
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
}

public class UpdateIncomeRequest
{
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
}
```

---

### 💳 ExpenseTypes Controller (`/api/expense-types`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/` | Listar tipos de despesa do usuário | ✅ |
| GET | `/{id}` | Buscar tipo específico | ✅ |
| POST | `/` | Criar novo tipo | ✅ |
| PUT | `/{id}` | Atualizar tipo | ✅ |
| DELETE | `/{id}` | Deletar tipo | ✅ |

**DTOs:**
```csharp
public class ExpenseTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CreateExpenseTypeRequest
{
    public string Name { get; set; }
}

public class UpdateExpenseTypeRequest
{
    public string Name { get; set; }
}
```

---

### 💸 Expenses Controller (`/api/expenses`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/month/{monthDataId}` | Listar despesas do mês | ✅ |
| GET | `/{id}` | Buscar despesa específica | ✅ |
| POST | `/` | Criar nova despesa | ✅ |
| PUT | `/{id}` | Atualizar despesa | ✅ |
| DELETE | `/{id}` | Deletar despesa | ✅ |

**DTOs:**
```csharp
public class ExpenseDto
{
    public int Id { get; set; }
    public int MonthDataId { get; set; }
    public int ExpenseTypeId { get; set; }
    public decimal Value { get; set; }
}

public class CreateExpenseRequest
{
    public int MonthDataId { get; set; }
    public int ExpenseTypeId { get; set; }
    public decimal Value { get; set; }
}

public class UpdateExpenseRequest
{
    public decimal Value { get; set; }
}
```

---

## 🐳 Docker Setup

### Dockerfile (Backend)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5150

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MonthBalance.API.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonthBalance.API.dll"]
```

### docker-compose.yml
```yaml
version: '3.8'

services:
  postgres:
    image: postgres:17-alpine
    container_name: month-balance-db
    environment:
      POSTGRES_DB: ${DB_NAME}
      POSTGRES_USER: ${DB_USER}
      POSTGRES_PASSWORD: ${DB_PASSWORD}
    ports:
      - "5433:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    networks:
      - month-balance-network

  api:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: month-balance-api
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: "Host=postgres;Port=5432;Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD}"
      Jwt__Secret: ${JWT_SECRET}
      Jwt__Issuer: "MonthBalanceAPI"
      Jwt__Audience: "MonthBalanceApp"
      Jwt__ExpirationHours: 24
    ports:
      - "5150:5150"
    depends_on:
      - postgres
    networks:
      - month-balance-network
    env_file:
      - .env

volumes:
  postgres_data:

networks:
  month-balance-network:
    driver: bridge
```

### .env (exemplo)
```env
# Database
DB_NAME=monthbalance
DB_USER=postgres
DB_PASSWORD=postgres123

# JWT
JWT_SECRET=your-super-secret-key-change-in-production-min-32-chars
```

### .env.example
```env
# Database
DB_NAME=monthbalance
DB_USER=postgres
DB_PASSWORD=

# JWT
JWT_SECRET=
```

---

## 📝 Plano de Implementação (Fases)

### ✅ Fase 1: Setup Inicial
- [ ] Criar projeto .NET 10 Web API
- [ ] Configurar Entity Framework Core
- [ ] Configurar PostgreSQL
- [ ] Criar DbContext
- [ ] Configurar Docker (Backend + Database)
- [ ] Configurar JWT Authentication
- [ ] Configurar CORS para frontend

### ✅ Fase 2: Entidades e Migrations
- [ ] Criar todas as entidades (Models)
- [ ] Configurar relacionamentos (Fluent API)
- [ ] Criar primeira migration
- [ ] Aplicar migration no banco
- [ ] Seed inicial (dados de teste)

### ✅ Fase 3: Auth Module
- [ ] Implementar AuthController
- [ ] Implementar AuthService (hash de senha, JWT)
- [ ] Endpoints: Register, Login, Logout, Me, Update, ChangePassword
- [ ] Validações (FluentValidation)
- [ ] Middleware de autenticação

### ✅ Fase 4: MonthData Module
- [ ] Implementar MonthDataController
- [ ] Implementar MonthDataService
- [ ] Implementar MonthDataRepository
- [ ] CRUD completo
- [ ] Validações

### ✅ Fase 5: IncomeTypes Module
- [ ] Implementar IncomeTypesController
- [ ] Implementar IncomeTypesService
- [ ] Implementar IncomeTypesRepository
- [ ] CRUD completo
- [ ] Validações

### ✅ Fase 6: Incomes Module
- [ ] Implementar IncomesController
- [ ] Implementar IncomesService (cálculo de CalculatedValue)
- [ ] Implementar IncomesRepository
- [ ] CRUD completo
- [ ] Validações
- [ ] Lógica de cálculo (Paycheck, Hourly, Extra)

### ✅ Fase 7: ExpenseTypes Module
- [ ] Implementar ExpenseTypesController
- [ ] Implementar ExpenseTypesService
- [ ] Implementar ExpenseTypesRepository
- [ ] CRUD completo
- [ ] Validações

### ✅ Fase 8: Expenses Module
- [ ] Implementar ExpensesController
- [ ] Implementar ExpensesService
- [ ] Implementar ExpensesRepository
- [ ] CRUD completo
- [ ] Validações

### ✅ Fase 9: Testes e Refinamento
- [ ] Testes unitários (Services)
- [ ] Testes de integração (Controllers)
- [ ] Tratamento de erros global
- [ ] Logging (Serilog)
- [ ] Documentação Swagger

### ✅ Fase 10: Deploy e Integração
- [ ] Testar Docker completo
- [ ] Integrar com frontend
- [ ] Ajustes finais
- [ ] README do backend

---

## 🔒 Segurança

- **JWT** para autenticação
- **BCrypt** para hash de senhas
- **CORS** configurado apenas para frontend
- **Validação** de todos os inputs
- **Authorization** em todos os endpoints protegidos
- **Rate Limiting** (opcional)
- **Isolamento de dados por usuário** (cada usuário só acessa seus próprios dados)

---

## 🛡️ Isolamento de Dados por Usuário

### Estratégia: Single Schema com Filtros por UserId

Para o MVP, todos os dados ficam no **mesmo schema (public)** do PostgreSQL. O isolamento é feito via **UserId** nas queries:

**Vantagens:**
- Simples de implementar
- Performance adequada para MVP
- Fácil de migrar para multi-tenant no futuro

**Implementação:**
```csharp
// Exemplo: Repository Base
public abstract class BaseRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly int _currentUserId;
    
    protected BaseRepository(DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _currentUserId = GetUserIdFromToken(httpContextAccessor);
    }
    
    // Todas as queries filtram por UserId automaticamente
}
```

**Regras de Negócio:**
- User só vê seus próprios MonthData, IncomeTypes, ExpenseTypes
- Ao criar Income/Expense, validar se MonthData pertence ao usuário
- Ao criar MonthData, validar se já existe para aquele mês/ano
- Ao deletar IncomeType/ExpenseType, validar se não há registros vinculados

**Migração Futura (se necessário):**
- Criar schema por tenant (ex: `user_123`)
- Usar Row Level Security (RLS) do PostgreSQL
- Implementar multi-tenancy com schema separation

---

## 📦 Pacotes NuGet Necessários

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
```

---

## 🎯 Regras de Negócio Importantes

### Income - Cálculo de CalculatedValue
```csharp
// Paycheck: usa NetValue (ou GrossValue se NetValue for null)
if (type == IncomeType.Paycheck)
    calculatedValue = netValue ?? grossValue ?? 0;

// Hourly: (HourlyRate * Hours) + (HourlyRate * Minutes / 60)
if (type == IncomeType.Hourly)
    calculatedValue = (hourlyRate * hours) + (hourlyRate * minutes / 60);

// Extra: usa NetValue (ou GrossValue se NetValue for null)
if (type == IncomeType.Extra)
    calculatedValue = netValue ?? grossValue ?? 0;
```

### MonthData
- Cada usuário pode ter apenas **1 MonthData por mês/ano** (índice único: UserId + Year + Month)
- Ao criar Income/Expense, verificar se MonthData pertence ao usuário logado
- Ao deletar MonthData, deletar em cascata Incomes e Expenses

### IncomeTypes e ExpenseTypes
- Cada usuário tem seus próprios tipos (isolamento por UserId)
- Não permitir deletar tipo se houver Incomes/Expenses vinculados
- Validar se tipo pertence ao usuário antes de qualquer operação

### Isolamento de Dados
- **TODAS** as queries devem filtrar por UserId do token JWT
- Validar ownership antes de UPDATE/DELETE
- Retornar 404 (não 403) para evitar information disclosure

---

## 📝 Configuração do Projeto

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=monthbalance;Username=postgres;Password=postgres123"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-change-in-production-min-32-chars",
    "Issuer": "MonthBalanceAPI",
    "Audience": "MonthBalanceApp",
    "ExpirationHours": 24
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": ["http://localhost:5173", "http://localhost:4173"]
  }
}
```

### appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

**IMPORTANTE:** 
- Em produção (Oracle Cloud), usar variáveis de ambiente
- Não commitar `.env` no git
- Adicionar `.env` no `.gitignore`

---

## 📞 Próximos Passos

1. **Aprovar este roadmap**
2. **Criar pasta do backend** (ex: `month-balance-api/`)
3. **Abrir Kiro na pasta do backend**
4. **Iniciar Fase 1** (Setup Inicial)

---

**Versão:** 1.0  
**Data:** 06/02/2026  
**Autor:** Kiro AI
