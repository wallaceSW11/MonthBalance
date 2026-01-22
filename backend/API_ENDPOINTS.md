# 📡 API Endpoints

Base URL: `http://localhost:5150/api`

## 🗓️ MonthData

### GET /monthdata
Lista todos os meses cadastrados (ordenado por ano/mês desc)

**Response:**
```json
[
  {
    "id": 1,
    "year": 2026,
    "month": 1,
    "incomes": [...],
    "expenses": [...],
    "totalIncome": 4000.00,
    "totalExpense": 1500.00,
    "balance": 2500.00
  }
]
```

### GET /monthdata/{id}
Busca mês por ID

**Response:**
```json
{
  "id": 1,
  "year": 2026,
  "month": 1,
  "incomes": [...],
  "expenses": [...],
  "totalIncome": 4000.00,
  "totalExpense": 1500.00,
  "balance": 2500.00
}
```

### GET /monthdata/{year}/{month}
Busca mês por ano e mês

**Example:** `GET /monthdata/2026/1`

**Response:** Mesmo formato acima

### POST /monthdata
Cria novo mês

**Request:**
```json
{
  "year": 2026,
  "month": 2
}
```

**Response:** 201 Created + objeto criado

### DELETE /monthdata/{id}
Deleta mês (cascade delete em incomes e expenses)

**Response:** 204 No Content

### POST /monthdata/duplicate
Duplica mês (copia incomes e expenses)

**Request:**
```json
{
  "sourceYear": 2026,
  "sourceMonth": 1,
  "targetYear": 2026,
  "targetMonth": 2
}
```

**Response:** 201 Created + objeto criado

---

## 💰 Incomes

### GET /months/{year}/{month}/incomes
Lista receitas do mês

**Example:** `GET /months/2026/1/incomes`

**Response:**
```json
[
  {
    "id": 1,
    "name": "Salary",
    "type": "manual",
    "grossValue": 5000.00,
    "netValue": 4000.00,
    "hourlyRate": null,
    "hours": null,
    "minutes": null
  }
]
```

### GET /months/{year}/{month}/incomes/{id}
Busca receita por ID

**Response:**
```json
{
  "id": 1,
  "name": "Salary",
  "type": "manual",
  "grossValue": 5000.00,
  "netValue": 4000.00,
  "hourlyRate": null,
  "hours": null,
  "minutes": null
}
```

### POST /months/{year}/{month}/incomes
Cria nova receita

**Request (Manual):**
```json
{
  "name": "Freelance",
  "type": "manual",
  "grossValue": 3000.00,
  "netValue": 2500.00
}
```

**Request (Hourly):**
```json
{
  "name": "Consulting",
  "type": "hourly",
  "hourlyRate": 100.00,
  "hours": 40,
  "minutes": 30
}
```

**Response:** 201 Created + objeto criado

### PUT /months/{year}/{month}/incomes/{id}
Atualiza receita

**Request:** Mesmo formato do POST

**Response:** 200 OK + objeto atualizado

### DELETE /months/{year}/{month}/incomes/{id}
Deleta receita

**Response:** 204 No Content

---

## 💸 Expenses

### GET /months/{year}/{month}/expenses
Lista despesas do mês

**Example:** `GET /months/2026/1/expenses`

**Response:**
```json
[
  {
    "id": 1,
    "name": "Rent",
    "value": 1500.00
  }
]
```

### GET /months/{year}/{month}/expenses/{id}
Busca despesa por ID

**Response:**
```json
{
  "id": 1,
  "name": "Rent",
  "value": 1500.00
}
```

### POST /months/{year}/{month}/expenses
Cria nova despesa

**Request:**
```json
{
  "name": "Internet",
  "value": 100.00
}
```

**Response:** 201 Created + objeto criado

### PUT /months/{year}/{month}/expenses/{id}
Atualiza despesa

**Request:**
```json
{
  "name": "Internet",
  "value": 120.00
}
```

**Response:** 200 OK + objeto atualizado

### DELETE /months/{year}/{month}/expenses/{id}
Deleta despesa

**Response:** 204 No Content

---

## 🏥 Health Check

### GET /health
Verifica status da API

**Response:**
```json
{
  "status": "healthy",
  "timestamp": "2026-01-22T01:00:00Z",
  "version": "1.0.0"
}
```

---

## 📝 Notas

### Tipos de Income
- `manual`: Receita manual (grossValue, netValue)
- `hourly`: Receita por hora (hourlyRate, hours, minutes)

### Validações
- Year: qualquer inteiro
- Month: 1-12
- Name: máximo 200 caracteres
- Values: decimal (18,2)

### Erros
- `400 Bad Request`: Dados inválidos ou mês já existe
- `404 Not Found`: Recurso não encontrado
- `500 Internal Server Error`: Erro no servidor

### CORS
Em desenvolvimento, aceita requisições de `http://localhost:5173`

---

## 🧪 Testando com cURL

```bash
# Listar meses
curl http://localhost:5150/api/monthdata

# Buscar mês específico
curl http://localhost:5150/api/monthdata/2026/1

# Criar receita
curl -X POST http://localhost:5150/api/months/2026/1/incomes \
  -H "Content-Type: application/json" \
  -d '{"name":"Salary","type":"manual","grossValue":5000,"netValue":4000}'

# Criar despesa
curl -X POST http://localhost:5150/api/months/2026/1/expenses \
  -H "Content-Type: application/json" \
  -d '{"name":"Rent","value":1500}'

# Duplicar mês
curl -X POST http://localhost:5150/api/monthdata/duplicate \
  -H "Content-Type: application/json" \
  -d '{"sourceYear":2026,"sourceMonth":1,"targetYear":2026,"targetMonth":2}'
```

---

**Versão:** 1.0  
**Última atualização:** Janeiro 2026
