<template>
  <v-container
    fluid
    class="dashboard-container pa-0"
  >
    <NavigationDrawer v-model="drawerOpen" />

    <div class="sticky-header">
      <div class="header-top">
        <v-btn
          icon
          size="small"
          variant="text"
          class="menu-button"
          @click="toggleDrawer"
        >
          <v-icon>mdi-menu</v-icon>
        </v-btn>

        <MonthNavigation />

        <div class="spacer" />
      </div>

      <MonthSummary />
    </div>

    <div class="content-scroll">
      <IncomeList @edit="handleEditIncome" />
      <ExpenseList @edit="handleEditExpense" />
    </div>

    <v-btn
      icon
      size="large"
      color="primary"
      class="fab"
      elevation="8"
      @click="openExpenseSelectionDialog"
    >
      <v-icon size="28">
        mdi-plus
      </v-icon>
    </v-btn>

    <IncomeSelectionDialog
      v-model="incomeSelectionDialogOpen"
      @select="handleSelectIncome"
    />

    <IncomeFormDialog
      v-model="incomeFormDialogOpen"
      :month-income="selectedMonthIncome"
      :selected-income="selectedIncome"
      @save="handleSaveIncome"
    />

    <ExpenseSelectionDialog
      v-model="expenseSelectionDialogOpen"
      @select="handleSelectExpense"
    />

    <ExpenseFormDialog
      v-model="expenseFormDialogOpen"
      :month-expense="selectedMonthExpense"
      :selected-expense="selectedExpense"
      @save="handleSaveExpense"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useMonthStore } from '@/stores/month'
import { useIncomeStore } from '@/stores/income'
import { useExpenseStore } from '@/stores/expense'
import { useIncomeGlobalStore } from '@/stores/incomeGlobal'
import { useExpenseGlobalStore } from '@/stores/expenseGlobal'
import NavigationDrawer from '@/components/NavigationDrawer.vue'
import MonthNavigation from '@/components/MonthNavigation.vue'
import MonthSummary from '@/components/MonthSummary.vue'
import IncomeList from '@/components/IncomeList.vue'
import ExpenseList from '@/components/ExpenseList.vue'
import IncomeSelectionDialog from '@/components/IncomeSelectionDialog.vue'
import IncomeFormDialog from '@/components/IncomeFormDialog.vue'
import ExpenseSelectionDialog from '@/components/ExpenseSelectionDialog.vue'
import ExpenseFormDialog from '@/components/ExpenseFormDialog.vue'
import type { Income } from '@/models/Income'
import type { Expense } from '@/models/Expense'
import type { MonthIncome } from '@/models/MonthIncome'
import type { MonthExpense } from '@/models/MonthExpense'

const monthStore = useMonthStore()
const incomeStore = useIncomeStore()
const expenseStore = useExpenseStore()
const incomeGlobalStore = useIncomeGlobalStore()
const expenseGlobalStore = useExpenseGlobalStore()

const drawerOpen = ref(false)
const incomeSelectionDialogOpen = ref(false)
const incomeFormDialogOpen = ref(false)
const expenseSelectionDialogOpen = ref(false)
const expenseFormDialogOpen = ref(false)
const selectedIncome = ref<Income | null>(null)
const selectedExpense = ref<Expense | null>(null)
const selectedMonthIncome = ref<MonthIncome | null>(null)
const selectedMonthExpense = ref<MonthExpense | null>(null)

function toggleDrawer(): void {
  drawerOpen.value = !drawerOpen.value
}

function handleSelectIncome(income: Income): void {
  selectedIncome.value = income
  selectedMonthIncome.value = null
  incomeFormDialogOpen.value = true
}

function handleEditIncome(monthIncome: MonthIncome): void {
  selectedMonthIncome.value = monthIncome
  selectedIncome.value = null
  incomeFormDialogOpen.value = true
}

async function handleSaveIncome(data: {
  incomeId: number
  grossValue?: number | null
  netValue?: number | null
  hourlyRate?: number | null
  hours?: number | null
  minutes?: number | null
}): Promise<void> {
  if (selectedMonthIncome.value) {
    await incomeStore.updateMonthIncome(
      monthStore.currentYear,
      monthStore.currentMonth,
      selectedMonthIncome.value.id,
      data
    )
  } else {
    await incomeStore.createMonthIncome(
      monthStore.currentYear,
      monthStore.currentMonth,
      data
    )
  }

  selectedIncome.value = null
  selectedMonthIncome.value = null
}

function openExpenseSelectionDialog(): void {
  expenseSelectionDialogOpen.value = true
}

function handleSelectExpense(expense: Expense): void {
  selectedExpense.value = expense
  selectedMonthExpense.value = null
  expenseFormDialogOpen.value = true
}

function handleEditExpense(monthExpense: MonthExpense): void {
  selectedMonthExpense.value = monthExpense
  selectedExpense.value = null
  expenseFormDialogOpen.value = true
}

async function handleSaveExpense(data: {
  expenseId: number
  value: number
}): Promise<void> {
  if (selectedMonthExpense.value) {
    await expenseStore.updateMonthExpense(
      monthStore.currentYear,
      monthStore.currentMonth,
      selectedMonthExpense.value.id,
      { value: data.value }
    )
  } else {
    await expenseStore.createMonthExpense(
      monthStore.currentYear,
      monthStore.currentMonth,
      data
    )
  }

  selectedExpense.value = null
  selectedMonthExpense.value = null
}

onMounted(async () => {
  await Promise.all([
    incomeGlobalStore.loadIncomes(),
    expenseGlobalStore.loadExpenses(),
    monthStore.loadMonthData(monthStore.currentYear, monthStore.currentMonth)
  ])
})
</script>

<style scoped>
.dashboard-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.sticky-header {
  position: sticky;
  top: 0;
  z-index: 10;
  background: rgba(var(--v-theme-surface), 0.95);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.08);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.header-top {
  display: grid;
  grid-template-columns: 1fr 3fr 1fr;
  align-items: center;
  padding: 12px 16px;
}

.menu-button {
  justify-self: start;
}

.spacer {
  justify-self: end;
}

.content-scroll {
  flex: 1;
  overflow-y: auto;
  width: 100%;
  max-width: 448px;
  margin: 0 auto;
}

.fab {
  position: fixed;
  bottom: 32px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 20;
  border: 2px solid rgba(var(--v-theme-surface), 1);
}
</style>
