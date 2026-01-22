# 🚀 MonthBalance Backend API

Backend API REST desenvolvido com .NET 10, Entity Framework Core e PostgreSQL.

## 📋 Pré-requisitos

- .NET 10 SDK
- PostgreSQL 16+
- Docker e Docker Compose (opcional)

## 🔧 Configuração

### 1. Instalar Dependências

```bash
cd backend
dotnet restore
```

### 2. Configurar Banco de Dados

Edite `appsettings.Development.json` com suas credenciais PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=monthbalance;Username=seu_usuario;Password=sua_senha"
  }
}
```

### 3. Criar Banco de Dados

**Opção 1: Docker (Recomendado)**

```bash
cd backend
docker-compose -f docker-compose.dev.yml up -d
```

**Opção 2: PostgreSQL Local**

```bash
createdb -U postgres monthbalance
```

### 4. Migrations

✨ **As migrations são aplicadas AUTOMATICAMENTE** ao rodar a API em Development!

Não precisa rodar scripts manuais. Basta iniciar a aplicação e o banco será criado/atualizado automaticamente.

Para mais detalhes sobre migrations, veja [DATABASE_MIGRATIONS.md](DATABASE_MIGRATIONS.md)

## 🚀 Executar

### Desenvolvimento

```bash
cd backend
dotnet run
```

API disponível em: `http://localhost:5000`

### Produção (Docker)

```bash
cd backend
docker-compose up -d
```

## 📚 Endpoints

Documentação completa em [API_ENDPOINTS.md](API_ENDPOINTS.md)

### Health Check

```
GET /api/health
```

Resposta:
```json
{
  "status": "healthy",
  "timestamp": "2026-01-22T00:00:00Z",
  "version": "1.0.0"
}
```

## 🧪 Testes

```bash
cd backend
dotnet test
```

## 📦 Build

```bash
dotnet build -c Release
dotnet publish -c Release -o ./publish
```

## 🐳 Docker

### Build da Imagem

```bash
docker build -t monthbalance-api .
```

### Executar com Docker Compose

```bash
docker-compose -f docker-compose.dev.yml up -d
```

## 📝 Estrutura do Projeto

```
backend/
├── Controllers/          # Controllers da API
├── Data/                # DbContext e configurações EF
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── Models/              # Entidades do banco
├── DTOs/                # Data Transfer Objects
├── Services/            # Lógica de negócio
├── Repositories/        # Acesso a dados
├── Migrations/          # EF Core Migrations (gerado automaticamente)
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

## 🔑 Variáveis de Ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente (Development/Production) | Development |
| `ConnectionStrings__DefaultConnection` | String de conexão PostgreSQL | - |
| `ASPNETCORE_URLS` | URLs de escuta | http://+:80 |

## 📖 Documentação

- [API Endpoints](API_ENDPOINTS.md) - Documentação completa dos endpoints
- [Database Migrations](DATABASE_MIGRATIONS.md) - Guia de migrations do EF Core
- [Database Setup](DATABASE_SETUP.md) - Setup inicial do banco
- [Docker Setup](DOCKER_SETUP.md) - Configuração Docker
- [Roadmap](ROADMAP.md) - Planejamento e próximos passos
- [Changelog](CHANGELOG.md) - Histórico de mudanças

## 🛠️ Tecnologias

- **.NET 10** - Framework principal
- **Entity Framework Core 10** - ORM
- **PostgreSQL 17** - Banco de dados
- **Npgsql** - Provider PostgreSQL para EF Core
- **ASP.NET Core** - Web API

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Add nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT.
