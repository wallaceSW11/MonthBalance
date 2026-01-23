# 🔄 Plano de Refatoração - Month Balance Frontend

## 📋 Resumo das Mudanças

### Backend (Já Implementado)
- ✅ Autenticação com JWT (email + senha)
- ✅ Receitas e Despesas agora são **cadastros globais** (templates)
- ✅ Cada mês tem **vínculos** para receitas/despesas com valores específicos
- ✅ Banco de dados PostgreSQL configurado

### Frontend (A Implementar)
- 🔄 Implementar autenticação (login/registro)
- 🔄 Refatorar receitas/despesas para modelo global + vínculo mensal
- 🔄 Remover todo código de LocalStorage
- 🔄 Manter layout atual do dashboard

---

## 🎯 Fluxo de Trabalho (UX)

### Receitas
1. Usuário clica no botão `+` flutuante na seção de receitas
2. Abre dialog com **lista de receitas globais** cadastradas
3. Usuário seleciona uma receita da lista
4. Abre form com campos específicos:
   - **Manual**: Valor Bruto + Valor Líquido
   - **Hourly**: Valor/hora + Horas + Minutos trabalhados
5. Ao salvar, cria vínculo mensal e exibe na lista
6. **Edição**: Click no item da lista → abre form para editar valores

### Despesas
1. Usuário clica no botão `+` flutuante na seção de despesas
2. Abre dialog com **lista de despesas globais** cadastradas
3. Usuário seleciona uma despesa da lista
4. Abre form com campo de **valor**
5. Ao salvar, cria vínculo mensal e exibe na lista
6. **Edição**: Click no item da lista → abre form para editar valor

### Cadastros Globais
- Menu já existe: "Receitas" e "Despesas"
- Essas telas vão gerenciar os cadastros globais (CRUD)
- Receita global: description + type (Manual/Hourly)
- Despesa global: description

---

## 🚀 Fases de Implementação

### **Fase 1: Autenticação** 🔐

#### 1.1 Models
- [ ] Criar `src/models/Auth.ts`
  ```typescript
  interface AuthResponse {
    token: string
    email: string
    expiresAt: string
  }
  
  interface LoginRequest {
    email: string
    password: string
  }
  
  interface RegisterRequest {
    email: string
    password: string
    confirmPassword: string
  }
  ```

#### 1.2 Service
- [ ] Criar `src/services/api/authService.ts`
  - `login(email, password)`
  - `register(email, password, confirmPassword)`
  - `checkEmail(email)`
  - `logout()`

#### 1.3 Store
- [ ] Criar `src/stores/auth.ts`
  - State: `token`, `email`, `authenticated`
  - Actions: `login`, `register`, `logout`, `checkAuth`
  - Persistir token no localStorage

#### 1.4 HTTP Client
- [ ] Atualizar `src/services/api/httpClient.ts`
  - Adicionar interceptor para incluir JWT token
  - Interceptor de erro 401 → logout automático

#### 1.5 Views
- [ ] Criar `src/views/LoginView.vue`
  - Form: email + password
  - Link para registro
- [ ] Criar `src/views/RegisterView.vue`
  - Form: email + password + confirmPassword
  - Validação: senhas iguais
  - Link para login

#### 1.6 Router
- [ ] Atualizar `src/router/index.ts`
  - Adicionar rotas `/login` e `/register`
  - Criar `beforeEach` guard: redirecionar para login se não autenticado
  - Rotas públicas: `/login`, `/register`
  - Rotas protegidas: todas as outras

---

### **Fase 2: Models e Services Base** 📦

#### 2.1 Atualizar Models Existentes
- [ ] Atualizar `src/models/Income.ts`
  ```typescript
  // Global Income (template)
  interface Income {
    id: number
    description: string
    type: IncomeTypeEnum // 0: Manual, 1: Hourly
  }
  
  enum IncomeTypeEnum {
    Manual = 0,
    Hourly = 1
  }
  ```

- [ ] Atualizar `src/models/Expense.ts`
  ```typescript
  // Global Expense (template)
  interface Expense {
    id: number
    description: string
  }
  ```

#### 2.2 Criar Novos Models (Vínculos Mensais)
- [ ] Criar `src/models/MonthIncome.ts`
  ```typescript
  interface MonthIncome {
    id: number
    incomeId: number
    incomeDescription: string
    incomeType: IncomeTypeEnum
    grossValue: number | null
    netValue: number | null
    hourlyRate: number | null
    hours: number | null
    minutes: number | null
  }
  ```

- [ ] Criar `src/models/MonthExpense.ts`
  ```typescript
  interface MonthExpense {
    id: number
    expenseId: number
    expenseDescription: string
    value: number
  }
  ```

- [ ] Atualizar `src/models/MonthData.ts`
  ```typescript
  interface MonthData {
    id: number
    year: number
    month: number
    incomes: MonthIncome[]
    expenses: MonthExpense[]
    totalIncome: number
    totalExpense: number
    balance: number
  }
  ```

