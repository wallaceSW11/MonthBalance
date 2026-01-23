<template>
  <v-dialog v-model="isOpen" max-width="500" persistent>
    <v-card>
      <v-card-title>
        <span>{{ isEditMode ? 'Editar Receita' : 'Nova Receita' }}</span>
      </v-card-title>

      <v-card-text>
        <v-form ref="formRef" validate-on="submit" @submit.prevent="handleSubmit">
          <v-select
            v-if="!isEditMode"
            v-model="formData.incomeId"
            label="Receita"
            :items="incomeOptions"
            item-title="description"
            item-value="id"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
            @update:model-value="handleIncomeChange"
          />

          <div v-if="isEditMode || formData.incomeId" class="mb-4">
            <strong>{{ selectedIncomeDescription }}</strong>
            <div class="text-caption text-grey">{{ selectedIncomeTypeLabel }}</div>
          </div>

          <template v-if="selectedIncomeType === IncomeTypeEnum.Manual">
            <MoneyField
              v-model="formData.grossValue"
              label="Valor Bruto"
              :rules="[rules.required]"
              currency="BRL"
              locale="pt-BR"
              density="comfortable"
              class="mb-2"
            />

            <MoneyField
              v-model="formData.netValue"
              label="Valor Líquido"
              :rules="[rules.required]"
              currency="BRL"
              locale="pt-BR"
              density="comfortable"
              class="mb-2"
            />
          </template>

          <template v-if="selectedIncomeType === IncomeTypeEnum.Hourly">
            <MoneyField
              v-model="formData.hourlyRate"
              label="Valor por Hora"
              :rules="[rules.required]"
              currency="BRL"
              locale="pt-BR"
              density="comfortable"
              class="mb-2"
            />

            <v-row no-gutters>
              <v-col cols="6" class="pr-1">
                <NumberField
                  v-model="formData.hours"
                  label="Horas"
                  :rules="[rules.required]"
                  :decimal-places="0"
                  locale="pt-BR"
                  density="comfortable"
                />
              </v-col>

              <v-col cols="6" class="pl-1">
                <NumberField
                  v-model="formData.minutes"
                  label="Minutos"
                  :rules="[rules.required, rules.maxMinutes]"
                  :decimal-places="0"
                  locale="pt-BR"
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
import { MoneyField, NumberField } from '@wallacesw11/base-lib'
import { useIncomeGlobalStore } from '@/stores/incomeGlobal'
import { IncomeTypeEnum, type Income } from '@/models/Income'
import type { MonthIncome } from '@/models/MonthIncome'

const props = defineProps<{
  monthIncome?: MonthIncome | null
  selectedIncome?: Income | null
}>()

const emit = defineEmits<{
  save: [data: {
    incomeId: number
    grossValue?: number | null
    netValue?: number | null
    hourlyRate?: number | null
    hours?: number | null
    minutes?: number | null
  }]
  close: []
}>()

const incomeGlobalStore = useIncomeGlobalStore()

const isOpen = defineModel<boolean>({ default: false })
const formRef = ref()

const formData = ref({
  incomeId: null as number | null,
  grossValue: 0,
  netValue: 0,
  hourlyRate: 0,
  hours: 0,
  minutes: 0
})

const isEditMode = computed(() => !!props.monthIncome)

const incomeOptions = computed(() => incomeGlobalStore.incomes)

const selectedIncomeType = computed(() => {
  if (isEditMode.value && props.monthIncome) {
    return props.monthIncome.incomeType
  }

  if (props.selectedIncome) {
    return props.selectedIncome.type
  }

  const income = incomeGlobalStore.incomes.find(i => i.id === formData.value.incomeId)
  
  return income?.type ?? IncomeTypeEnum.Manual
})

const selectedIncomeDescription = computed(() => {
  if (isEditMode.value && props.monthIncome) {
    return props.monthIncome.incomeDescription
  }

  if (props.selectedIncome) {
    return props.selectedIncome.description
  }

  const income = incomeGlobalStore.incomes.find(i => i.id === formData.value.incomeId)
  
  return income?.description ?? ''
})

const selectedIncomeTypeLabel = computed(() => 
  selectedIncomeType.value === IncomeTypeEnum.Manual ? 'Manual' : 'Por Hora'
)

const rules = {
  required: (value: string | number | null) => (!!value || value === 0) || 'Campo obrigatório',
  maxMinutes: (value: string | number) => Number(value) <= 59 || 'Máximo 59 minutos'
}

watch(() => props.monthIncome, (newMonthIncome) => {
  if (!newMonthIncome) {
    resetForm()
    
    return
  }

  formData.value = {
    incomeId: newMonthIncome.incomeId,
    grossValue: newMonthIncome.grossValue ?? 0,
    netValue: newMonthIncome.netValue ?? 0,
    hourlyRate: newMonthIncome.hourlyRate ?? 0,
    hours: newMonthIncome.hours ?? 0,
    minutes: newMonthIncome.minutes ?? 0
  }
}, { immediate: true })

watch(() => props.selectedIncome, (newIncome) => {
  if (!newIncome) return

  formData.value.incomeId = newIncome.id
})

function handleIncomeChange(): void {
  formData.value.grossValue = 0
  formData.value.netValue = 0
  formData.value.hourlyRate = 0
  formData.value.hours = 0
  formData.value.minutes = 0
}

function resetForm(): void {
  formData.value = {
    incomeId: null,
    grossValue: 0,
    netValue: 0,
    hourlyRate: 0,
    hours: 0,
    minutes: 0
  }

  formRef.value?.resetValidation()
}

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return

  const data: any = {
    incomeId: formData.value.incomeId!
  }

  if (selectedIncomeType.value === IncomeTypeEnum.Manual) {
    data.grossValue = Number(formData.value.grossValue)
    data.netValue = Number(formData.value.netValue)
  } else {
    data.hourlyRate = Number(formData.value.hourlyRate)
    data.hours = Number(formData.value.hours)
    data.minutes = Number(formData.value.minutes)
  }

  emit('save', data)
  closeDialog()
}

function closeDialog(): void {
  isOpen.value = false
  resetForm()
  emit('close')
}
</script>
