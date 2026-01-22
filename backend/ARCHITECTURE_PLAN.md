# 🏗️ Plano de Arquitetura - Backend (Global Types)

## 🎯 Objetivo

Implementar tipos globais de receitas e despesas que são reutilizados entre meses, permitindo cadastro rápido e consistência de dados.

---

## 📊 Mudanças no Banco de Dados

### Novas Tabelas

#### 1. IncomeTypes (Tipos de Receita)
```sql
CREATE TABLE IncomeTypes (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Type VARCHAR(20) NOT NULL, -- 'manual' ou 'hourly'
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);
```

#### 2. ExpenseTypes (Tipos de Despesa)
```sql
CREATE TABLE ExpenseTypes (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);
```

### Tabelas Modificadas

#### 3. Incomes (Lançamentos de Receita)
```sql
-- ANTES
CREATE TABLE Incomes (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,  -- ❌ REMOVER
    Type VARCHAR(20) NOT NULL,   -- ❌ REMOVER
    GrossValue DECIMAL(18,2),
    NetValue DECIMAL(18,2),
    HourlyRate DECIMAL(18,2),
    Hours INT,
    Minutes INT,
    MonthDataId INT NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NOT NULL
);

-- DEPOIS
CREATE TABLE Incomes (
    Id SERIAL PRIMARY KEY,
    IncomeTypeId INT NOT NULL,   -- ✅ NOVO (FK para IncomeTypes)
    GrossValue DECIMAL(18,2),
    NetValue DECIMAL(18,2),
    HourlyRate DECIMAL(18,2),
    Hours INT,
    Minutes INT,
    MonthDataId INT NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NOT NULL,
    FOREIGN KEY (IncomeTypeId) REFERENCES IncomeTypes(Id),
    FOREIGN KEY (MonthDataId) REFERENCES MonthData(Id)
);
```

#### 4. Expenses (Lançamentos de Despesa)
```sql
-- ANTES
CREATE TABLE Expenses (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,  -- ❌ REMOVER
    Value DECIMAL(18,2) NOT NULL,
    MonthDataId INT NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NOT NULL
);

-- DEPOIS
CREATE TABLE Expenses (
    Id SERIAL PRIMARY KEY,
    ExpenseTypeId INT NOT NULL,  -- ✅ NOVO (FK para ExpenseTypes)
    Value DECIMAL(18,2) NOT NULL,
    MonthDataId INT NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NOT NULL,
    FOREIGN KEY (ExpenseTypeId) REFERENCES ExpenseTypes(Id),
    FOREIGN KEY (MonthDataId) REFERENCES MonthData(Id)
);
```

---

## 🔧 Implementação Backend

### 1. Models

#### IncomeType.cs
```csharp
public class IncomeType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "manual"; // "manual" ou "hourly"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
}
```

#### ExpenseType.cs
```csharp
public class ExpenseType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
```

#### Income.cs (Modificado)
```csharp
public class Income
{
    public int Id { get; set; }
    public int IncomeTypeId { get; set; }  // ✅ NOVO
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    public int MonthDataId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public IncomeType IncomeType { get; set; } = null!;  // ✅ NOVO
    public MonthData MonthData { get; set; } = null!;
}
```

#### Expense.cs (Modificado)
```csharp
public class Expense
{
    public int Id { get; set; }
    public int ExpenseTypeId { get; set; }  // ✅ NOVO
    public decimal Value { get; set; }
    public int MonthDataId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public ExpenseType ExpenseType { get; set; } = null!;  // ✅ NOVO
    public MonthData MonthData { get; set; } = null!;
}
```

### 2. DTOs

#### IncomeType DTOs
```csharp
public record IncomeTypeDto(int Id, string Name, string Type);
public record CreateIncomeTypeDto(string Name, string Type);
public record UpdateIncomeTypeDto(string Name, string Type);
```

#### ExpenseType DTOs
```csharp
public record ExpenseTypeDto(int Id, string Name);
public record CreateExpenseTypeDto(string Name);
public record UpdateExpenseTypeDto(string Name);
```

#### Income DTOs (Modificado)
```csharp
public record IncomeDto(
    int Id,
    int IncomeTypeId,           // ✅ NOVO
    string IncomeTypeName,      // ✅ NOVO (para exibição)
    string IncomeTypeType,      // ✅ NOVO (para exibição)
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes,
    int MonthDataId
);

public record CreateIncomeDto(
    int IncomeTypeId,           // ✅ NOVO
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);

public record UpdateIncomeDto(
    int IncomeTypeId,           // ✅ NOVO
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);
```

