# Script para iniciar o ambiente de desenvolvimento no Windows

Write-Host "🚀 Iniciando Month Balance - Desenvolvimento" -ForegroundColor Green
Write-Host ""

# Verificar se Docker está rodando
try {
    docker info | Out-Null
} catch {
    Write-Host "❌ Docker não está rodando. Inicie o Docker e tente novamente." -ForegroundColor Red
    exit 1
}

# Verificar se .env existe
if (-not (Test-Path .env)) {
    Write-Host "📝 Criando arquivo .env..." -ForegroundColor Yellow
    Copy-Item .env.development .env
}

# Subir banco e backend
Write-Host "🐘 Iniciando PostgreSQL e Backend..." -ForegroundColor Cyan
docker compose -f docker-compose.dev.yml up -d

Write-Host ""
Write-Host "✅ Backend rodando em: http://localhost:5000" -ForegroundColor Green
Write-Host "✅ PostgreSQL rodando em: localhost:5432" -ForegroundColor Green
Write-Host ""
Write-Host "📱 Para iniciar o frontend:" -ForegroundColor Yellow
Write-Host "   cd frontend"
Write-Host "   npm install"
Write-Host "   npm run dev"
Write-Host ""
Write-Host "📊 Ver logs: docker compose -f docker-compose.dev.yml logs -f" -ForegroundColor Cyan
Write-Host "🛑 Parar: docker compose -f docker-compose.dev.yml down" -ForegroundColor Cyan
