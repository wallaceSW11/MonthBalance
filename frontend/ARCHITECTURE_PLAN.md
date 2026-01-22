# 🎨 Plano de Arquitetura - Frontend (Global Types)

## 🎯 Objetivo

Implementar interface para gerenciar tipos globais de receitas/despesas e permitir cadastro rápido de lançamentos mensais.

---

## 📊 Mudanças nos Models

### Novos Models

#### IncomeType.ts
```typescript
export interface IncomeType {
  id: number
  name: string
  type: 'manual' | 'hourly'
}

export interface CreateIncomeTypeDto {
  name: string
  type: 'manual' | 'hourly'
}

export interface UpdateIncomeTypeDto {
  name: string
  type: 'manual' | 'hourly'
}
```

#### ExpenseType.ts
```typescript
export interface ExpenseType {
  id: number
  name: string
}

export interface CreateExpenseTypeDto {
  name: string
}

export interface UpdateExpenseTypeDto {
  name: string
}
```

### Models Modificados

#### Income.ts (Modificado)
```typescript
// ANTES
export interface Income {
  id: number
  name: string              // ❌ REMOVER
  type: 'manual' | 'hourly' // ❌ REMOVER
  grossValue?: number
  netValue?: number
  hourlyRate?: number
  hours?: number
  minutes?: number
  monthDataId: number
}

// DEPOIS
export interface Income {
  id: number
  incomeTypeId: number      // ✅ NOVO
  incomeTypeName: string    // ✅ NOVO (vem do backend)
  incomeTypeType: 'manual' | 'hourly' // ✅ NOVO (vem do backend)
  grossValue?: number
  netValue?: number
  hourlyRate?: number
  hours?: number
  minutes?: number
  monthDataId: number
}

export interface CreateIncomeDto {
  incomeTypeId: number      // ✅ NOVO
  grossValue?: number
  netValue?: number
  hourlyRate?: number
  hours?: number
  minutes?: number
}
```

#### Expense.ts (Modificado)
```typescript
// ANTES
export interface Expense {
  id: number
  name: string              // ❌ REMOVER
  value: number
  monthDataId: number
}

// DEPOIS
export interface Expense {
  id: number
  expenseTypeId: number     // ✅ NOVO
  expenseTypeName: string   // ✅ NOVO (vem do backend)
  value: number
  monthDataId: number
}

export interface CreateExpenseDto {
  expenseTypeId: number     // ✅ NOVO
  value: number
}
```

---

## 🔧 Services

### Novos Services

#### incomeTypeService.ts
```typescript
import { httpClient } from './httpClient'
import type { IncomeType, CreateIncomeTypeDto, UpdateIncomeTypeDto } from '@/models/IncomeType'

export const incomeTypeService = {
  async getAll(): Promise<IncomeType[]> {
    const response = await httpClient.get('/incometypes')
    return response.data
  },

  async getById(id: number): Promise<IncomeType> {
    const response = await httpClient.get(`/incometypes/${id}`)
    return response.data
  },

  async create(dto: CreateIncomeTypeDto): Promise<IncomeType> {
    const response = await httpClient.post('/incometypes', dto)
    return response.data
  },

  async update(id: number, dto: UpdateIncomeTypeDto): Promise<IncomeType> {
    const response = await httpClient.put(`/incometypes/${id}`, dto)
    return response.data
  },

  async delete(id: number): Promise<void> {
    await httpClient.delete(`/incometypes/${id}`)
  }
}
```

