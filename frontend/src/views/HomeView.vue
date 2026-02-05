<template>
  <div class="home-view">
    <v-navigation-drawer v-model="drawerOpen" temporary>
      <v-list>
        <v-list-item prepend-icon="mdi-cash-multiple" title="Receitas" to="/income-types" />
        <v-list-item prepend-icon="mdi-credit-card-outline" title="Despesas" to="/expense-types" />

        <v-divider class="my-2" />

        <v-list-item>
          <div class="theme-toggle-container">
            <span class="theme-label">{{ t('theme.title') }}</span>
            <ThemeToggle />
          </div>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

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
        @edit="handleEditExpenseInline"
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

    <IncomeTypeSelectModal
      v-model:open="incomeTypeSelectOpen"
      :income-types="incomeTypes"
      @select="handleIncomeTypeSelected"
    />

    <IncomeFormModal
      v-model:open="incomeFormOpen"
      :mode="incomeFormMode"
      :income-type="selectedIncomeType"
      :income-type-id="selectedIncomeTypeId"
      :month-data-id="currentMonthDataId"
      :initial-data="selectedIncome"
      @saved="handleIncomeSaved"
    />

    <ExpenseTypeSelectModal
      v-model:open="expenseTypeSelectOpen"
      :expense-types="expenseTypes"
      @select="handleExpenseTypeSelected"
    />

    <ExpenseFormModal
      v-model:open="expenseFormOpen"
      :mode="expenseFormMode"
      :expense-type-id="selectedExpenseTypeId"
      :month-data-id="currentMonthDataId"
      @saved="handleExpenseSaved"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useI18n } from 'vue-i18n';
import { confirm, notify, loading, ThemeToggle } from '@wallacesw11/base-lib';
import { localStorageService } from '@/services/localStorageService';
import { IncomeType, FormMode } from '@/models';
import type { MonthData, Income, Expense, IncomeTypeModel, ExpenseTypeModel } from '@/models';
import MonthNavigator from '@/components/MonthNavigator.vue';
import IncomeList from '@/components/IncomeList.vue';
import ExpenseList from '@/components/ExpenseList.vue';
import IncomeTypeSelectModal from '@/components/IncomeTypeSelectModal.vue';
import IncomeFormModal from '@/components/IncomeFormModal.vue';
import ExpenseTypeSelectModal from '@/components/ExpenseTypeSelectModal.vue';
import ExpenseFormModal from '@/components/ExpenseFormModal.vue';

const { t } = useI18n();

const LAST_MONTH_KEY = 'monthbalance_last_month';
const MIN_YEAR = 2026;
const MIN_MONTH = 1;

const currentYear = ref<number>(new Date().getFullYear());
const currentMonth = ref<number>(new Date().getMonth() + 1);
const incomes = ref<Income[]>([]);
const expenses = ref<Expense[]>([]);
const currentMonthData = ref<MonthData | null>(null);
const incomeTypes = ref<IncomeTypeModel[]>([]);
const expenseTypes = ref<ExpenseTypeModel[]>([]);
const incomeTypeSelectOpen = ref<boolean>(false);
const incomeFormOpen = ref<boolean>(false);
const incomeFormMode = ref<FormMode>(FormMode.ADD);
const selectedIncomeType = ref<IncomeType | null>(null);
const selectedIncomeTypeId = ref<string>('');
const selectedIncome = ref<Income | undefined>(undefined);
const expenseTypeSelectOpen = ref<boolean>(false);
const expenseFormOpen = ref<boolean>(false);
const expenseFormMode = ref<FormMode>(FormMode.ADD);
const selectedExpenseTypeId = ref<string>('');
const drawerOpen = ref<boolean>(false);

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

const currentMonthDataId = computed(() => {
  return currentMonthData.value?.id ?? '';
});

const incomesWithNames = computed(() => {
  return incomes.value.map(income => {
    const incomeType = incomeTypes.value.find(it => it.id === income.incomeTypeId);
    const typeLabel = incomeType ? getTypeLabel(incomeType.type) : '';

    return {
      id: income.id,
      name: incomeType?.name ?? 'Receita',
      type: typeLabel,
      value: income.calculatedValue
    };
  });
});

function getTypeLabel(type: string): string {
  const labels: Record<string, string> = {
    [IncomeType.PAYCHECK]: t('incomeTypes.typePaycheck'),
    [IncomeType.HOURLY]: t('incomeTypes.typeHourly'),
    [IncomeType.EXTRA]: t('incomeTypes.typeExtra')
  };

  return labels[type] ?? type;
}

const expensesWithNames = computed(() => {
  return expenses.value.map(expense => {
    const expenseType = expenseTypes.value.find(et => et.id === expense.expenseTypeId);

    return {
      id: expense.id,
      name: expenseType?.name ?? 'Despesa',
      value: expense.value
    };
  });
});

