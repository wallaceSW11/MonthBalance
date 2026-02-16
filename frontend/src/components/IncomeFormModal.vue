<template>
  <ModalBase
    v-model="internalOpen"
    :title="modalTitle"
    :actions="actions"
    :max-width="500"
  >
    <div v-if="!props.incomeType" class="empty-state">
      {{ t('monthBalance.selectIncomeType') }}
    </div>

    <div v-if="isPaycheck" class="form-fields">
      <MoneyField
        v-model="form.grossValue"
        :label="t('monthBalance.grossValue')"
        class="mb-4"
      />

      <MoneyField
        v-model="form.netValue"
        :label="t('monthBalance.netValue')"
        class="mb-4"
      />
    </div>

    <div v-if="isHourly" class="form-fields">
      <MoneyField
        v-model="form.hourlyRate"
        :label="t('monthBalance.hourlyRate')"
        class="mb-4"
      />

      <NumberField
        v-model="form.hours"
        :label="t('monthBalance.hours')"
        :decimal-places="0"
        class="mb-4"
      />

      <NumberField
        v-model="form.minutes"
        :label="t('monthBalance.minutes')"
        :decimal-places="0"
        class="mb-4"
      />

      <div v-if="hourlyPreview > 0" class="preview-value">
        <span class="preview-label">{{ t('monthBalance.calculatedValue') }}:</span>
        <span class="preview-amount">{{ formattedHourlyPreview }}</span>
      </div>
    </div>

    <div v-if="isExtra" class="form-fields">
      <MoneyField
        v-model="form.value"
        :label="t('monthBalance.value')"
        class="mb-4"
      />
    </div>
  </ModalBase>
</template>

<script setup lang="ts">
import { ref, computed, watch, nextTick } from 'vue';
import { useI18n } from 'vue-i18n';
import { ModalBase, MoneyField, NumberField, notify, loading } from '@wallacesw11/base-lib';
import type { ModalAction } from '@wallacesw11/base-lib/components';
import { incomeService } from '@/services/incomeService';
import { IncomeType, FormMode } from '@/models';
import type { Income } from '@/models';

interface IncomeForm {
  grossValue: number;
  netValue: number;
  hourlyRate: number;
  hours: number;
  minutes: number;
  value: number;
}

const props = defineProps<{
  open: boolean;
  mode: FormMode;
  incomeType: IncomeType | null;
  incomeTypeId: string;
  monthDataId: string;
  initialData?: Income;
}>();

const emit = defineEmits<{
  'update:open': [value: boolean];
  saved: [];
}>();

const { t } = useI18n();

const formRef = ref();
const form = ref<IncomeForm>({
  grossValue: 0,
  netValue: 0,
  hourlyRate: 0,
  hours: 0,
  minutes: 0,
  value: 0
});

const internalOpen = computed({
  get: () => props.open,
  set: (value: boolean) => emit('update:open', value)
});

const modalTitle = computed(() => t('monthBalance.incomes'));

const isPaycheck = computed(() => props.incomeType === IncomeType.PAYCHECK);
const isHourly = computed(() => props.incomeType === IncomeType.HOURLY);
const isExtra = computed(() => props.incomeType === IncomeType.EXTRA);
const isAddMode = computed(() => props.mode === FormMode.ADD);

const hourlyPreview = computed(() => {
  if (!isHourly.value) return 0;

  const totalHours = form.value.hours + form.value.minutes / 60;

  return totalHours * form.value.hourlyRate;
});

const formattedHourlyPreview = computed(() => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(hourlyPreview.value);
});

const actions = computed<ModalAction[]>(() => [
  {
    text: t('common.cancel'),
    color: 'secondary',
    variant: 'outlined',
    handler: handleCancel
  },
  {
    text: t('common.save'),
    color: 'primary',
    variant: 'elevated',
    handler: handleSave
  }
]);

function resetForm(): void {
  form.value = {
    grossValue: 0,
    netValue: 0,
    hourlyRate: 0,
    hours: 0,
    minutes: 0,
    value: 0
  };

  formRef.value?.resetValidation();
}

function loadInitialData(): void {
  if (!props.initialData) return;

  form.value = {
    grossValue: props.initialData.grossValue ?? 0,
    netValue: props.initialData.netValue ?? 0,
    hourlyRate: props.initialData.hourlyRate ?? 0,
    hours: props.initialData.hours ?? 0,
    minutes: props.initialData.minutes ?? 0,
    value: props.initialData.calculatedValue ?? 0
  };
}

async function handleSave(): Promise<void> {
  loading.show(t('common.loading'));

  try {
    const incomeData: Partial<Income> = {
      monthDataId: Number(props.monthDataId),
      incomeTypeId: Number(props.incomeTypeId),
      grossValue: null,
      netValue: null,
      hourlyRate: null,
      hours: null,
      minutes: null
    };

    if (isPaycheck.value) {
      incomeData.grossValue = form.value.grossValue;
      incomeData.netValue = form.value.netValue;
    }

    if (isHourly.value) {
      incomeData.hourlyRate = form.value.hourlyRate;
      incomeData.hours = form.value.hours;
      incomeData.minutes = form.value.minutes;
    }

    if (isExtra.value) {
      incomeData.grossValue = form.value.value;
      incomeData.netValue = form.value.value;
    }

    if (isAddMode.value) {
      const createData = {
        monthDataId: incomeData.monthDataId!,
        incomeTypeId: incomeData.incomeTypeId!,
        grossValue: incomeData.grossValue ?? null,
        netValue: incomeData.netValue ?? null,
        hourlyRate: incomeData.hourlyRate ?? null,
        hours: incomeData.hours ?? null,
        minutes: incomeData.minutes ?? null
      };

      await incomeService.create(createData);
      notify.success(t('monthBalance.incomeAdded'), '');
      resetForm();
      internalOpen.value = false;
    } else {
      if (!props.initialData?.id) return;

      const updateData = {
        grossValue: incomeData.grossValue,
        netValue: incomeData.netValue,
        hourlyRate: incomeData.hourlyRate,
        hours: incomeData.hours,
        minutes: incomeData.minutes
      };

      await incomeService.update(props.initialData.id, updateData);
      notify.success(t('monthBalance.incomeUpdated'), '');
      internalOpen.value = false;
    }

    emit('saved');
  } catch (error) {
    notify.error(t('monthBalance.saveError'), String(error));
  } finally {
    loading.hide();
  }
}

function handleCancel(): void {
  internalOpen.value = false;
}

watch(() => props.open, (newValue) => {
  if (!newValue) return;

  if (props.mode === FormMode.EDIT) {
    loadInitialData();
  } else {
    resetForm();
  }

  nextTick(() => {
    const firstInput = document.querySelector('.form-fields input') as HTMLInputElement;
    if (firstInput) firstInput.focus();
  });
});
</script>

<style scoped>
.form-fields {
  padding: 16px 0;
}

.empty-state {
  text-align: center;
  padding: 32px 16px;
  opacity: 0.6;
  font-size: 0.875rem;
}

.preview-value {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  background-color: rgba(var(--v-theme-primary), 0.1);
  border-radius: 8px;
  margin-top: 8px;
}

.preview-label {
  font-size: 0.875rem;
  opacity: 0.8;
}

.preview-amount {
  font-size: 1.25rem;
  font-weight: 700;
  color: rgb(var(--v-theme-success));
}
</style>
