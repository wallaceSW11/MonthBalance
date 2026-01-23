# 📚 Referência do Projeto - Month Balance Frontend

## 🎯 Sobre o Projeto

Aplicação web para controle financeiro pessoal mensal. Permite gerenciar receitas e despesas por mês, com cálculo automático de saldo, duplicação de meses e navegação temporal.

---

## 🛠️ Stack Técnico

### Core
- **Vue.js**: 3.5.13 (Composition API)
- **Pinia**: 2.3.0 (State Management)
- **Vue Router**: 4.5.0
- **Vuetify**: 3.7.5 (UI Framework - Material Design)

### Build & Dev
- **Vite**: 6.3.5 (Build Tool)
- **Vitest**: 3.2.4 (Test Runner)
- **TypeScript**: 5.7.3

### Testing
- **Vitest**: 3.2.4
- **@vue/test-utils**: 2.4.6
- **jsdom**: 26.1.0
- **happy-dom**: 16.10.1

### HTTP & API
- **Axios**: 1.7.9

### Utilities
- **uuid**: 11.0.5
- **@wallacesw11/base-lib**: Biblioteca de componentes base (ModalBase, IconToolTip, Confirm, etc)

### UI & Icons
- **Material Design Icons**: Incluído no Vuetify
- **@mdi/font**: 7.4.47

---

## 📜 Scripts

### Desenvolvimento
```bash
npm run dev          # Servidor desenvolvimento (http://localhost:5173)
```

### Build
```bash
npm run build        # Build produção
npm run preview      # Preview do build
```

### Testes
```bash
npm run test         # Todos os testes
npm run test:ui      # UI de testes (Vitest UI)
```

### Qualidade
```bash
npm run lint         # ESLint
npm run type-check   # TypeScript check
```

---

## 📂 Estrutura do Projeto

```
frontend/
├── src/
│   ├── components/       # Componentes Vue reutilizáveis
│   │   ├── DuplicateMonthDialog.vue
│   │   ├── ExpenseFormDialog.vue
│   │   ├── ExpenseList.vue
│   │   ├── IncomeFormDialog.vue
│   │   ├── IncomeList.vue
│   │   └── MonthNavigation.vue
│   ├── composables/      # Composables Vue (lógica reutilizável)
│   ├── locales/          # Arquivos de tradução (i18n)
│   ├── models/           # Interfaces TypeScript
│   │   ├── Expense.ts
│   │   ├── Income.ts
│   │   └── MonthData.ts
│   ├── plugins/          # Plugins Vue
│   │   ├── i18n.ts
│   │   ├── index.ts
│   │   └── vuetify.ts
│   ├── router/           # Configuração de rotas
│   │   └── index.ts
│   ├── services/         # Serviços (API, Storage)
│   │   ├── api/          # Serviços HTTP
│   │   │   ├── expenseService.ts
│   │   │   ├── httpClient.ts
│   │   │   ├── incomeService.ts
│   │   │   └── monthDataService.ts
│   │   └── storage/      # LocalStorage (legado)
│   ├── stores/           # Pinia stores
│   │   ├── expense.ts
│   │   ├── income.ts
│   │   └── month.ts
│   ├── styles/           # Estilos globais
│   ├── utils/            # Funções utilitárias
│   │   ├── currency.ts
│   │   └── uuid.ts
│   ├── views/            # Páginas/Views
│   │   ├── DashboardView.vue
│   │   ├── ExpensesView.vue
│   │   └── IncomesView.vue
│   ├── App.vue           # Componente raiz
│   └── main.ts           # Entry point
├── public/               # Arquivos públicos estáticos
├── .env                  # Variáveis de ambiente
├── vite.config.ts        # Configuração Vite
└── vitest.config.ts      # Configuração Vitest
```

---

## 🔧 Convenções de Nomenclatura

### Arquivos
| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Componentes Vue | PascalCase.vue | `ExpenseList.vue` |
| Views | PascalCase.vue | `DashboardView.vue` |
| Services | camelCase.ts | `expenseService.ts` |
| Models | PascalCase.ts | `Income.ts` |
| Stores | camelCase.ts | `expense.ts` |
| Utils | camelCase.ts | `currency.ts` |
| Composables | camelCase.ts | `useMonthData.ts` |
| Testes | ComponentName.spec.ts | `ExpenseList.spec.ts` |

