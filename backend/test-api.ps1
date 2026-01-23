# Month Balance API - Testes via PowerShell
$baseUrl = "http://localhost:5150"

Write-Host "🚀 Month Balance API - Testes" -ForegroundColor Cyan
Write-Host ""

# 1. Registrar usuário
Write-Host "1️⃣  Registrando usuário..." -ForegroundColor Yellow
$registerBody = @{
    email = "teste@teste.com"
    password = "Senha123!"
    confirmPassword = "Senha123!"
} | ConvertTo-Json

try {
    $registerResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/register" -Method Post -Body $registerBody -ContentType "application/json"
    Write-Host "✅ Usuário registrado!" -ForegroundColor Green
    Write-Host "   Email: $($registerResponse.email)" -ForegroundColor Gray
} catch {
    if ($_.Exception.Response.StatusCode -eq 400) {
        Write-Host "⚠️  Usuário já existe, fazendo login..." -ForegroundColor Yellow
    } else {
        Write-Host "❌ Erro ao registrar: $($_.Exception.Message)" -ForegroundColor Red
        exit
    }
}

Write-Host ""

# 2. Login
Write-Host "2️⃣  Fazendo login..." -ForegroundColor Yellow
$loginBody = @{
    email = "teste@teste.com"
    password = "Senha123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method Post -Body $loginBody -ContentType "application/json"
    $token = $loginResponse.token
    Write-Host "✅ Login realizado!" -ForegroundColor Green
    Write-Host "   Token: $($token.Substring(0, 20))..." -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao fazer login: $($_.Exception.Message)" -ForegroundColor Red
    exit
}

Write-Host ""

# Headers com token
$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# 3. Criar receita
Write-Host "3️⃣  Criando receita..." -ForegroundColor Yellow
$incomeBody = @{
    description = "Salário"
    type = 0
} | ConvertTo-Json

try {
    $incomeResponse = Invoke-RestMethod -Uri "$baseUrl/api/incomes" -Method Post -Body $incomeBody -Headers $headers
    $incomeId = $incomeResponse.id
    Write-Host "✅ Receita criada!" -ForegroundColor Green
    Write-Host "   ID: $incomeId - $($incomeResponse.description)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao criar receita: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# 4. Criar despesa
Write-Host "4️⃣  Criando despesa..." -ForegroundColor Yellow
$expenseBody = @{
    description = "Aluguel"
} | ConvertTo-Json

try {
    $expenseResponse = Invoke-RestMethod -Uri "$baseUrl/api/expenses" -Method Post -Body $expenseBody -Headers $headers
    $expenseId = $expenseResponse.id
    Write-Host "✅ Despesa criada!" -ForegroundColor Green
    Write-Host "   ID: $expenseId - $($expenseResponse.description)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao criar despesa: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# 5. Buscar mês (cria automaticamente)
Write-Host "5️⃣  Buscando mês (Janeiro 2026)..." -ForegroundColor Yellow
try {
    $monthResponse = Invoke-RestMethod -Uri "$baseUrl/api/months/2026/1" -Method Get -Headers $headers
    Write-Host "✅ Mês encontrado/criado!" -ForegroundColor Green
    Write-Host "   Ano: $($monthResponse.year) - Mês: $($monthResponse.month)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao buscar mês: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# 6. Adicionar receita ao mês
Write-Host "6️⃣  Adicionando receita ao mês..." -ForegroundColor Yellow
$monthIncomeBody = @{
    incomeId = $incomeId
    grossValue = 5000.00
    netValue = 4000.00
    hourlyRate = $null
    hours = $null
    minutes = $null
} | ConvertTo-Json

try {
    $monthIncomeResponse = Invoke-RestMethod -Uri "$baseUrl/api/months/2026/1/incomes" -Method Post -Body $monthIncomeBody -Headers $headers
    Write-Host "✅ Receita adicionada ao mês!" -ForegroundColor Green
    Write-Host "   $($monthIncomeResponse.incomeDescription): R$ $($monthIncomeResponse.netValue)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao adicionar receita ao mês: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# 7. Adicionar despesa ao mês
Write-Host "7️⃣  Adicionando despesa ao mês..." -ForegroundColor Yellow
$monthExpenseBody = @{
    expenseId = $expenseId
    value = 1500.00
} | ConvertTo-Json

try {
    $monthExpenseResponse = Invoke-RestMethod -Uri "$baseUrl/api/months/2026/1/expenses" -Method Post -Body $monthExpenseBody -Headers $headers
    Write-Host "✅ Despesa adicionada ao mês!" -ForegroundColor Green
    Write-Host "   $($monthExpenseResponse.expenseDescription): R$ $($monthExpenseResponse.value)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao adicionar despesa ao mês: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# 8. Ver dados completos do mês
Write-Host "8️⃣  Buscando dados completos do mês..." -ForegroundColor Yellow
try {
    $finalMonthResponse = Invoke-RestMethod -Uri "$baseUrl/api/months/2026/1" -Method Get -Headers $headers
    Write-Host "✅ Dados do mês:" -ForegroundColor Green
    Write-Host "   📅 Mês: $($finalMonthResponse.month)/$($finalMonthResponse.year)" -ForegroundColor Gray
    Write-Host "   💰 Total Receitas: R$ $($finalMonthResponse.totalIncome)" -ForegroundColor Green
    Write-Host "   💸 Total Despesas: R$ $($finalMonthResponse.totalExpense)" -ForegroundColor Red
    Write-Host "   💵 Saldo: R$ $($finalMonthResponse.balance)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro ao buscar dados do mês: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "✅ Testes concluídos!" -ForegroundColor Green
