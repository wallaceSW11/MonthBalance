# 🌐 API Design - Month Balance Backend

## 🎯 Princípios REST

### 1. Recursos como Substantivos
```
✅ CERTO: /api/incomes
❌ ERRADO: /api/getIncomes
```

### 2. Verbos HTTP
- **GET**: Buscar dados
- **POST**: Criar recurso
- **PUT**: Atualizar recurso completo
- **PATCH**: Atualizar recurso parcial
- **DELETE**: Remover recurso

### 3. Status Codes
- **200 OK**: Sucesso (GET, PUT)
- **201 Created**: Recurso criado (POST)
- **204 No Content**: Sucesso sem retorno (DELETE)
- **400 Bad Request**: Dados inválidos
- **404 Not Found**: Recurso não encontrado
- **500 Internal Server Error**: Erro no servidor

---

## 📋 Estrutura de Endpoints

### Padrão de Rotas
```
/api/months/{year}/{month}/incomes
/api/months/{year}/{month}/expenses
/api/monthdata
```

### Exemplo Completo (Incomes)
```csharp
[ApiController]
[Route("api/months/{year}/{month}/incomes")]
public class IncomesController : ControllerBase
{
    // GET /api/months/2026/1/incomes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByMonth(int year, int month)
    {
        var incomes = await _incomeService.GetByMonthAsync(year, month);
        return Ok(incomes);
    }

    // GET /api/months/2026/1/incomes/123
    [HttpGet("{id}")]
    public async Task<ActionResult<IncomeDto>> GetById(int id)
    {
        var income = await _incomeService.GetByIdAsync(id);
        
        if (income == null) return NotFound();
        
        return Ok(income);
    }

    // POST /api/months/2026/1/incomes
    [HttpPost]
    public async Task<ActionResult<IncomeDto>> Create(int year, int month, [FromBody] CreateIncomeDto dto)
    {
        var created = await _incomeService.CreateAsync(year, month, dto);
        
        return CreatedAtAction(nameof(GetById), new { year, month, id = created.Id }, created);
    }

    // PUT /api/months/2026/1/incomes/123
    [HttpPut("{id}")]
    public async Task<ActionResult<IncomeDto>> Update(int id, [FromBody] UpdateIncomeDto dto)
    {
        try
        {
            var updated = await _incomeService.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE /api/months/2026/1/incomes/123
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _incomeService.DeleteAsync(id);
        return NoContent();
    }
}
```

---

## 📦 DTOs (Data Transfer Objects)

### Tipos de DTOs

#### 1. Create DTOs
Para criar recursos (POST).

```csharp
public record CreateIncomeDto(
    string Name,
    string Type,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);
```

#### 2. Update DTOs
Para atualizar recursos (PUT).

```csharp
public record UpdateIncomeDto(
    string Name,
    string Type,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);
```

#### 3. Response DTOs
Para retornar recursos (GET).

```csharp
public record IncomeDto(
    int Id,
    string Name,
    string Type,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes,
    int MonthDataId
);
```

### Regras de DTOs

✅ **SEMPRE:**
- Use `record` para DTOs imutáveis
- Nomes descritivos (`CreateIncomeDto`, não `IncomeRequest`)
- Propriedades em PascalCase (backend)
- JSON em camelCase (configurado no `Program.cs`)

❌ **NUNCA:**
- Expor entidades diretamente
- Incluir propriedades de navegação
- Incluir `CreatedAt`, `UpdatedAt` em Create/Update DTOs

---

## 🔒 Validação

### FluentValidation (Futuro)

```csharp
public class CreateIncomeDtoValidator : AbstractValidator<CreateIncomeDto>
{
    public CreateIncomeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Tipo é obrigatório")
            .Must(x => x == "manual" || x == "hourly")
            .WithMessage("Tipo deve ser 'manual' ou 'hourly'");
        
        When(x => x.Type == "manual", () =>
        {
            RuleFor(x => x.NetValue)
                .NotNull().WithMessage("Valor líquido é obrigatório para tipo manual")
                .GreaterThan(0).WithMessage("Valor líquido deve ser maior que zero");
        });
        
        When(x => x.Type == "hourly", () =>
        {
            RuleFor(x => x.HourlyRate)
                .NotNull().WithMessage("Valor por hora é obrigatório para tipo hourly")
                .GreaterThan(0).WithMessage("Valor por hora deve ser maior que zero");
            
            RuleFor(x => x.Hours)
                .NotNull().WithMessage("Horas são obrigatórias para tipo hourly")
                .GreaterThanOrEqualTo(0).WithMessage("Horas devem ser maior ou igual a zero");
        });
    }
}
```

### Data Annotations (Atual)

```csharp
public class Income
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = "manual";

    [Column(TypeName = "decimal(18,2)")]
    public decimal? GrossValue { get; set; }
}
```

---

