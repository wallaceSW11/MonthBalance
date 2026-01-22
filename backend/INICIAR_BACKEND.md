# 🚀 Iniciar Backend do Zero - Month Balance v2

## 📋 Visão Geral

Refazer o backend com arquitetura correta: **cadastros globais** de receitas/despesas e **vínculos mensais** com valores.

---

## 🎯 Arquitetura Correta

### Conceito Principal
- **Receitas e Despesas são GLOBAIS** (cadastradas uma vez)
- **No mês você VINCULA** elas com valores específicos
- Exemplo: Cadastro "Salário" uma vez, todo mês vincula com valor diferente

### Fluxo de Uso
1. Cadastrar tipos de receitas (ex: "Salário" - Manual, "Freelance" - Horário)
2. Cadastrar tipos de despesas (ex: "Aluguel", "Conta de Luz")
3. Acessar mês (cria automaticamente se não existir)
4. Clicar em + na receita → seleciona tipo → informa valores
5. Clicar em + na despesa → seleciona tipo → informa valor
6. Pode atualizar valores individuais no mês
7. Duplicar mês → sobrescreve destino com vínculos da origem

---

## 🗄️ Modelos do Banco

### 1. User (Usuário)
```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }  // Criptografada
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### 2. Income (Receita Global)
```csharp
public enum IncomeTypeEnum
{
    Manual = 0,    // Valor fixo (GrossValue, NetValue)
    Hourly = 1     // Por hora (HourlyRate, Hours, Minutes)
}

public class Income
{
    public int Id { get; set; }
    public string Description { get; set; }  // Ex: "Salário", "Freelance"
    public IncomeTypeEnum Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public ICollection<MonthIncome> MonthIncomes { get; set; }
}
```

### 3. Expense (Despesa Global)
```csharp
public class Expense
{
    public int Id { get; set; }
    public string Description { get; set; }  // Ex: "Aluguel", "Conta de Luz"
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public ICollection<MonthExpense> MonthExpenses { get; set; }
}
```

### 4. MonthData (Mês)
```csharp
public class MonthData
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public ICollection<MonthIncome> MonthIncomes { get; set; }
    public ICollection<MonthExpense> MonthExpenses { get; set; }
}
```

### 5. MonthIncome (Vínculo Receita ↔ Mês)
```csharp
public class MonthIncome
{
    public int Id { get; set; }
    public int MonthDataId { get; set; }
    public int IncomeId { get; set; }
    
    // Valores específicos do mês (dependem do tipo)
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? HourlyRate { get; set; }
    public int? Hours { get; set; }
    public int? Minutes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public MonthData MonthData { get; set; }
    public Income Income { get; set; }
}
```

### 6. MonthExpense (Vínculo Despesa ↔ Mês)
```csharp
public class MonthExpense
{
    public int Id { get; set; }
    public int MonthDataId { get; set; }
    public int ExpenseId { get; set; }
    public decimal Value { get; set; }  // Valor específico do mês
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Relacionamentos
    public MonthData MonthData { get; set; }
    public Expense Expense { get; set; }
}
```

---

## 🔗 Relacionamentos

```
User (1) ──────────────────────────────────────────────────┐
                                                            │
Income (N) ────┐                                           │
               │                                            │
               ├──> MonthIncome (N) ──┐                    │
               │                       │                    │
Expense (N) ───┤                       ├──> MonthData (N) ─┘
               │                       │
               └──> MonthExpense (N) ──┘
```

---

## � Autenticação e Segurança

### Estratégia
- **JWT (JSON Web Token)** para autenticação
- **BCrypt** para hash de senha
- **Token em memória no frontend** (não persiste)
- **Sempre pede senha** ao abrir app/site

### Como Funciona
1. Usuário abre app → Tela de login
2. Digita email + senha
3. Backend valida e retorna JWT
4. Frontend guarda token **em memória** (variável JS)
5. Todas as requisições enviam token no header
6. Fecha app/aba → perde token → pede senha novamente

### Configuração JWT

**appsettings.json:**
```json
{
  "Jwt": {
    "Key": "sua-chave-super-secreta-com-no-minimo-32-caracteres",
    "Issuer": "MonthBalanceAPI",
    "Audience": "MonthBalanceApp",
    "ExpiresInHours": 24
  }
}
```

**Program.cs:**
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Adicionar autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// Depois de app.UseRouting()
app.UseAuthentication();
app.UseAuthorization();
```

