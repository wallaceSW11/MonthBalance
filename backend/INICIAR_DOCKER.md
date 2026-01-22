# 🐳 Como Iniciar o Docker Desktop

## 🎯 Problema Atual

```
❌ Docker Engine NÃO está rodando!
```

O Docker está instalado, mas o **Docker Desktop** precisa ser aberto.

---

## ✅ Solução (Passo a Passo)

### 1️⃣ Abrir Docker Desktop

**Opção A: Menu Iniciar**
1. Pressionar tecla `Windows`
2. Digitar: `Docker Desktop`
3. Clicar no aplicativo

**Opção B: Atalho na Área de Trabalho**
1. Procurar ícone do Docker na área de trabalho
2. Dar duplo clique

**Opção C: Busca do Windows**
1. Pressionar `Windows + S`
2. Digitar: `Docker`
3. Clicar em "Docker Desktop"

---

### 2️⃣ Aguardar Inicialização

Após abrir o Docker Desktop:

1. **Aguardar 30-60 segundos** (primeira vez pode levar até 2 minutos)
2. Observar o **ícone na bandeja** (canto inferior direito da tela)

**Status do Ícone:**

```
🔴 Vermelho/Laranja = Iniciando... (aguarde)
🟢 Verde = Pronto! (pode usar)
```

**Mensagem no Docker Desktop:**
- ❌ "Docker Desktop is starting..." → Aguarde
- ✅ "Docker Desktop is running" → Pronto!

---

### 3️⃣ Verificar se Funcionou

Abrir PowerShell e executar:

```powershell
cd C:\git\MB1_Month_Balance\MonthBalance\backend
.\check-docker.ps1
```

**Deve mostrar:**
```
✅ Docker instalado
✅ Docker Engine está rodando!
```

---

### 4️⃣ Subir PostgreSQL

Agora sim, pode subir o banco:

```powershell
docker-compose -f docker-compose.dev.yml up -d
```

**Deve mostrar:**
```
[+] Running 3/3
 ✔ Network mb-network-dev        Created
 ✔ Volume "postgres-data-dev"    Created
 ✔ Container mb-postgres-dev     Started
```

---

### 5️⃣ Verificar PostgreSQL

```powershell
docker ps
```

**Deve mostrar:**
```
CONTAINER ID   IMAGE         STATUS         PORTS                    NAMES
xxxxx          postgres:17   Up 10 seconds  0.0.0.0:5432->5432/tcp   mb-postgres-dev
```

---

### 6️⃣ Rodar API

```powershell
dotnet run
```

**Deve mostrar:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5150
```

✨ **Migrations aplicadas automaticamente!**

---

## 🔄 Se Não Funcionar

### Problema: Docker Desktop não abre

**Solução:**
1. Reiniciar o PC
2. Tentar abrir novamente

### Problema: Docker Desktop abre mas fica "Starting..."

**Solução:**
1. Fechar Docker Desktop (botão direito no ícone → Quit)
2. Aguardar 10 segundos
3. Abrir novamente

### Problema: Erro "WSL 2 installation is incomplete"

**Solução:**
1. Abrir PowerShell como Administrador
2. Executar: `wsl --install`
3. Reiniciar PC
4. Abrir Docker Desktop

### Problema: Erro "Hardware virtualization is not enabled"

**Solução:**
1. Reiniciar PC
2. Entrar na BIOS (geralmente F2, F10, Del ou Esc)
3. Procurar "Virtualization Technology" ou "VT-x" ou "AMD-V"
4. Habilitar
5. Salvar e sair

---

## 📋 Checklist Completo

- [ ] Docker Desktop aberto
- [ ] Ícone verde na bandeja
- [ ] `.\check-docker.ps1` mostra ✅
- [ ] `docker-compose up -d` funcionou
- [ ] `docker ps` mostra container rodando
- [ ] `dotnet run` conecta no banco

---

## 💡 Dica: Iniciar Automaticamente

Para não precisar abrir manualmente toda vez:

1. Abrir Docker Desktop
2. Clicar no ícone de engrenagem (Settings)
3. Ir em "General"
4. Marcar: ✅ "Start Docker Desktop when you log in"
5. Clicar em "Apply & restart"

Pronto! Docker vai iniciar automaticamente quando você ligar o PC! 🎉

---

## 🎯 Resumo Visual

```
1. Abrir Docker Desktop
   ↓
2. Aguardar ícone ficar verde (30-60s)
   ↓
3. Verificar: .\check-docker.ps1
   ↓
4. Subir PostgreSQL: docker-compose up -d
   ↓
5. Rodar API: dotnet run
   ↓
6. Testar: curl http://localhost:5150/api/health
```

---

## 🚀 Próximo Passo

Depois que o Docker Desktop estiver rodando (ícone verde):

1. Voltar para o PowerShell
2. Executar: `docker-compose -f docker-compose.dev.yml up -d`
3. Executar: `dotnet run`

**Pronto!** Backend rodando com banco de dados! 🎉

---

**Versão:** 1.0  
**Última atualização:** 22/01/2026