#### 2.3 Services - Receitas/Despesas Globais
- [ ] Criar `src/services/api/incomeService.ts`
  - `getAll()` → Income[]
  - `getById(id)` → Income
  - `create(description, type)` → Income
  - `update(id, description, type)` → Income
  - `delete(id)` → void

- [ ] Criar `src/services/api/expenseService.ts`
  - `getAll()` → Expense[]
  - `getById(id)` → Expense
  - `create(description)` → Expense
  - `update(id, description)` → Expense
  - `delete(id)` → void

#### 2.4 Services - Vínculos Mensais
- [ ] Criar `src/services/api/monthIncomeService.ts`
  - `getByMonth(year, month)` → MonthIncome[]
  - `create(year, month, data)` → MonthIncome
  - `update(year, month, id, data)` → MonthIncome
  - `delete(year, month, id)` → void

- [ ] Criar `src/services/api/monthExpenseService.ts`
  - `getByMonth(year, month)` → MonthExpense[]
  - `create(year, month, data)` → MonthExpense
  - `update(year, month, id, data)` → MonthExpense
  - `delete(year, month, id)` → void

- [ ] Atualizar `src/services/api/monthDataService.ts`
  - `getOrCreate(year, month)` → MonthData
  - `getAll()` → MonthData[]
  - `duplicate(sourceYear, sourceMonth, targetYear, targetMonth)` → MonthData
  - `delete(year, month)` → void

#### 2.5 Remover LocalStorage
- [ ] Deletar `src/services/storage/ExpenseStorageService.ts`
- [ ] Deletar `src/services/storage/IncomeStorageService.ts`
- [ ] Deletar `src/services/storage/SettingsStorageService.ts`
- [ ] Deletar `src/services/storage/StorageService.ts`
- [ ] Deletar `src/services/storage/UIStorageService.ts`
- [ ] Deletar pasta `src/services/storage/`

---

### **Fase 3: Stores** 🏪

#### 3.1 Stores de Cadastros Globais
- [ ] Criar `src/stores/incomeGlobal.ts`
  - State: `incomes: Income[]`, `loading`
  - Actions: `loadIncomes`, `createIncome`, `updateIncome`, `deleteIncome`

- [ ] Criar `src/stores/expenseGlobal.ts`
  - State: `expenses: Expense[]`, `loading`
  - Actions: `loadExpenses`, `createExpense`, `updateExpense`, `deleteExpense`

#### 3.2 Refatorar Stores de Vínculos Mensais
- [ ] Refatorar `src/stores/income.ts`
  - State: `monthIncomes: MonthIncome[]`, `loading`
  - Actions: `loadMonthIncomes`, `createMonthIncome`, `updateMonthIncome`, `deleteMonthIncome`
  - Computed: `totalIncome` (soma dos netValue)

- [ ] Refatorar `src/stores/expense.ts`
  - State: `monthExpenses: MonthExpense[]`, `loading`
  - Actions: `loadMonthExpenses`, `createMonthExpense`, `updateMonthExpense`, `deleteMonthExpense`
  - Computed: `totalExpense` (soma dos value)

#### 3.3 Atualizar Store de Mês
- [ ] Atualizar `src/stores/month.ts`
  - State: `currentYear`, `currentMonth`, `monthData: MonthData | null`
  - Actions: `loadMonthData`, `duplicateMonth`, `deleteMonth`
  - Integrar com stores de income e expense

---

### **Fase 4: Views de Cadastro Global** 📝

#### 4.1 View de Receitas Globais
- [ ] Criar `src/views/IncomesGlobalView.vue`
  - Lista de receitas globais (tabela ou cards)
  - Botão `+` para adicionar
  - Ações: Editar, Deletar
  - Validação: não deletar se tiver vínculo (mensagem amigável)

- [ ] Criar `src/components/IncomeGlobalFormDialog.vue`
  - Form: description + type (Manual/Hourly)
  - Validações

#### 4.2 View de Despesas Globais
- [ ] Criar `src/views/ExpensesGlobalView.vue`
  - Lista de despesas globais (tabela ou cards)
  - Botão `+` para adicionar
  - Ações: Editar, Deletar
  - Validação: não deletar se tiver vínculo (mensagem amigável)

- [ ] Criar `src/components/ExpenseGlobalFormDialog.vue`
  - Form: description
  - Validações

#### 4.3 Atualizar Router
- [ ] Atualizar `src/router/index.ts`
  - Rota `/incomes` → `IncomesGlobalView`
  - Rota `/expenses` → `ExpensesGlobalView`

#### 4.4 Atualizar Menu
- [ ] Verificar `src/components/NavigationDrawer.vue`
  - Menu "Receitas" → `/incomes`
  - Menu "Despesas" → `/expenses`

---

### **Fase 5: Refatorar Dashboard (Fluxo Principal)** 🎨