#### Expense DTOs (Modificado)
```csharp
public record ExpenseDto(
    int Id,
    int ExpenseTypeId,          // ✅ NOVO
    string ExpenseTypeName,     // ✅ NOVO (para exibição)
    decimal Value,
    int MonthDataId
);

public record CreateExpenseDto(
    int ExpenseTypeId,          // ✅ NOVO
    decimal Value
);

public record UpdateExpenseDto(
    int ExpenseTypeId,          // ✅ NOVO
    decimal Value
);
```

### 3. Repositories

#### IIncomeTypeRepository.cs
```csharp
public interface IIncomeTypeRepository
{
    Task<List<IncomeType>> GetAllAsync();
    Task<IncomeType?> GetByIdAsync(int id);
    Task<IncomeType> CreateAsync(IncomeType incomeType);
    Task UpdateAsync(IncomeType incomeType);
    Task DeleteAsync(int id);
}
```

#### IExpenseTypeRepository.cs
```csharp
public interface IExpenseTypeRepository
{
    Task<List<ExpenseType>> GetAllAsync();
    Task<ExpenseType?> GetByIdAsync(int id);
    Task<ExpenseType> CreateAsync(ExpenseType expenseType);
    Task UpdateAsync(ExpenseType expenseType);
    Task DeleteAsync(int id);
}
```

### 4. Services

#### IIncomeTypeService.cs
```csharp
public interface IIncomeTypeService
{
    Task<List<IncomeTypeDto>> GetAllAsync();
    Task<IncomeTypeDto?> GetByIdAsync(int id);
    Task<IncomeTypeDto> CreateAsync(CreateIncomeTypeDto dto);
    Task<IncomeTypeDto> UpdateAsync(int id, UpdateIncomeTypeDto dto);
    Task DeleteAsync(int id);
}
```

#### IExpenseTypeService.cs
```csharp
public interface IExpenseTypeService
{
    Task<List<ExpenseTypeDto>> GetAllAsync();
    Task<ExpenseTypeDto?> GetByIdAsync(int id);
    Task<ExpenseTypeDto> CreateAsync(CreateExpenseTypeDto dto);
    Task<ExpenseTypeDto> UpdateAsync(int id, UpdateExpenseTypeDto dto);
    Task DeleteAsync(int id);
}
```

### 5. Controllers

#### IncomeTypesController.cs
```csharp
[ApiController]
[Route("api/incometypes")]
public class IncomeTypesController : ControllerBase
{
    // GET /api/incometypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncomeTypeDto>>> GetAll();
    
    // GET /api/incometypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<IncomeTypeDto>> GetById(int id);
    
    // POST /api/incometypes
    [HttpPost]
    public async Task<ActionResult<IncomeTypeDto>> Create(CreateIncomeTypeDto dto);
    
    // PUT /api/incometypes/5
    [HttpPut("{id}")]
    public async Task<ActionResult<IncomeTypeDto>> Update(int id, UpdateIncomeTypeDto dto);
    
    // DELETE /api/incometypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id);
}
```

#### ExpenseTypesController.cs
```csharp
[ApiController]
[Route("api/expensetypes")]
public class ExpenseTypesController : ControllerBase
{
    // GET /api/expensetypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseTypeDto>>> GetAll();
    
    // GET /api/expensetypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseTypeDto>> GetById(int id);
    
    // POST /api/expensetypes
    [HttpPost]
    public async Task<ActionResult<ExpenseTypeDto>> Create(CreateExpenseTypeDto dto);
    
    // PUT /api/expensetypes/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseTypeDto>> Update(int id, UpdateExpenseTypeDto dto);
    
    // DELETE /api/expensetypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id);
}
```

---

## 🔄 Migração de Dados

### Estratégia de Migração

1. **Criar novas tabelas** (IncomeTypes, ExpenseTypes)
2. **Migrar dados existentes:**
   - Para cada `Income` único (Name + Type), criar um `IncomeType`
   - Para cada `Expense` único (Name), criar um `ExpenseType`
   - Atualizar `Incomes` com `IncomeTypeId`
   - Atualizar `Expenses` com `ExpenseTypeId`
3. **Remover colunas antigas** (Name, Type)