### Código
| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Variáveis | camelCase | `totalIncome`, `isLoading` |
| Funções | camelCase | `calculateTotal()` |
| Interfaces | PascalCase | `Income`, `MonthData` |
| Types | PascalCase | `IncomeType` |
| Constantes | UPPER_SNAKE_CASE | `API_BASE_URL` |
| Componentes (template) | PascalCase | `<ExpenseList />` |

---

## 🎯 Regras de Organização

### 1. Máximo 2 Níveis de Aninhamento
```
✅ CORRETO: src/components/ExpenseList.vue
✅ CORRETO: src/services/api/expenseService.ts
❌ ERRADO: src/components/expenses/list/ExpenseList.vue
```

### 2. Testes Co-localizados
```
✅ CORRETO:
src/components/
├── ExpenseList.vue
└── ExpenseList.spec.ts
```

### 3. Imports com Alias `@/`
```typescript
// ✅ CORRETO
import { expenseService } from '@/services/api/expenseService'

// ❌ ERRADO
import { expenseService } from '../../../services/api/expenseService'
```

---

## 🏗️ Módulos Principais

### Dashboard
Visão geral do mês atual com receitas, despesas e saldo.

**Componentes:** `DashboardView.vue`, `IncomeList.vue`, `ExpenseList.vue`, `MonthNavigation.vue`  
**Stores:** `month.ts`, `income.ts`, `expense.ts`

### Receitas (Incomes)
Gerenciamento de receitas mensais (manual ou por hora).

**Componentes:** `IncomesView.vue`, `IncomeFormDialog.vue`  
**Services:** `incomeService.ts`  
**Models:** `Income.ts`

### Despesas (Expenses)
Gerenciamento de despesas mensais.

**Componentes:** `ExpensesView.vue`, `ExpenseFormDialog.vue`  
**Services:** `expenseService.ts`  
**Models:** `Expense.ts`

### Navegação de Meses
Navegação entre meses, duplicação e limpeza.

**Componentes:** `MonthNavigation.vue`, `DuplicateMonthDialog.vue`  
**Services:** `monthDataService.ts`  
**Models:** `MonthData.ts`

---

## 🎓 Glossário

- **Income**: Receita mensal (salário, freelance, etc)
- **Expense**: Despesa mensal (aluguel, contas, etc)
- **MonthData**: Dados de um mês específico (ano + mês + receitas + despesas)
- **Balance**: Saldo (receitas - despesas)
- **Duplicate**: Copiar dados de um mês para outro

---

## 🔄 Fluxo de Dados

### Composition API + Pinia
```typescript
// Store (Pinia)
export const useIncomeStore = defineStore('income', () => {
  const incomes = ref<Income[]>([])
  
  async function loadIncomes(year: number, month: number) {
    incomes.value = await incomeService.getByMonth(year, month)
  }
  
  return { incomes, loadIncomes }
})

// Component
const incomeStore = useIncomeStore()
await incomeStore.loadIncomes(2026, 1)
```

### HTTP Client (Axios)
```typescript
// Service
export const incomeService = {
  async getByMonth(year: number, month: number): Promise<Income[]> {
    const response = await httpClient.get(`/months/${year}/${month}/incomes`)
    return response.data
  }
}
```

---

## ⚠️ Notas Importantes

### Versões
- Vue 3.5+ (Composition API)
- Vuetify 3.7+ (Material Design 3)
- TypeScript 5.7+
- Vite 6.3+

### Backend Integration
- API Base URL: `http://localhost:5150/api`
- Configurado via `.env`: `VITE_API_BASE_URL`
- Axios interceptor para tratamento de erros

### Estado
- Pinia para state management
- Stores separadas por domínio (income, expense, month)
- Composition API com `ref` e `computed`

---

**Versão:** 1.0  
**Data:** 22/01/2026
