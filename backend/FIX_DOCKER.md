# 🐳 Fix Docker - Solução Rápida

## 🔴 Problema

```
unable to get image 'postgres:17': error during connect: 
open //./pipe/dockerDesktopLinuxEngine: O sistema não pode encontrar o arquivo especificado.
```

**Causa:** Docker Desktop está instalado, mas o Docker Engine (daemon) não está rodando.

---

## ✅ Solução

### Opção 1: Iniciar Docker Desktop (Recomendado)

1. **Abrir Docker Desktop**
   - Procurar "Docker Desktop" no menu Iniciar
   - Clicar para abrir
   - **Aguardar 30-60 segundos** até o ícone ficar verde na bandeja

2. **Verificar Status**
   ```powershell
   docker info
   ```
   
   Se mostrar informações do servidor (Server:), está funcionando! ✅

3. **Subir PostgreSQL**
   ```powershell
   cd backend
   docker-compose -f docker-compose.dev.yml up -d
   ```

---

### Opção 2: Reiniciar Docker Desktop

Se a Opção 1 não funcionar:

1. **Fechar Docker Desktop**
   - Clicar com botão direito no ícone do Docker na bandeja
   - Selecionar "Quit Docker Desktop"

2. **Aguardar 10 segundos**

3. **Abrir Docker Desktop novamente**
   - Procurar "Docker Desktop" no menu Iniciar
   - Aguardar inicializar (ícone verde)

4. **Testar**
   ```powershell
   docker info
   ```

---

### Opção 3: Reiniciar Serviço Docker (Windows)

Se ainda não funcionar:

1. **Abrir PowerShell como Administrador**

2. **Reiniciar serviço**
   ```powershell
   Restart-Service -Name com.docker.service
   ```

3. **Abrir Docker Desktop**

4. **Testar**
   ```powershell
   docker info
   ```

---

### Opção 4: Reiniciar PC (Última Opção)

Se nada funcionar:

1. Reiniciar o PC
2. Abrir Docker Desktop
3. Aguardar inicializar
4. Testar

---

## 🧪 Verificar se Funcionou

### 1. Docker Info

```powershell
docker info
```

**Deve mostrar:**
```
Server:
 Containers: 0
 Running: 0
 ...
```

Se mostrar erro, Docker Engine não está rodando.

### 2. Docker Version

```powershell
docker --version
docker-compose --version
```

**Deve mostrar:**
```
Docker version 29.x.x
Docker Compose version v5.x.x
```

### 3. Listar Containers

```powershell
docker ps
```

**Deve mostrar:**
```
CONTAINER ID   IMAGE     COMMAND   CREATED   STATUS    PORTS     NAMES
```

(Vazio é normal se não tiver containers rodando)

---

## 🚀 Após Docker Funcionar

### 1. Subir PostgreSQL

```powershell
cd backend
docker-compose -f docker-compose.dev.yml up -d
```

**Deve mostrar:**
```
[+] Running 3/3
 ✔ Network mb-network-dev        Created
 ✔ Volume "postgres-data-dev"    Created
 ✔ Container mb-postgres-dev     Started
```

### 2. Verificar Container

```powershell
docker ps
```

**Deve mostrar:**
```
CONTAINER ID   IMAGE         COMMAND                  STATUS         PORTS                    NAMES
xxxxx          postgres:17   "docker-entrypoint.s…"   Up 10 seconds  0.0.0.0:5432->5432/tcp   mb-postgres-dev
```

### 3. Ver Logs

```powershell
docker-compose -f docker-compose.dev.yml logs -f postgres
```

**Deve mostrar:**
```
database system is ready to accept connections
```

### 4. Rodar API

```powershell
dotnet run
```

**Deve conectar no banco e aplicar migrations automaticamente!** ✨

---

## 🐛 Troubleshooting

### Erro: "Docker Desktop is starting..."

**Solução:** Aguardar mais tempo (pode levar até 2 minutos na primeira vez)

### Erro: "WSL 2 installation is incomplete"

**Solução:** 
1. Abrir PowerShell como Administrador
2. Executar: `wsl --install`
3. Reiniciar PC

### Erro: "Hardware assisted virtualization is not enabled"

**Solução:**
1. Entrar na BIOS
2. Habilitar Virtualization Technology (VT-x/AMD-V)
3. Salvar e reiniciar

### Erro: "Port 5432 already in use"

**Solução:**
```powershell
# Ver o que está usando a porta
netstat -ano | findstr :5432

# Matar processo (substitua <PID> pelo número)
taskkill /PID <PID> /F
```

---

## 📋 Checklist

- [ ] Docker Desktop instalado
- [ ] Docker Desktop aberto e rodando (ícone verde)
- [ ] `docker info` funciona
- [ ] `docker ps` funciona
- [ ] PostgreSQL subiu com `docker-compose up -d`
- [ ] Container `mb-postgres-dev` está rodando
- [ ] API conecta no banco

---

## 💡 Dicas

### Iniciar Docker Automaticamente

1. Abrir Docker Desktop
2. Settings → General
3. Marcar "Start Docker Desktop when you log in"

### Ver Status do Docker

```powershell
# Status geral
docker info

# Containers rodando
docker ps

# Todos os containers (incluindo parados)
docker ps -a

# Imagens baixadas
docker images

# Uso de recursos
docker stats
```

---

## 🎯 Resumo

**Problema:** Docker Engine não está rodando

**Solução Rápida:**
1. Abrir Docker Desktop
2. Aguardar ícone ficar verde
3. Testar: `docker info`
4. Subir PostgreSQL: `docker-compose -f docker-compose.dev.yml up -d`

**Se não funcionar:** Reiniciar Docker Desktop ou PC

---

**Versão:** 1.0  
**Última atualização:** 22/01/2026