### User Model (Atualizado)

```csharp
public class User
{
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;  // BCrypt hash
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Relacionamentos
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<MonthData> MonthData { get; set; } = new List<MonthData>();
}
```

### Auth DTOs

```csharp
public record LoginDto(
    string Email,
    string Password
);

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword
);

public record AuthResponseDto(
    string Token,
    string Email,
    DateTime ExpiresAt
);
```

### Auth Service

```csharp
public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<bool> EmailExistsAsync(string email);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        
        if (user == null)
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        
        var token = GenerateJwtToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(
            int.Parse(_configuration["Jwt:ExpiresInHours"]!));
        
        return new AuthResponseDto(token, user.Email, expiresAt);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            throw new InvalidOperationException("Senhas não conferem");
        
        if (await EmailExistsAsync(dto.Email))
            throw new InvalidOperationException("Email já cadastrado");
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash
        };
        
        var created = await _userRepository.CreateAsync(user);
        
        var token = GenerateJwtToken(created);
        var expiresAt = DateTime.UtcNow.AddHours(
            int.Parse(_configuration["Jwt:ExpiresInHours"]!));
        
        return new AuthResponseDto(token, created.Email, expiresAt);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userRepository.ExistsAsync(email);
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                int.Parse(_configuration["Jwt:ExpiresInHours"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### Auth Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        try
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var response = await _authService.RegisterAsync(dto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("check-email/{email}")]
    public async Task<ActionResult<bool>> CheckEmail(string email)
    {
        var exists = await _authService.EmailExistsAsync(email);
        return Ok(new { exists });
    }
}
```

### Proteger Endpoints

```csharp
// Adicionar [Authorize] nos controllers que precisam autenticação
[ApiController]
[Route("api/[controller]")]
[Authorize]  // ← Requer token JWT
public class IncomesController : ControllerBase
{
    // Todos os endpoints precisam de autenticação
}

// Ou em endpoints específicos
[HttpGet]
[Authorize]
public async Task<ActionResult<List<IncomeDto>>> GetAll()
{
    // Precisa de token
}

[HttpGet("public")]
[AllowAnonymous]  // Permite sem token
public async Task<ActionResult> GetPublicData()
{
    // Não precisa de token
}
```

### Pegar Usuário Logado

```csharp
// Em qualquer controller
private int GetCurrentUserId()
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return int.Parse(userIdClaim!);
}

// Exemplo de uso
[HttpGet]
[Authorize]
public async Task<ActionResult<List<IncomeDto>>> GetMyIncomes()
{
    var userId = GetCurrentUserId();
    var incomes = await _incomeService.GetByUserIdAsync(userId);
    return Ok(incomes);
}
```

### Pacotes Necessários

```bash
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
```

### Frontend (Como Usar)

```javascript
// ❌ NÃO FAZER (persiste):
localStorage.setItem('token', token)

// ✅ FAZER (não persiste):
// Opção 1: Variável em memória
let authToken = null

async function login(email, password) {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  })
  
  const data = await response.json()
  authToken = data.token  // Guarda em memória
}

// Usar em requisições
async function getIncomes() {
  const response = await fetch('/api/incomes', {
    headers: {
      'Authorization': `Bearer ${authToken}`
    }
  })
  return response.json()
}

// Opção 2: sessionStorage (limpa ao fechar aba)
sessionStorage.setItem('token', token)
const token = sessionStorage.getItem('token')
```

---

## 📡 Endpoints REST

### Auth (Autenticação)
```
POST   /api/auth/login              # Login (retorna JWT)
POST   /api/auth/register           # Cadastro
GET    /api/auth/check-email/{email} # Verifica se email existe
```

### Incomes (Receitas Globais)
```
GET    /api/incomes              # Listar todas
GET    /api/incomes/{id}         # Buscar por ID
POST   /api/incomes              # Criar
PUT    /api/incomes/{id}         # Atualizar
DELETE /api/incomes/{id}         # Deletar
```

### Expenses (Despesas Globais)
```
GET    /api/expenses             # Listar todas
GET    /api/expenses/{id}        # Buscar por ID
POST   /api/expenses             # Criar
PUT    /api/expenses/{id}        # Atualizar
DELETE /api/expenses/{id}        # Deletar
```