#### expenseTypeService.ts
```typescript
import { httpClient } from './httpClient'
import type { ExpenseType, CreateExpenseTypeDto, UpdateExpenseTypeDto } from '@/models/ExpenseType'

export const expenseTypeService = {
  async getAll(): Promise<ExpenseType[]> {
    const response = await httpClient.get('/expensetypes')
    return response.data
  },

  async getById(id: number): Promise<ExpenseType> {
    const response = await httpClient.get(`/expensetypes/${id}`)
    return response.data
  },

  async create(dto: CreateExpenseTypeDto): Promise<ExpenseType> {
    const response = await httpClient.post('/expensetypes', dto)
    return response.data
  },

  async update(id: number, dto: UpdateExpenseTypeDto): Promise<ExpenseType> {
    const response = await httpClient.put(`/expensetypes/${id}`, dto)
    return response.data
  },

  async delete(id: number): Promise<void> {
    await httpClient.delete(`/expensetypes/${id}`)
  }
}
```

---

## 🗂️ Stores

### Novas Stores

#### incomeType.ts
```typescript
import { defineStore } from 'pinia'
import { ref } from 'vue'
import { incomeTypeService } from '@/services/api/incomeTypeService'
import type { IncomeType, CreateIncomeTypeDto, UpdateIncomeTypeDto } from '@/models/IncomeType'

export const useIncomeTypeStore = defineStore('incomeType', () => {
  const incomeTypes = ref<IncomeType[]>([])
  const loading = ref(false)

  async function loadIncomeTypes() {
    loading.value = true
    
    try {
      incomeTypes.value = await incomeTypeService.getAll()
    } finally {
      loading.value = false
    }
  }

  async function createIncomeType(dto: CreateIncomeTypeDto) {
    const created = await incomeTypeService.create(dto)
    incomeTypes.value.push(created)
    return created
  }

  async function updateIncomeType(id: number, dto: UpdateIncomeTypeDto) {
    const updated = await incomeTypeService.update(id, dto)
    const index = incomeTypes.value.findIndex(t => t.id === id)
    
    if (index !== -1) {
      incomeTypes.value[index] = updated
    }
    
    return updated
  }

  async function deleteIncomeType(id: number) {
    await incomeTypeService.delete(id)
    incomeTypes.value = incomeTypes.value.filter(t => t.id !== id)
  }

  return {
    incomeTypes,
    loading,
    loadIncomeTypes,
    createIncomeType,
    updateIncomeType,
    deleteIncomeType
  }
})
```

#### expenseType.ts
```typescript
import { defineStore } from 'pinia'
import { ref } from 'vue'
import { expenseTypeService } from '@/services/api/expenseTypeService'
import type { ExpenseType, CreateExpenseTypeDto, UpdateExpenseTypeDto } from '@/models/ExpenseType'

export const useExpenseTypeStore = defineStore('expenseType', () => {
  const expenseTypes = ref<ExpenseType[]>([])
  const loading = ref(false)

  async function loadExpenseTypes() {
    loading.value = true
    
    try {
      expenseTypes.value = await expenseTypeService.getAll()
    } finally {
      loading.value = false
    }
  }

  async function createExpenseType(dto: CreateExpenseTypeDto) {
    const created = await expenseTypeService.create(dto)
    expenseTypes.value.push(created)
    return created
  }

  async function updateExpenseType(id: number, dto: UpdateExpenseTypeDto) {
    const updated = await expenseTypeService.update(id, dto)
    const index = expenseTypes.value.findIndex(t => t.id === id)
    
    if (index !== -1) {
      expenseTypes.value[index] = updated
    }
    
    return updated
  }

  async function deleteExpenseType(id: number) {
    await expenseTypeService.delete(id)
    expenseTypes.value = expenseTypes.value.filter(t => t.id !== id)
  }

  return {
    expenseTypes,
    loading,
    loadExpenseTypes,
    createExpenseType,
    updateExpenseType,
    deleteExpenseType
  }
})
```

---

## 🎨 Componentes

### Novos Componentes

#### 1. IncomeTypeListDialog.vue
**Propósito:** Modal para selecionar tipo de receita existente ou criar novo

**Props:**
- `modelValue: boolean` - Controla visibilidade do modal
- `year: number` - Ano do mês atual
- `month: number` - Mês atual

