# 🐳 Docker Setup - Month Balance

Configuração completa do projeto com Docker Compose para rodar frontend, backend e PostgreSQL juntos.

## 📋 Pré-requisitos

- Docker Desktop instalado (com WSL2 no Windows 11)
- Docker Compose v3.8+

## 🚀 Como usar

### 1. Configurar variáveis de ambiente

Copie o arquivo de exemplo e configure suas credenciais:

```bash
cp .env.example .env
```

Edite o arquivo `.env` e defina:
- `DB_PASSWORD`: Senha do PostgreSQL
- `JWT_SECRET`: Chave secreta para JWT (mínimo 32 caracteres)
- `FRONTEND_PORT`: Porta do frontend (opcional, padrão: 8080)

### 2. Subir todos os serviços

```bash
docker-compose up -d
```

Ou para ver os logs em tempo real:

```bash
docker-compose up
```

### 3. Acessar os serviços

- **Frontend**: http://localhost:8080
- **Backend API**: http://localhost:5150
- **PostgreSQL**: localhost:5433

### 4. Verificar status dos containers

```bash
docker-compose ps
```

### 5. Ver logs

```bash
# Todos os serviços
docker-compose logs -f

# Apenas um serviço específico
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f postgres
```

### 6. Parar os serviços

```bash
# Parar sem remover containers
docker-compose stop

# Parar e remover containers
docker-compose down

# Parar, remover containers e volumes (CUIDADO: apaga o banco!)
docker-compose down -v
```

## 🔧 Comandos úteis

### Rebuild dos containers

Se você fez alterações no código:

```bash
# Rebuild e restart
docker-compose up -d --build

# Rebuild apenas um serviço
docker-compose up -d --build backend
```

### Executar migrations no backend

```bash
docker-compose exec backend dotnet ef database update
```

### Acessar o banco de dados

```bash
docker-compose exec postgres psql -U postgres -d monthbalance
```

### Limpar tudo e recomeçar

```bash
docker-compose down -v
docker-compose up -d --build
```

## 📦 Estrutura dos serviços

### PostgreSQL
- **Porta externa**: 5433
- **Porta interna**: 5432
- **Volume**: `postgres_data` (persistente)

### Backend (.NET)
- **Porta**: 5150
- **Depende de**: PostgreSQL
- **Healthcheck**: Aguarda PostgreSQL estar pronto

### Frontend (Vue.js + Nginx)
- **Porta**: 8080 (configurável via .env)
- **Depende de**: Backend
- **API URL**: http://localhost:5150/api

## 🔍 Troubleshooting

### Porta já em uso

Se alguma porta estiver em uso, você pode alterar no `.env` ou diretamente no `docker-compose.yml`.

### Erro de conexão com o banco

Verifique se o PostgreSQL está rodando:
```bash
docker-compose logs postgres
```

### Frontend não conecta no backend

Verifique se a variável `VITE_API_BASE_URL` está correta no build do frontend.

### Rebuild completo

```bash
docker-compose down -v
docker system prune -a
docker-compose up -d --build
```

## 🌐 Deploy para Oracle Cloud

Este setup está preparado para migrar facilmente para Oracle Cloud Free Tier:
- Todas as configurações via `.env`
- PostgreSQL pode ser substituído por Oracle Database
- Containers prontos para deploy em Oracle Container Instances

## 📝 Notas

- O PostgreSQL roda na porta **5433** externamente para não conflitar com outras instâncias
- Os dados do banco são persistidos no volume `postgres_data`
- O frontend é servido via Nginx em produção
- Todos os serviços estão na mesma rede Docker para comunicação interna
