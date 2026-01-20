# BaseProject - Quick Setup Script
# Run this script in PowerShell to set up the project

Write-Host "🚀 BaseProject - Setup Script" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Checking Node.js installation..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "✅ Node.js $nodeVersion found" -ForegroundColor Green
} catch {
    Write-Host "❌ Node.js not found. Please install Node.js 18.x or higher" -ForegroundColor Red
    exit 1
}

Write-Host "Checking pnpm installation..." -ForegroundColor Yellow
try {
    $pnpmVersion = pnpm --version
    Write-Host "✅ pnpm $pnpmVersion found" -ForegroundColor Green
} catch {
    Write-Host "❌ pnpm not found. Installing pnpm..." -ForegroundColor Yellow
    npm install -g pnpm
}

Write-Host ""
Write-Host "Installing dependencies..." -ForegroundColor Yellow
pnpm install

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Dependencies installed successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to install dependencies" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Creating .env file from example..." -ForegroundColor Yellow
if (Test-Path ".env") {
    Write-Host "⚠️  .env file already exists, skipping" -ForegroundColor Yellow
} else {
    Copy-Item ".env.example" ".env"
    Write-Host "✅ .env file created" -ForegroundColor Green
}

Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "✅ Setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  1. Start development server: pnpm dev" -ForegroundColor White
Write-Host "  2. Run unit tests: pnpm test:unit" -ForegroundColor White
Write-Host "  3. Run E2E tests: pnpm test:e2e:open" -ForegroundColor White
Write-Host ""
Write-Host "📖 Read README.md for documentation" -ForegroundColor Cyan
Write-Host "🎉 Happy coding!" -ForegroundColor Green
