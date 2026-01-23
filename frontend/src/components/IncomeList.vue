<template>
  <div class="income-list">
    <div class="section-header-wrapper">
      <button
        class="section-header"
        @click="toggleCollapse"
      >
        <div class="header-content">
          <v-icon :class="{ 'rotated': collapsed }">
            mdi-chevron-down
          </v-icon>
          <h3 class="section-title">
            Receitas
          </h3>
        </div>
        <div class="divider" />
      </button>
      <IconToolTip
        icon="mdi-plus"
        tooltip="Adicionar receita"
        color="primary"
        size="small"
        @click="$emit('add')"
      />
    </div>

    <div
      v-if="!collapsed"
      class="items-container"
    >
      <div
        v-for="monthIncome in monthIncomes"
        :key="monthIncome.id"
        class="income-item"
      >
        <div class="item-actions">
          <IconToolTip
            icon="mdi-delete"
            tooltip="Remover"
            color="error"
            size="small"
            @click="handleDelete(monthIncome.id)"
          />
        </div>

        <div class="item-info">
          <span class="item-name">{{ monthIncome.incomeDescription }}</span>
        </div>

        <div class="item-value">
          <button
            class="value-button"
            @click="$emit('edit', monthIncome)"
          >
            {{ formatValue(monthIncome) }}
          </button>
        </div>
      </div>

      <div
        v-if="monthIncomes.length === 0"
        class="empty-state"
      >
        Nenhuma receita cadastrada
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useIncomeStore } from '@/stores/income'
import { useMonthStore } from '@/stores/month'
import { formatCurrency } from '@/utils/currency'
import type { MonthIncome } from '@/models/MonthIncome'
import { IconToolTip, confirm } from '@wallacesw11/base-lib'

defineEmits<{
  edit: [monthIncome: MonthIncome]
  add: []
}>()

const incomeStore = useIncomeStore()
const monthStore = useMonthStore()

const collapsed = ref(false)

const monthIncomes = computed(() => incomeStore.monthIncomes)

function toggleCollapse(): void {
  collapsed.value = !collapsed.value
}

function formatValue(monthIncome: MonthIncome): string {
  const value = monthIncome.netValue ?? 0
  
  return formatCurrency(value, 'pt-BR')
}

async function handleDelete(id: number): Promise<void> {
  const confirmed = await confirm.show(
    'Confirmar exclusão',
    'Tem certeza que deseja remover esta receita do mês?',
    {
      confirmText: 'Remover',
      cancelText: 'Cancelar'
    }
  )

  if (!confirmed) return

  try {
    await incomeStore.deleteMonthIncome(
      monthStore.currentYear,
      monthStore.currentMonth,
      id
    )
  } catch (error: any) {
    alert(error.response?.data?.message || 'Erro ao remover receita')
  }
}
</script>

<style scoped>
.income-list {
  padding-top: 8px;
  padding-bottom: 8px;
}

.section-header-wrapper {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 0 16px;
  margin-bottom: 8px;
}

.section-header {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: transparent;
  border: none;
  cursor: pointer;
  transition: opacity 0.2s;
  padding: 0;
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
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.08);
  transition: background-color 0.2s;
}

.income-item:hover {
  background-color: rgba(var(--v-theme-on-surface), 0.02);
}

.item-actions {
  display: flex;
  align-items: center;
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

.value-button {
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
  cursor: pointer;
}

.value-button:hover {
  background-color: rgba(var(--v-theme-on-surface), 0.04);
  color: rgb(var(--v-theme-primary));
}

.value-button:active {
  background-color: rgba(var(--v-theme-on-surface), 0.08);
}

.empty-state {
  padding: 32px 16px;
  text-align: center;
  color: rgba(var(--v-theme-on-surface), 0.4);
  font-size: 14px;
}
</style>
