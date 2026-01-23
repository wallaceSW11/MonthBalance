<template>
  <ModalBase
    :model-value="modelValue"
    title="Selecionar Despesa"
    max-width="600"
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-list v-if="!loading">
      <v-list-item
        v-for="expense in availableExpenses"
        :key="expense.id"
        @click="handleSelect(expense)"
      >
        <v-list-item-title>{{ expense.description }}</v-list-item-title>
      </v-list-item>

      <v-list-item v-if="availableExpenses.length === 0">
        <v-list-item-title class="text-center text-grey">
          Nenhuma despesa disponível
        </v-list-item-title>
      </v-list-item>
    </v-list>

    <div v-else class="text-center pa-4">
      <v-progress-circular indeterminate color="primary" />
    </div>

    <template #actions>
      <v-btn
        v-if="availableExpenses.length === 0 && !loading"
        color="primary"
        @click="goToExpenses"
      >
        Cadastrar Despesa
      </v-btn>
      <v-btn
        text
        @click="$emit('update:modelValue', false)"
      >
        Cancelar
      </v-btn>
    </template>
  </ModalBase>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useExpenseGlobalStore } from '@/stores/expenseGlobal'
import { useExpenseStore } from '@/stores/expense'
import type { Expense } from '@/models/Expense'
import { ModalBase } from '@wallacesw11/base-lib'

defineProps<{
  modelValue: boolean
}>()

const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  select: [expense: Expense]
}>()

const router = useRouter()
const expenseGlobalStore = useExpenseGlobalStore()
const expenseStore = useExpenseStore()

const loading = computed(() => expenseGlobalStore.loading)

const availableExpenses = computed(() => {
  const linkedIds = expenseStore.monthExpenses.map(me => me.expenseId)
  
  return expenseGlobalStore.expenses.filter(expense => !linkedIds.includes(expense.id))
})

function handleSelect(expense: Expense): void {
  emit('select', expense)
  emit('update:modelValue', false)
}

function goToExpenses(): void {
  emit('update:modelValue', false)
  router.push('/expenses-global')
}
</script>
