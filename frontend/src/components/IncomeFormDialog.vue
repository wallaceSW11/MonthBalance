<template>
  <v-dialog v-model="isOpen" max-width="500" persistent>
    <v-card>
      <v-card-title class="d-flex justify-space-between align-center">
        <span>{{ isEditMode ? t('incomes.editIncome') : t('incomes.addIncome') }}</span>
        <v-btn icon size="small" variant="text" @click="closeDialog">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-form ref="formRef" validate-on="submit" @submit.prevent="handleSubmit">
          <v-text-field
            ref="nameFieldRef"
            v-model="formData.name"
            :label="t('incomes.form.name')"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
          />

          <v-select
            v-model="formData.type"
            :label="t('incomes.form.type')"
            :items="incomeTypes"
            item-title="label"
            item-value="value"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
          />

          <template v-if="formData.type === 'manual'">
            <MoneyField
              v-model="formData.grossValue"
              :label="t('incomes.form.grossValue')"
              :rules="[rules.required]"
              :currency="locale === 'pt-BR' ? 'BRL' : 'USD'"
              :locale="locale"
              density="comfortable"
              class="mb-2"
            />

            <MoneyField
              v-model="formData.netValue"
              :label="t('incomes.form.netValue')"
              :rules="[rules.required]"
              :currency="locale === 'pt-BR' ? 'BRL' : 'USD'"
              :locale="locale"
              density="comfortable"
              class="mb-2"
            />
          </template>

          <template v-if="formData.type === 'hourly'">
            <MoneyField
              v-model="formData.hourlyRate"
              :label="t('incomes.form.hourlyRate')"
              :rules="[rules.required]"
              :currency="locale === 'pt-BR' ? 'BRL' : 'USD'"
              :locale="locale"
              density="comfortable"
              class="mb-2"
            />

            <v-row no-gutters>
              <v-col cols="6" class="pr-1">
                <NumberField
                  v-model="formData.hours"
                  :label="t('incomes.form.hours')"
                  :rules="[rules.required]"
                  :decimal-places="0"
                  :locale="locale"
                  density="comfortable"
                />
              </v-col>

              <v-col cols="6" class="pl-1">
                <NumberField
                  v-model="formData.minutes"
                  :label="t('incomes.form.minutes')"
                  :rules="[rules.required, rules.maxMinutes]"
                  :decimal-places="0"
                  :locale="locale"
                  density="comfortable"
                />
              </v-col>
            </v-row>
          </template>
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
import { MoneyField, NumberField } from '@wallacesw11/base-lib'
import type { Income } from '@/models/Income'

interface Props {
  income?: Income | null
}

const props = defineProps<Props>()
const emit = defineEmits<{
  save: [income: Omit<Income, 'id'>]
  close: []
}>()

const { t, locale } = useI18n()

const isOpen = defineModel<boolean>({ default: false })
const formRef = ref()
const nameFieldRef = ref()

const formData = ref({
  name: '',
  type: 'manual' as 'manual' | 'hourly',
  grossValue: 0,
  netValue: 0,
  hourlyRate: 0,
  hours: 0,
  minutes: 0,
})

const incomeTypes = computed(() => [
  { label: t('incomes.types.manual'), value: 'manual' },
  { label: t('incomes.types.hourly'), value: 'hourly' },
])

const isEditMode = computed(() => !!props.income)

const rules = {
  required: (value: string | number) => !!value || value === 0 || t('common.required'),
  maxMinutes: (value: string | number) => Number(value) <= 59 || t('incomes.form.maxMinutesError'),
}

watch(() => props.income, (newIncome) => {
  if (!newIncome) {
    resetForm()
    
    return
  }
  
  formData.value = {
    name: newIncome.name,
    type: newIncome.type,
    grossValue: newIncome.grossValue || 0,
    netValue: newIncome.netValue || 0,
    hourlyRate: newIncome.hourlyRate || 0,
    hours: newIncome.hours || 0,
    minutes: newIncome.minutes || 0,
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
    type: 'manual',
    grossValue: 0,
    netValue: 0,
    hourlyRate: 0,
    hours: 0,
    minutes: 0,
  }
  
  formRef.value?.resetValidation()
}

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return
  
  const incomeData: Omit<Income, 'id'> = {
    name: formData.value.name,
    type: formData.value.type,
  }
  
  if (formData.value.type === 'manual') {
    incomeData.grossValue = Number(formData.value.grossValue)
    incomeData.netValue = Number(formData.value.netValue)
  } else {
    incomeData.hourlyRate = Number(formData.value.hourlyRate)
    incomeData.hours = Number(formData.value.hours)
    incomeData.minutes = Number(formData.value.minutes)
  }
  
  emit('save', incomeData)
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