#### 5.1 Componente de Lista de Receitas
- [ ] Refatorar `src/components/IncomeList.vue`
  - Exibir `MonthIncome[]` (não `Income[]`)
  - Mostrar `incomeDescription` e valores
  - Botão `+` → abre `IncomeSelectionDialog`
  - Click no item → abre `IncomeFormDialog` (modo edição)
  - Deletar vínculo mensal (não receita global)

- [ ] Criar `src/components/IncomeSelectionDialog.vue`
  - Lista de receitas globais disponíveis
  - Filtrar receitas já vinculadas ao mês
  - Ao selecionar → abre `IncomeFormDialog` (modo criação)

- [ ] Refatorar `src/components/IncomeFormDialog.vue`
  - Props: `monthIncomeId` (null = criar, number = editar)
  - Se criar: dropdown de receitas globais
  - Campos condicionais por tipo:
    - **Manual**: Valor Bruto + Valor Líquido
    - **Hourly**: Valor/hora + Horas + Minutos
  - Validações
  - Salvar → criar/atualizar vínculo mensal

#### 5.2 Componente de Lista de Despesas
- [ ] Refatorar `src/components/ExpenseList.vue`
  - Exibir `MonthExpense[]` (não `Expense[]`)
  - Mostrar `expenseDescription` e valor
  - Botão `+` → abre `ExpenseSelectionDialog`
  - Click no item → abre `ExpenseFormDialog` (modo edição)
  - Deletar vínculo mensal (não despesa global)

- [ ] Criar `src/components/ExpenseSelectionDialog.vue`
  - Lista de despesas globais disponíveis
  - Filtrar despesas já vinculadas ao mês
  - Ao selecionar → abre `ExpenseFormDialog` (modo criação)

- [ ] Refatorar `src/components/ExpenseFormDialog.vue`
  - Props: `monthExpenseId` (null = criar, number = editar)
  - Se criar: dropdown de despesas globais
  - Campo: Valor
  - Validações
  - Salvar → criar/atualizar vínculo mensal

#### 5.3 Componente de Resumo
- [ ] Atualizar `src/components/MonthSummary.vue`
  - Total de receitas: soma dos `netValue` (não `grossValue`)
  - Total de despesas: soma dos `value`
  - Saldo: totalIncome - totalExpense

#### 5.4 Navegação de Meses
- [ ] Verificar `src/components/MonthNavigation.vue`
  - Ao mudar mês: carregar vínculos mensais
  - Duplicar mês: usar novo endpoint

- [ ] Atualizar `src/components/DuplicateMonthDialog.vue`
  - Usar `monthDataService.duplicate()`

#### 5.5 Dashboard
- [ ] Verificar `src/views/DashboardView.vue`
  - Carregar dados do mês atual ao montar
  - Integrar com stores refatoradas

---

### **Fase 6: Ajustes Finais** ✅

#### 6.1 Limpeza de Código
- [ ] Remover imports de services de storage
- [ ] Remover código legado de localStorage
- [ ] Verificar todos os componentes

#### 6.2 Validações e Mensagens
- [ ] Mensagens de erro amigáveis
- [ ] Validações de formulários
- [ ] Loading states
- [ ] Toasts de sucesso/erro

#### 6.3 Testes Básicos
- [ ] Testar fluxo de autenticação
- [ ] Testar CRUD de receitas/despesas globais
- [ ] Testar vínculo mensal (criar/editar/deletar)
- [ ] Testar navegação entre meses
- [ ] Testar duplicação de mês
- [ ] Testar cálculos de totais

#### 6.4 Documentação
- [ ] Atualizar README.md
- [ ] Atualizar QUICK_START.md
- [ ] Documentar novos fluxos

---

## 📊 Progresso

- [x] Fase 1: Autenticação (100%)
- [x] Fase 2: Models e Services (100%)
- [x] Fase 3: Stores (100%)
- [x] Fase 4: Views Globais (100%)
- [x] Fase 5: Dashboard (100%)
- [ ] Fase 6: Ajustes Finais (0%)

---

## 🎯 Decisões Confirmadas

1. ✅ Manter layout atual do dashboard
2. ✅ Menu de "Receitas" e "Despesas" já existe
3. ✅ Validar exclusão de receita/despesa global se tiver vínculo
4. ✅ Após login, redirecionar para dashboard do mês atual
5. ✅ Remover todo código de LocalStorage
6. ✅ Começar do zero (sem migração de dados)

---

## 🔗 Referências

- API Documentation: `API_DOCUMENTATION.md`
- Code Style: `.kiro/steering/1_code-style.md`
- Project Reference: `.kiro/steering/3_project-reference.md`
- Testing Guide: `.kiro/steering/7_testing-best-practices.md`

---

**Data de Criação:** 22/01/2026  
**Última Atualização:** 22/01/2026  
**Status:** 🟡 Planejamento Concluído
