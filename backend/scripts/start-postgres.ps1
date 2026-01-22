# Script para iniciar PostgreSQL via Docker Compose

Write-Host "🐘 Iniciando PostgreSQL..." -ForegroundColor Cyan

$dockerComposePath = Join-Path $PSScriptRoot "..\docker-compose.dev.yml"

docker-compose -f $dockerComposePath up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ PostgreSQL iniciado com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📋 Informações de conexão:" -ForegroundColor Yellow
    Write-Host "  Host: localhost" -ForegroundColor White
    Write-Host "  Port: 5432" -ForegroundColor White
    Write-Host "  Database: monthbalance" -ForegroundColor White
    Write-Host "  User: mbuser" -ForegroundColor White
    Write-Host "  Password: mbpass123" -ForegroundColor White
    Write-Host ""
    Write-Host "🔍 Para ver logs: docker-compose -f docker-compose.dev.yml logs -f" -ForegroundColor Cyan
    Write-Host "🛑 Para parar: docker-compose -f docker-compose.dev.yml down" -ForegroundColor Cyan
} else {
    Write-Host "❌ Erro ao iniciar PostgreSQL" -ForegroundColor Red
    exit 1
}
