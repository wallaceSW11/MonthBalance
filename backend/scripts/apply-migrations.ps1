# Script para aplicar migrations no banco de dados

Write-Host "🔄 Aplicando migrations..." -ForegroundColor Cyan

Set-Location (Join-Path $PSScriptRoot "..")

dotnet ef database update --project MonthBalance.API --startup-project MonthBalance.API

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migrations aplicadas com sucesso!" -ForegroundColor Green
} else {
    Write-Host "❌ Erro ao aplicar migrations" -ForegroundColor Red
    exit 1
}
