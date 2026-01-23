# API Documentation - Month Balance Backend

## Base URL
```
Development: http://localhost:5150
Production: https://your-domain.com
```

## Authentication
All endpoints except `/api/auth/*` require JWT Bearer token in the Authorization header:
```
Authorization: Bearer <token>
```

---

## Endpoints

### Authentication

#### POST /api/auth/register
Register a new user.

**Request Body:**
```json
{
  "email": "string",
  "password": "string",
  "confirmPassword": "string"
}
```

**Response (200 OK):**
```json
{
  "token": "string",
  "email": "string",
  "expiresAt": "2026-01-24T00:00:00Z"
}
```

**Error Responses:**
- 400 Bad Request: Passwords don't match or email already exists

---

#### POST /api/auth/login
Login with existing user.

**Request Body:**
```json
{
  "email": "string",
  "password": "string"
}
```

**Response (200 OK):**
```json
{
  "token": "string",
  "email": "string",
  "expiresAt": "2026-01-24T00:00:00Z"
}
```

**Error Responses:**
- 401 Unauthorized: Invalid email or password

---

#### GET /api/auth/check-email/{email}
Check if email is already registered.

**Response (200 OK):**
```json
{
  "exists": true
}
```

---

### Incomes (Global)

#### GET /api/incomes
List all incomes for the authenticated user.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "description": "Salary",
    "type": 0
  }
]
```

**Income Types:**
- 0: Manual (fixed values)
- 1: Hourly (calculated from hourly rate)

---

#### GET /api/incomes/{id}
Get income by ID.

**Response (200 OK):**
```json
{
  "id": 1,
  "description": "Salary",
  "type": 0
}
```

**Error Responses:**
- 404 Not Found: Income not found

---

#### POST /api/incomes
Create a new income.

**Request Body:**
```json
{
  "description": "string",
  "type": 0
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "description": "Salary",
  "type": 0
}
```

---

#### PUT /api/incomes/{id}
Update an existing income.

**Request Body:**
```json
{
  "description": "string",
  "type": 0
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "description": "Updated Salary",
  "type": 0
}
```

**Error Responses:**
- 404 Not Found: Income not found

---

#### DELETE /api/incomes/{id}
Delete an income.

**Response (204 No Content)**

**Error Responses:**
- 400 Bad Request: Cannot delete income linked to months

---

### Expenses (Global)

#### GET /api/expenses
List all expenses for the authenticated user.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "description": "Rent"
  }
]
```

---

#### GET /api/expenses/{id}
Get expense by ID.

**Response (200 OK):**
```json
{
  "id": 1,
  "description": "Rent"
}
```

**Error Responses:**
- 404 Not Found: Expense not found

---

#### POST /api/expenses
Create a new expense.

**Request Body:**
```json
{
  "description": "string"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "description": "Rent"
}
```

---

#### PUT /api/expenses/{id}
Update an existing expense.

**Request Body:**
```json
{
  "description": "string"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "description": "Updated Rent"
}
```

**Error Responses:**
- 404 Not Found: Expense not found

---

#### DELETE /api/expenses/{id}
Delete an expense.

**Response (204 No Content)**

**Error Responses:**
- 400 Bad Request: Cannot delete expense linked to months

---

### Month Data

#### GET /api/months/{year}/{month}
Get or create month data. If the month doesn't exist, it will be created automatically.

**Response (200 OK):**
```json
{
  "id": 1,
  "year": 2026,
  "month": 1,
  "incomes": [],
  "expenses": [],
  "totalIncome": 0,
  "totalExpense": 0,
  "balance": 0
}
```

---

#### GET /api/months
List all months for the authenticated user.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "year": 2026,
    "month": 1,
    "incomes": [],
    "expenses": [],
    "totalIncome": 0,
    "totalExpense": 0,
    "balance": 0
  }
]
```

---

#### POST /api/months/duplicate
Duplicate a month. If target month exists, it will be overwritten.

**Request Body:**
```json
{
  "sourceYear": 2026,
  "sourceMonth": 1,
  "targetYear": 2026,
  "targetMonth": 2
}
```

**Response (200 OK):**
```json
{
  "id": 2,
  "year": 2026,
  "month": 2,
  "incomes": [...],
  "expenses": [...],
  "totalIncome": 4000,
  "totalExpense": 1500,
  "balance": 2500
}
```

**Error Responses:**
- 400 Bad Request: Source month not found

---

#### DELETE /api/months/{year}/{month}
Delete a month and all its linked incomes and expenses.

**Response (204 No Content)**

---

### Month Incomes

#### GET /api/months/{year}/{month}/incomes
List all incomes for a specific month.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "incomeId": 1,
    "incomeDescription": "Salary",
    "incomeType": 0,
    "grossValue": 5000,
    "netValue": 4000,
    "hourlyRate": null,
    "hours": null,
    "minutes": null
  },
  {
    "id": 2,
    "incomeId": 2,
    "incomeDescription": "Freelance",
    "incomeType": 1,
    "grossValue": null,
    "netValue": null,
    "hourlyRate": 50,
    "hours": 40,
    "minutes": 30
  }
]
```

