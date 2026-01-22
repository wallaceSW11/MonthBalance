# 📊 Comparação Frontend (LocalStorage) vs Backend (PostgreSQL)

## ✅ O Que Temos

### LocalStorage (Antigo)
```javascript
// monthbalance:incomes
{
  "2026-01": [
    { id: "uuid", name: "Salary", type: "manual", netValue: 5000, ... }
  ],
  "2026-02": [...]
}

// monthbalance:expenses
{
  "2026-01": [
    { id: "uuid", name: "Rent", value: 1500 }
  ],
  "2026-02": [...]
}
```

### Backend (Novo)
```sql
-- MonthData (Entidade Pai)
| id | year | month | created_at | updated_at |
|----|------|-------|------------|------------|
| 1  | 2026 | 1     | ...        | ...        |

-- Incomes (Filhos de MonthData)
| id | name   | type   | net_value | month_data_id |
|----|--------|--------|-----------|---------------|
| 1  | Salary | manual | 5000      | 1             |

-- Expenses (Filhos de MonthData)
| id | name | value | month_data_id |
|----|------|-------|---------------|
| 1  | Rent | 1500  | 1             |
```

---

## 🔄 Funcionalidades Migradas

### ✅ CRUD de Incomes
- **Create**: `POST /api/months/{year}/{month}/incomes`
- **Read**: `GET /api/months/{year}/{month}/incomes`
- **Update**: `PUT /api/months/{year}/{month}/incomes/{id}`
- **Delete**: `DELETE /api/months/{year}/{month}/incomes/{id}`

### ✅ CRUD de Expenses
- **Create**: `POST /api/months/{year}/{month}/expenses`
- **Read**: `GET /api/months/{year}/{month}/expenses`
- **Update**: `PUT /api/months/{year}/{month}/expenses/{id}`
- **Delete**: `DELETE /api/months/{year}/{month}/expenses/{id}`

### ✅ Gerenciamento de Meses
- **Criar mês**: `POST /api/monthdata` (automático ao adicionar income/expense)
- **Buscar mês**: `GET /api/monthdata/{year}/{month}`
- **Listar todos**: `GET /api/monthdata`
- **Deletar mês**: `DELETE /api/monthdata/{id}`
- **Duplicar mês**: `POST /api/monthdata/duplicate`

### ✅ Cálculos Automáticos
- **Total Income**: Calculado no backend
- **Total Expense**: Calculado no backend
- **Balance**: Calculado no backend (totalIncome - totalExpense)

---

## 🆕 Melhorias do Backend

### 1. Integridade Referencial
- Incomes e Expenses sempre pertencem a um MonthData
- Deletar MonthData deleta automaticamente todos os Incomes e Expenses (CASCADE)

### 2. Validações
- Não permite criar mês duplicado (unique constraint em Year + Month)
- Validações de dados obrigatórios

### 3. Timestamps
- `CreatedAt` e `UpdatedAt` automáticos em todas as entidades

### 4. Tipos de Income
- **Manual**: `grossValue`, `netValue`
- **Hourly**: `hourlyRate`, `hours`, `minutes`

---

## 🔍 Diferenças Importantes

### IDs
- **Antes**: UUIDs (strings) gerados no frontend
- **Agora**: INTs (numbers) gerados pelo PostgreSQL

### Estrutura
- **Antes**: Flat (incomes e expenses separados por mês)
- **Agora**: Relacional (MonthData → Incomes/Expenses)

### Duplicar Mês
- **Antes**: Frontend copiava arrays e gerava novos UUIDs
- **Agora**: Backend copia registros e gera novos IDs automaticamente

---

## ⚠️ Comportamento Atual

### Criar Mês Automaticamente
Quando você adiciona um income ou expense, o frontend verifica se o mês existe:
- Se **não existir**: Cria o mês vazio primeiro
- Se **existir**: Adiciona direto

### Duplicar Mês
Quando você duplica um mês:
- Backend copia todos os incomes e expenses
- Gera novos IDs automaticamente
- **Requer que o mês de origem exista**

### Navegar Entre Meses
- Verifica se o mês existe
- Se não existir, pergunta se quer duplicar
- Se clicar "Não", cria mês vazio

---

## 🐛 Problema Atual

**Erro ao duplicar**: Você está tentando duplicar de janeiro, mas janeiro ainda não foi criado no banco (banco foi resetado).

**Solução**: 
1. Crie alguns incomes/expenses em janeiro primeiro
2. Depois tente duplicar para fevereiro

---

## 📝 Resumo

**Tudo que o localStorage fazia, o backend faz melhor:**
- ✅ CRUD completo de Incomes e Expenses
- ✅ Gerenciamento de meses
- ✅ Duplicar meses
- ✅ Cálculos automáticos
- ✅ Validações
- ✅ Integridade de dados
- ✅ Timestamps automáticos

**Única diferença**: Agora é relacional e mais robusto! 🚀

---

**Versão:** 1.0  
**Data:** 22/01/2026
