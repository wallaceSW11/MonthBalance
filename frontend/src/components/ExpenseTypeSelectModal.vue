<template>
  <ModalBase
    v-model="internalOpen"
    :title="t('monthBalance.selectExpenseType')"
    :actions="actions"
    :max-width="500"
  >
    <div v-if="expenseTypes.length === 0" class="empty-state">
      {{ t('expenseTypes.noData') }}
    </div>

    <div v-else class="expense-types-grid">
      <v-card
        v-for="expenseType in expenseTypes"
        :key="expenseType.id"
        class="expense-type-card"
        @click="handleSelect(expenseType)"
      >
        <v-card-text>
          <div class="expense-type-name">{{ expenseType.name }}</div>
        </v-card-text>
      </v-card>
    </div>
  </ModalBase>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { ModalBase } from '@wallacesw11/base-lib';
import type { ModalAction } from '@wallacesw11/base-lib/components';
import type { ExpenseTypeModel } from '@/models';

const props = defineProps<{
  open: boolean;
  expenseTypes: ExpenseTypeModel[];
}>();

const emit = defineEmits<{
  'update:open': [value: boolean];
  select: [expenseType: ExpenseTypeModel];
}>();

const { t } = useI18n();

const internalOpen = computed({
  get: () => props.open,
  set: (value: boolean) => emit('update:open', value)
});

const actions = computed<ModalAction[]>(() => [
  {
    text: t('common.cancel'),
    color: 'secondary',
    variant: 'outlined',
    handler: handleCancel
  }
]);

function handleCancel(): void {
  internalOpen.value = false;
}

function handleSelect(expenseType: ExpenseTypeModel): void {
  emit('select', expenseType);
  internalOpen.value = false;
}
</script>

<style scoped>
.empty-state {
  text-align: center;
  padding: 32px 16px;
  opacity: 0.6;
  font-size: 0.875rem;
}

.expense-types-grid {
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 8px 0;
}

.expense-type-card {
  cursor: pointer;
  transition: transform 0.2s, opacity 0.2s;
}

.expense-type-card:hover {
  transform: translateY(-2px);
  opacity: 0.9;
}

.expense-type-card:active {
  transform: translateY(0);
}

.expense-type-name {
  font-weight: 600;
  font-size: 1rem;
}
</style>