**Emits:**
- `update:modelValue` - Atualiza visibilidade
- `selected` - Emite quando tipo é selecionado

**Estrutura:**
```vue
<template>
  <v-dialog :model-value="modelValue" max-width="600">
    <v-card>
      <v-card-title>Selecionar Tipo de Receita</v-card-title>
      
      <v-card-text>
        <!-- Lista de tipos existentes -->
        <v-list v-if="incomeTypes.length > 0">
          <v-list-item
            v-for="type in incomeTypes"
            :key="type.id"
            @click="selectType(type)"
          >
            <v-list-item-title>{{ type.name }}</v-list-item-title>
            <v-list-item-subtitle>
              {{ type.type === 'manual' ? 'Manual' : 'Por Hora' }}
            </v-list-item-subtitle>
          </v-list-item>
        </v-list>
        
        <!-- Mensagem se não houver tipos -->
        <v-alert v-else type="info">
          Nenhum tipo de receita cadastrado. Crie um novo abaixo.
        </v-alert>
        
        <!-- Botão para criar novo -->
        <v-btn
          color="primary"
          block
          @click="showCreateForm = true"
        >
          + Criar Novo Tipo
        </v-btn>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
```

#### 2. IncomeValueDialog.vue
**Propósito:** Modal para informar valores da receita após selecionar tipo

**Props:**
- `modelValue: boolean`
- `incomeType: IncomeType` - Tipo selecionado
- `year: number`
- `month: number`

**Emits:**
- `update:modelValue`
- `saved` - Emite quando receita é salva

**Estrutura:**
```vue
<template>
  <v-dialog :model-value="modelValue" max-width="500">
    <v-card>
      <v-card-title>{{ incomeType.name }}</v-card-title>
      
      <v-card-text>
        <!-- Campos para tipo Manual -->
        <template v-if="incomeType.type === 'manual'">
          <v-text-field
            v-model="form.grossValue"
            label="Valor Bruto"
            type="number"
          />
          <v-text-field
            v-model="form.netValue"
            label="Valor Líquido"
            type="number"
          />
        </template>
        
        <!-- Campos para tipo Hourly -->
        <template v-else>
          <v-text-field
            v-model="form.hourlyRate"
            label="Valor por Hora"
            type="number"
          />
          <v-text-field
            v-model="form.hours"
            label="Horas"
            type="number"
          />
          <v-text-field
            v-model="form.minutes"
            label="Minutos"
            type="number"
          />
        </template>
      </v-card-text>
      
      <v-card-actions>
        <v-btn @click="close">Cancelar</v-btn>
        <v-btn color="primary" @click="save">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
```

#### 3. ExpenseTypeListDialog.vue
**Propósito:** Modal para selecionar tipo de despesa (com botão flutuante)

**Props:**
- `modelValue: boolean`
- `year: number`
- `month: number`

**Emits:**
- `update:modelValue`
- `selected` - Emite quando tipo é selecionado

**Estrutura:**
```vue
<template>
  <v-dialog :model-value="modelValue" max-width="600" persistent>
    <v-card>
      <v-card-title>Adicionar Despesas</v-card-title>
      
      <v-card-text>
        <!-- Lista de tipos -->
        <v-list v-if="expenseTypes.length > 0">
          <v-list-item
            v-for="type in expenseTypes"
            :key="type.id"
            @click="selectType(type)"
          >
            <v-list-item-title>{{ type.name }}</v-list-item-title>
          </v-list-item>
        </v-list>
        
        <!-- Mensagem se não houver tipos -->
        <v-alert v-else type="info">
          Nenhum tipo de despesa cadastrado. Crie um novo abaixo.
        </v-alert>
        
        <!-- Botão para criar novo -->
        <v-btn
          color="primary"
          block
          @click="showCreateForm = true"
        >
          + Criar Novo Tipo
        </v-btn>
      </v-card-text>
      
      <v-card-actions>
        <v-btn @click="close">Fechar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
```

