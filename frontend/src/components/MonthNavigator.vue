<template>
  <div class="month-navigator-wrapper">
    <div class="month-navigator-header">
      <div class="header-grid">
        <div class="header-left">
          <v-btn icon="mdi-menu" variant="text" @click="emit('menu-click')" />
        </div>

        <div class="header-center">
          <v-btn
            icon="mdi-chevron-left"
            variant="text"
            size="small"
            :disabled="!canNavigatePrevious"
            @click="emit('navigate-previous')"
          />

          <v-menu>
            <template #activator="{ props }">
              <h2 class="month-title" v-bind="props">{{ monthLabel }}</h2>
            </template>

            <v-list>
              <v-list-item @click="emit('duplicate-month')">
                <v-list-item-title>{{ t('monthBalance.duplicateMonth') }}</v-list-item-title>
              </v-list-item>

              <v-list-item @click="emit('clear-month')">
                <v-list-item-title>{{ t('monthBalance.clearMonth') }}</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>

          <v-btn
            icon="mdi-chevron-right"
            variant="text"
            size="small"
            :disabled="!canNavigateNext"
            @click="emit('navigate-next')"
          />
        </div>

        <div class="header-right" />
      </div>
    </div>

    <div class="summary-container">
      <div class="summary-item">
        <div class="summary-label">{{ t('monthBalance.incomes') }}</div>
        <div class="summary-value income-value">{{ formatCurrency(totalIncome) }}</div>
      </div>

      <v-divider vertical class="summary-divider" />

      <div class="summary-item">
        <div class="summary-label">{{ t('monthBalance.expenses') }}</div>
        <div class="summary-value expense-value">{{ formatCurrency(totalExpense) }}</div>
      </div>

      <v-divider vertical class="summary-divider" />

      <div class="summary-item">
        <div class="summary-label">{{ t('monthBalance.balance') }}</div>
        <div class="summary-value" :class="balanceClass">{{ formatCurrency(balance) }}</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useI18n } from 'vue-i18n';

const props = defineProps<{
  year: number;
  month: number;
  canNavigatePrevious: boolean;
  canNavigateNext: boolean;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}>();

const emit = defineEmits<{
  'menu-click': [];
  'navigate-previous': [];
  'navigate-next': [];
  'duplicate-month': [];
  'clear-month': [];
}>();

const { t } = useI18n();

const monthLabel = computed(() => {
  const monthName = t(`monthBalance.months.${props.month}`);

  return `${monthName} ${props.year}`;
});

const balanceClass = computed(() => {
  if (props.balance > 0) return 'positive-balance';

  if (props.balance < 0) return 'negative-balance';

  return 'neutral-balance';
});

const formatCurrency = (value: number): string => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
};
</script>

<style scoped>
.month-navigator-wrapper {
  position: sticky;
  top: 0;
  z-index: 50;
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  background-color: rgba(var(--v-theme-surface), 0.9);
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.12);
}

.month-navigator-header {
  padding: 12px 16px;
}

.header-grid {
  display: grid;
  grid-template-columns: 1fr 3fr 1fr;
  align-items: center;
}

.header-left {
  display: flex;
  justify-content: flex-start;
}

.header-center {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.header-right {
  display: flex;
  justify-content: flex-end;
}

.month-title {
  font-size: 18px;
  font-weight: 700;
  letter-spacing: -0.02em;
  white-space: nowrap;
  cursor: pointer;
  user-select: none;
}

.summary-container {
  display: grid;
  grid-template-columns: 1fr auto 1fr auto 1fr;
  gap: 8px;
  padding: 4px 16px 16px;
}

.summary-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.summary-label {
  font-size: 10px;
  font-weight: 500;
  letter-spacing: 0.05em;
  opacity: 0.6;
  margin-bottom: 4px;
}

.summary-value {
  font-size: 18px;
  font-weight: 700;
  font-variant-numeric: tabular-nums;
}

.income-value {
  color: rgb(var(--v-theme-success));
}

.expense-value {
  color: rgb(var(--v-theme-error));
}

.positive-balance {
  color: rgb(var(--v-theme-success));
}

.negative-balance {
  color: rgb(var(--v-theme-error));
}

.neutral-balance {
  color: rgb(var(--v-theme-primary));
}

.summary-divider {
  opacity: 0.3;
}
</style>
