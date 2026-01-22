# ✨ Estrutura Atualizada - MonthBalance Backend

## 🎉 O que mudou?

O backend foi **reorganizado** para uma estrutura mais simples e direta, com todos os arquivos na raiz da pasta `backend/`.

---

## 📂 Estrutura Anterior vs Nova

### ❌ Antes (Estrutura Aninhada)

```
backend/
└── MonthBalance.API/
    ├── Controllers/
    ├── Data/
    ├── Models/
    ├── Services/
    ├── Repositories/
    ├── Program.cs
    └── ...
```

### ✅ Agora (Estrutura Simplificada)

```
backend/
├── Controllers/          # API Controllers
├── Data/                # DbContext + Migrations
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── Models/              # Entidades do banco
├── DTOs/                # Data Transfer Objects
├── Services/            # Lógica de negócio
├── Repositories/        # Acesso a dados
├── Migrations/          # EF Core Migrations (auto-gerado)
├── Middleware/          # Middlewares customizados
├── Extensions/          # Extension methods
├── Validators/          # Validações
├── Mappings/            # AutoMapper profiles
├── Properties/          # Launch settings
├── scripts/             # Scripts auxiliares
├── Program.cs           # Entry point
├── appsettings.json     # Configurações
└── MonthBalance.API.csproj
```

---

## 🔄 Mudanças nos Namespaces

### ❌ Antes

```csharp
using MonthBalance.API.Controllers;
using MonthBalance.API.Services;
using MonthBalance.API.Models;

namespace MonthBalance.API.Controllers;
```

### ✅ Agora

```csharp
using MonthBalance.Controllers;
using MonthBalance.Services;
using MonthBalance.Models;

namespace MonthBalance.Controllers;
```

**Mais limpo e direto!** 🎯

---

## 🗄️ Migrations Automáticas

### ✨ Configuração Automática

As migrations agora rodam **automaticamente** quando a aplicação inicia em Development:

```csharp
// Program.cs
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // ← Aplica migrations automaticamente
        DbInitializer.Initialize(context); // ← Seed de dados
    }
}
```

### 🎯 Benefícios

- ✅ **Sem scripts manuais** - Basta rodar `dotnet run`
- ✅ **Banco sempre atualizado** - Migrations aplicadas no startup
- ✅ **Perfeito para CI/CD** - Deploy automático
- ✅ **Histórico versionado** - Todas as mudanças rastreadas

### 📝 Comandos Úteis

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations manualmente (opcional)
dotnet ef database update

# Listar migrations
dotnet ef migrations list

# Reverter migration
dotnet ef database update NomeDaMigrationAnterior
```

Veja mais em [DATABASE_MIGRATIONS.md](DATABASE_MIGRATIONS.md)

---

## 🚀 Como Usar

### 1. Instalar Dependências

```bash
cd backend
dotnet restore
```

### 2. Configurar Banco (se necessário)

Edite `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=monthbalance;Username=mbuser;Password=mbpass123"
  }
}
```

### 3. Rodar Aplicação

```bash
cd backend
dotnet run
```

**Pronto!** As migrations serão aplicadas automaticamente. 🎉

---

## 🔧 Comandos Atualizados

### ❌ Antes

```bash
cd backend/MonthBalance.API
dotnet run
```

### ✅ Agora

```bash
cd backend
dotnet run
```

---

## 📦 Build e Deploy

### Build

```bash
cd backend
dotnet build
```

### Publish

```bash
cd backend
dotnet publish -c Release -o ./publish
```

### Docker

```bash
cd backend
docker-compose -f docker-compose.dev.yml up -d
```

---

## 🎯 Vantagens da Nova Estrutura

### ✅ Simplicidade

- Menos níveis de aninhamento
- Navegação mais fácil
- Estrutura mais clara

### ✅ Produtividade

- Comandos mais curtos
- Menos pastas para navegar
- Imports mais limpos

### ✅ Manutenibilidade

- Código mais organizado
- Namespaces simplificados
- Migrations automáticas

### ✅ Escalabilidade

- Fácil adicionar novos módulos
- Estrutura preparada para crescimento
- Padrão consistente

---

## 📚 Documentação

- [README.md](README.md) - Guia principal
- [DATABASE_MIGRATIONS.md](DATABASE_MIGRATIONS.md) - Guia completo de migrations
- [API_ENDPOINTS.md](API_ENDPOINTS.md) - Documentação dos endpoints
- [DATABASE_SETUP.md](DATABASE_SETUP.md) - Setup do banco
- [DOCKER_SETUP.md](DOCKER_SETUP.md) - Configuração Docker

---

## 🔄 Migração de Código Existente

Se você tem código que referencia a estrutura antiga:

### 1. Atualizar Imports

```bash
# Buscar e substituir em todos os arquivos
MonthBalance.API. → MonthBalance.
```

### 2. Atualizar Caminhos

```bash
# Antes
cd backend/MonthBalance.API

# Agora
cd backend
```

### 3. Recompilar

```bash
cd backend
dotnet clean
dotnet build
```

---

## ✅ Checklist de Verificação

- [x] Estrutura reorganizada
- [x] Namespaces atualizados
- [x] Migrations recriadas
- [x] Build funcionando
- [x] Migrations automáticas configuradas
- [x] Documentação atualizada
- [x] README atualizado

---

## 🎉 Conclusão

A nova estrutura está **mais simples**, **mais limpa** e **mais fácil de manter**!

Migrations rodam automaticamente, namespaces estão simplificados e a navegação ficou muito mais intuitiva.

**Bora codar!** 🚀

---

**Data da Atualização:** 22/01/2026  
**Versão:** 2.0
