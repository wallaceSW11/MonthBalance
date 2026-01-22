# 🗄️ Database Migrations - MonthBalance Backend

## 📋 Visão Geral

Este projeto usa **Entity Framework Core** com **PostgreSQL** para gerenciamento de banco de dados e migrations automáticas.

---

## 🚀 Migrations Automáticas

As migrations são aplicadas **automaticamente** quando a aplicação inicia em modo Development.

```csharp
// Program.cs - Migrations automáticas no startup
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // ← Aplica migrations pendentes
        DbInitializer.Initialize(context); // ← Seed de dados iniciais
    }
}
```

**Vantagens:**
- ✅ Sem necessidade de rodar scripts manuais
- ✅ Banco sempre atualizado ao iniciar a aplicação
- ✅ Perfeito para CI/CD
- ✅ Histórico de mudanças versionado

---

## 🛠️ Comandos EF Core

### Criar Nova Migration

```bash
dotnet ef migrations add NomeDaMigration
```

**Exemplo:**
```bash
dotnet ef migrations add AddUserTable
dotnet ef migrations add AddIndexToMonthData
```

### Aplicar Migrations Manualmente

```bash
dotnet ef database update
```

### Reverter Migration

```bash
# Reverter para migration específica
dotnet ef database update NomeDaMigrationAnterior

# Reverter todas
dotnet ef database update 0
```

### Remover Última Migration (não aplicada)

```bash
dotnet ef migrations remove
```

### Listar Migrations

```bash
dotnet ef migrations list
```

### Gerar Script SQL

```bash
# Gerar SQL de todas as migrations
dotnet ef migrations script

# Gerar SQL de uma migration específica
dotnet ef migrations script FromMigration ToMigration
```

---

## 📂 Estrutura de Migrations

```
backend/
├── Data/
│   ├── ApplicationDbContext.cs    # DbContext principal
│   └── DbInitializer.cs           # Seed de dados iniciais
├── Migrations/                     # Gerado automaticamente pelo EF
│   ├── 20260122_InitialCreate.cs
│   ├── 20260122_InitialCreate.Designer.cs
│   └── ApplicationDbContextModelSnapshot.cs
└── Models/                         # Entidades do banco
    ├── MonthData.cs
    ├── Income.cs
    └── Expense.cs
```

---

## 🔧 Configuração do Banco

### Connection String

Configurada em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=monthbalance;Username=mbuser;Password=mbpass123"
  }
}
```

### Variáveis de Ambiente (Produção)

```bash
# Linux/Mac
export ConnectionStrings__DefaultConnection="Host=prod-server;Database=monthbalance;Username=user;Password=pass"

# Windows
set ConnectionStrings__DefaultConnection=Host=prod-server;Database=monthbalance;Username=user;Password=pass
```

---

## 📝 Exemplo: Criar Nova Entidade

### 1. Criar Model

```csharp
// Models/Category.cs
using System.ComponentModel.DataAnnotations;

namespace MonthBalance.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
}
```

### 2. Adicionar no DbContext

```csharp
// Data/ApplicationDbContext.cs
public DbSet<Category> Categories { get; set; }
```

### 3. Criar Migration

```bash
dotnet ef migrations add AddCategoryTable
```

### 4. Aplicar (automático no próximo startup ou manual)

```bash
dotnet ef database update
```

---

## 🎯 Boas Práticas

### ✅ DO

- Criar migrations com nomes descritivos: `AddUserTable`, `AddIndexToExpenses`
- Revisar o código gerado antes de aplicar
- Testar migrations em ambiente de desenvolvimento primeiro
- Manter migrations pequenas e focadas
- Versionar migrations no Git

### ❌ DON'T

- Editar migrations já aplicadas em produção
- Deletar migrations que já foram aplicadas
- Fazer mudanças diretas no banco sem migration
- Criar migrations muito grandes com muitas mudanças

---

## 🐳 Docker + Migrations

As migrations rodam automaticamente quando a aplicação sobe no Docker:

```yaml
# docker-compose.dev.yml
services:
  api:
    build: .
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Database=monthbalance;Username=mbuser;Password=mbpass123
    depends_on:
      - db
  
  db:
    image: postgres:17
    environment:
      POSTGRES_DB: monthbalance
      POSTGRES_USER: mbuser
      POSTGRES_PASSWORD: mbpass123
```

---

## 🚀 CI/CD

### GitHub Actions / Azure DevOps

```yaml
- name: Apply Migrations
  run: dotnet ef database update --connection "${{ secrets.CONNECTION_STRING }}"
```

### Ou deixar automático no startup (recomendado)

A aplicação já aplica migrations automaticamente ao iniciar, então basta fazer o deploy normalmente.

---

## 🔍 Troubleshooting

### Erro: "No migrations found"

```bash
# Criar primeira migration
dotnet ef migrations add InitialCreate
```

### Erro: "Database already exists"

```bash
# Dropar e recriar
dotnet ef database drop
dotnet ef database update
```

### Erro: "Pending model changes"

```bash
# Criar migration com as mudanças pendentes
dotnet ef migrations add FixPendingChanges
```

---

## 📚 Referências

- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Npgsql EF Core Provider](https://www.npgsql.org/efcore/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

---

**Versão:** 1.0  
**Última atualização:** 22/01/2026