### MonthData (Meses)
```
GET    /api/months/{year}/{month}           # Buscar mês (cria se não existir)
GET    /api/months                          # Listar todos
POST   /api/months/duplicate                # Duplicar mês
DELETE /api/months/{year}/{month}           # Limpar mês
```

### MonthIncomes (Vínculos Receita ↔ Mês)
```
GET    /api/months/{year}/{month}/incomes              # Listar receitas do mês
POST   /api/months/{year}/{month}/incomes              # Adicionar receita ao mês
PUT    /api/months/{year}/{month}/incomes/{id}         # Atualizar valores
DELETE /api/months/{year}/{month}/incomes/{id}         # Remover do mês
```

### MonthExpenses (Vínculos Despesa ↔ Mês)
```
GET    /api/months/{year}/{month}/expenses             # Listar despesas do mês
POST   /api/months/{year}/{month}/expenses             # Adicionar despesa ao mês
PUT    /api/months/{year}/{month}/expenses/{id}        # Atualizar valor
DELETE /api/months/{year}/{month}/expenses/{id}        # Remover do mês
```

---

## 🎨 DTOs

### IncomeDto
```csharp
public record IncomeDto(
    int Id,
    string Description,
    IncomeTypeEnum Type
);

public record CreateIncomeDto(
    string Description,
    IncomeTypeEnum Type
);

public record UpdateIncomeDto(
    string Description,
    IncomeTypeEnum Type
);
```

### ExpenseDto
```csharp
public record ExpenseDto(
    int Id,
    string Description
);

public record CreateExpenseDto(
    string Description
);

public record UpdateExpenseDto(
    string Description
);
```

### MonthIncomeDto
```csharp
public record MonthIncomeDto(
    int Id,
    int IncomeId,
    string IncomeDescription,
    IncomeTypeEnum IncomeType,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);

public record CreateMonthIncomeDto(
    int IncomeId,
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);

public record UpdateMonthIncomeDto(
    decimal? GrossValue,
    decimal? NetValue,
    decimal? HourlyRate,
    int? Hours,
    int? Minutes
);
```

### MonthExpenseDto
```csharp
public record MonthExpenseDto(
    int Id,
    int ExpenseId,
    string ExpenseDescription,
    decimal Value
);

public record CreateMonthExpenseDto(
    int ExpenseId,
    decimal Value
);

public record UpdateMonthExpenseDto(
    decimal Value
);
```

### MonthDataDto
```csharp
public record MonthDataDto(
    int Id,
    int Year,
    int Month,
    List<MonthIncomeDto> Incomes,
    List<MonthExpenseDto> Expenses,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);
```

---

## 🔄 Lógica de Negócio

### Acessar Mês
```csharp
// Se mês não existe, cria automaticamente vazio
public async Task<MonthDataDto> GetOrCreateMonthAsync(int year, int month)
{
    var monthData = await _repository.GetByYearAndMonthAsync(year, month);
    
    if (monthData == null)
    {
        monthData = await _repository.CreateAsync(new MonthData
        {
            Year = year,
            Month = month
        });
    }
    
    return MapToDto(monthData);
}
```

### Adicionar Receita ao Mês
```csharp
// Valida se Income existe
// Valida se já não está vinculado ao mês
// Cria vínculo com valores
public async Task<MonthIncomeDto> AddIncomeToMonthAsync(
    int year, 
    int month, 
    CreateMonthIncomeDto dto)
{
    var monthData = await GetOrCreateMonthAsync(year, month);
    var income = await _incomeRepository.GetByIdAsync(dto.IncomeId);
    
    if (income == null)
        throw new InvalidOperationException("Income not found");
    
    var exists = await _monthIncomeRepository
        .ExistsAsync(monthData.Id, dto.IncomeId);
    
    if (exists)
        throw new InvalidOperationException("Income already added to this month");
    
    var monthIncome = new MonthIncome
    {
        MonthDataId = monthData.Id,
        IncomeId = dto.IncomeId,
        GrossValue = dto.GrossValue,
        NetValue = dto.NetValue,
        HourlyRate = dto.HourlyRate,
        Hours = dto.Hours,
        Minutes = dto.Minutes
    };
    
    var created = await _monthIncomeRepository.CreateAsync(monthIncome);
    return MapToDto(created);
}
```

