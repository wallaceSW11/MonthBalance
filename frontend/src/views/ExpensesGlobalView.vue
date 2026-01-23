<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <div class="d-flex align-center mb-4">
          <v-btn
            icon="mdi-arrow-left"
            variant="text"
            @click="goBack"
          />
          <h1 class="text-h4 ml-2">Despesas</h1>
        </div>
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
      <v-col
        v-for="expense in expenses"
        :key="expense.id"
        cols="12"
      >
        <v-card>
          <v-card-text>
            <div class="d-flex justify-space-between align-center">
              <div>
                <div class="text-h6">{{ expense.description }}</div>
              </div>
              <div class="d-flex gap-2">
                <IconToolTip
                  icon="mdi-pencil"
                  tooltip="Editar"
                  @click="openEditDialog(expense)"
                />
                <IconToolTip
                  icon="mdi-delete"
                  tooltip="Excluir"
                  color="error"
                  @click="handleDelete(expense.id)"
                />
              </div>
            </div>
          </v-card-text>
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
import { useRouter } from 'vue-router'
import { useExpenseGlobalStore } from '@/stores/expenseGlobal'
import type { Expense } from '@/models/Expense'
import ExpenseGlobalFormDialog from '@/components/ExpenseGlobalFormDialog.vue'
import { IconToolTip, confirm } from '@wallacesw11/base-lib'

const router = useRouter()
const expenseGlobalStore = useExpenseGlobalStore()

const dialogOpen = ref(false)
const selectedExpense = ref<Expense | null>(null)

const expenses = computed(() => expenseGlobalStore.expenses)
const loading = computed(() => expenseGlobalStore.loading)

function goBack(): void {
  router.back()
}

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
  const confirmed = await confirm.show(
    'Confirmar exclusão',
    'Tem certeza que deseja excluir esta despesa?',
    {
      confirmText: 'Excluir',
      cancelText: 'Cancelar'
    }
  )

  if (!confirmed) return

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
