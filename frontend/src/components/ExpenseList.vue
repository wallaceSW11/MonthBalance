<template>
  <div class="expense-list">
    <button
      class="section-header"
      @click="toggleCollapse"
    >
      <div class="header-content">
        <v-icon :class="{ 'rotated': collapsed }">
          mdi-chevron-down
        </v-icon>
        <h3 class="section-title">
          {{ t('dashboard.expenses') }}
        </h3>
      </div>
      <div class="divider" />
    </button>

    <div
      v-if="!collapsed"
      class="items-container"
    >
      <div
        v-for="expense in expenses"
        :key="expense.id"
        class="expense-item"
      >
        <div class="item-info">
          <span class="item-name">{{ expense.name }}</span>
        </div>

        <div class="item-value">
          <input
            :value="formatValue(expense.value)"
            type="text"
            inputmode="decimal"
            class="value-input"
            @input="handleValueChange(expense, $event)"
            @blur="saveExpense(expense)"
          >
        </div>
      </div>

      <div
        v-if="expenses.length === 0"
        class="empty-state"
      >
        {{ t('dashboard.noExpenses') }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useExpenseStore } from '@/stores/expense'
import { formatCurrency, parseCurrency } from '@/utils/currency'
import { uiStorageService } from '@/services/storage/UIStorageService'
import type { Expense } from '@/models/Expense'

const { t, locale } = useI18n()
const expenseStore = useExpenseStore()

const collapsed = ref(false)
const pendingChanges = ref<Map<string, Expense>>(new Map())

const expenses = computed(() => expenseStore.expenses)

function toggleCollapse(): void {
  collapsed.value = !collapsed.value
  uiStorageService.updateExpensesCollapsed(collapsed.value)
}

function formatValue(value: number): string {
  return formatCurrency(value, locale.value)
}

function handleValueChange(expense: Expense, event: Event): void {
  const target = event.target as HTMLInputElement
  const newValue = parseCurrency(target.value, locale.value)
  
  const updatedExpense: Expense = {
    ...expense,
    value: newValue,
  }
  
  pendingChanges.value.set(expense.id, updatedExpense)
}

function saveExpense(expense: Expense): void {
  const pending = pendingChanges.value.get(expense.id)
  
  if (!pending) return
  
  expenseStore.updateExpense(pending)
  pendingChanges.value.delete(expense.id)
}

onMounted(() => {
  const uiState = uiStorageService.getUIState()
  
  collapsed.value = uiState.expensesCollapsed
})
</script>

<style scoped>
.expense-list {
  padding-top: 4px;
  padding-bottom: 96px;
}

.section-header {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 16px;
  margin-bottom: 8px;
  background: transparent;
  border: none;
  cursor: pointer;
  transition: opacity 0.2s;
}

.section-header:hover {
  opacity: 0.8;
}

.header-content {
  display: flex;
  align-items: center;
  gap: 8px;
}

.section-title {
  font-size: 12px;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.15em;
  color: rgba(var(--v-theme-on-surface), 0.6);
}

.section-header:hover .section-title {
  color: rgba(var(--v-theme-on-surface), 1);
}

.divider {
  flex: 1;
  height: 1px;
  background: rgba(var(--v-theme-on-surface), 0.08);
  margin-left: 16px;
}

.v-icon {
  color: rgba(var(--v-theme-on-surface), 0.6);
  font-size: 20px;
  transition: transform 0.3s, color 0.2s;
}

.v-icon.rotated {
  transform: rotate(-90deg);
}

.section-header:hover .v-icon {
  color: rgba(var(--v-theme-on-surface), 1);
}

.items-container {
  display: flex;
  flex-direction: column;
}

.expense-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.08);
  transition: background-color 0.2s;
}

.expense-item:hover {
  background-color: rgba(var(--v-theme-on-surface), 0.02);
}

.item-info {
  flex: 1;
  min-width: 0;
}

.item-name {
  font-size: 14px;
  font-weight: 500;
  color: rgba(var(--v-theme-on-surface), 1);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.item-value {
  width: 128px;
}

.value-input {
  width: 100%;
  background: transparent;
  text-align: right;
  font-size: 16px;
  font-weight: 500;
  color: rgba(var(--v-theme-on-surface), 1);
  border: none;
  padding: 4px 8px;
  border-radius: 4px;
  transition: background-color 0.2s, color 0.2s;
  font-variant-numeric: tabular-nums;
}

.value-input:hover {
  background-color: rgba(var(--v-theme-on-surface), 0.04);
}

.value-input:focus {
  outline: none;
  background-color: rgba(var(--v-theme-on-surface), 0.08);
  color: rgb(var(--v-theme-primary));
}

.empty-state {
  padding: 32px 16px;
  text-align: center;
  color: rgba(var(--v-theme-on-surface), 0.4);
  font-size: 14px;
}
</style>
