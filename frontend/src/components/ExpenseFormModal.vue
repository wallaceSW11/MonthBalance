<template>
  <ModalBase
    v-model="internalOpen"
    :title="modalTitle"
    :actions="actions"
    :max-width="500"
  >
    <div class="form-fields">
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
import { ModalBase, MoneyField, notify, loading } from '@wallacesw11/base-lib';
import type { ModalAction } from '@wallacesw11/base-lib/components';
import { expenseService } from '@/services/expenseService';
import { FormMode } from '@/models';
import type { Expense } from '@/models';

interface ExpenseForm {
  value: number;
}

const props = defineProps<{
  open: boolean;
  mode: FormMode;
  expenseTypeId: string;
  monthDataId: string;
  initialData?: Expense;
}>();

const emit = defineEmits<{
  'update:open': [value: boolean];
  saved: [];
}>();

const { t } = useI18n();

const formRef = ref();
const form = ref<ExpenseForm>({
  value: 0
});

const internalOpen = computed({
  get: () => props.open,
  set: (value: boolean) => emit('update:open', value)
});

const modalTitle = computed(() => t('monthBalance.expenses'));

const isAddMode = computed(() => props.mode === FormMode.ADD);

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
    value: 0
  };

  formRef.value?.resetValidation();
}

function loadInitialData(): void {
  if (!props.initialData) return;

  form.value = {
    value: props.initialData.value ?? 0
  };
}

async function handleSave(): Promise<void> {
  loading.show(t('common.loading'));

  try {
    if (isAddMode.value) {
      await expenseService.create(
        Number(props.monthDataId),
        Number(props.expenseTypeId),
        form.value.value
      );
      notify.success(t('monthBalance.expenseAdded'), '');
      resetForm();
      internalOpen.value = false;
    } else {
      if (!props.initialData?.id) return;

      await expenseService.update(props.initialData.id, form.value.value);
      notify.success(t('monthBalance.expenseUpdated'), '');
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
</style>
