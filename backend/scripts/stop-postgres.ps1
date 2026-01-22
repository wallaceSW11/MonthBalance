# Script para parar PostgreSQL via Docker Compose

Write-Host "🛑 Parando PostgreSQL..." -ForegroundColor Cyan

$dockerComposePath = Join-Path $PSScriptRoot "..\docker-compose.dev.yml"

docker-compose -f $dockerComposePath down

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ PostgreSQL parado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "❌ Erro ao parar PostgreSQL" -ForegroundColor Red
    exit 1
}