#### 4. ExpenseValueDialog.vue
**Propósito:** Modal para informar valor da despesa após selecionar tipo

**Props:**
- `modelValue: boolean`
- `expenseType: ExpenseType`
- `year: number`
- `month: number`

**Emits:**
- `update:modelValue`
- `saved` - Emite quando despesa é salva
- `continue` - Emite para continuar adicionando mais despesas

**Estrutura:**
```vue
<template>
  <v-dialog :model-value="modelValue" max-width="400">
    <v-card>
      <v-card-title>{{ expenseType.name }}</v-card-title>
      
      <v-card-text>
        <v-text-field
          v-model="form.value"
          label="Valor"
          type="number"
          autofocus
        />
      </v-card-text>
      
      <v-card-actions>
        <v-btn @click="close">Cancelar</v-btn>
        <v-btn color="primary" @click="saveAndContinue">
          Salvar e Adicionar Mais
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
```

#### 5. IncomeTypeFormDialog.vue
**Propósito:** Modal para criar/editar tipo de receita

**Props:**
- `modelValue: boolean`
- `incomeType?: IncomeType` - Se fornecido, é edição

**Emits:**
- `update:modelValue`
- `saved`

#### 6. ExpenseTypeFormDialog.vue
**Propósito:** Modal para criar/editar tipo de despesa

**Props:**
- `modelValue: boolean`
- `expenseType?: ExpenseType` - Se fornecido, é edição

**Emits:**
- `update:modelValue`
- `saved`

### Componentes Modificados

#### IncomeList.vue (Modificado)
```vue
<template>
  <v-card>
    <v-card-title>
      Receitas
      <!-- Botão + para adicionar -->
      <v-btn
        icon="mdi-plus"
        size="small"
        @click="showTypeListDialog = true"
      />
    </v-card-title>
    
    <v-card-text>
      <v-list>
        <v-list-item v-for="income in incomes" :key="income.id">
          <!-- Exibir incomeTypeName ao invés de name -->
          <v-list-item-title>{{ income.incomeTypeName }}</v-list-item-title>
          <v-list-item-subtitle>
            {{ formatValue(income) }}
          </v-list-item-subtitle>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
  
  <!-- Dialogs -->
  <IncomeTypeListDialog
    v-model="showTypeListDialog"
    :year="year"
    :month="month"
    @selected="handleTypeSelected"
  />
  
  <IncomeValueDialog
    v-model="showValueDialog"
    :income-type="selectedType"
    :year="year"
    :month="month"
    @saved="handleSaved"
  />
</template>
```

#### ExpenseList.vue (Modificado)
```vue
<template>
  <v-card>
    <v-card-title>Despesas</v-card-title>
    
    <v-card-text>
      <v-list>
        <v-list-item v-for="expense in expenses" :key="expense.id">
          <!-- Exibir expenseTypeName ao invés de name -->
          <v-list-item-title>{{ expense.expenseTypeName }}</v-list-item-title>
          <v-list-item-subtitle>
            {{ formatCurrency(expense.value) }}
          </v-list-item-subtitle>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
  
  <!-- Botão flutuante -->
  <v-btn
    icon="mdi-plus"
    color="primary"
    position="fixed"
    location="bottom right"
    @click="showTypeListDialog = true"
  />
  
  <!-- Dialogs -->
  <ExpenseTypeListDialog
    v-model="showTypeListDialog"
    :year="year"
    :month="month"
    @selected="handleTypeSelected"
  />
  
  <ExpenseValueDialog
    v-model="showValueDialog"
    :expense-type="selectedType"
    :year="year"
    :month="month"
    @saved="handleSaved"
    @continue="handleContinue"
  />
</template>
```

---

## 🗺️ Rotas

### Novas Views

