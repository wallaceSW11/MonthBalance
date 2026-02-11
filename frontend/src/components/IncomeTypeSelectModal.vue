<template>
  <ModalBase
    v-model="internalOpen"
    :title="t('monthBalance.selectIncomeType')"
    :actions="actions"
    :max-width="500"
  >
    <div v-if="incomeTypes.length === 0" class="empty-state">
      {{ t('incomeTypes.noData') }}
    </div>

    <div v-else class="income-types-grid">
      <v-card
        v-for="incomeType in incomeTypes"
        :key="incomeType.id"
        class="income-type-card"
        @click="handleSelect(incomeType)"
      >
        <v-card-text>
          <div class="income-type-name">{{ incomeType.name }}</div>
          <div class="income-type-label">{{ getTypeLabel(incomeType.type) }}</div>
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
import type { IncomeTypeModel } from '@/models';

const props = defineProps<{
  open: boolean;
  incomeTypes: IncomeTypeModel[];
}>();

const emit = defineEmits<{
  'update:open': [value: boolean];
  select: [incomeType: IncomeTypeModel];
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
    handler: handleCancel
  }
]);

function handleCancel(): void {
  internalOpen.value = false;
}

function getTypeLabel(type: string): string {
  const labels: Record<string, string> = {
    paycheck: t('incomeTypes.typePaycheck'),
    hourly: t('incomeTypes.typeHourly'),
    extra: t('incomeTypes.typeExtra')
  };

  return labels[type] ?? type;
}

function handleSelect(incomeType: IncomeTypeModel): void {
  emit('select', incomeType);
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

.income-types-grid {
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 8px 0;
}

.income-type-card {
  cursor: pointer;
  transition: transform 0.2s, opacity 0.2s;
}

.income-type-card:hover {
  transform: translateY(-2px);
  opacity: 0.9;
}

.income-type-card:active {
  transform: translateY(0);
}

.income-type-name {
  font-weight: 600;
  font-size: 1rem;
  margin-bottom: 4px;
}

.income-type-label {
  font-size: 0.875rem;
  opacity: 0.7;
}
</style>