### Script de Migração (C#)
```csharp
// Migration: AddGlobalTypes
public partial class AddGlobalTypes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // 1. Criar tabelas IncomeTypes e ExpenseTypes
        migrationBuilder.CreateTable(
            name: "IncomeTypes",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(maxLength: 200, nullable: false),
                Type = table.Column<string>(maxLength: 20, nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false),
                UpdatedAt = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IncomeTypes", x => x.Id);
            });
        
        // 2. Adicionar coluna IncomeTypeId em Incomes (nullable temporariamente)
        migrationBuilder.AddColumn<int>(
            name: "IncomeTypeId",
            table: "Incomes",
            nullable: true);
        
        // 3. Migrar dados (executar via SQL ou código)
        // Será feito em DbInitializer ou migration custom
        
        // 4. Tornar IncomeTypeId NOT NULL
        migrationBuilder.AlterColumn<int>(
            name: "IncomeTypeId",
            table: "Incomes",
            nullable: false);
        
        // 5. Adicionar FK
        migrationBuilder.AddForeignKey(
            name: "FK_Incomes_IncomeTypes_IncomeTypeId",
            table: "Incomes",
            column: "IncomeTypeId",
            principalTable: "IncomeTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
        
        // 6. Remover colunas antigas
        migrationBuilder.DropColumn(name: "Name", table: "Incomes");
        migrationBuilder.DropColumn(name: "Type", table: "Incomes");
        
        // Repetir para Expenses...
    }
}
```

---

## 📋 Endpoints da API

### IncomeTypes
- `GET /api/incometypes` - Listar todos os tipos de receita
- `GET /api/incometypes/{id}` - Buscar tipo por ID
- `POST /api/incometypes` - Criar novo tipo
- `PUT /api/incometypes/{id}` - Atualizar tipo
- `DELETE /api/incometypes/{id}` - Deletar tipo

### ExpenseTypes
- `GET /api/expensetypes` - Listar todos os tipos de despesa
- `GET /api/expensetypes/{id}` - Buscar tipo por ID
- `POST /api/expensetypes` - Criar novo tipo
- `PUT /api/expensetypes/{id}` - Atualizar tipo
- `DELETE /api/expensetypes/{id}` - Deletar tipo

### Incomes (Modificado)
- `GET /api/months/{year}/{month}/incomes` - Listar receitas do mês (com nome do tipo)
- `POST /api/months/{year}/{month}/incomes` - Criar lançamento (requer IncomeTypeId)
- `PUT /api/months/{year}/{month}/incomes/{id}` - Atualizar lançamento
- `DELETE /api/months/{year}/{month}/incomes/{id}` - Deletar lançamento

### Expenses (Modificado)
- `GET /api/months/{year}/{month}/expenses` - Listar despesas do mês (com nome do tipo)
- `POST /api/months/{year}/{month}/expenses` - Criar lançamento (requer ExpenseTypeId)
- `PUT /api/months/{year}/{month}/expenses/{id}` - Atualizar lançamento
- `DELETE /api/months/{year}/{month}/expenses/{id}` - Deletar lançamento

---

## ✅ Checklist de Implementação

### Fase 1: Estrutura Base
- [ ] Criar models `IncomeType` e `ExpenseType`
- [ ] Criar DTOs para tipos
- [ ] Criar repositories para tipos
- [ ] Criar services para tipos
- [ ] Criar controllers para tipos
- [ ] Registrar DI no `Program.cs`

### Fase 2: Migration
- [ ] Criar migration `AddGlobalTypes`
- [ ] Implementar script de migração de dados
- [ ] Testar migration em banco de desenvolvimento
- [ ] Validar integridade dos dados

### Fase 3: Modificar Incomes/Expenses
- [ ] Modificar models `Income` e `Expense`
- [ ] Atualizar DTOs
- [ ] Atualizar repositories (incluir `.Include(IncomeType)`)
- [ ] Atualizar services (mapear nome do tipo)
- [ ] Atualizar controllers
- [ ] Testar endpoints

### Fase 4: Duplicação de Mês
- [ ] Atualizar lógica de duplicação
- [ ] Copiar lançamentos com `IncomeTypeId` e `ExpenseTypeId`
- [ ] Testar duplicação

### Fase 5: Testes
- [ ] Testar CRUD de IncomeTypes
- [ ] Testar CRUD de ExpenseTypes
- [ ] Testar criação de lançamentos com tipos
- [ ] Testar duplicação de mês
- [ ] Testar exclusão de tipos (validar se tem lançamentos)

---

**Versão:** 1.0  
**Data:** 22/01/2026
