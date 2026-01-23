<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 class="text-h4 mb-4">Despesas Globais</h1>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-btn
          color="primary"
          @click="openCreateDialog"
        >
          <v-icon left>mdi-plus</v-icon>
          Nova Despesa
        </v-btn>
      </v-col>
    </v-row>

    <v-row v-if="loading">
      <v-col cols="12" class="text-center">
        <v-progress-circular indeterminate color="primary" />
      </v-col>
    </v-row>

    <v-row v-else>
      <v-col cols="12">
        <v-card>
          <v-list>
            <v-list-item
              v-for="expense in expenses"
              :key="expense.id"
            >
              <v-list-item-title>{{ expense.description }}</v-list-item-title>

              <template #append>
                <v-btn
                  icon="mdi-pencil"
                  size="small"
                  @click="openEditDialog(expense)"
                />
                <v-btn
                  icon="mdi-delete"
                  size="small"
                  color="error"
                  @click="handleDelete(expense.id)"
                />
              </template>
            </v-list-item>
          </v-list>
        </v-card>
      </v-col>
    </v-row>

    <ExpenseGlobalFormDialog
      v-model="dialogOpen"
      :expense="selectedExpense"
      @save="handleSave"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useExpenseGlobalStore } from '@/stores/expenseGlobal'
import type { Expense } from '@/models/Expense'
import ExpenseGlobalFormDialog from '@/components/ExpenseGlobalFormDialog.vue'

const expenseGlobalStore = useExpenseGlobalStore()

const dialogOpen = ref(false)
const selectedExpense = ref<Expense | null>(null)

const expenses = computed(() => expenseGlobalStore.expenses)
const loading = computed(() => expenseGlobalStore.loading)

function openCreateDialog(): void {
  selectedExpense.value = null
  dialogOpen.value = true
}

function openEditDialog(expense: Expense): void {
  selectedExpense.value = expense
  dialogOpen.value = true
}

async function handleSave(): Promise<void> {
  dialogOpen.value = false
  selectedExpense.value = null
  
  await expenseGlobalStore.loadExpenses()
}

async function handleDelete(id: number): Promise<void> {
  if (!confirm('Tem certeza que deseja excluir esta despesa?')) return

  try {
    await expenseGlobalStore.deleteExpense(id)
  } catch (error: any) {
    alert(error.response?.data?.message || 'Erro ao excluir despesa')
  }
}

onMounted(() => {
  expenseGlobalStore.loadExpenses()
})
</script>
