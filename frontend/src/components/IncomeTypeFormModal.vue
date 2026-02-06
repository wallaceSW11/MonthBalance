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
        :label="t('incomeTypes.name')"
        :rules="[validateRequired]"
        variant="outlined"
        density="comfortable"
        class="mb-4"
      />

      <v-select
        v-model="form.type"
        :label="t('incomeTypes.type')"
        :items="typeOptions"
        item-title="label"
        item-value="value"
        :rules="[validateRequired]"
        :disabled="mode === FormMode.EDIT"
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
import { incomeTypeService } from '@/services/incomeTypeService';
import type { IncomeTypeModel } from '@/models';
import { IncomeType, FormMode } from '@/models';

interface ModalAction {
  text: string
  color?: string
  handler: () => void
}

const props = defineProps<{
  modelValue: boolean
  incomeType: IncomeTypeModel | null
  mode: FormMode
}>()

const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  saved: []
}>()

const { t } = useI18n()
const formRef = ref()
const nameFieldRef = ref()
const form = ref({
  name: '',
  type: IncomeType.PAYCHECK
})

const typeOptions = computed(() => [
  { label: t('incomeTypes.typePaycheck'), value: IncomeType.PAYCHECK },
  { label: t('incomeTypes.typeHourly'), value: IncomeType.HOURLY },
  { label: t('incomeTypes.typeExtra'), value: IncomeType.EXTRA }
])

const title = computed(() => {
  return props.mode === FormMode.ADD 
    ? t('incomeTypes.addTitle') 
    : t('incomeTypes.editTitle')
})

const validateRequired = (value: string): boolean | string => {
  return !!value || t('incomeTypes.requiredField')
}

const resetForm = (): void => {
  form.value = {
    name: '',
    type: IncomeType.PAYCHECK
  }
  
  nextTick(() => {
    formRef.value?.resetValidation()
    nameFieldRef.value?.focus()
  })
}

const loadFormData = (): void => {
  if (!props.modelValue) return

  if (props.mode === FormMode.EDIT && props.incomeType) {
    form.value = {
      name: props.incomeType.name,
      type: props.incomeType.type
    }
    return
  }

  if (props.mode === FormMode.ADD) {
    resetForm()
    
    nextTick(() => {
      nameFieldRef.value?.focus()
    })
  }
}

const handleSave = async (): Promise<void> => {
  const { valid } = await formRef.value.validate()

  if (!valid) return

  loading.show(t('incomeTypes.saving'))

  try {
    if (props.mode === FormMode.ADD) {
      await incomeTypeService.create(form.value.name, form.value.type);

      notify.success(t('incomeTypes.saved'), '');
      emit('saved');
      resetForm();
      return;
    }

    if (!props.incomeType) return;

    await incomeTypeService.update(props.incomeType.id, form.value.name);

    notify.success(t('incomeTypes.updated'), '');
    emit('saved');
    emit('update:modelValue', false);
  } catch (error) {
    const errorMessage = props.mode === FormMode.ADD 
      ? t('incomeTypes.saveError') 
      : t('incomeTypes.updateError')

    notify.error(t('messages.error'), errorMessage)
  } finally {
    loading.hide()
  }
}

const handleModalUpdate = (value: boolean): void => {
  if (value) return

  if (props.mode === FormMode.EDIT) {
    emit('update:modelValue', false)
    return
  }

  emit('update:modelValue', false)
  resetForm()
}

const handleCancel = (): void => {
  emit('update:modelValue', false)
  
  if (props.mode === FormMode.ADD) {
    resetForm()
  }
}

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
])

watch(() => props.modelValue, () => {
  loadFormData()
})

defineExpose({
  loadFormData
})
</script>
