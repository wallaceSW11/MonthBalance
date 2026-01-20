<template>
  <div class="summary-cards">
    <div class="summary-card">
      <span class="summary-label">{{ t('dashboard.incomes') }}</span>
      <span class="summary-value income">{{ formattedIncome }}</span>
    </div>

    <div class="summary-card divider-left">
      <span class="summary-label">{{ t('dashboard.expenses') }}</span>
      <span class="summary-value expense">{{ formattedExpense }}</span>
    </div>

    <div class="summary-card divider-left">
      <span class="summary-label">{{ t('dashboard.balance') }}</span>
      <span class="summary-value balance">{{ formattedBalance }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useMonthStore } from '@/stores/month'
import { formatCurrency } from '@/utils/currency'

const { t, locale } = useI18n()
const monthStore = useMonthStore()

const formattedIncome = computed(() => formatCurrency(monthStore.totalIncome, locale.value))
const formattedExpense = computed(() => formatCurrency(monthStore.totalExpense, locale.value))
const formattedBalance = computed(() => formatCurrency(monthStore.balance, locale.value))
</script>

<style scoped>
.summary-cards {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 8px;
  padding: 16px;
  padding-top: 4px;
  padding-bottom: 16px;
}

.summary-card {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.divider-left {
  border-left: 1px solid rgba(var(--v-theme-on-surface), 0.12);
}

.summary-label {
  font-size: 10px;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: rgba(var(--v-theme-on-surface), 0.6);
  font-weight: 500;
  margin-bottom: 4px;
}

.summary-value {
  font-size: 18px;
  font-weight: 700;
  font-variant-numeric: tabular-nums;
}

.summary-value.income {
  color: rgb(var(--v-theme-success));
}

.summary-value.expense {
  color: rgb(var(--v-theme-error));
}

.summary-value.balance {
  color: rgb(var(--v-theme-primary));
}
</style>