### Atualizar Valores no Mês
```csharp
// Atualiza apenas os valores, não o vínculo
public async Task<MonthIncomeDto> UpdateMonthIncomeAsync(
    int id, 
    UpdateMonthIncomeDto dto)
{
    var monthIncome = await _monthIncomeRepository.GetByIdAsync(id);
    
    if (monthIncome == null)
        throw new InvalidOperationException("MonthIncome not found");
    
    monthIncome.GrossValue = dto.GrossValue;
    monthIncome.NetValue = dto.NetValue;
    monthIncome.HourlyRate = dto.HourlyRate;
    monthIncome.Hours = dto.Hours;
    monthIncome.Minutes = dto.Minutes;
    
    var updated = await _monthIncomeRepository.UpdateAsync(monthIncome);
    return MapToDto(updated);
}
```

### Duplicar Mês
```csharp
// Apaga destino se existir
// Cria novo mês com vínculos da origem
public async Task<MonthDataDto> DuplicateMonthAsync(
    int sourceYear, 
    int sourceMonth, 
    int targetYear, 
    int targetMonth)
{
    var source = await _repository.GetByYearAndMonthAsync(sourceYear, sourceMonth);
    
    if (source == null)
        throw new InvalidOperationException("Source month not found");
    
    // Apaga destino se existir
    var existing = await _repository.GetByYearAndMonthAsync(targetYear, targetMonth);
    if (existing != null)
    {
        await _repository.DeleteAsync(existing.Id);
    }
    
    // Cria novo mês
    var target = await _repository.CreateAsync(new MonthData
    {
        Year = targetYear,
        Month = targetMonth
    });
    
    // Copia vínculos de receitas
    foreach (var monthIncome in source.MonthIncomes)
    {
        await _monthIncomeRepository.CreateAsync(new MonthIncome
        {
            MonthDataId = target.Id,
            IncomeId = monthIncome.IncomeId,
            GrossValue = monthIncome.GrossValue,
            NetValue = monthIncome.NetValue,
            HourlyRate = monthIncome.HourlyRate,
            Hours = monthIncome.Hours,
            Minutes = monthIncome.Minutes
        });
    }
    
    // Copia vínculos de despesas
    foreach (var monthExpense in source.MonthExpenses)
    {
        await _monthExpenseRepository.CreateAsync(new MonthExpense
        {
            MonthDataId = target.Id,
            ExpenseId = monthExpense.ExpenseId,
            Value = monthExpense.Value
        });
    }
    
    return MapToDto(target);
}
```

---

## 🛠️ Stack Técnica

### Backend
- **.NET**: 10.0 (mais novo disponível)
- **C#**: 12.0
- **ASP.NET Core**: 10.0
- **Entity Framework Core**: 10.0
- **PostgreSQL**: 17+
- **BCrypt.Net-Next**: Para hash de senha
- **JWT**: Para autenticação

### Configurações
- **Migrations**: Automáticos no startup (Development)
- **CORS**: Configurado para frontend
- **JSON**: camelCase
- **Static Files**: Exportar frontend (produção)

### Database
- **PostgreSQL local**: Porta 5433
- **Connection String**: `Host=localhost;Port=5433;Database=monthbalance_v2;Username=postgres;Password=senhas`
- **Criar banco**: `monthbalance_v2`

### Docker (Futuro)
- **docker-compose.yml**: Backend + PostgreSQL + Frontend (static files)
- **Volumes**: Persistência do banco
- **Networks**: Comunicação entre containers

---

## 📁 Estrutura do Projeto

**IMPORTANTE:** Todos os arquivos ficam direto na pasta `backend/`, sem subpastas do projeto.