---

#### POST /api/months/{year}/{month}/incomes
Add an income to a specific month.

**Request Body (Manual Income - Type 0):**
```json
{
  "incomeId": 1,
  "grossValue": 5000,
  "netValue": 4000,
  "hourlyRate": null,
  "hours": null,
  "minutes": null
}
```

**Request Body (Hourly Income - Type 1):**
```json
{
  "incomeId": 2,
  "grossValue": null,
  "netValue": null,
  "hourlyRate": 50,
  "hours": 40,
  "minutes": 30
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "incomeId": 1,
  "incomeDescription": "Salary",
  "incomeType": 0,
  "grossValue": 5000,
  "netValue": 4000,
  "hourlyRate": null,
  "hours": null,
  "minutes": null
}
```

**Error Responses:**
- 400 Bad Request: Income not found or already added to this month

---

#### PUT /api/months/{year}/{month}/incomes/{id}
Update income values for a specific month.

**Request Body:**
```json
{
  "grossValue": 5500,
  "netValue": 4400,
  "hourlyRate": null,
  "hours": null,
  "minutes": null
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "incomeId": 1,
  "incomeDescription": "Salary",
  "incomeType": 0,
  "grossValue": 5500,
  "netValue": 4400,
  "hourlyRate": null,
  "hours": null,
  "minutes": null
}
```

**Error Responses:**
- 404 Not Found: Month income not found

---

#### DELETE /api/months/{year}/{month}/incomes/{id}
Remove an income from a specific month.

**Response (204 No Content)**

---

### Month Expenses

#### GET /api/months/{year}/{month}/expenses
List all expenses for a specific month.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "expenseId": 1,
    "expenseDescription": "Rent",
    "value": 1500
  }
]
```

---

#### POST /api/months/{year}/{month}/expenses
Add an expense to a specific month.

**Request Body:**
```json
{
  "expenseId": 1,
  "value": 1500
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "expenseId": 1,
  "expenseDescription": "Rent",
  "value": 1500
}
```

**Error Responses:**
- 400 Bad Request: Expense not found or already added to this month

---

#### PUT /api/months/{year}/{month}/expenses/{id}
Update expense value for a specific month.

**Request Body:**
```json
{
  "value": 1600
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "expenseId": 1,
  "expenseDescription": "Rent",
  "value": 1600
}
```

**Error Responses:**
- 404 Not Found: Month expense not found

---

#### DELETE /api/months/{year}/{month}/expenses/{id}
Remove an expense from a specific month.

**Response (204 No Content)**

---

## Data Models

### IncomeTypeEnum
```typescript
enum IncomeTypeEnum {
  Manual = 0,
  Hourly = 1
}
```

### AuthResponseDto
```typescript
interface AuthResponseDto {
  token: string;
  email: string;
  expiresAt: string; // ISO 8601 date
}
```

### IncomeDto
```typescript
interface IncomeDto {
  id: number;
  description: string;
  type: IncomeTypeEnum;
}
```

### ExpenseDto
```typescript
interface ExpenseDto {
  id: number;
  description: string;
}
```

### MonthIncomeDto
```typescript
interface MonthIncomeDto {
  id: number;
  incomeId: number;
  incomeDescription: string;
  incomeType: IncomeTypeEnum;
  grossValue: number | null;
  netValue: number | null;
  hourlyRate: number | null;
  hours: number | null;
  minutes: number | null;
}
```

### MonthExpenseDto
```typescript
interface MonthExpenseDto {
  id: number;
  expenseId: number;
  expenseDescription: string;
  value: number;
}
```

### MonthDataDto
```typescript
interface MonthDataDto {
  id: number;
  year: number;
  month: number;
  incomes: MonthIncomeDto[];
  expenses: MonthExpenseDto[];
  totalIncome: number;
  totalExpense: number;
  balance: number;
}
```

---

## Error Responses

All error responses follow this format:

```json
{
  "message": "Error description"
}
```

### Common HTTP Status Codes
- 200 OK: Request successful
- 201 Created: Resource created successfully
- 204 No Content: Request successful, no content to return
- 400 Bad Request: Invalid request data
- 401 Unauthorized: Missing or invalid authentication token
- 404 Not Found: Resource not found
- 500 Internal Server Error: Server error

---

## CORS Configuration

The API accepts requests from:
- http://localhost:5173 (Development)

For production, update CORS settings in the backend configuration.

---

## Notes

1. All monetary values are decimal numbers with 2 decimal places
2. Dates are in ISO 8601 format (YYYY-MM-DDTHH:mm:ssZ)
3. JWT tokens expire after 24 hours
4. Month numbers are 1-12 (January = 1, December = 12)
5. When creating a month income/expense, the month is automatically created if it doesn't exist
6. Deleting a month cascades to all its incomes and expenses
7. Cannot delete global incomes/expenses that are linked to any month
8. Duplicating a month overwrites the target month if it exists
