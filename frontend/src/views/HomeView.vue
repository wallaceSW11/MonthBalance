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
import { confirm, notify, loading } from '@wallacesw11/base-lib';
import { incomeService } from '@/services/incomeService';
import { incomeTypeService } from '@/services/incomeTypeService';
import { expenseService } from '@/services/expenseService';
import { expenseTypeService } from '@/services/expenseTypeService';
import { monthDataService } from '@/services/monthDataService';
import { IncomeType, FormMode } from '@/models';
import type { MonthData, Income, Expense, IncomeTypeModel, ExpenseTypeModel } from '@/models';
import MonthNavigator from '@/components/MonthNavigator.vue';
import IncomeList from '@/components/IncomeList.vue';
import ExpenseList from '@/components/ExpenseList.vue';
import IncomeTypeSelectModal from '@/components/IncomeTypeSelectModal.vue';
import IncomeFormModal from '@/components/IncomeFormModal.vue';
import ExpenseTypeSelectModal from '@/components/ExpenseTypeSelectModal.vue';
import ExpenseFormModal from '@/components/ExpenseFormModal.vue';

const emit = defineEmits<{
  toggleDrawer: []
}>();

const { t } = useI18n();

const LAST_MONTH_KEY = 'monthbalance_last_month';
const MIN_YEAR = 2026;
const MIN_MONTH = 1;
const MAX_MONTHS_AHEAD = 5;

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
const selectedIncomeTypeId = ref<string>('0');
const selectedIncome = ref<Income | undefined>(undefined);
const expenseTypeSelectOpen = ref<boolean>(false);
const expenseFormOpen = ref<boolean>(false);
const expenseFormMode = ref<FormMode>(FormMode.ADD);
const selectedExpenseTypeId = ref<string>('0');
const allMonthData = ref<MonthData[]>([]);

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
  const lastMonth = getLastRegisteredMonth();

  if (!lastMonth) return true;

  const monthsAhead = calculateMonthsDifference(
    lastMonth.year,
    lastMonth.month,
    currentYear.value,
    currentMonth.value
  );

  return monthsAhead < MAX_MONTHS_AHEAD;
});

const currentMonthDataId = computed(() => {
  return currentMonthData.value?.id ? String(currentMonthData.value.id) : '0';
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
    incomeTypes.value = await incomeTypeService.getAll();
  } catch (error) {
    notify.error(t('incomeTypes.loadError'), String(error));
  }
}

async function loadExpenseTypes(): Promise<void> {
  try {
    expenseTypes.value = await expenseTypeService.getAll();
  } catch (error) {
    notify.error(t('expenseTypes.loadError'), String(error));
  }
}

