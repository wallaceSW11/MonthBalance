# ⚡ Quick Start - MonthBalance Backend

Guia rápido para começar a desenvolver em **2 minutos**!

---

## 🚀 Setup Inicial (Primeira Vez)

### 1. Pré-requisitos

```powershell
# Verificar .NET
dotnet --version
# Deve mostrar: 10.x.x

# Verificar Docker
docker --version
docker-compose --version
```

### 2. Subir PostgreSQL

```powershell
cd backend
docker-compose -f docker-compose.dev.yml up -d
```

### 3. Rodar API

```powershell
dotnet run
```

✨ **Pronto!** Migrations aplicadas automaticamente!

API rodando em: `http://localhost:5150`

---

## 🧪 Testar

```powershell
# Health check
curl http://localhost:5150/api/health

# Listar meses
curl http://localhost:5150/api/monthdata

# Buscar mês específico
curl http://localhost:5150/api/monthdata/2026/1
```

---

## 📝 Desenvolvimento Diário

### Iniciar Desenvolvimento

```powershell
# 1. Subir banco (se não estiver rodando)
cd backend
docker-compose -f docker-compose.dev.yml up -d

# 2. Rodar API
dotnet run
```

### Parar Desenvolvimento

```powershell
# Ctrl+C para parar API

# Parar banco (opcional)
docker-compose -f docker-compose.dev.yml down
```

---

## 🗄️ Migrations

### Criar Nova Migration

```powershell
cd backend
dotnet ef migrations add NomeDaMigration
```

### Aplicar Migrations

✨ **Automático!** Só rodar `dotnet run`

Ou manualmente:
```powershell
dotnet ef database update
```

### Listar Migrations

```powershell
dotnet ef migrations list
```

---

## 🆕 Criar Nova Feature

### 1. Criar Model

```csharp
// Models/Category.cs
namespace MonthBalance.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

### 2. Adicionar no DbContext

```csharp
// Data/ApplicationDbContext.cs
public DbSet<Category> Categories { get; set; }
```

### 3. Criar Migration

```powershell
dotnet ef migrations add AddCategoryTable
```

### 4. Criar Repository

```csharp
// Repositories/ICategoryRepository.cs
namespace MonthBalance.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}

// Repositories/CategoryRepository.cs
namespace MonthBalance.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
    
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
}
```

### 5. Criar Service

```csharp
// Services/ICategoryService.cs
namespace MonthBalance.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
}

// Services/CategoryService.cs
namespace MonthBalance.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    
    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}
```

### 6. Criar Controller

```csharp
// Controllers/CategoriesController.cs
using Microsoft.AspNetCore.Mvc;
using MonthBalance.Services;

namespace MonthBalance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;
    
    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Ok(categories);
    }
}
```

### 7. Registrar no DI

```csharp
// Program.cs
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
```

### 8. Rodar e Testar

```powershell
dotnet run

# Testar
curl http://localhost:5150/api/categories
```

---

## 🛠️ Comandos Úteis

### Build

```powershell
dotnet build
```

### Limpar Build

```powershell
dotnet clean
```

### Restaurar Pacotes

```powershell
dotnet restore
```

### Ver Logs do PostgreSQL

```powershell
docker-compose -f docker-compose.dev.yml logs -f postgres
```

### Resetar Banco (CUIDADO!)

```powershell
docker-compose -f docker-compose.dev.yml down -v
docker-compose -f docker-compose.dev.yml up -d
dotnet run
```

---

## 📚 Documentação

| Arquivo | Descrição |
|---------|-----------|
| [README.md](README.md) | Guia completo |
| [DATABASE_MIGRATIONS.md](DATABASE_MIGRATIONS.md) | Guia de migrations |
| [API_ENDPOINTS.md](API_ENDPOINTS.md) | Documentação dos endpoints |
| [ESTRUTURA_ATUALIZADA.md](ESTRUTURA_ATUALIZADA.md) | Mudanças na estrutura |
| [ANTES_DEPOIS.md](ANTES_DEPOIS.md) | Comparação visual |

---

## 🐛 Troubleshooting

### Erro: "Database connection failed"

```powershell
# Verificar se PostgreSQL está rodando
docker ps

# Se não estiver, subir
docker-compose -f docker-compose.dev.yml up -d
```

### Erro: "Port 5150 already in use"

```powershell
# Matar processo na porta 5150
netstat -ano | findstr :5150
taskkill /PID <PID> /F
```

### Erro: "Migration already exists"

```powershell
# Remover última migration
dotnet ef migrations remove
```

---

## 🎯 Fluxo Completo

```
1. Subir banco → docker-compose up -d
2. Criar model → Models/NovoModel.cs
3. Atualizar DbContext → Data/ApplicationDbContext.cs
4. Criar migration → dotnet ef migrations add NomeDaMigration
5. Criar repository → Repositories/NovoRepository.cs
6. Criar service → Services/NovoService.cs
7. Criar controller → Controllers/NovoController.cs
8. Registrar DI → Program.cs
9. Rodar → dotnet run (migrations automáticas!)
10. Testar → curl http://localhost:5150/api/novo
```

---

## ⚡ TL;DR

```powershell
# Setup (primeira vez)
cd backend
docker-compose -f docker-compose.dev.yml up -d
dotnet run

# Desenvolvimento diário
cd backend
dotnet run

# Criar feature
# 1. Model → 2. DbContext → 3. Migration → 4. Repository → 5. Service → 6. Controller → 7. DI → 8. Run
```

---

**Pronto para codar!** 🚀

---

**Versão:** 1.0  
**Última atualização:** 22/01/2026