```
backend/
├── Controllers/
│   ├── AuthController.cs
│   ├── IncomesController.cs
│   ├── ExpensesController.cs
│   ├── MonthDataController.cs
│   ├── MonthIncomesController.cs
│   └── MonthExpensesController.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── DTOs/
│   ├── AuthDto.cs
│   ├── IncomeDto.cs
│   ├── ExpenseDto.cs
│   ├── MonthIncomeDto.cs
│   ├── MonthExpenseDto.cs
│   └── MonthDataDto.cs
├── Models/
│   ├── User.cs
│   ├── Income.cs
│   ├── Expense.cs
│   ├── MonthData.cs
│   ├── MonthIncome.cs
│   ├── MonthExpense.cs
│   └── IncomeTypeEnum.cs
├── Repositories/
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── IIncomeRepository.cs
│   ├── IncomeRepository.cs
│   ├── IExpenseRepository.cs
│   ├── ExpenseRepository.cs
│   ├── IMonthDataRepository.cs
│   ├── MonthDataRepository.cs
│   ├── IMonthIncomeRepository.cs
│   ├── MonthIncomeRepository.cs
│   ├── IMonthExpenseRepository.cs
│   └── MonthExpenseRepository.cs
├── Services/
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   ├── IIncomeService.cs
│   ├── IncomeService.cs
│   ├── IExpenseService.cs
│   ├── ExpenseService.cs
│   ├── IMonthDataService.cs
│   ├── MonthDataService.cs
│   ├── IMonthIncomeService.cs
│   ├── MonthIncomeService.cs
│   ├── IMonthExpenseService.cs
│   └── MonthExpenseService.cs
├── Migrations/
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── backend.csproj  ← Nome do projeto
└── INICIAR_BACKEND.md
```

---

## 🎯 Ordem de Implementação

### Fase 1: Setup Inicial
1. Criar projeto .NET 10
2. Adicionar pacotes (EF Core, PostgreSQL, BCrypt, JWT)
3. Configurar EF Core + PostgreSQL
4. Configurar CORS e JSON
5. Configurar JWT
6. Criar DbContext

### Fase 2: Models e Migrations
1. Criar todos os models (User, Income, Expense, etc)
2. Configurar relacionamentos no DbContext
3. Criar migration inicial
4. Aplicar migration

### Fase 3: Autenticação
1. User Repository
2. Auth Service (Login, Register)
3. Auth Controller
4. Testar login/register

### Fase 4: Cadastros Globais
1. Income (CRUD completo) - com [Authorize]
2. Expense (CRUD completo) - com [Authorize]

### Fase 5: Meses
1. MonthData (Get or Create, List, Delete)
2. MonthIncome (Add, Update, Remove, List)
3. MonthExpense (Add, Update, Remove, List)

### Fase 6: Funcionalidades Avançadas
1. Duplicate Month
2. Cálculos (TotalIncome, TotalExpense, Balance)
3. Validações

### Fase 7: Testes
1. Testar autenticação
2. Testar todos os endpoints
3. Testar fluxo completo
4. Testar duplicate

---

## ✅ Checklist de Validações

### Income
- [ ] Description obrigatório (max 200 chars)
- [ ] Type obrigatório (Manual ou Hourly)
- [ ] Não permitir deletar se houver vínculos

### Expense
- [ ] Description obrigatório (max 200 chars)
- [ ] Não permitir deletar se houver vínculos

### MonthIncome
- [ ] IncomeId obrigatório e deve existir
- [ ] Não permitir duplicar Income no mesmo mês
- [ ] Se tipo Manual: NetValue obrigatório
- [ ] Se tipo Hourly: HourlyRate e Hours obrigatórios

### MonthExpense
- [ ] ExpenseId obrigatório e deve existir
- [ ] Value obrigatório e > 0
- [ ] Não permitir duplicar Expense no mesmo mês

### MonthData
- [ ] Year e Month obrigatórios
- [ ] Não permitir duplicar Year/Month
- [ ] Duplicate: Source deve existir

---

## 🚀 Comandos Úteis

### Criar Projeto
```bash
# Na pasta backend/ (não criar subpasta)
dotnet new webapi -f net10.0 -o .
```

**IMPORTANTE:** O comando acima cria os arquivos direto na pasta `backend/`, sem criar subpasta `MonthBalance.API/`

### Adicionar Pacotes
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
```

### Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef database drop --force
```

### Rodar
```bash
dotnet run
dotnet watch run  # Hot reload
```

---

## 📝 Notas Importantes

1. **Migrations automáticos**: Aplicar no startup apenas em Development
2. **Sem seed data**: Banco limpo, usuário cadastra tudo
3. **Validações**: FluentValidation (futuro) ou Data Annotations
4. **Autenticação**: JWT com BCrypt para senha
5. **Token em memória**: Frontend não persiste (sempre pede senha)
6. **OAuth**: Google/Apple para futuro (v2)
7. **Recuperar senha**: Email (futuro)
8. **Testes**: Criar depois que tudo estiver funcionando
9. **Docker**: Configurar depois que backend estiver pronto

---

**Versão:** 2.0  
**Data:** 22/01/2026  
**Status:** Planejamento completo com autenticação - Pronto para implementar ✅