#### IncomeTypesView.vue
**Rota:** `/income-types`

**Propósito:** Gerenciar tipos de receita (CRUD)

**Estrutura:**
- Lista de tipos com botões editar/excluir
- Botão para criar novo tipo
- Modal para criar/editar

#### ExpenseTypesView.vue
**Rota:** `/expense-types`

**Propósito:** Gerenciar tipos de despesa (CRUD)

**Estrutura:**
- Lista de tipos com botões editar/excluir
- Botão para criar novo tipo
- Modal para criar/editar

### Atualizar router/index.ts
```typescript
const routes = [
  // ... rotas existentes
  {
    path: '/income-types',
    name: 'IncomeTypes',
    component: () => import('@/views/IncomeTypesView.vue')
  },
  {
    path: '/expense-types',
    name: 'ExpenseTypes',
    component: () => import('@/views/ExpenseTypesView.vue')
  }
]
```

---

## 🎯 Fluxo de Usuário

### Adicionar Receita

1. Usuário acessa mês (DashboardView)
2. Clica no botão "+" ao lado de "Receitas"
3. Abre `IncomeTypeListDialog`
   - Se houver tipos: exibe lista
   - Se não houver: exibe mensagem + botão criar
4. Usuário seleciona tipo (ou cria novo)
5. Abre `IncomeValueDialog` com campos específicos do tipo
6. Usuário preenche valores e salva
7. Receita é criada e lista é atualizada

### Adicionar Despesa

1. Usuário acessa mês (DashboardView)
2. Clica no botão flutuante "+"
3. Abre `ExpenseTypeListDialog`
   - Se houver tipos: exibe lista
   - Se não houver: exibe mensagem + botão criar
4. Usuário seleciona tipo (ou cria novo)
5. Abre `ExpenseValueDialog`
6. Usuário preenche valor e clica "Salvar e Adicionar Mais"
7. Despesa é criada, modal fecha e volta para `ExpenseTypeListDialog`
8. Usuário pode continuar adicionando ou fechar

### Gerenciar Tipos

1. Usuário acessa menu de navegação
2. Clica em "Tipos de Receita" ou "Tipos de Despesa"
3. Visualiza lista de tipos
4. Pode criar, editar ou excluir tipos

---

## ✅ Checklist de Implementação

### Fase 1: Models e Services
- [ ] Criar models `IncomeType` e `ExpenseType`
- [ ] Criar services `incomeTypeService` e `expenseTypeService`
- [ ] Criar stores `incomeType` e `expenseType`
- [ ] Atualizar models `Income` e `Expense`

### Fase 2: Componentes de Tipos
- [ ] Criar `IncomeTypeFormDialog.vue`
- [ ] Criar `ExpenseTypeFormDialog.vue`
- [ ] Criar `IncomeTypesView.vue`
- [ ] Criar `ExpenseTypesView.vue`
- [ ] Adicionar rotas

### Fase 3: Componentes de Lançamento
- [ ] Criar `IncomeTypeListDialog.vue`
- [ ] Criar `IncomeValueDialog.vue`
- [ ] Criar `ExpenseTypeListDialog.vue`
- [ ] Criar `ExpenseValueDialog.vue`

### Fase 4: Integração
- [ ] Modificar `IncomeList.vue` (botão +, dialogs)
- [ ] Modificar `ExpenseList.vue` (botão flutuante, dialogs)
- [ ] Atualizar `IncomeFormDialog.vue` (usar tipos)
- [ ] Atualizar `ExpenseFormDialog.vue` (usar tipos)

### Fase 5: Testes
- [ ] Testar criação de tipos
- [ ] Testar seleção de tipos
- [ ] Testar criação de lançamentos
- [ ] Testar fluxo completo (tipo → valor → salvar)
- [ ] Testar "Salvar e Adicionar Mais" (despesas)

---

**Versão:** 1.0  
**Data:** 22/01/2026
