# Script para verificar status do Docker

Write-Host "🐳 Verificando Docker..." -ForegroundColor Cyan
Write-Host ""

# Verificar se Docker está instalado
Write-Host "1. Verificando instalação..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "   ✅ Docker instalado: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Docker não está instalado!" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Verificar se Docker Engine está rodando
Write-Host "2. Verificando Docker Engine..." -ForegroundColor Yellow
try {
    $dockerInfo = docker info 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Docker Engine está rodando!" -ForegroundColor Green
        
        # Mostrar informações básicas
        $containers = docker ps -q | Measure-Object | Select-Object -ExpandProperty Count
        Write-Host "   📦 Containers rodando: $containers" -ForegroundColor Cyan
    } else {
        Write-Host "   ❌ Docker Engine NÃO está rodando!" -ForegroundColor Red
        Write-Host ""
        Write-Host "   💡 Solução:" -ForegroundColor Yellow
        Write-Host "   1. Abrir Docker Desktop" -ForegroundColor White
        Write-Host "   2. Aguardar ícone ficar verde na bandeja" -ForegroundColor White
        Write-Host "   3. Rodar este script novamente" -ForegroundColor White
        exit 1
    }
} catch {
    Write-Host "   ❌ Erro ao verificar Docker Engine!" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Verificar se PostgreSQL está rodando
Write-Host "3. Verificando PostgreSQL..." -ForegroundColor Yellow
$postgresContainer = docker ps --filter "name=mb-postgres-dev" --format "{{.Names}}"
if ($postgresContainer) {
    Write-Host "   ✅ PostgreSQL está rodando!" -ForegroundColor Green
    Write-Host "   📦 Container: $postgresContainer" -ForegroundColor Cyan
} else {
    Write-Host "   ⚠️  PostgreSQL NÃO está rodando" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "   💡 Para subir PostgreSQL:" -ForegroundColor Yellow
    Write-Host "   docker-compose -f docker-compose.dev.yml up -d" -ForegroundColor White
}

Write-Host ""
Write-Host "✅ Verificação completa!" -ForegroundColor Green
