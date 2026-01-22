# 🎨 Code Style - Month Balance Frontend

## ⚠️ ESTAS REGRAS SÃO LEI - NUNCA VIOLE

---

## 🔴 Regra #1: Early Return SEMPRE

```typescript
// ✅ CERTO
async function processIncome(income: Income | null) {
  if (!income) return null
  
  if (!income.value) return null
  
  const result = await calculateTotal(income)
  
  return result
}

// ❌ ERRADO
async function processIncome(income: Income | null) {
  if (income) {
    if (income.value) {
      const result = await calculateTotal(income)
      return result
    }
  }
  return null
}
```

---

## 🔴 Regra #2: Ternário para 2 Caminhos

```typescript
// ✅ CERTO
const message = isActive ? 'Ativo' : 'Inativo'
const value = hasPermission ? processData() : null

// ❌ ERRADO
let message
if (isActive) {
  message = 'Ativo'
} else {
  message = 'Inativo'
}
```

---

## 🔴 Regra #3: Async/Await SEMPRE

```typescript
// ✅ CERTO
try {
  const data = await api.getData()
  processData(data)
} catch (error) {
  console.error(error)
}

// ❌ ERRADO
api.getData()
  .then(data => processData(data))
  .catch(error => console.error(error))
```

---

## 🔴 Regra #4: Optional Chaining

```typescript
// ✅ CERTO
const value = user?.profile?.name
const total = income?.netValue ?? 0

// ❌ ERRADO
const value = user && user.profile && user.profile.name
const total = income && income.netValue ? income.netValue : 0
```

---

## 🔴 Regra #5: ZERO Lógica no Template

```vue
<!-- ❌ ERRADO -->
<template>
  <div>{{ user.name ? user.name.toUpperCase() : 'Sem nome' }}</div>
  <span>{{ items.filter(i => i.active).length }}</span>
</template>

<!-- ✅ CERTO -->
<template>
  <div>{{ displayName }}</div>
  <span>{{ activeItemsCount }}</span>
</template>

<script setup lang="ts">
const displayName = computed(() => 
  user.value?.name?.toUpperCase() ?? 'Sem nome'
)

const activeItemsCount = computed(() => 
  items.value.filter(i => i.active).length
)
</script>
```

---

## 🔴 Regra #6: Componentes em PascalCase

```vue
<!-- ✅ CERTO -->
<ExpenseList />
<IncomeFormDialog />

<!-- ❌ ERRADO -->
<expense-list />
<income-form-dialog />
```

---

## 🔴 Regra #7: Props Booleanas sem Valor

```vue
<!-- ✅ CERTO -->
<v-btn disabled />
<v-dialog persistent />

<!-- ❌ ERRADO -->
<v-btn :disabled="true" />
<v-dialog :persistent="true" />
```

---

## 🔴 Regra #8: Imports Organizados

```typescript
// Services
import { incomeService } from '@/services/api/incomeService'
import { monthDataService } from '@/services/api/monthDataService'

// Models
import type { Income } from '@/models/Income'
import type { MonthData } from '@/models/MonthData'

// Stores
import { useIncomeStore } from '@/stores/income'

// Components
import ExpenseList from '@/components/ExpenseList.vue'

// Utils
import { formatCurrency } from '@/utils/currency'
```

---

## 🔴 Regra #9: Variáveis Booleanas - Formato de Pergunta

```typescript
// ✅ CERTO
const loading = ref(false)
const userActive = ref(true)
const dataValid = ref(false)

// ❌ ERRADO
const isLoading = ref(false)
const ehUsuarioAtivo = ref(true)
const temDados = ref(false)
```

---

## 🔴 Regra #10: Const/Let (NUNCA Var)

```typescript
// ✅ CERTO
const API_URL = 'http://localhost:5150'
let counter = 0

// ❌ ERRADO
var API_URL = 'http://localhost:5150'
var counter = 0
```

---

## 🔴 Regra #11: Sem Comentários no Código

