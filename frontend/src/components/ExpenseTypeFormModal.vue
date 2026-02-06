<template>
  <ModalBase
    :model-value="modelValue"
    :title="title"
    :actions="actions"
    :max-width="500"
    @update:model-value="handleModalUpdate"
  >
    <v-form ref="formRef" @submit.prevent="handleSave">
      <v-text-field
        ref="nameFieldRef"
        v-model="form.name"
        :label="t('expenseTypes.name')"
        :rules="[validateRequired]"
        variant="outlined"
        density="comfortable"
      />
    </v-form>
  </ModalBase>
</template>

<script setup lang="ts">
import { ref, computed, watch, nextTick } from 'vue';
import { useI18n } from 'vue-i18n';
import { ModalBase, notify, loading } from '@wallacesw11/base-lib';
import { expenseTypeService } from '@/services/expenseTypeService';
import type { ExpenseTypeModel } from '@/models';
import { FormMode } from '@/models';

interface ModalAction {
  text: string;
  color?: string;
  handler: () => void;
}

const props = defineProps<{
  modelValue: boolean;
  expenseType: ExpenseTypeModel | null;
  mode: FormMode;
}>();

const emit = defineEmits<{
  'update:modelValue': [value: boolean];
  saved: [];
}>();

const { t } = useI18n();
const formRef = ref();
const nameFieldRef = ref();
const form = ref({
  name: ''
});

const title = computed(() => {
  return props.mode === FormMode.ADD 
    ? t('expenseTypes.addTitle') 
    : t('expenseTypes.editTitle');
});

const validateRequired = (value: string): boolean | string => {
  return !!value || t('expenseTypes.requiredField');
};

const resetForm = (): void => {
  form.value = {
    name: ''
  };
  
  nextTick(() => {
    formRef.value?.resetValidation();
    nameFieldRef.value?.focus();
  });
};

const loadFormData = (): void => {
  if (!props.modelValue) return;

  if (props.mode === FormMode.EDIT && props.expenseType) {
    form.value = {
      name: props.expenseType.name
    };
    return;
  }

  if (props.mode === FormMode.ADD) {
    resetForm();
    
    nextTick(() => {
      nameFieldRef.value?.focus();
    });
  }
};

const handleSave = async (): Promise<void> => {
  const { valid } = await formRef.value.validate();

  if (!valid) return;

  loading.show(t('expenseTypes.saving'));

  try {
    if (props.mode === FormMode.ADD) {
      await localStorageService.post<ExpenseTypeModel>('expenseTypes', {
        name: form.value.name
      });

      notify.success(t('expenseTypes.saved'), '');
      emit('saved');
      resetForm();
      return;
    }

    if (!props.expenseType) return;

    await localStorageService.put<ExpenseTypeModel>(
      'expenseTypes',
      props.expenseType.id,
      { name: form.value.name }
    );

    notify.success(t('expenseTypes.updated'), '');
    emit('saved');
    emit('update:modelValue', false);
  } catch (error) {
    const errorMessage = props.mode === FormMode.ADD 
      ? t('expenseTypes.saveError') 
      : t('expenseTypes.updateError');

    notify.error(t('messages.error'), errorMessage);
  } finally {
    loading.hide();
  }
};

const handleModalUpdate = (value: boolean): void => {
  if (value) return;

  if (props.mode === FormMode.EDIT) {
    emit('update:modelValue', false);
    return;
  }

  emit('update:modelValue', false);
  resetForm();
};

const handleCancel = (): void => {
  emit('update:modelValue', false);
  
  if (props.mode === FormMode.ADD) {
    resetForm();
  }
};

const actions = computed<ModalAction[]>(() => [
  {
    text: t('common.save'),
    color: 'primary',
    handler: handleSave
  },
  {
    text: t('common.cancel'),
    color: 'secondary',
    handler: handleCancel
  }
]);

watch(() => props.modelValue, () => {
  loadFormData();
});

defineExpose({
  loadFormData
});
</script>
