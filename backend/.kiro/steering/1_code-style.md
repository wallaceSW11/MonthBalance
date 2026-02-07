---
inclusion: always
priority: highest
---

# 🎨 Code Style - Month Balance Backend

## ⚠️ ESTAS REGRAS SÃO LEI - NUNCA VIOLE

---

## 🔴 Regra #1: Early Return SEMPRE

```csharp
// ✅ CERTO
public async Task<Income?> GetIncomeAsync(int id)
{
    if (id <= 0) return null;
    
    var income = await _repository.GetByIdAsync(id);
    
    if (income == null) return null;
    
    return income;
}

// ❌ ERRADO
public async Task<Income?> GetIncomeAsync(int id)
{
    if (id > 0)
    {
        var income = await _repository.GetByIdAsync(id);
        if (income != null)
        {
            return income;
        }
    }
    return null;
}
```

---

## 🔴 Regra #2: Ternário para 2 Caminhos

```csharp
// ✅ CERTO
var message = isActive ? "Ativo" : "Inativo";
var value = hasPermission ? ProcessData() : null;

// ❌ ERRADO
string message;
if (isActive)
{
    message = "Ativo";
}
else
{
    message = "Inativo";
}
```

---

## 🔴 Regra #3: Async/Await SEMPRE

```csharp
// ✅ CERTO
public async Task<List<Income>> GetIncomesAsync(int year, int month)
{
    try
    {
        var incomes = await _repository.GetByMonthAsync(year, month);
        return incomes;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting incomes");
        throw;
    }
}

// ❌ ERRADO
public Task<List<Income>> GetIncomesAsync(int year, int month)
{
    return _repository.GetByMonthAsync(year, month)
        .ContinueWith(task => task.Result);
}
```

---

## 🔴 Regra #4: Null Conditional Operator

```csharp
// ✅ CERTO
var name = user?.Profile?.Name;
var total = income?.NetValue ?? 0;

// ❌ ERRADO
var name = user != null && user.Profile != null ? user.Profile.Name : null;
var total = income != null && income.NetValue.HasValue ? income.NetValue.Value : 0;
```

---

## 🔴 Regra #5: Controllers Finos

```csharp
// ✅ CERTO
[HttpGet("{year}/{month}")]
public async Task<ActionResult<List<IncomeDto>>> GetByMonth(int year, int month)
{
    var incomes = await _incomeService.GetByMonthAsync(year, month);
    return Ok(incomes);
}

// ❌ ERRADO - Lógica no controller
[HttpGet("{year}/{month}")]
public async Task<ActionResult<List<IncomeDto>>> GetByMonth(int year, int month)
{
    if (year < 2000 || year > 2100) return BadRequest("Invalid year");
    if (month < 1 || month > 12) return BadRequest("Invalid month");
    
    var incomes = await _context.Incomes
        .Where(i => i.Year == year && i.Month == month)
        .ToListAsync();
    
    var dtos = incomes.Select(i => new IncomeDto
    {
        Id = i.Id,
        Name = i.Name,
        Value = i.Value
    }).ToList();
    
    return Ok(dtos);
}
```

---

## 🔴 Regra #6: DTOs para Requests/Responses

```csharp
// ✅ CERTO
public record CreateIncomeDto(string Name, decimal Value, int Year, int Month);
public record IncomeDto(int Id, string Name, decimal Value, int Year, int Month);

[HttpPost]
public async Task<ActionResult<IncomeDto>> Create(CreateIncomeDto dto)
{
    var income = await _incomeService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = income.Id }, income);
}

// ❌ ERRADO - Expor entidade diretamente
[HttpPost]
public async Task<ActionResult<Income>> Create(Income income)
{
    await _context.Incomes.AddAsync(income);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { id = income.Id }, income);
}
```

---

## 🔴 Regra #7: Repository Pattern

```csharp
// ✅ CERTO
public interface IIncomeRepository
{
    Task<List<Income>> GetByMonthAsync(int year, int month);
    Task<Income?> GetByIdAsync(int id);
    Task<Income> CreateAsync(Income income);
    Task UpdateAsync(Income income);
    Task DeleteAsync(int id);
}

public class IncomeRepository : IIncomeRepository
{
    private readonly AppDbContext _context;
    
    public IncomeRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Income>> GetByMonthAsync(int year, int month)
    {
        return await _context.Incomes
            .Where(i => i.Year == year && i.Month == month)
            .ToListAsync();
    }
}

// ❌ ERRADO - DbContext direto no service
public class IncomeService
{
    private readonly AppDbContext _context;
    
    public async Task<List<Income>> GetByMonthAsync(int year, int month)
    {
        return await _context.Incomes
            .Where(i => i.Year == year && i.Month == month)
            .ToListAsync();
    }
}
```

---

## 🔴 Regra #8: Service Layer para Lógica

