<template>
  <v-dialog
    :model-value="modelValue"
    max-width="500"
    persistent
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-card>
      <v-card-title>
        {{ editMode ? 'Editar Despesa' : 'Nova Despesa' }}
      </v-card-title>

      <v-card-text>
        <v-form ref="formRef">
          <v-text-field
            v-model="formData.description"
            label="Descrição"
            :rules="descriptionRules"
            required
          />
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
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
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import { useExpenseGlobalStore } from '@/stores/expenseGlobal'
import type { Expense } from '@/models/Expense'

const props = defineProps<{
  modelValue: boolean
  expense: Expense | null
}>()

const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  save: []
}>()

const expenseGlobalStore = useExpenseGlobalStore()

const formRef = ref()
const loading = ref(false)
const formData = ref({
  description: ''
})

const editMode = computed(() => !!props.expense)

const descriptionRules = [
  (v: string) => !!v || 'Descrição é obrigatória'
]

watch(() => props.modelValue, (value) => {
  if (!value) return

  if (props.expense) {
    formData.value = {
      description: props.expense.description
    }
    
    return
  }

  formData.value = {
    description: ''
  }
})

async function handleSave(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return

  loading.value = true

  try {
    if (editMode.value && props.expense) {
      await expenseGlobalStore.updateExpense(
        props.expense.id,
        formData.value.description
      )
    } else {
      await expenseGlobalStore.createExpense(formData.value.description)
    }

    emit('save')
  } catch (error: any) {
    alert(error.response?.data?.message || 'Erro ao salvar despesa')
  } finally {
    loading.value = false
  }
}

function handleCancel(): void {
  emit('update:modelValue', false)
}
</script>
