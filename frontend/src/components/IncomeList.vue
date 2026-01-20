<template>
  <div class="income-list">
    <button
      class="section-header"
      @click="toggleCollapse"
    >
      <div class="header-content">
        <v-icon :class="{ 'rotated': collapsed }">
          mdi-chevron-down
        </v-icon>
        <h3 class="section-title">
          {{ t('dashboard.incomes') }}
        </h3>
      </div>
      <div class="divider" />
    </button>

    <div
      v-show="!collapsed"
      class="items-container"
    >
      <div
        v-for="income in incomes"
        :key="income.id"
        class="income-item"
      >
        <div class="item-info">
          <span class="item-name">{{ income.name }}</span>
        </div>

        <div class="item-value">
          <input
            :value="formatValue(income)"
            type="text"
            inputmode="decimal"
            class="value-input"
            @input="handleValueChange(income, $event)"
            @blur="saveIncome(income)"
          >
        </div>
      </div>

      <div
        v-if="incomes.length === 0"
        class="empty-state"
      >
        {{ t('dashboard.noIncomes') }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useIncomeStore } from '@/stores/income'
import { formatCurrency, parseCurrency } from '@/utils/currency'
import { uiStorageService } from '@/services/storage/UIStorageService'
import type { Income } from '@/models/Income'

const { t, locale } = useI18n()
const incomeStore = useIncomeStore()

const collapsed = ref(false)
const pendingChanges = ref<Map<string, Income>>(new Map())

const incomes = computed(() => incomeStore.incomes)

function toggleCollapse(): void {
  collapsed.value = !collapsed.value
  uiStorageService.updateIncomesCollapsed(collapsed.value)
}

function formatValue(income: Income): string {
  const value = incomeStore.calculateIncomeValue(income)
  
  return formatCurrency(value, locale.value)
}

function handleValueChange(income: Income, event: Event): void {
  const target = event.target as HTMLInputElement
  const newValue = parseCurrency(target.value, locale.value)
  
  if (income.type === 'manual') {
    const updatedIncome: Income = {
      ...income,
      netValue: newValue,
    }
    
    pendingChanges.value.set(income.id, updatedIncome)
  }
}

function saveIncome(income: Income): void {
  const pending = pendingChanges.value.get(income.id)
  
  if (!pending) return
  
  incomeStore.updateIncome(pending)
  pendingChanges.value.delete(income.id)
}

onMounted(() => {
  const uiState = uiStorageService.getUIState()
  
  collapsed.value = uiState.incomesCollapsed
})
</script>

<style scoped>
.income-list {
  padding-top: 24px;
  padding-bottom: 8px;
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

.income-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.08);
  transition: background-color 0.2s;
}

.income-item:hover {
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