```csharp
// ✅ CERTO
public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _repository;
    private readonly IMapper _mapper;
    
    public async Task<IncomeDto> CreateAsync(CreateIncomeDto dto)
    {
        var income = _mapper.Map<Income>(dto);
        
        var created = await _repository.CreateAsync(income);
        
        return _mapper.Map<IncomeDto>(created);
    }
}

// ❌ ERRADO - Lógica no controller
[HttpPost]
public async Task<ActionResult<IncomeDto>> Create(CreateIncomeDto dto)
{
    var income = new Income
    {
        Name = dto.Name,
        Value = dto.Value,
        Year = dto.Year,
        Month = dto.Month
    };
    
    await _context.Incomes.AddAsync(income);
    await _context.SaveChangesAsync();
    
    return Ok(new IncomeDto(income.Id, income.Name, income.Value, income.Year, income.Month));
}
```

---

## 🔴 Regra #9: FluentValidation

```csharp
// ✅ CERTO
public class CreateIncomeDtoValidator : AbstractValidator<CreateIncomeDto>
{
    public CreateIncomeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");
        
        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero");
        
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100).WithMessage("Ano inválido");
        
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("Mês inválido");
    }
}

// ❌ ERRADO - Validação manual no controller
[HttpPost]
public async Task<ActionResult<IncomeDto>> Create(CreateIncomeDto dto)
{
    if (string.IsNullOrEmpty(dto.Name))
        return BadRequest("Nome é obrigatório");
    
    if (dto.Value <= 0)
        return BadRequest("Valor deve ser maior que zero");
    
    // ...
}
```

---

## 🔴 Regra #10: Nomenclatura

```csharp
// ✅ CERTO
public class IncomeService { }
public interface IIncomeService { }
public record CreateIncomeDto { }
private readonly IIncomeRepository _repository;
public async Task<Income> GetByIdAsync(int id) { }

// ❌ ERRADO
public class incomeService { }  // PascalCase
public interface IncomeService { }  // Sem I
public record CreateIncomeRequest { }  // Usar Dto
private readonly IIncomeRepository repository;  // Sem _
public async Task<Income> GetById(int id) { }  // Sem Async
```

---

## 🔴 Regra #11: Migrations para Mudanças no Banco

```bash
# ✅ CERTO
dotnet ef migrations add AddIncomeTypeTable
dotnet ef database update

# ❌ ERRADO - Alterar banco manualmente via SQL
```

---

## 🔴 Regra #12: Dependency Injection

```csharp
// ✅ CERTO - Program.cs
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IIncomeService, IncomeService>();

// Controller
public class IncomesController : ControllerBase
{
    private readonly IIncomeService _incomeService;
    
    public IncomesController(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }
}

// ❌ ERRADO - new direto
public class IncomesController : ControllerBase
{
    private readonly IncomeService _incomeService;
    
    public IncomesController()
    {
        _incomeService = new IncomeService();
    }
}
```

---

## 🔴 Regra #13: AutoMapper

```csharp
// ✅ CERTO
public class IncomeProfile : Profile
{
    public IncomeProfile()
    {
        CreateMap<Income, IncomeDto>();
        CreateMap<CreateIncomeDto, Income>();
    }
}

// Service
var income = _mapper.Map<Income>(dto);
var incomeDto = _mapper.Map<IncomeDto>(income);

// ❌ ERRADO - Mapeamento manual
var incomeDto = new IncomeDto
{
    Id = income.Id,
    Name = income.Name,
    Value = income.Value,
    Year = income.Year,
    Month = income.Month
};
```

---

## 🔴 Regra #14: Async Naming

```csharp
// ✅ CERTO
public async Task<Income> GetByIdAsync(int id) { }
public async Task<List<Income>> GetAllAsync() { }
public async Task CreateAsync(Income income) { }

// ❌ ERRADO
public async Task<Income> GetById(int id) { }
public async Task<List<Income>> GetAll() { }
public async Task Create(Income income) { }
```

---

## 🔴 Regra #15: Exception Handling

```csharp
// ✅ CERTO
public async Task<Income> GetByIdAsync(int id)
{
    try
    {
        return await _repository.GetByIdAsync(id);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting income {Id}", id);
        throw;
    }
}

// ❌ ERRADO - Engolir exceção
public async Task<Income> GetByIdAsync(int id)
{
    try
    {
        return await _repository.GetByIdAsync(id);
    }
    catch
    {
        return null;
    }
}
```

---

## 🎯 Checklist Rápido

### Estrutura
- [ ] Controllers finos (apenas orquestração)
- [ ] Services para lógica de negócio
- [ ] Repositories para acesso a dados
- [ ] DTOs para requests/responses
- [ ] AutoMapper para mapeamentos

### Código
- [ ] Early return
- [ ] Async/await
- [ ] Null conditional operator (`?.`)
- [ ] Nomenclatura correta (PascalCase, _private, Async)

### Validação
- [ ] FluentValidation para DTOs
- [ ] Validações no service layer

### Banco de Dados
- [ ] Migrations para mudanças
- [ ] Repository pattern
- [ ] DbContext apenas no repository

### Dependency Injection
- [ ] Interfaces registradas no Program.cs
- [ ] Injeção via construtor

---

**Versão:** 1.0 (Month Balance Backend)
**Data:** 22/01/2026
