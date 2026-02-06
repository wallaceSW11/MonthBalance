<template>
  <div class="income-list">
    <div class="list-header" @click="toggleExpanded">
      <div class="d-flex align-center gap-2">
        <v-icon :class="expandIconClass" size="small">mdi-chevron-down</v-icon>
        <h3 class="section-title">{{ t('monthBalance.incomes') }}</h3>
      </div>

      <div class="header-divider" />

      <v-btn icon="mdi-plus" size="small" variant="text" @click.stop="emit('add')" />
    </div>

    <div v-if="expanded" class="list-content">
      <div v-if="incomes.length === 0" class="empty-state">
        {{ t('monthBalance.noIncomes') }}
      </div>

      <IncomeItem
        v-for="income in incomes"
        :key="income.id"
        :name="income.name"
        :type="income.type"
        :value="income.value"
        @edit="emit('edit', income.id)"
        @delete="emit('delete', income.id)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useI18n } from 'vue-i18n';
import IncomeItem from './IncomeItem.vue';

interface IncomeListItem {
  id: string;
  name: string;
  type: string;
  value: number;
}

defineProps<{
  incomes: IncomeListItem[];
}>();

const emit = defineEmits<{
  add: [];
  edit: [id: string];
  delete: [id: string];
}>();

const { t } = useI18n();

const STORAGE_KEY = 'monthbalance_incomes_expanded';

const expanded = ref(loadExpandedState());

const expandIconClass = computed(() => {
  return expanded.value ? '' : 'rotate-icon';
});

function loadExpandedState(): boolean {
  const saved = localStorage.getItem(STORAGE_KEY);

  return saved ? JSON.parse(saved) : true;
}

function saveExpandedState(): void {
  localStorage.setItem(STORAGE_KEY, JSON.stringify(expanded.value));
}

function toggleExpanded(): void {
  expanded.value = !expanded.value;
  saveExpandedState();
}
</script>

<style scoped>
.income-list {
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
