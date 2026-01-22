# 🔗 Guia de Integração Frontend-Backend

## ✅ Status da Integração

**Backend**: ✅ Rodando em `http://localhost:5150`  
**Frontend**: ✅ Rodando em `http://localhost:5173`  
**Database**: ✅ PostgreSQL local (porta 5432)

---

## 📋 O Que Foi Feito

### 1. Models Atualizados
- ✅ `Income.id`: `string` → `number`
- ✅ `Expense.id`: `string` → `number`
- ✅ `MonthData`: Adicionado `id`, `totalIncome`, `totalExpense`, `balance`

### 2. Serviços HTTP Criados
- ✅ `httpClient.ts`: Cliente HTTP base com axios
- ✅ `incomeService.ts`: CRUD de incomes
- ✅ `expenseService.ts`: CRUD de expenses
- ✅ `monthDataService.ts`: CRUD de month data + duplicate

### 3. Stores Atualizados
- ✅ `income.ts`: Agora usa API ao invés de localStorage
- ✅ `expense.ts`: Agora usa API ao invés de localStorage
- ✅ `month.ts`: Agora usa API para duplicar e limpar meses

### 4. Componentes Atualizados
- ✅ `ExpenseList.vue`: `pendingChanges` agora usa `Map<number, Expense>`

### 5. Configuração
- ✅ `.env`: `VITE_API_BASE_URL=http://localhost:5150/api`
- ✅ `.env.example`: Atualizado com a URL correta

---

## 🧪 Como Testar

### 1. Verificar Backend
```bash
curl http://localhost:5150/api/monthdata
```

**Resposta esperada**: JSON com dados do mês atual (Janeiro 2026)

### 2. Abrir Frontend
Acesse: `http://localhost:5173`

### 3. Testar Funcionalidades

#### ✅ Carregar Dados
- Ao abrir o dashboard, deve carregar os dados do backend
- Deve exibir: "Sample Salary" (R$ 4.000,00) e "Sample Rent" (R$ 1.500,00)

#### ✅ Adicionar Income
1. Clique em "Adicionar Receita"
2. Preencha os dados
3. Salve
4. Verifique se aparece na lista

#### ✅ Adicionar Expense
1. Clique em "Adicionar Despesa"
2. Preencha os dados
3. Salve
4. Verifique se aparece na lista

#### ✅ Editar Income/Expense
1. Clique no item para editar
2. Altere os dados
3. Salve
4. Verifique se foi atualizado

#### ✅ Deletar Income/Expense
1. Clique no botão de deletar
2. Confirme
3. Verifique se foi removido

#### ✅ Duplicar Mês
1. Clique em "Duplicar Mês"
2. Verifique se criou o próximo mês com os mesmos dados

#### ✅ Navegar Entre Meses
1. Use as setas para navegar
2. Verifique se carrega os dados corretos

---

## 🔍 Endpoints da API

### MonthData
- `GET /api/monthdata` - Lista todos os meses
- `GET /api/monthdata/{id}` - Busca por ID
- `GET /api/monthdata/{year}/{month}` - Busca por ano/mês
- `POST /api/monthdata` - Cria novo mês
- `DELETE /api/monthdata/{id}` - Deleta mês
- `POST /api/monthdata/duplicate` - Duplica mês

### Incomes
- `GET /api/months/{year}/{month}/incomes` - Lista incomes do mês
- `GET /api/months/{year}/{month}/incomes/{id}` - Busca income por ID
- `POST /api/months/{year}/{month}/incomes` - Cria income
- `PUT /api/months/{year}/{month}/incomes/{id}` - Atualiza income
- `DELETE /api/months/{year}/{month}/incomes/{id}` - Deleta income

### Expenses
- `GET /api/months/{year}/{month}/expenses` - Lista expenses do mês
- `GET /api/months/{year}/{month}/expenses/{id}` - Busca expense por ID
- `POST /api/months/{year}/{month}/expenses` - Cria expense
- `PUT /api/months/{year}/{month}/expenses/{id}` - Atualiza expense
- `DELETE /api/months/{year}/{month}/expenses/{id}` - Deleta expense

---

## 🐛 Troubleshooting

### Backend não inicia
```bash
cd backend
dotnet run
```

### Frontend não inicia
```bash
cd frontend
npm run dev
```

### Erro de CORS
O backend já está configurado com CORS permitindo `http://localhost:5173`

### Erro 404 ao carregar dados
Verifique se o backend está rodando e se a URL no `.env` está correta

### Dados não aparecem
1. Abra o DevTools (F12)
2. Vá na aba Network
3. Verifique se as requisições estão sendo feitas
4. Verifique se há erros no console

---

## 📝 Próximos Passos

Depois de testar a integração, você mencionou que quer fazer "uma pequena mudança". 

Quando estiver pronto, me avisa o que você quer ajustar! 🚀

---

**Versão**: 1.0  
**Data**: 22/01/2026
