# Script PowerShell para iniciar o projeto Month Balance com Docker

Write-Host "🐳 Iniciando Month Balance..." -ForegroundColor Cyan

# Verifica se o arquivo .env existe
if (-not (Test-Path .env)) {
    Write-Host "⚠️  Arquivo .env não encontrado!" -ForegroundColor Yellow
    Write-Host "📝 Copiando .env.example para .env..." -ForegroundColor Yellow
    Copy-Item .env.example .env
    Write-Host "✏️  Por favor, edite o arquivo .env com suas configurações antes de continuar." -ForegroundColor Yellow
    exit 1
}

# Verifica se o Docker está rodando
try {
    docker info | Out-Null
} catch {
    Write-Host "❌ Docker não está rodando. Por favor, inicie o Docker Desktop." -ForegroundColor Red
    exit 1
}

Write-Host "🔨 Construindo e iniciando containers..." -ForegroundColor Cyan
docker-compose up -d --build

Write-Host ""
Write-Host "✅ Containers iniciados!" -ForegroundColor Green
Write-Host ""
Write-Host "📍 Serviços disponíveis:" -ForegroundColor Cyan
Write-Host "   Frontend:  http://localhost:8080" -ForegroundColor White
Write-Host "   Backend:   http://localhost:5150" -ForegroundColor White
Write-Host "   Database:  localhost:5433" -ForegroundColor White
Write-Host ""
Write-Host "📊 Para ver os logs: docker-compose logs -f" -ForegroundColor Yellow
Write-Host "🛑 Para parar: docker-compose down" -ForegroundColor Yellow
