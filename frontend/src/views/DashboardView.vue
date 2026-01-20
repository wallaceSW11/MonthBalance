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
      <ExpenseList />
    </div>

    <v-btn
      icon
      size="large"
      color="primary"
      class="fab"
      elevation="8"
      @click="openExpenseDialog"
    >
      <v-icon size="28">
        mdi-plus
      </v-icon>
    </v-btn>

    <IncomeFormDialog
      v-model="incomeDialogOpen"
      :income="selectedIncome"
      @save="handleSaveIncome"
    />

    <ExpenseFormDialog
      v-model="expenseDialogOpen"
      :expense="selectedExpense"
      @save="handleSaveExpense"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useMonthStore } from '@/stores/month'
import { useIncomeStore } from '@/stores/income'
import { useExpenseStore } from '@/stores/expense'
import NavigationDrawer from '@/components/NavigationDrawer.vue'
import MonthNavigation from '@/components/MonthNavigation.vue'
import MonthSummary from '@/components/MonthSummary.vue'
import IncomeList from '@/components/IncomeList.vue'
import ExpenseList from '@/components/ExpenseList.vue'
import IncomeFormDialog from '@/components/IncomeFormDialog.vue'
import ExpenseFormDialog from '@/components/ExpenseFormDialog.vue'
import type { Income } from '@/models/Income'
import type { Expense } from '@/models/Expense'

const monthStore = useMonthStore()
const incomeStore = useIncomeStore()
const expenseStore = useExpenseStore()

const drawerOpen = ref(false)
const incomeDialogOpen = ref(false)
const expenseDialogOpen = ref(false)
const selectedIncome = ref<Income | null>(null)
const selectedExpense = ref<Expense | null>(null)

function toggleDrawer(): void {
  drawerOpen.value = !drawerOpen.value
}

function handleEditIncome(income: Income): void {
  selectedIncome.value = income
  incomeDialogOpen.value = true
}

function handleSaveIncome(incomeData: Omit<Income, 'id'>): void {
  if (selectedIncome.value) {
    incomeStore.updateIncome({
      ...selectedIncome.value,
      ...incomeData,
    })
    selectedIncome.value = null
  } else {
    incomeStore.addIncome(incomeData)
  }
}

function openExpenseDialog(): void {
  selectedExpense.value = null
  expenseDialogOpen.value = true
}

function handleSaveExpense(expenseData: Omit<Expense, 'id'>): void {
  if (selectedExpense.value) {
    expenseStore.updateExpense({
      ...selectedExpense.value,
      ...expenseData,
    })
    selectedExpense.value = null
  } else {
    expenseStore.addExpense(expenseData)
  }
}

onMounted(() => {
  monthStore.initializeCurrentMonth()
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
