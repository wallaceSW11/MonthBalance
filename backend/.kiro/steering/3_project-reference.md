---
inclusion: manual
priority: low
---


# 📚 Referência do Projeto - Month Balance Backend

## 🎯 Sobre o Projeto

API REST para controle financeiro pessoal mensal. Gerencia receitas e despesas por mês, com cálculo automático de saldo, duplicação de meses e navegação temporal.

---

## 🛠️ Stack Técnico

### Core
- **.NET**: 10.0
- **C#**: 12.0
- **ASP.NET Core**: 10.0.2
- **Entity Framework Core**: 10.0.0

### Database
- **PostgreSQL**: 17+
- **Npgsql.EntityFrameworkCore.PostgreSQL**: 10.0.0

### Tools
- **Microsoft.AspNetCore.OpenApi**: 10.0.2 (Swagger/OpenAPI)
- **EF Core Tools**: 10.0.0 (Migrations)

---

## 📜 Scripts

### Desenvolvimento
```bash
dotnet run                    # Servidor desenvolvimento (http://localhost:5150)
dotnet watch run              # Hot reload
```

### Database
```bash
dotnet ef migrations add MigrationName    # Criar migration
dotnet ef database update                 # Aplicar migrations
dotnet ef migrations remove               # Remover última migration
dotnet ef database drop                   # Dropar banco
```

### Build
```bash
dotnet build                  # Build
dotnet publish                # Publish para produção
```

---

## 📂 Estrutura do Projeto

```
backend/
├── Controllers/              # Endpoints REST
│   ├── ExpensesController.cs
│   ├── IncomesController.cs
│   └── MonthDataController.cs
├── Data/                     # DbContext e inicialização
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── DTOs/                     # Data Transfer Objects
│   ├── CreateExpenseDto.cs
│   ├── CreateIncomeDto.cs
│   ├── DuplicateMonthDto.cs
│   ├── ExpenseDto.cs
│   ├── IncomeDto.cs
│   ├── MonthDataDto.cs
│   ├── UpdateExpenseDto.cs
│   └── UpdateIncomeDto.cs
├── Mappings/                 # AutoMapper profiles (futuro)
├── Migrations/               # EF Core migrations
│   └── 20260122114542_InitialCreate.cs
├── Models/                   # Entidades do banco
│   ├── Expense.cs
│   ├── Income.cs
│   └── MonthData.cs
├── Repositories/             # Acesso a dados
│   ├── ExpenseRepository.cs
│   ├── IExpenseRepository.cs
│   ├── IIncomeRepository.cs
│   ├── IMonthDataRepository.cs
│   ├── IncomeRepository.cs
│   └── MonthDataRepository.cs
├── Services/                 # Lógica de negócio
│   ├── ExpenseService.cs
│   ├── IExpenseService.cs
│   ├── IIncomeService.cs
│   ├── IMonthDataService.cs
│   ├── IncomeService.cs
│   └── MonthDataService.cs
├── Validators/               # FluentValidation (futuro)
├── appsettings.json          # Configurações
├── appsettings.Development.json
├── Program.cs                # Entry point
└── MonthBalance.API.csproj   # Projeto
```

---

## 🔧 Convenções de Nomenclatura

### Arquivos
| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Controllers | PascalCase + Controller | `IncomesController.cs` |
| Services | PascalCase + Service | `IncomeService.cs` |
| Repositories | PascalCase + Repository | `IncomeRepository.cs` |
| Models | PascalCase | `Income.cs` |
| DTOs | PascalCase + Dto | `CreateIncomeDto.cs` |
| Interfaces | I + PascalCase | `IIncomeService.cs` |

### Código
| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Classes | PascalCase | `IncomeService`, `Income` |
| Interfaces | I + PascalCase | `IIncomeService` |
| Métodos | PascalCase + Async | `GetByIdAsync()` |
| Propriedades | PascalCase | `Name`, `Value` |
| Parâmetros | camelCase | `year`, `month` |
| Campos privados | _camelCase | `_repository`, `_context` |
| Constantes | PascalCase | `MaxNameLength` |

---

## 🎯 Regras de Organização

### 1. Camadas Bem Definidas
```
Controller → Service → Repository → Database
```

### 2. Dependency Injection
Todas as dependências registradas em `Program.cs`:
```csharp
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
```

### 3. DTOs para Comunicação
- `CreateXDto`: Criar entidade
- `UpdateXDto`: Atualizar entidade
- `XDto`: Retornar entidade

---

## 🏗️ Módulos Principais

### Receitas (Incomes)
Gerenciamento de receitas mensais (manual ou por hora).

**Endpoints:** `/api/months/{year}/{month}/incomes`  
**Controller:** `IncomesController.cs`  
**Service:** `IncomeService.cs`  
**Repository:** `IncomeRepository.cs`  
**Model:** `Income.cs`

### Despesas (Expenses)
Gerenciamento de despesas mensais.

**Endpoints:** `/api/months/{year}/{month}/expenses`  
**Controller:** `ExpensesController.cs`  
**Service:** `ExpenseService.cs`  
**Repository:** `ExpenseRepository.cs`  
**Model:** `Expense.cs`

### Dados do Mês (MonthData)
Gerenciamento de meses, duplicação e limpeza.

**Endpoints:** `/api/monthdata`  
**Controller:** `MonthDataController.cs`  
**Service:** `MonthDataService.cs`  
**Repository:** `MonthDataRepository.cs`  
**Model:** `MonthData.cs`

---

## 🎓 Glossário

- **Income**: Receita mensal (salário, freelance, etc)
- **Expense**: Despesa mensal (aluguel, contas, etc)
- **MonthData**: Dados de um mês específico (ano + mês + receitas + despesas)
- **DTO**: Data Transfer Object (objeto para transferência de dados)
- **Repository**: Camada de acesso a dados
- **Service**: Camada de lógica de negócio

---

## 🔄 Fluxo de Dados

### Request → Response
```
1. Client envia request para Controller
2. Controller chama Service
3. Service chama Repository
4. Repository acessa Database via EF Core
5. Database retorna entidade
6. Repository retorna entidade para Service
7. Service converte para DTO
8. Controller retorna DTO para Client
```

### Exemplo Prático
```csharp
// 1. Controller
[HttpGet("{id}")]
public async Task<ActionResult<IncomeDto>> GetById(int id)
{
    var income = await _incomeService.GetByIdAsync(id);
    return Ok(income);
}

// 2. Service
public async Task<IncomeDto?> GetByIdAsync(int id)
{
    var income = await _repository.GetByIdAsync(id);
    return income != null ? MapToDto(income) : null;
}

// 3. Repository
public async Task<Income?> GetByIdAsync(int id)
{
    return await _context.Incomes.FindAsync(id);
}
```

---

## ⚠️ Notas Importantes

### Versões
- .NET 10.0
- Entity Framework Core 10.0
- PostgreSQL 17+

### Frontend Integration
- API Base URL: `http://localhost:5150/api`
- CORS configurado para `http://localhost:5173`
- JSON em camelCase (configurado no `Program.cs`)

### Database
- PostgreSQL local (porta 5432)
- Connection string em `appsettings.Development.json`
- Migrations aplicadas automaticamente no startup (desenvolvimento)
- Sem seed data (banco limpo)

### Estrutura
- Controllers finos (apenas orquestração)
- Services com lógica de negócio
- Repositories para acesso a dados
- DTOs para requests/responses
- Dependency Injection para tudo

---

**Versão:** 1.0  
**Data:** 22/01/2026
