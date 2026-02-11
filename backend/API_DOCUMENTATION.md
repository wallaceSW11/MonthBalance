# 📚 Month Balance API - Documentação para Frontend

**Base URL:** `http://localhost:5000/api`

**Autenticação:** JWT Bearer Token (exceto Register e Login)

---

## 🔐 Authentication

### Register
Criar nova conta de usuário.

**Endpoint:** `POST /auth/register`  
**Auth:** ❌ Não requer

**Request Body:**
```json
{
  "name": "João Silva",
  "email": "joao@example.com",
  "password": "senha123"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao@example.com",
    "avatar": null,
    "notificationsEnabled": true
  }
}
```

---

### Login
Fazer login e obter token JWT.

**Endpoint:** `POST /auth/login`  
**Auth:** ❌ Não requer

**Request Body:**
```json
{
  "email": "joao@example.com",
  "password": "senha123"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao@example.com",
    "avatar": null,
    "notificationsEnabled": true
  }
}
```

---

### Get Current User
Obter dados do usuário logado.

**Endpoint:** `GET /auth/me`  
**Auth:** ✅ Bearer Token

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao@example.com",
  "avatar": null,
  "notificationsEnabled": true
}
```

---

### Update User
Atualizar perfil do usuário.

**Endpoint:** `PUT /auth/me`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "name": "João Silva Santos",
  "avatar": "https://example.com/avatar.jpg",
  "notificationsEnabled": false
}
```

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "João Silva Santos",
  "email": "joao@example.com",
  "avatar": "https://example.com/avatar.jpg",
  "notificationsEnabled": false
}
```

---

### Change Password
Alterar senha do usuário.

**Endpoint:** `POST /auth/change-password`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "currentPassword": "senha123",
  "newPassword": "novaSenha456"
}
```

**Response:** `204 No Content`

---

## 📅 Month Data

### List All Months
Listar todos os meses do usuário.

**Endpoint:** `GET /month-data`  
**Auth:** ✅ Bearer Token

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "year": 2026,
    "month": 2,
    "lastAccessed": "2026-02-06T18:30:00Z"
  },
  {
    "id": 2,
    "year": 2026,
    "month": 1,
    "lastAccessed": "2026-01-15T10:00:00Z"
  }
]
```

---

### Get Month by Year/Month
Buscar mês específico.

**Endpoint:** `GET /month-data/{year}/{month}`  
**Auth:** ✅ Bearer Token

**Example:** `GET /month-data/2026/2`

**Response:** `200 OK`
```json
{
  "id": 1,
  "year": 2026,
  "month": 2,
  "lastAccessed": "2026-02-06T18:30:00Z"
}
```

---

### Create Month
Criar novo mês.

**Endpoint:** `POST /month-data`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "year": 2026,
  "month": 3
}
```

**Response:** `201 Created`
```json
{
  "id": 3,
  "year": 2026,
  "month": 3,
  "lastAccessed": "2026-02-06T18:30:00Z"
}
```

---

### Update Last Accessed
Atualizar último acesso do mês.

**Endpoint:** `PUT /month-data/{id}/last-accessed`  
**Auth:** ✅ Bearer Token

**Example:** `PUT /month-data/1/last-accessed`

**Response:** `204 No Content`

---

### Delete Month
Deletar mês (e todas receitas/despesas associadas).

**Endpoint:** `DELETE /month-data/{id}`  
**Auth:** ✅ Bearer Token

**Example:** `DELETE /month-data/1`

**Response:** `204 No Content`

---

## 💰 Income Types

### List Income Types
Listar todos os tipos de receita do usuário.

**Endpoint:** `GET /income-types`  
**Auth:** ✅ Bearer Token

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "Salário CLT",
    "type": "paycheck"
  },
  {
    "id": 2,
    "name": "Freelance",
    "type": "hourly"
  },
  {
    "id": 3,
    "name": "Bônus",
    "type": "extra"
  }
]
```

**Tipos válidos:** `paycheck`, `hourly`, `extra`

---

### Get Income Type
Buscar tipo de receita específico.

**Endpoint:** `GET /income-types/{id}`  
**Auth:** ✅ Bearer Token

**Example:** `GET /income-types/1`

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "Salário CLT",
  "type": "paycheck"
}
```

---

### Create Income Type
Criar novo tipo de receita.