async function loadIncomeTypes(): Promise<void> {
  try {
    const types = (await localStorageService.get('incomeTypes')) as IncomeTypeModel[];
    incomeTypes.value = types;
  } catch (error) {
    notify.error(t('incomeTypes.loadError'), String(error));
  }
}

async function loadExpenseTypes(): Promise<void> {
  try {
    const types = (await localStorageService.get('expenseTypes')) as ExpenseTypeModel[];
    expenseTypes.value = types;
  } catch (error) {
    notify.error(t('expenseTypes.loadError'), String(error));
  }
}

async function loadMonth(): Promise<void> {
  loading.show(t('common.loading'));

  try {
    const monthDataList = (await localStorageService.get('month_data')) as MonthData[];
    let foundMonthData = monthDataList.find(
      md => md.year === currentYear.value && md.month === currentMonth.value
    );

    if (!foundMonthData) {
      const newMonthData = await localStorageService.post('month_data', {
        year: currentYear.value,
        month: currentMonth.value,
        lastAccessed: new Date()
      } as Partial<MonthData>);

      foundMonthData = newMonthData as MonthData;
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
  drawerOpen.value = true;
}

function handleAddIncome(): void {
  if (!currentMonthData.value) {
    notify.error('Erro', 'Nenhum mês selecionado');

    return;
  }

  incomeTypeSelectOpen.value = true;
}

function handleIncomeTypeSelected(incomeType: IncomeTypeModel): void {
  selectedIncomeType.value = incomeType.type as IncomeType;
  selectedIncomeTypeId.value = incomeType.id;
  incomeFormMode.value = FormMode.ADD;
  selectedIncome.value = undefined;
  incomeFormOpen.value = true;
}

function handleEditIncome(id: string): void {
  const income = incomes.value.find(i => i.id === id);
  if (!income) return;

  const incomeType = incomeTypes.value.find(it => it.id === income.incomeTypeId);
  if (!incomeType) return;

  selectedIncomeType.value = incomeType.type as IncomeType;
  selectedIncomeTypeId.value = incomeType.id;
  selectedIncome.value = income;
  incomeFormMode.value = FormMode.EDIT;
  incomeFormOpen.value = true;
}

async function handleDeleteIncome(id: string): Promise<void> {
  const confirmed = await confirm.show(
    t('common.delete'),
    t('monthBalance.deleteIncomeConfirm'),
    {
      confirmText: t('common.yes'),
      cancelText: t('common.no'),
      confirmColor: 'error'
    }
  );

  if (!confirmed) return;

  loading.show(t('common.loading'));

  try {
    await localStorageService.delete('incomes', id);
    notify.success(t('monthBalance.incomeDeleted'), '');
    await loadMonth();
  } catch (error) {
    notify.error(t('monthBalance.deleteError'), String(error));
  } finally {
    loading.hide();
  }
}

async function handleIncomeSaved(): Promise<void> {
  await loadMonth();
}

function handleAddExpense(): void {
  if (!currentMonthData.value) {
    notify.error('Erro', 'Nenhum mês selecionado');

    return;
  }

  expenseTypeSelectOpen.value = true;
}

function handleExpenseTypeSelected(expenseType: ExpenseTypeModel): void {
  console.log('🔍 Tipo de despesa selecionado:', expenseType);
  console.log('🔍 ID do tipo:', expenseType.id);
  console.log('🔍 Nome do tipo:', expenseType.name);
  
  selectedExpenseTypeId.value = expenseType.id;
  expenseFormMode.value = FormMode.ADD;
  expenseFormOpen.value = true;
  
  console.log('🔍 selectedExpenseTypeId após set:', selectedExpenseTypeId.value);
}

async function handleEditExpenseInline(id: string, value: number): Promise<void> {
  loading.show(t('common.loading'));

  try {
    await localStorageService.put<Expense>('expenses', id, { value });
    notify.success(t('monthBalance.expenseUpdated'), '');
    await loadMonth();
  } catch (error) {
    notify.error(t('monthBalance.saveError'), String(error));
  } finally {
    loading.hide();
  }
}

async function handleDeleteExpense(id: string): Promise<void> {
  const confirmed = await confirm.show(
    t('common.delete'),
    t('monthBalance.deleteExpenseConfirm'),
    {
      confirmText: t('common.yes'),
      cancelText: t('common.no'),
      confirmColor: 'error'
    }
  );

  if (!confirmed) return;

  loading.show(t('common.loading'));

  try {
    await localStorageService.delete('expenses', id);
    notify.success(t('monthBalance.expenseDeleted'), '');
    await loadMonth();
  } catch (error) {
    notify.error(t('monthBalance.deleteError'), String(error));
  } finally {
    loading.hide();
  }
}

async function handleExpenseSaved(): Promise<void> {
  await loadMonth();
}

onMounted(async () => {
  loadLastAccessedMonth();
  await loadIncomeTypes();
  await loadExpenseTypes();
  await loadMonth();
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

.theme-toggle-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  padding: 0 16px;
}

.theme-label {
  font-size: 0.875rem;
  opacity: 0.8;
}
</style>
