<template>
  <v-dialog v-model="isOpen" max-width="500" persistent>
    <v-card>
      <v-card-title>
        <span>{{ isEditMode ? 'Editar Despesa' : 'Nova Despesa' }}</span>
      </v-card-title>

      <v-card-text>
        <v-form ref="formRef" validate-on="submit" @submit.prevent="handleSubmit">
          <v-select
            v-if="!isEditMode"
            v-model="formData.expenseId"
            label="Despesa"
            :items="expenseOptions"
            item-title="description"
            item-value="id"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
          />

          <div v-if="isEditMode || formData.expenseId" class="mb-4">
            <strong>{{ selectedExpenseDescription }}</strong>
          </div>

          <MoneyField
            v-model="formData.value"
            label="Valor"
            :rules="[rules.required]"
            currency="BRL"
            locale="pt-BR"
            density="comfortable"
            class="mb-2"
          />
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn variant="text" @click="closeDialog">
          Cancelar
        </v-btn>
        <v-btn color="primary" variant="flat" @click="handleSubmit">
          Salvar
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { MoneyField } from '@wallacesw11/base-lib'
import { useExpenseGlobalStore } from '@/stores/expenseGlobal'
import type { Expense } from '@/models/Expense'
import type { MonthExpense } from '@/models/MonthExpense'

const props = defineProps<{
  monthExpense?: MonthExpense | null
  selectedExpense?: Expense | null
}>()

const emit = defineEmits<{
  save: [data: { expenseId: number; value: number }]
  close: []
}>()

const expenseGlobalStore = useExpenseGlobalStore()

const isOpen = defineModel<boolean>({ default: false })
const formRef = ref()

const formData = ref({
  expenseId: null as number | null,
  value: 0
})

const isEditMode = computed(() => !!props.monthExpense)

const expenseOptions = computed(() => expenseGlobalStore.expenses)

const selectedExpenseDescription = computed(() => {
  if (isEditMode.value && props.monthExpense) {
    return props.monthExpense.expenseDescription
  }

  if (props.selectedExpense) {
    return props.selectedExpense.description
  }

  const expense = expenseGlobalStore.expenses.find(e => e.id === formData.value.expenseId)
  
  return expense?.description ?? ''
})

const rules = {
  required: (value: string | number | null) => (!!value || value === 0) || 'Campo obrigatório'
}

watch(() => props.monthExpense, (newMonthExpense) => {
  if (!newMonthExpense) {
    resetForm()
    
    return
  }

  formData.value = {
    expenseId: newMonthExpense.expenseId,
    value: newMonthExpense.value
  }
}, { immediate: true })

watch(() => props.selectedExpense, (newExpense) => {
  if (!newExpense) return

  formData.value.expenseId = newExpense.id
})

function resetForm(): void {
  formData.value = {
    expenseId: null,
    value: 0
  }

  formRef.value?.resetValidation()
}

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return

  emit('save', {
    expenseId: formData.value.expenseId!,
    value: Number(formData.value.value)
  })
  
  closeDialog()
}

function closeDialog(): void {
  isOpen.value = false
  resetForm()
  emit('close')
}
</script>
