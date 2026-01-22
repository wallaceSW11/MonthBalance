# Script para habilitar Hyper-V e recursos necessários para Docker
# IMPORTANTE: Execute como Administrador!

Write-Host "🔧 Habilitando Hyper-V e recursos do Docker..." -ForegroundColor Cyan
Write-Host ""

# Verificar se está rodando como Admin
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "❌ Este script precisa ser executado como Administrador!" -ForegroundColor Red
    Write-Host ""
    Write-Host "   💡 Como executar:" -ForegroundColor Yellow
    Write-Host "   1. Fechar este PowerShell" -ForegroundColor White
    Write-Host "   2. Abrir PowerShell como Administrador" -ForegroundColor White
    Write-Host "   3. Navegar até: cd C:\git\MB1_Month_Balance\MonthBalance\backend" -ForegroundColor White
    Write-Host "   4. Executar: .\habilitar-hyperv.ps1" -ForegroundColor White
    Write-Host ""
    Read-Host "Pressione Enter para sair"
    exit 1
}

Write-Host "✅ Executando como Administrador" -ForegroundColor Green
Write-Host ""

# Habilitar Hyper-V
Write-Host "1. Habilitando Hyper-V..." -ForegroundColor Yellow
try {
    Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All -NoRestart
    Write-Host "   ✅ Hyper-V habilitado!" -ForegroundColor Green
} catch {
    Write-Host "   ⚠️  Erro ao habilitar Hyper-V: $_" -ForegroundColor Red
}

Write-Host ""

# Habilitar Plataforma de Máquina Virtual
Write-Host "2. Habilitando Plataforma de Máquina Virtual..." -ForegroundColor Yellow
try {
    Enable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform -All -NoRestart
    Write-Host "   ✅ Plataforma de Máquina Virtual habilitada!" -ForegroundColor Green
} catch {
    Write-Host "   ⚠️  Erro ao habilitar Plataforma de Máquina Virtual: $_" -ForegroundColor Red
}

Write-Host ""

# Habilitar WSL
Write-Host "3. Habilitando WSL (Windows Subsystem for Linux)..." -ForegroundColor Yellow
try {
    Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux -All -NoRestart
    Write-Host "   ✅ WSL habilitado!" -ForegroundColor Green
} catch {
    Write-Host "   ⚠️  Erro ao habilitar WSL: $_" -ForegroundColor Red
}

Write-Host ""
Write-Host "✅ Configuração completa!" -ForegroundColor Green
Write-Host ""
Write-Host "🔄 IMPORTANTE: Você precisa REINICIAR o PC agora!" -ForegroundColor Yellow
Write-Host ""
Write-Host "   Após reiniciar:" -ForegroundColor Cyan
Write-Host "   1. Abrir Docker Desktop" -ForegroundColor White
Write-Host "   2. Aguardar ícone ficar verde" -ForegroundColor White
Write-Host "   3. Executar: .\check-docker.ps1" -ForegroundColor White
Write-Host "   4. Executar: docker-compose -f docker-compose.dev.yml up -d" -ForegroundColor White
Write-Host ""

$restart = Read-Host "Deseja reiniciar agora? (S/N)"

if ($restart -eq "S" -or $restart -eq "s") {
    Write-Host ""
    Write-Host "🔄 Reiniciando em 10 segundos..." -ForegroundColor Yellow
    Write-Host "   (Pressione Ctrl+C para cancelar)" -ForegroundColor White
    Start-Sleep -Seconds 10
    Restart-Computer -Force
} else {
    Write-Host ""
    Write-Host "⚠️  Lembre-se de reiniciar o PC manualmente!" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Pressione Enter para sair"
}
