<template>
  <div class="expense-list">
    <div class="list-header" @click="toggleExpanded">
      <div class="d-flex align-center gap-2">
        <v-icon :class="expandIconClass" size="small">mdi-chevron-down</v-icon>
        <h3 class="section-title">{{ t('monthBalance.expenses') }}</h3>
      </div>

      <div class="header-divider" />
    </div>

    <div v-if="expanded" class="list-content">
      <div v-if="expenses.length === 0" class="empty-state">
        {{ t('monthBalance.noExpenses') }}
      </div>

      <ExpenseItem
        v-for="expense in expenses"
        :key="expense.id"
        :name="expense.name"
        :value="expense.value"
        @edit="emit('edit', expense.id)"
        @delete="emit('delete', expense.id)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useI18n } from 'vue-i18n';
import ExpenseItem from './ExpenseItem.vue';

interface ExpenseListItem {
  id: string;
  name: string;
  value: number;
}

defineProps<{
  expenses: ExpenseListItem[];
}>();

const emit = defineEmits<{
  edit: [id: string];
  delete: [id: string];
}>();

const { t } = useI18n();

const expanded = ref(true);

const expandIconClass = computed(() => {
  return expanded.value ? '' : 'rotate-icon';
});

function toggleExpanded(): void {
  expanded.value = !expanded.value;
}
</script>

<style scoped>
.expense-list {
  padding: 16px;
  padding-top: 0;
}

.list-header {
  display: flex;
  align-items: center;
  gap: 12px;
  cursor: pointer;
  user-select: none;
}

.gap-2 {
  gap: 8px;
}

.section-title {
  font-size: 12px;
  font-weight: 700;
  letter-spacing: 0.1em;
  opacity: 0.6;
  white-space: nowrap;
}

.header-divider {
  flex: 1;
  height: 1px;
  background-color: rgba(var(--v-theme-on-surface), 0.12);
}

.empty-state {
  text-align: center;
  padding: 32px 16px;
  opacity: 0.6;
  font-size: 0.875rem;
}

.rotate-icon {
  transform: rotate(-90deg);
}

.rotate-icon,
.list-header v-icon {
  transition: transform 0.3s ease;
}
</style>
