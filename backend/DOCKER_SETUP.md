# 🐳 Docker Desktop Setup

## 📋 Pré-requisitos

- Windows 10/11 (64-bit)
- Virtualização habilitada na BIOS
- Docker Desktop instalado

## 🚀 Setup Docker Desktop

### 1. Verificar Instalação

Abra um **novo terminal PowerShell** (importante!) e execute:

```powershell
docker --version
docker-compose --version
```

Se não funcionar, siga os passos abaixo.

### 2. Adicionar Docker ao PATH (se necessário)

O Docker Desktop geralmente adiciona automaticamente ao PATH, mas se não funcionar:

1. Feche **todos** os terminais abertos
2. Abra o Docker Desktop
3. Aguarde inicializar completamente (ícone na bandeja do sistema)
4. Abra um **novo** PowerShell
5. Teste novamente: `docker --version`

### 3. Habilitar WSL 2 (Recomendado)

O Docker Desktop funciona melhor com WSL 2:

**Verificar se WSL 2 está instalado:**

```powershell
wsl --list --verbose
```

**Se não estiver instalado:**

```powershell
# Executar como Administrador
wsl --install
```

**Configurar Docker para usar WSL 2:**

1. Abra Docker Desktop
2. Settings → General
3. Marque "Use the WSL 2 based engine"
4. Apply & Restart

### 4. Testar Docker

```powershell
docker run hello-world
```

Se funcionar, você verá uma mensagem de sucesso!

## 🐘 Iniciar PostgreSQL

Agora que o Docker está funcionando:

```powershell
cd backend
docker-compose -f docker-compose.dev.yml up -d
```

Verificar se está rodando:

```powershell
docker ps
```

Você deve ver o container `mb-postgres-dev` rodando.

## ⚠️ Troubleshooting

### Erro: "docker: command not found"

1. Feche **todos** os terminais
2. Abra Docker Desktop e aguarde inicializar
3. Abra um **novo** PowerShell
4. Teste: `docker --version`

### Erro: "Docker daemon is not running"

1. Abra Docker Desktop
2. Aguarde inicializar (ícone na bandeja)
3. Tente novamente

### Erro: "WSL 2 installation is incomplete"

Execute como Administrador:

```powershell
wsl --install
wsl --set-default-version 2
```

Reinicie o computador.

### Erro: "Hardware assisted virtualization"

1. Entre na BIOS/UEFI
2. Habilite "Intel VT-x" ou "AMD-V"
3. Salve e reinicie

### Docker Desktop não abre

1. Desinstale Docker Desktop
2. Reinicie o computador
3. Reinstale Docker Desktop
4. Reinicie novamente

## 📝 Comandos Úteis

```powershell
# Ver containers rodando
docker ps

# Ver todos os containers
docker ps -a

# Parar container
docker stop mb-postgres-dev

# Remover container
docker rm mb-postgres-dev

# Ver logs
docker logs mb-postgres-dev

# Entrar no container
docker exec -it mb-postgres-dev bash
```

## 🎯 Próximos Passos

Após Docker funcionando:

1. Inicie PostgreSQL: `docker-compose -f docker-compose.dev.yml up -d`
2. Rode a API: `dotnet run --project MonthBalance.API`
3. A API aplicará as migrations automaticamente

## 📚 Referências

- [Docker Desktop para Windows](https://docs.docker.com/desktop/install/windows-install/)
- [WSL 2 Setup](https://learn.microsoft.com/pt-br/windows/wsl/install)
