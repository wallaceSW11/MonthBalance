# 🔧 Habilitar Hyper-V - Solução para Docker

## 🎯 Situação Atual

```
✅ Virtualização está HABILITADA (na BIOS)
❌ Hyper-V está DESABILITADO (no Windows)
```

**Resultado:** Docker não funciona porque precisa do Hyper-V habilitado.

---

## ✅ Solução: Habilitar Hyper-V

### Método 1: PowerShell (Recomendado - Mais Rápido)

1. **Abrir PowerShell como Administrador**
   - Pressionar `Windows + X`
   - Clicar em "Windows PowerShell (Admin)" ou "Terminal (Admin)"
   - Ou: Menu Iniciar → Digitar "PowerShell" → Botão direito → "Executar como administrador"

2. **Executar comando:**
   ```powershell
   Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All
   ```

3. **Aguardar** (pode levar 2-5 minutos)

4. **Quando perguntar para reiniciar:**
   - Digitar `Y` e pressionar Enter
   - Ou: Reiniciar manualmente depois

5. **Após reiniciar:**
   - Abrir Docker Desktop
   - Deve funcionar! ✅

---

### Método 2: Interface Gráfica (Alternativa)

1. **Abrir "Ativar ou desativar recursos do Windows"**
   - Pressionar `Windows + R`
   - Digitar: `optionalfeatures`
   - Pressionar Enter

2. **Procurar e marcar:**
   - ☑️ Hyper-V
   - ☑️ Plataforma de Máquina Virtual
   - ☑️ Plataforma do Windows Hypervisor

3. **Clicar em "OK"**

4. **Aguardar instalação** (2-5 minutos)

5. **Reiniciar quando solicitado**

6. **Após reiniciar:**
   - Abrir Docker Desktop
   - Deve funcionar! ✅

---

### Método 3: Comando DISM (Alternativa)

1. **Abrir CMD como Administrador**
   - Menu Iniciar → Digitar "cmd"
   - Botão direito → "Executar como administrador"

2. **Executar comandos:**
   ```cmd
   dism.exe /Online /Enable-Feature /FeatureName:Microsoft-Hyper-V-All /All /NoRestart
   dism.exe /Online /Enable-Feature /FeatureName:VirtualMachinePlatform /All /NoRestart
   ```

3. **Reiniciar:**
   ```cmd
   shutdown /r /t 0
   ```

4. **Após reiniciar:**
   - Abrir Docker Desktop
   - Deve funcionar! ✅

---

## 🔍 Verificar se Funcionou

Após reiniciar e habilitar o Hyper-V:

### 1. Verificar Hyper-V

```powershell
# PowerShell como Admin
Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V-All
```

**Deve mostrar:**
```
State : Enabled
```

### 2. Abrir Docker Desktop

- Menu Iniciar → Docker Desktop
- Aguardar inicializar (ícone verde)

### 3. Verificar Docker

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

## 🐛 Troubleshooting

### Erro: "Hyper-V cannot be installed: A hypervisor is already running"

**Solução:**
1. Desabilitar outros hypervisors (VirtualBox, VMware)
2. Reiniciar
3. Tentar novamente

### Erro: "Hyper-V cannot be installed: The processor does not have required virtualization capabilities"

**Solução:**
1. Entrar na BIOS
2. Habilitar VT-x/AMD-V
3. Salvar e reiniciar

### Erro: "This host does not support Intel VT-x/EPT"

**Solução:**
1. Verificar se CPU suporta virtualização
2. Entrar na BIOS
3. Habilitar Intel VT-x ou AMD-V

### Docker Desktop ainda não funciona após habilitar Hyper-V

**Solução:**
1. Reiniciar o PC novamente
2. Abrir Docker Desktop
3. Se ainda não funcionar:
   - Docker Desktop → Settings → General
   - Marcar "Use WSL 2 based engine"
   - Apply & Restart

---

## 📋 Checklist Completo

- [ ] Virtualização habilitada na BIOS (✅ Já está!)
- [ ] PowerShell aberto como Administrador
- [ ] Comando `Enable-WindowsOptionalFeature` executado
- [ ] PC reiniciado
- [ ] Docker Desktop aberto
- [ ] `.\check-docker.ps1` mostra ✅
- [ ] PostgreSQL subiu com `docker-compose up -d`

---

## 🎯 Resumo Visual

```
Situação Atual:
✅ Virtualização (BIOS) → Habilitada
❌ Hyper-V (Windows) → Desabilitado
❌ Docker → Não funciona

Após Habilitar Hyper-V:
✅ Virtualização (BIOS) → Habilitada
✅ Hyper-V (Windows) → Habilitado
✅ Docker → Funciona!
```

---

## 🚀 Próximos Passos

Depois de habilitar o Hyper-V e reiniciar:

1. Abrir Docker Desktop
2. Aguardar ícone ficar verde
3. Executar: `.\check-docker.ps1`
4. Executar: `docker-compose -f docker-compose.dev.yml up -d`
5. Executar: `dotnet run`

**Pronto!** Backend rodando! 🎉

---

## 💡 Dica

Depois que funcionar, configure para iniciar automaticamente:

```
Docker Desktop → Settings → General → 
✅ Start Docker Desktop when you log in
```

---

**Versão:** 1.0  
**Última atualização:** 22/01/2026