## 📤 Respostas Padronizadas

### Sucesso (200 OK)
```json
{
  "id": 123,
  "name": "Salário",
  "type": "manual",
  "grossValue": 5000.00,
  "netValue": 4000.00,
  "hourlyRate": null,
  "hours": null,
  "minutes": null,
  "monthDataId": 1
}
```

### Criado (201 Created)
```json
{
  "id": 124,
  "name": "Freelance",
  "type": "hourly",
  "grossValue": null,
  "netValue": null,
  "hourlyRate": 50.00,
  "hours": 40,
  "minutes": 30,
  "monthDataId": 1
}
```

### Erro (400 Bad Request)
```json
{
  "message": "Dados inválidos",
  "errors": {
    "name": ["Nome é obrigatório"],
    "netValue": ["Valor líquido deve ser maior que zero"]
  }
}
```

### Não Encontrado (404 Not Found)
```json
{
  "message": "Receita não encontrada"
}
```

---

## 🔄 Mapeamento (Entity ↔ DTO)

### Manual (Atual)
```csharp
private IncomeDto MapToDto(Income income)
{
    return new IncomeDto(
        income.Id,
        income.Name,
        income.Type,
        income.GrossValue,
        income.NetValue,
        income.HourlyRate,
        income.Hours,
        income.Minutes,
        income.MonthDataId
    );
}

private Income MapToEntity(CreateIncomeDto dto, int monthDataId)
{
    return new Income
    {
        Name = dto.Name,
        Type = dto.Type,
        GrossValue = dto.GrossValue,
        NetValue = dto.NetValue,
        HourlyRate = dto.HourlyRate,
        Hours = dto.Hours,
        Minutes = dto.Minutes,
        MonthDataId = monthDataId
    };
}
```

### AutoMapper (Futuro)
```csharp
public class IncomeProfile : Profile
{
    public IncomeProfile()
    {
        CreateMap<Income, IncomeDto>();
        CreateMap<CreateIncomeDto, Income>();
        CreateMap<UpdateIncomeDto, Income>();
    }
}

// Uso
var incomeDto = _mapper.Map<IncomeDto>(income);
var income = _mapper.Map<Income>(createDto);
```

---

## 🎯 Checklist de API

### Endpoint
- [ ] Rota RESTful (substantivos, não verbos)
- [ ] Verbos HTTP corretos
- [ ] Status codes apropriados
- [ ] Parâmetros de rota tipados

### DTOs
- [ ] Create DTO para POST
- [ ] Update DTO para PUT
- [ ] Response DTO para GET
- [ ] Validações apropriadas

### Controller
- [ ] Fino (apenas orquestração)
- [ ] Async/await
- [ ] Try/catch quando necessário
- [ ] Retornos tipados (`ActionResult<T>`)

### Service
- [ ] Lógica de negócio
- [ ] Validações
- [ ] Mapeamento Entity ↔ DTO
- [ ] Exception handling

### Repository
- [ ] Acesso a dados
- [ ] Queries otimizadas
- [ ] Async/await

---

## 📚 Exemplos Completos

### GET List
```csharp
// Controller
[HttpGet]
public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByMonth(int year, int month)
{
    var incomes = await _incomeService.GetByMonthAsync(year, month);
    return Ok(incomes);
}

// Service
public async Task<List<IncomeDto>> GetByMonthAsync(int year, int month)
{
    var monthData = await _monthDataRepository.GetByYearAndMonthAsync(year, month);
    
    if (monthData == null) return new List<IncomeDto>();
    
    var incomes = await _incomeRepository.GetByMonthDataIdAsync(monthData.Id);
    
    return incomes.Select(MapToDto).ToList();
}

// Repository
public async Task<List<Income>> GetByMonthDataIdAsync(int monthDataId)
{
    return await _context.Incomes
        .Where(i => i.MonthDataId == monthDataId)
        .ToListAsync();
}
```

### POST Create
```csharp
// Controller
[HttpPost]
public async Task<ActionResult<IncomeDto>> Create(int year, int month, [FromBody] CreateIncomeDto dto)
{
    var created = await _incomeService.CreateAsync(year, month, dto);
    return CreatedAtAction(nameof(GetById), new { year, month, id = created.Id }, created);
}

// Service
public async Task<IncomeDto> CreateAsync(int year, int month, CreateIncomeDto dto)
{
    var monthData = await _monthDataRepository.GetOrCreateAsync(year, month);
    
    var income = MapToEntity(dto, monthData.Id);
    
    var created = await _incomeRepository.CreateAsync(income);
    
    return MapToDto(created);
}

// Repository
public async Task<Income> CreateAsync(Income income)
{
    _context.Incomes.Add(income);
    await _context.SaveChangesAsync();
    return income;
}
```

---

**Versão:** 1.0 (Month Balance Backend)  
**Data:** 22/01/2026
