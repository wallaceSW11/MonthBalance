<template>
  <v-container fluid class="expenses-view pa-0">
    <v-app-bar color="primary" density="compact">
      <v-btn icon @click="goBack">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-app-bar-title>{{ t('navigation.expenses') }}</v-app-bar-title>

      <v-btn icon @click="openAddDialog">
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-app-bar>

    <div class="content-container">
      <v-list v-if="expenses.length > 0" lines="two">
        <v-list-item
          v-for="expense in expenses"
          :key="expense.id"
          @click="openEditDialog(expense)"
        >
          <template #prepend>
            <v-icon color="error">
              mdi-cash-minus
            </v-icon>
          </template>

          <v-list-item-title>{{ expense.name }}</v-list-item-title>
          <v-list-item-subtitle>
            {{ formatExpenseValue(expense.value) }}
          </v-list-item-subtitle>

          <template #append>
            <v-btn
              icon
              size="small"
              variant="text"
              @click.stop="confirmDelete(expense)"
            >
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-list-item>
      </v-list>

      <v-card v-else class="ma-4" elevation="0" color="surface">
        <v-card-text class="text-center pa-8">
          <v-icon size="64" color="error" class="mb-4">
            mdi-cash-minus
          </v-icon>
          
          <h2 class="text-h6 mb-2">{{ t('expenses.emptyTitle') }}</h2>
          <p class="text-body-2 text-medium-emphasis mb-4">
            {{ t('expenses.emptyDescription') }}
          </p>

          <v-btn color="primary" @click="openAddDialog">
            <v-icon start>mdi-plus</v-icon>
            {{ t('expenses.addExpense') }}
          </v-btn>
        </v-card-text>
      </v-card>
    </div>

    <ExpenseFormDialog
      v-model="dialogOpen"
      :expense="selectedExpense"
      @save="handleSave"
      @close="closeDialog"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { useExpenseStore } from '@/stores/expense'
import { useMonthStore } from '@/stores/month'
import { formatCurrency } from '@/utils/currency'
import type { Expense } from '@/models/Expense'
import ExpenseFormDialog from '@/components/ExpenseFormDialog.vue'
import { useConfirmStore } from '@wallacesw11/base-lib'

const { t, locale } = useI18n()
const router = useRouter()
const expenseStore = useExpenseStore()
const monthStore = useMonthStore()
const confirmStore = useConfirmStore()

const dialogOpen = ref(false)
const selectedExpense = ref<Expense | null>(null)

const expenses = computed(() => expenseStore.expenses)

function goBack(): void {
  router.push('/')
}

function openAddDialog(): void {
  selectedExpense.value = null
  dialogOpen.value = true
}

function openEditDialog(expense: Expense): void {
  selectedExpense.value = expense
  dialogOpen.value = true
}

function closeDialog(): void {
  selectedExpense.value = null
  dialogOpen.value = false
}

function handleSave(expenseData: Omit<Expense, 'id'>): void {
  if (selectedExpense.value) {
    expenseStore.updateExpense({
      ...expenseData,
      id: selectedExpense.value.id,
    })
  } else {
    expenseStore.addExpense(expenseData)
  }
}

async function confirmDelete(expense: Expense): Promise<void> {
  const confirmRef = confirmStore.confirmRef
  
  if (!confirmRef || !confirmRef.$confirm) return
  
  const confirmed = await confirmRef.$confirm.show(
    t('expenses.deleteTitle'),
    t('expenses.deleteMessage', { name: expense.name })
  )
  
  if (confirmed) {
    expenseStore.deleteExpense(expense.id)
  }
}

function formatExpenseValue(value: number): string {
  return formatCurrency(value, locale.value)
}

onMounted(() => {
  expenseStore.loadExpenses(monthStore.currentYear, monthStore.currentMonth)
})
</script>

<style scoped>
.expenses-view {
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.content-container {
  flex: 1;
  overflow-y: auto;
}
</style>
