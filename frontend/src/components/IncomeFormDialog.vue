<template>
  <v-dialog v-model="isOpen" max-width="500" persistent>
    <v-card>
      <v-card-title>
        <span>{{ isEditMode ? t('incomes.editIncome') : t('incomes.addIncome') }}</span>
      </v-card-title>

      <v-card-text>
        <v-form ref="formRef" validate-on="submit" @submit.prevent="handleSubmit">
          <v-select
            ref="typeFieldRef"
            v-model="formData.incomeTypeId"
            :label="t('incomes.form.type')"
            :items="incomeTypeOptions"
            item-title="label"
            item-value="value"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
          />

          <template v-if="selectedIncomeType?.type === IncomeTypeEnum.Manual">
            <MoneyField
              v-model="formData.grossValue"
              :label="t('incomes.form.grossValue')"
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

          <template v-if="selectedIncomeType?.type === IncomeTypeEnum.Hourly">
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
import { ref, computed, watch, nextTick, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { MoneyField, NumberField } from '@wallacesw11/base-lib'
import { useIncomeTypeStore } from '@/stores/incomeType'
import { IncomeTypeEnum } from '@/models/IncomeType'
import type { Income, IncomeFormData } from '@/models/Income'

interface Props {
  income?: Income | null
}

const props = defineProps<Props>()
const emit = defineEmits<{
  save: [income: IncomeFormData]
  close: []
}>()

const { t, locale } = useI18n()
const incomeTypeStore = useIncomeTypeStore()

const isOpen = defineModel<boolean>({ default: false })
const formRef = ref()
const typeFieldRef = ref()

const formData = ref<IncomeFormData>({
  incomeTypeId: 0,
  grossValue: undefined,
  netValue: undefined,
  hourlyRate: undefined,
  hours: undefined,
  minutes: undefined,
})

const incomeTypeOptions = computed(() => 
  incomeTypeStore.incomeTypes.map(type => ({
    label: type.name,
    value: type.id
  }))
)

const selectedIncomeType = computed(() => 
  incomeTypeStore.getIncomeTypeById(formData.value.incomeTypeId)
)

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
    incomeTypeId: newIncome.incomeTypeId,
    grossValue: newIncome.grossValue,
    netValue: newIncome.netValue,
    hourlyRate: newIncome.hourlyRate,
    hours: newIncome.hours,
    minutes: newIncome.minutes,
  }
}, { immediate: true })

watch(isOpen, async (opened) => {
  if (opened && !isEditMode.value) {
    await nextTick()
    typeFieldRef.value?.focus()
  }
})

function resetForm(): void {
  formData.value = {
    incomeTypeId: 0,
    grossValue: undefined,
    netValue: undefined,
    hourlyRate: undefined,
    hours: undefined,
    minutes: undefined,
  }
  
  formRef.value?.resetValidation()
}

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return
  
  const incomeData: IncomeFormData = {
    incomeTypeId: formData.value.incomeTypeId,
  }
  
  if (selectedIncomeType.value?.type === IncomeTypeEnum.Manual) {
    incomeData.grossValue = formData.value.grossValue ? Number(formData.value.grossValue) : undefined
    incomeData.netValue = formData.value.netValue ? Number(formData.value.netValue) : undefined
  } else {
    incomeData.hourlyRate = formData.value.hourlyRate ? Number(formData.value.hourlyRate) : undefined
    incomeData.hours = formData.value.hours ? Number(formData.value.hours) : undefined
    incomeData.minutes = formData.value.minutes ? Number(formData.value.minutes) : undefined
  }
  
  emit('save', incomeData)
  
  if (isEditMode.value) {
    closeDialog()
    return
  }
  
  resetForm()
  
  await nextTick()
  typeFieldRef.value?.focus()
}

function closeDialog(): void {
  isOpen.value = false
  resetForm()
  emit('close')
}

onMounted(() => {
  incomeTypeStore.loadIncomeTypes()
})
</script>
