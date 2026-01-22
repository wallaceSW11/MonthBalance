# Script para verificar se virtualização está habilitada

Write-Host "🔍 Verificando suporte à virtualização..." -ForegroundColor Cyan
Write-Host ""

# Verificar se virtualização está habilitada
$virtualization = Get-ComputerInfo | Select-Object -ExpandProperty HyperVisorPresent

if ($virtualization) {
    Write-Host "✅ Virtualização está HABILITADA!" -ForegroundColor Green
    Write-Host ""
    Write-Host "   O Docker deveria funcionar." -ForegroundColor White
    Write-Host "   Se ainda não funciona, tente:" -ForegroundColor Yellow
    Write-Host "   1. Reiniciar o PC" -ForegroundColor White
    Write-Host "   2. Abrir Docker Desktop novamente" -ForegroundColor White
} else {
    Write-Host "❌ Virtualização está DESABILITADA!" -ForegroundColor Red
    Write-Host ""
    Write-Host "   💡 Solução:" -ForegroundColor Yellow
    Write-Host "   1. Reiniciar o PC" -ForegroundColor White
    Write-Host "   2. Entrar na BIOS (F2, F10, Del ou Esc)" -ForegroundColor White
    Write-Host "   3. Procurar 'Virtualization Technology' ou 'VT-x' ou 'AMD-V'" -ForegroundColor White
    Write-Host "   4. Habilitar (Enabled)" -ForegroundColor White
    Write-Host "   5. Salvar e sair (F10)" -ForegroundColor White
    Write-Host ""
    Write-Host "   📚 Guia completo: HABILITAR_VIRTUALIZACAO.md" -ForegroundColor Cyan
}

Write-Host ""

# Verificar Hyper-V
Write-Host "🔍 Verificando Hyper-V..." -ForegroundColor Cyan
$hyperv = Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V-All

if ($hyperv.State -eq "Enabled") {
    Write-Host "✅ Hyper-V está habilitado!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Hyper-V está desabilitado" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "   💡 Para habilitar (PowerShell como Admin):" -ForegroundColor Yellow
    Write-Host "   Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All" -ForegroundColor White
}

Write-Host ""
Write-Host "✅ Verificação completa!" -ForegroundColor Green
