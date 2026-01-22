# 🚀 MonthBalance Backend API

Backend API REST desenvolvido com .NET 10, Entity Framework Core e PostgreSQL.

## 📋 Pré-requisitos

- .NET 10 SDK
- PostgreSQL 16+
- Docker e Docker Compose (opcional)

## 🔧 Configuração

### 1. Instalar Dependências

```bash
cd backend/MonthBalance.API
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

```bash
# Criar banco manualmente no PostgreSQL
createdb -U postgres monthbalance

# OU via Docker
docker run --name mb-postgres -e POSTGRES_DB=monthbalance -e POSTGRES_USER=mbuser -e POSTGRES_PASSWORD=mbpass123 -p 5432:5432 -d postgres:16-alpine
```

### 4. Aplicar Migrations

```bash
dotnet ef database update
```

## 🚀 Executar

### Desenvolvimento

```bash
cd backend/MonthBalance.API
dotnet run
```

API disponível em: `http://localhost:5000`

### Produção (Docker)

```bash
cd backend
docker-compose up -d
```

## 📚 Endpoints

### Health Check

```
GET /api/health
```

Resposta:
```json
{
  "status": "healthy",
  "timestamp": "2026-01-21T23:00:00Z",
  "version": "1.0.0"
}
```

## 🧪 Testes

```bash
cd backend/MonthBalance.Tests
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
docker build -t monthbalance-api -f docker/Dockerfile .
```

### Executar com Docker Compose

```bash
docker-compose up -d
```

## 📝 Estrutura do Projeto

```
MonthBalance.API/
├── Controllers/       # Controllers da API
├── Data/             # DbContext e configurações EF
├── Models/           # Entidades do banco
├── DTOs/             # Data Transfer Objects
├── Services/         # Lógica de negócio
├── Repositories/     # Acesso a dados
├── Middleware/       # Middlewares customizados
├── Extensions/       # Extension methods
├── Validators/       # Validações
├── Mappings/         # AutoMapper profiles
└── wwwroot/          # Static files (frontend)
```

## 🔑 Variáveis de Ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente (Development/Production) | Development |
| `ConnectionStrings__DefaultConnection` | String de conexão PostgreSQL | - |
| `ASPNETCORE_URLS` | URLs de escuta | http://+:80 |

## 📖 Documentação API

Swagger disponível em: `http://localhost:5000/swagger`

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Add nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT.
