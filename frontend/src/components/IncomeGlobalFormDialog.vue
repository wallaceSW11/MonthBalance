<template>
  <ModalBase
    :model-value="modelValue"
    :title="editMode ? 'Editar Receita' : 'Nova Receita'"
    max-width="500"
    persistent
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-form ref="formRef">
      <v-text-field
        ref="descriptionFieldRef"
        v-model="formData.description"
        label="Descrição"
        :rules="descriptionRules"
        required
      />

      <v-select
        v-model="formData.type"
        label="Tipo"
        :items="typeOptions"
        :rules="typeRules"
        required
      />
    </v-form>

    <template #actions>
      <v-btn
        text
        @click="handleCancel"
      >
        Cancelar
      </v-btn>
      <v-btn
        color="primary"
        :loading="loading"
        @click="handleSave"
      >
        Salvar
      </v-btn>
    </template>
  </ModalBase>
</template>

<script setup lang="ts">
import { ref, watch, computed, nextTick } from 'vue'
import { useIncomeGlobalStore } from '@/stores/incomeGlobal'
import { IncomeTypeEnum, type Income } from '@/models/Income'
import { ModalBase } from '@wallacesw11/base-lib'

const props = defineProps<{
  modelValue: boolean
  income: Income | null
}>()

const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  save: []
}>()

const incomeGlobalStore = useIncomeGlobalStore()

const formRef = ref()
const descriptionFieldRef = ref()
const loading = ref(false)
const formData = ref({
  description: '',
  type: IncomeTypeEnum.Manual
})

const editMode = computed(() => !!props.income)

const typeOptions = [
  { title: 'Manual', value: IncomeTypeEnum.Manual },
  { title: 'Por Hora', value: IncomeTypeEnum.Hourly }
]

const descriptionRules = [
  (v: string) => !!v || 'Descrição é obrigatória'
]

const typeRules = [
  (v: number) => v !== undefined || 'Tipo é obrigatório'
]

watch(() => props.modelValue, async (value) => {
  if (!value) return

  if (props.income) {
    formData.value = {
      description: props.income.description,
      type: props.income.type
    }
    
    return
  }

  formData.value = {
    description: '',
    type: IncomeTypeEnum.Manual
  }

  await nextTick()
  
  descriptionFieldRef.value?.focus()
})

async function handleSave(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return

  loading.value = true

  try {
    if (editMode.value && props.income) {
      await incomeGlobalStore.updateIncome(
        props.income.id,
        formData.value.description,
        formData.value.type
      )
    } else {
      await incomeGlobalStore.createIncome(
        formData.value.description,
        formData.value.type
      )
    }

    formData.value = {
      description: '',
      type: IncomeTypeEnum.Manual
    }

    await nextTick()
    
    descriptionFieldRef.value?.focus()

    emit('save')
  } catch (error: any) {
    alert(error.response?.data?.message || 'Erro ao salvar receita')
  } finally {
    loading.value = false
  }
}

function handleCancel(): void {
  emit('update:modelValue', false)
}
</script>
