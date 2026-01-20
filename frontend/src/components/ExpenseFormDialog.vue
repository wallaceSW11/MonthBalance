<template>
  <v-dialog v-model="isOpen" max-width="500" persistent>
    <v-card>
      <v-card-title class="d-flex justify-space-between align-center">
        <span>{{ isEditMode ? t('expenses.editExpense') : t('expenses.addExpense') }}</span>
        <v-btn icon size="small" variant="text" @click="closeDialog">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-form ref="formRef" validate-on="submit" @submit.prevent="handleSubmit">
          <v-text-field
            ref="nameFieldRef"
            v-model="formData.name"
            :label="t('expenses.form.name')"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
          />

          <MoneyField
            v-model="formData.value"
            :label="t('expenses.form.value')"
            :rules="[rules.required]"
            :currency="locale === 'pt-BR' ? 'BRL' : 'USD'"
            :locale="locale"
            density="comfortable"
            class="mb-2"
          />
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn variant="text" @click="closeDialog">
          {{ t('common.cancel') }}
        </v-btn>
        <v-btn color="primary" variant="flat" @click="handleSubmit">
          {{ t('common.save') }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, computed, watch, nextTick } from 'vue'
import { useI18n } from 'vue-i18n'
import { MoneyField } from '@wallacesw11/base-lib'
import type { Expense } from '@/models/Expense'

interface Props {
  expense?: Expense | null
}

const props = defineProps<Props>()
const emit = defineEmits<{
  save: [expense: Omit<Expense, 'id'>]
  close: []
}>()

const { t, locale } = useI18n()

const isOpen = defineModel<boolean>({ default: false })
const formRef = ref()
const nameFieldRef = ref()

const formData = ref({
  name: '',
  value: 0,
})

const isEditMode = computed(() => !!props.expense)

const rules = {
  required: (value: string | number) => !!value || value === 0 || t('common.required'),
}

watch(() => props.expense, (newExpense) => {
  if (!newExpense) {
    resetForm()
    
    return
  }
  
  formData.value = {
    name: newExpense.name,
    value: newExpense.value,
  }
}, { immediate: true })

watch(isOpen, async (opened) => {
  if (opened && !isEditMode.value) {
    await nextTick()
    nameFieldRef.value?.focus()
  }
})

function resetForm(): void {
  formData.value = {
    name: '',
    value: 0,
  }
  
  formRef.value?.resetValidation()
}

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return
  
  const expenseData: Omit<Expense, 'id'> = {
    name: formData.value.name,
    value: Number(formData.value.value),
  }
  
  emit('save', expenseData)
  resetForm()
  
  await nextTick()
  nameFieldRef.value?.focus()
}

function closeDialog(): void {
  isOpen.value = false
  resetForm()
  emit('close')
}
</script>
