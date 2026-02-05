<template>
  <div class="home-view">
    <MonthNavigator
      :year="currentYear"
      :month="currentMonth"
      :can-navigate-previous="canNavigatePrevious"
      :can-navigate-next="canNavigateNext"
      :total-income="totalIncome"
      :total-expense="totalExpense"
      :balance="balance"
      @menu-click="handleMenuClick"
      @navigate-previous="navigatePrevious"
      @navigate-next="navigateNext"
      @duplicate-month="handleDuplicateMonth"
      @clear-month="handleClearMonth"
    />

    <div class="scrollable-content">
      <IncomeList
        :incomes="incomesWithNames"
        @add="handleAddIncome"
        @edit="handleEditIncome"
        @delete="handleDeleteIncome"
      />

      <ExpenseList
        :expenses="expensesWithNames"
        @edit="handleEditExpense"
        @delete="handleDeleteExpense"
      />
    </div>

    <div class="floating-button-container">
      <v-btn
        class="floating-button"
        color="primary"
        icon="mdi-plus"
        size="x-large"
        elevation="8"
        @click="handleAddExpense"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useI18n } from 'vue-i18n';
import { confirm, notify, loading } from '@wallacesw11/base-lib';
import { localStorageService } from '@/services/localStorageService';
import type { MonthData, Income, Expense } from '@/models';
import MonthNavigator from '@/components/MonthNavigator.vue';
import IncomeList from '@/components/IncomeList.vue';
import ExpenseList from '@/components/ExpenseList.vue';

const { t } = useI18n();

const LAST_MONTH_KEY = 'monthbalance_last_month';
const MIN_YEAR = 2026;
const MIN_MONTH = 1;

const currentYear = ref<number>(new Date().getFullYear());
const currentMonth = ref<number>(new Date().getMonth() + 1);
const incomes = ref<Income[]>([]);
const expenses = ref<Expense[]>([]);
const currentMonthData = ref<MonthData | null>(null);

const totalIncome = computed(() => {
  return incomes.value.reduce((sum, income) => sum + income.calculatedValue, 0);
});

const totalExpense = computed(() => {
  return expenses.value.reduce((sum, expense) => sum + expense.value, 0);
});

const balance = computed(() => {
  return totalIncome.value - totalExpense.value;
});

const canNavigatePrevious = computed(() => {
  if (currentYear.value === MIN_YEAR && currentMonth.value === MIN_MONTH) return false;

  return true;
});

const canNavigateNext = computed(() => {
  return true;
});

const incomesWithNames = computed(() => {
  return incomes.value.map(income => ({
    id: income.id,
    name: 'Receita',
    value: income.calculatedValue
  }));
});

const expensesWithNames = computed(() => {
  return expenses.value.map(expense => ({
    id: expense.id,
    name: 'Despesa',
    value: expense.value
  }));
});

async function loadMonth(): Promise<void> {
  loading.show(t('common.loading'));

  try {
    const monthDataList = (await localStorageService.get('month_data')) as MonthData[];
    const foundMonthData = monthDataList.find(
      md => md.year === currentYear.value && md.month === currentMonth.value
    );

    if (!foundMonthData) {
      currentMonthData.value = null;
      incomes.value = [];
      expenses.value = [];
      loading.hide();

      return;
    }

    currentMonthData.value = foundMonthData;

    const allIncomes = (await localStorageService.get('incomes')) as Income[];
    incomes.value = allIncomes.filter(i => i.monthDataId === foundMonthData.id);

    const allExpenses = (await localStorageService.get('expenses')) as Expense[];
    expenses.value = allExpenses.filter(e => e.monthDataId === foundMonthData.id);

    await localStorageService.put<MonthData>('month_data', foundMonthData.id, {
      lastAccessed: new Date()
    });

    saveLastAccessedMonth();
  } catch (error) {
    notify.error(t('monthBalance.loadError'), String(error));
  } finally {
    loading.hide();
  }
}

function saveLastAccessedMonth(): void {
  localStorage.setItem(
    LAST_MONTH_KEY,
    JSON.stringify({ year: currentYear.value, month: currentMonth.value })
  );
}

function loadLastAccessedMonth(): void {
  const saved = localStorage.getItem(LAST_MONTH_KEY);

  if (!saved) return;

  try {
    const { year, month } = JSON.parse(saved);
    currentYear.value = year;
    currentMonth.value = month;
  } catch (error) {
    console.error('Failed to load last accessed month', error);
  }
}

function navigatePrevious(): void {
  if (!canNavigatePrevious.value) return;

  if (currentMonth.value === 1) {
    currentMonth.value = 12;
    currentYear.value -= 1;
  } else {
    currentMonth.value -= 1;
  }

  loadMonth();
}

function navigateNext(): void {
  if (!canNavigateNext.value) return;

  if (currentMonth.value === 12) {
    currentMonth.value = 1;
    currentYear.value += 1;
  } else {
    currentMonth.value += 1;
  }

  loadMonth();
}

async function handleDuplicateMonth(): Promise<void> {
  const previousMonth = currentMonth.value === 1 ? 12 : currentMonth.value - 1;
  const previousYear = currentMonth.value === 1 ? currentYear.value - 1 : currentYear.value;
  const monthName = t(`monthBalance.months.${previousMonth}`);

  const confirmed = await confirm.show(
    t('monthBalance.duplicateMonth'),
    t('monthBalance.duplicateMonthConfirm', { month: `${monthName} ${previousYear}` }),
    { confirmText: t('common.yes'), cancelText: t('common.no') }
  );

  if (!confirmed) return;

  notify.info(t('monthBalance.duplicateMonth'), 'Funcionalidade em desenvolvimento');
}

async function handleClearMonth(): Promise<void> {
  const confirmed = await confirm.show(
    t('monthBalance.clearMonth'),
    t('monthBalance.clearMonthConfirm'),
    {
      confirmText: t('common.yes'),
      cancelText: t('common.no'),
      confirmColor: 'error'
    }
  );

  if (!confirmed) return;

  notify.info(t('monthBalance.clearMonth'), 'Funcionalidade em desenvolvimento');
}

function handleMenuClick(): void {
  notify.info('Menu', 'Navigation drawer em desenvolvimento');
}

function handleAddIncome(): void {
  notify.info('Adicionar Receita', 'Funcionalidade em desenvolvimento');
}

function handleEditIncome(id: string): void {
  notify.info('Editar Receita', `ID: ${id} - Funcionalidade em desenvolvimento`);
}

function handleDeleteIncome(id: string): void {
  notify.info('Excluir Receita', `ID: ${id} - Funcionalidade em desenvolvimento`);
}

function handleAddExpense(): void {
  notify.info('Adicionar Despesa', 'Funcionalidade em desenvolvimento');
}

function handleEditExpense(id: string): void {
  notify.info('Editar Despesa', `ID: ${id} - Funcionalidade em desenvolvimento`);
}

function handleDeleteExpense(id: string): void {
  notify.info('Excluir Despesa', `ID: ${id} - Funcionalidade em desenvolvimento`);
}

onMounted(() => {
  loadLastAccessedMonth();
  loadMonth();
});
</script>

<style scoped>
.home-view {
  position: relative;
  height: 100dvh;
  overflow: hidden;
}

.scrollable-content {
  height: calc(100dvh - 160px);
  overflow-y: auto;
  padding-bottom: 100px;
}

.floating-button-container {
  position: fixed;
  bottom: 32px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 10;
}

.floating-button {
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.4);
}
</style>