async function loadMonth(skipDuplicatePrompt: boolean = false): Promise<void> {
  loading.show(t('common.loading'));

  try {
    const monthDataList = await monthDataService.getAll();

    allMonthData.value = monthDataList;

    let foundMonthData = monthDataList.find(
      md => md.year === currentYear.value && md.month === currentMonth.value
    );

    if (!foundMonthData) {
      if (!skipDuplicatePrompt) {
        loading.hide();

        const shouldDuplicate = await promptDuplicatePreviousMonth();

        if (shouldDuplicate) {
          await duplicatePreviousMonth();

          return;
        }

        loading.show(t('common.loading'));
      }

      const newMonthData = await monthDataService.create(currentYear.value, currentMonth.value);

      foundMonthData = newMonthData;
      allMonthData.value.push(foundMonthData);
    }

    currentMonthData.value = foundMonthData;

    incomes.value = await incomeService.getByMonth(foundMonthData.id);
    expenses.value = await expenseService.getByMonth(foundMonthData.id);

    await monthDataService.updateLastAccessed(foundMonthData.id);

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

function getLastRegisteredMonth(): { year: number; month: number } | null {
  if (allMonthData.value.length === 0) return null;

  const sorted = [...allMonthData.value].sort((a, b) => {
    if (a.year !== b.year) return b.year - a.year;

    return b.month - a.month;
  });

  return { year: sorted[0].year, month: sorted[0].month };
}

function calculateMonthsDifference(
  fromYear: number,
  fromMonth: number,
  toYear: number,
  toMonth: number
): number {
  return (toYear - fromYear) * 12 + (toMonth - fromMonth);
}

function getPreviousMonth(): { year: number; month: number } {
  if (currentMonth.value === 1) {
    return { year: currentYear.value - 1, month: 12 };
  }

  return { year: currentYear.value, month: currentMonth.value - 1 };
}

async function promptDuplicatePreviousMonth(): Promise<boolean> {
  const previous = getPreviousMonth();
  const previousMonthData = allMonthData.value.find(
    md => md.year === previous.year && md.month === previous.month
  );

  if (!previousMonthData) return false;

  const monthName = t(`monthBalance.months.${previous.month}`);

  const confirmed = await confirm.show(
    t('monthBalance.duplicateMonth'),
    t('monthBalance.duplicateMonthConfirm', { month: `${monthName} ${previous.year}` }),
    { confirmText: t('common.yes'), cancelText: t('common.no') }
  );

  return confirmed;
}

async function duplicatePreviousMonth(): Promise<void> {
  loading.show(t('common.loading'));

  try {
    const previous = getPreviousMonth();
    const previousMonthData = allMonthData.value.find(
      md => md.year === previous.year && md.month === previous.month
    );

    if (!previousMonthData) {
      notify.error(t('monthBalance.duplicateError'), 'Mês anterior não encontrado');

      return;
    }

    const newMonthData = await monthDataService.create(currentYear.value, currentMonth.value);

    const previousIncomes = await incomeService.getByMonth(previousMonthData.id);

    for (const income of previousIncomes) {
      await incomeService.create({
        monthDataId: newMonthData.id,
        incomeTypeId: income.incomeTypeId,
        grossValue: income.grossValue,
        netValue: income.netValue,
        hourlyRate: income.hourlyRate,
        hours: income.hours,
        minutes: income.minutes
      });
    }

    const previousExpenses = await expenseService.getByMonth(previousMonthData.id);

    for (const expense of previousExpenses) {
      await expenseService.create(newMonthData.id, expense.expenseTypeId, expense.value);
    }

    await loadMonth(true);
  } catch (error) {
    notify.error(t('monthBalance.duplicateError'), String(error));
  } finally {
    loading.hide();
    notify.success(t('monthBalance.duplicateSuccess'), '');
  }
}

async function handleDuplicateMonth(): Promise<void> {
  if (!currentMonthData.value) {
    notify.error('Erro', 'Nenhum mês selecionado');

    return;
  }

  const nextMonth = currentMonth.value === 12 ? 1 : currentMonth.value + 1;
  const nextYear = currentMonth.value === 12 ? currentYear.value + 1 : currentYear.value;

  const nextMonthExists = allMonthData.value.find(
    md => md.year === nextYear && md.month === nextMonth
  );

  if (nextMonthExists) {
    notify.error(t('monthBalance.duplicateError'), 'Próximo mês já existe');

    return;
  }

  const lastMonth = getLastRegisteredMonth();

  if (lastMonth) {
    const monthsAhead = calculateMonthsDifference(lastMonth.year, lastMonth.month, nextYear, nextMonth);

    if (monthsAhead > MAX_MONTHS_AHEAD) {
      notify.error(t('monthBalance.duplicateError'), `Não é possível criar mês mais de ${MAX_MONTHS_AHEAD} meses à frente`);

      return;
    }
  }

  const monthName = t(`monthBalance.months.${nextMonth}`);

  const confirmed = await confirm.show(
    t('monthBalance.duplicateMonth'),
    `Copiar dados do mês atual para ${monthName} ${nextYear}?`,
    { confirmText: t('common.yes'), cancelText: t('common.no') }
  );

  if (!confirmed) return;

  loading.show(t('common.loading'));

  try {
    const newMonthData = await monthDataService.create(nextYear, nextMonth);

    for (const income of incomes.value) {
      await incomeService.create({
        monthDataId: newMonthData.id,
        incomeTypeId: income.incomeTypeId,
        grossValue: income.grossValue,
        netValue: income.netValue,
        hourlyRate: income.hourlyRate,
        hours: income.hours,
        minutes: income.minutes
      });
    }

    for (const expense of expenses.value) {
      await expenseService.create(newMonthData.id, expense.expenseTypeId, expense.value);
    }

    currentYear.value = nextYear;
    currentMonth.value = nextMonth;
    await loadMonth(true);
  } catch (error) {
    notify.error(t('monthBalance.duplicateError'), String(error));
  } finally {
    loading.hide();
    notify.success(t('monthBalance.duplicateSuccess'), '');
  }
}

async function handleClearMonth(): Promise<void> {
  if (!currentMonthData.value) {
    notify.error('Erro', 'Nenhum mês selecionado');

    return;
  }

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

  loading.show(t('common.loading'));

  try {
    for (const income of incomes.value) {
      await incomeService.remove(income.id);
    }

    for (const expense of expenses.value) {
      await expenseService.remove(expense.id);
    }

    await loadMonth(true);
  } catch (error) {
    notify.error(t('monthBalance.clearError'), String(error));
  } finally {
    loading.hide();
    notify.success(t('monthBalance.clearSuccess'), '');
  }
}

function handleMenuClick(): void {
  emit('toggleDrawer');
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
  selectedIncomeTypeId.value = String(incomeType.id);
  incomeFormMode.value = FormMode.ADD;
  selectedIncome.value = undefined;
  incomeFormOpen.value = true;
}

function handleEditIncome(id: number): void {
  const income = incomes.value.find(i => i.id === id);

  if (!income) return;

  const incomeType = incomeTypes.value.find(it => it.id === income.incomeTypeId);

  if (!incomeType) return;

  selectedIncomeType.value = incomeType.type as IncomeType;
  selectedIncomeTypeId.value = String(incomeType.id);
  selectedIncome.value = income;
  incomeFormMode.value = FormMode.EDIT;
  incomeFormOpen.value = true;
}

async function handleDeleteIncome(id: number): Promise<void> {
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
    await incomeService.remove(id);
    await loadMonth();
  } catch (error) {
    notify.error(t('monthBalance.deleteError'), String(error));
  } finally {
    loading.hide();
    notify.success(t('monthBalance.incomeDeleted'), '');
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

  selectedExpenseTypeId.value = String(expenseType.id);
  expenseFormMode.value = FormMode.ADD;
  expenseFormOpen.value = true;

  console.log('🔍 selectedExpenseTypeId após set:', selectedExpenseTypeId.value);
}

async function handleEditExpenseInline(id: number, value: number): Promise<void> {
  loading.show(t('common.loading'));

  try {
    await expenseService.update(id, value);
    await loadMonth();
  } catch (error) {
    notify.error(t('monthBalance.saveError'), String(error));
  } finally {
    loading.hide();
    notify.success(t('monthBalance.expenseUpdated'), '');
  }
}

async function handleDeleteExpense(id: number): Promise<void> {
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
    await expenseService.remove(id);
    await loadMonth();
  } catch (error) {
    notify.error(t('monthBalance.deleteError'), String(error));
  } finally {
    loading.hide();
    notify.success(t('monthBalance.expenseDeleted'), '');
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
</style>