**Endpoint:** `POST /income-types`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "name": "Salário CLT",
  "type": "paycheck"
}
```

**Tipos válidos:** `paycheck`, `hourly`, `extra`

**Response:** `201 Created`
```json
{
  "id": 1,
  "name": "Salário CLT",
  "type": "paycheck"
}
```

---

### Update Income Type
Atualizar tipo de receita (apenas o nome).

**Endpoint:** `PUT /income-types/{id}`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "name": "Salário CLT Atualizado"
}
```

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "Salário CLT Atualizado",
  "type": "paycheck"
}
```

---

### Delete Income Type
Deletar tipo de receita (não pode ter receitas associadas).

**Endpoint:** `DELETE /income-types/{id}`  
**Auth:** ✅ Bearer Token

**Response:** `204 No Content`

**Error:** `400 Bad Request` se houver receitas associadas
```json
{
  "message": "Cannot delete income type with associated incomes"
}
```

---

## 💵 Incomes

### List Incomes by Month
Listar receitas de um mês.

**Endpoint:** `GET /incomes/month/{monthDataId}`  
**Auth:** ✅ Bearer Token

**Example:** `GET /incomes/month/1`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "monthDataId": 1,
    "incomeTypeId": 1,
    "grossValue": 5000.00,
    "netValue": 4000.00,
    "hourlyRate": null,
    "hours": null,
    "minutes": null,
    "calculatedValue": 4000.00
  },
  {
    "id": 2,
    "monthDataId": 1,
    "incomeTypeId": 2,
    "grossValue": null,
    "netValue": null,
    "hourlyRate": 50.00,
    "hours": 40,
    "minutes": 30,
    "calculatedValue": 2025.00
  }
]
```

---

### Get Income
Buscar receita específica.

**Endpoint:** `GET /incomes/{id}`  
**Auth:** ✅ Bearer Token

**Response:** `200 OK`
```json
{
  "id": 1,
  "monthDataId": 1,
  "incomeTypeId": 1,
  "grossValue": 5000.00,
  "netValue": 4000.00,
  "hourlyRate": null,
  "hours": null,
  "minutes": null,
  "calculatedValue": 4000.00
}
```

---

### Create Income
Criar nova receita.

**Endpoint:** `POST /incomes`  
**Auth:** ✅ Bearer Token

**Request Body (Paycheck/Extra):**
```json
{
  "monthDataId": 1,
  "incomeTypeId": 1,
  "grossValue": 5000.00,
  "netValue": 4000.00,
  "hourlyRate": null,
  "hours": null,
  "minutes": null
}
```

**Request Body (Hourly):**
```json
{
  "monthDataId": 1,
  "incomeTypeId": 2,
  "grossValue": null,
  "netValue": null,
  "hourlyRate": 50.00,
  "hours": 40,
  "minutes": 30
}
```

**Response:** `201 Created`
```json
{
  "id": 1,
  "monthDataId": 1,
  "incomeTypeId": 1,
  "grossValue": 5000.00,
  "netValue": 4000.00,
  "hourlyRate": null,
  "hours": null,
  "minutes": null,
  "calculatedValue": 4000.00
}
```

**Cálculo Automático:**
- **Paycheck:** `calculatedValue = netValue ?? grossValue ?? 0`
- **Hourly:** `calculatedValue = hourlyRate * (hours + minutes/60)`
- **Extra:** `calculatedValue = netValue ?? grossValue ?? 0`

---

### Update Income
Atualizar receita.

**Endpoint:** `PUT /incomes/{id}`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "grossValue": 5500.00,
  "netValue": 4400.00,
  "hourlyRate": null,
  "hours": null,
  "minutes": null
}
```

**Response:** `200 OK`
```json
{
  "id": 1,
  "monthDataId": 1,
  "incomeTypeId": 1,
  "grossValue": 5500.00,
  "netValue": 4400.00,
  "hourlyRate": null,
  "hours": null,
  "minutes": null,
  "calculatedValue": 4400.00
}
```

---

### Delete Income
Deletar receita.

**Endpoint:** `DELETE /incomes/{id}`  
**Auth:** ✅ Bearer Token

**Response:** `204 No Content`

---

## 💳 Expense Types

### List Expense Types
Listar todos os tipos de despesa do usuário.

**Endpoint:** `GET /expense-types`  
**Auth:** ✅ Bearer Token

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "Aluguel"
  },
  {
    "id": 2,
    "name": "Alimentação"
  },
  {
    "id": 3,
    "name": "Transporte"
  }
]
```

---

### Get Expense Type
Buscar tipo de despesa específico.