```typescript
// ❌ ERRADO
// Calcula o total de receitas
const total = incomes.reduce((sum, i) => sum + i.value, 0)

// ✅ CERTO
const calculateTotalIncome = (incomes: Income[]) =>
  incomes.reduce((sum, income) => sum + income.value, 0)

const total = calculateTotalIncome(incomes)
```

---

## 🔴 Regra #12: Composition API - Ordem

```vue
<script setup lang="ts">
// 1. Imports
import { ref, computed, onMounted } from 'vue'
import { useIncomeStore } from '@/stores/income'

// 2. Props & Emits
const props = defineProps<{
  year: number
  month: number
}>()

const emit = defineEmits<{
  save: [income: Income]
}>()

// 3. Stores
const incomeStore = useIncomeStore()

// 4. Refs
const loading = ref(false)
const incomes = ref<Income[]>([])

// 5. Computed
const totalIncome = computed(() => 
  incomes.value.reduce((sum, i) => sum + i.value, 0)
)

// 6. Functions
async function loadIncomes() {
  loading.value = true
  
  try {
    incomes.value = await incomeService.getByMonth(props.year, props.month)
  } finally {
    loading.value = false
  }
}

// 7. Lifecycle
onMounted(() => {
  loadIncomes()
})
</script>
```

---

## 🔴 Regra #13: TypeScript - Sempre Tipar

```typescript
// ✅ CERTO
const incomes = ref<Income[]>([])
const loading = ref<boolean>(false)

async function loadIncomes(year: number, month: number): Promise<void> {
  // ...
}

// ❌ ERRADO
const incomes = ref([])
const loading = ref(false)

async function loadIncomes(year, month) {
  // ...
}
```

---

## 🔴 Regra #14: Vuetify - Grid System

```vue
<!-- ✅ CERTO -->
<v-row>
  <v-col cols="12" md="6">
    <v-text-field />
  </v-col>
  <v-col cols="12" md="6">
    <v-text-field />
  </v-col>
</v-row>

<!-- ❌ ERRADO -->
<v-row>
  <v-col :cols="isMobile ? 12 : 6">
    <v-text-field />
  </v-col>
</v-row>
```

---

## 🔴 Regra #15: Sem !important

```vue
<!-- ❌ ERRADO -->
<style scoped>
.my-class {
  color: red !important;
}
</style>

<!-- ✅ CERTO -->
<style scoped>
.my-class {
  color: red;
}
</style>
```

---

## 🔴 Regra #16: Pinia - Composition API Style

```typescript
// ✅ CERTO
export const useIncomeStore = defineStore('income', () => {
  const incomes = ref<Income[]>([])
  
  const totalIncome = computed(() => 
    incomes.value.reduce((sum, i) => sum + i.value, 0)
  )
  
  async function loadIncomes(year: number, month: number) {
    incomes.value = await incomeService.getByMonth(year, month)
  }
  
  return { incomes, totalIncome, loadIncomes }
})

// ❌ ERRADO - Options API style
export const useIncomeStore = defineStore('income', {
  state: () => ({
    incomes: []
  }),
  getters: {
    totalIncome: (state) => state.incomes.reduce((sum, i) => sum + i.value, 0)
  },
  actions: {
    async loadIncomes(year, month) {
      this.incomes = await incomeService.getByMonth(year, month)
    }
  }
})
```

---

## 🎯 Checklist Rápido

### TypeScript
- [ ] Todas as variáveis tipadas
- [ ] Funções com tipos de retorno
- [ ] Interfaces para objetos complexos

### Vue 3
- [ ] Composition API (não Options API)
- [ ] `<script setup>` ao invés de `export default`
- [ ] Sem lógica no template

### Código Limpo
- [ ] Early return
- [ ] Async/await
- [ ] Optional chaining
- [ ] Sem comentários
- [ ] Nomes claros

### Pinia
- [ ] Composition API style
- [ ] Stores separadas por domínio
- [ ] Tipagem completa

---

**Versão:** 1.0 (Month Balance)  
**Data:** 22/01/2026