**Endpoint:** `GET /expense-types/{id}`  
**Auth:** ✅ Bearer Token

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "Aluguel"
}
```

---

### Create Expense Type
Criar novo tipo de despesa.

**Endpoint:** `POST /expense-types`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "name": "Aluguel"
}
```

**Response:** `201 Created`
```json
{
  "id": 1,
  "name": "Aluguel"
}
```

---

### Update Expense Type
Atualizar tipo de despesa.

**Endpoint:** `PUT /expense-types/{id}`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "name": "Aluguel Atualizado"
}
```

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "Aluguel Atualizado"
}
```

---

### Delete Expense Type
Deletar tipo de despesa (não pode ter despesas associadas).

**Endpoint:** `DELETE /expense-types/{id}`  
**Auth:** ✅ Bearer Token

**Response:** `204 No Content`

**Error:** `400 Bad Request` se houver despesas associadas
```json
{
  "message": "Cannot delete expense type with associated expenses"
}
```

---

## 💸 Expenses

### List Expenses by Month
Listar despesas de um mês.

**Endpoint:** `GET /expenses/month/{monthDataId}`  
**Auth:** ✅ Bearer Token

**Example:** `GET /expenses/month/1`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "monthDataId": 1,
    "expenseTypeId": 1,
    "value": 1200.00
  },
  {
    "id": 2,
    "monthDataId": 1,
    "expenseTypeId": 2,
    "value": 800.00
  }
]
```

---

### Get Expense
Buscar despesa específica.

**Endpoint:** `GET /expenses/{id}`  
**Auth:** ✅ Bearer Token

**Response:** `200 OK`
```json
{
  "id": 1,
  "monthDataId": 1,
  "expenseTypeId": 1,
  "value": 1200.00
}
```

---

### Create Expense
Criar nova despesa.

**Endpoint:** `POST /expenses`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "monthDataId": 1,
  "expenseTypeId": 1,
  "value": 1200.00
}
```

**Response:** `201 Created`
```json
{
  "id": 1,
  "monthDataId": 1,
  "expenseTypeId": 1,
  "value": 1200.00
}
```

---

### Update Expense
Atualizar despesa.

**Endpoint:** `PUT /expenses/{id}`  
**Auth:** ✅ Bearer Token

**Request Body:**
```json
{
  "value": 1300.00
}
```

**Response:** `200 OK`
```json
{
  "id": 1,
  "monthDataId": 1,
  "expenseTypeId": 1,
  "value": 1300.00
}
```

---

### Delete Expense
Deletar despesa.

**Endpoint:** `DELETE /expenses/{id}`  
**Auth:** ✅ Bearer Token

**Response:** `204 No Content`

---

## 🔒 Autenticação no Frontend

### Como usar o token JWT

1. **Após Login/Register**, salvar o token:
```javascript
const { token, user } = response.data;
localStorage.setItem('token', token);
localStorage.setItem('user', JSON.stringify(user));
```

2. **Em todas as requisições protegidas**, enviar o header:
```javascript
headers: {
  'Authorization': `Bearer ${token}`
}
```

3. **Exemplo com Axios:**
```javascript
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api'
});

// Interceptor para adicionar token automaticamente
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
```

4. **Exemplo com Fetch:**
```javascript
const token = localStorage.getItem('token');

fetch('http://localhost:5000/api/month-data', {
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
});
```

---

## ⚠️ Tratamento de Erros

### Status Codes Comuns

| Status | Significado | Ação |
|--------|-------------|------|
| `200` | OK | Sucesso |
| `201` | Created | Recurso criado |
| `204` | No Content | Sucesso sem retorno |
| `400` | Bad Request | Dados inválidos |
| `401` | Unauthorized | Token inválido/expirado |
| `403` | Forbidden | Sem permissão |
| `404` | Not Found | Recurso não encontrado |
| `500` | Server Error | Erro no servidor |

### Exemplo de Erro
```json
{
  "message": "Email already registered"
}
```

---

## 📝 Notas Importantes

1. **Isolamento de Dados:** Cada usuário só acessa seus próprios dados
2. **Token Expiration:** Token JWT expira em 24 horas
3. **CORS:** Configurado para `http://localhost:5173` e `http://localhost:4173`
4. **JSON Format:** Todas as propriedades em camelCase
5. **Timestamps:** Formato ISO 8601 (UTC)
6. **Decimal Values:** Sempre com 2 casas decimais

---

**Versão:** 1.0  
**Data:** 06/02/2026
