<template>
  <v-dialog
    v-model="isOpen"
    max-width="500"
    :fullscreen="isMobileOrTablet"
    persistent
    scrollable
    content-class="duplicate-month-dialog"
  >
    <v-card>
      <v-card-title class="d-flex justify-space-between align-center pa-4">
        <span class="dialog-title">{{ t('dashboard.duplicateMonthDialog.title') }}</span>
        <v-btn icon size="small" variant="text" @click="closeDialog">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>

      <v-card-text class="pa-4">
        <v-form ref="formRef" validate-on="submit" @submit.prevent="handleSubmit">
          <v-select
            v-model="selectedMonth"
            :label="t('dashboard.duplicateMonthDialog.month')"
            :items="monthOptions"
            item-title="label"
            item-value="value"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
          />

          <v-select
            v-model="selectedYear"
            :label="t('dashboard.duplicateMonthDialog.year')"
            :items="yearOptions"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-2"
          />

          <v-alert
            v-if="targetMonthExists"
            type="warning"
            variant="tonal"
            density="compact"
            class="text-caption"
          >
            {{ t('dashboard.duplicateMonthDialog.warning') }}
          </v-alert>
        </v-form>
      </v-card-text>

      <v-card-actions class="pa-4">
        <v-spacer />
        <v-btn variant="text" @click="closeDialog">
          {{ t('common.cancel') }}
        </v-btn>
        <v-btn color="primary" variant="flat" @click="handleSubmit">
          {{ t('common.duplicate') }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useBreakpoint } from '@wallacesw11/base-lib'
import { useMonthStore } from '@/stores/month'

interface Props {
  currentYear: number
  currentMonth: number
}

const props = defineProps<Props>()
const emit = defineEmits<{
  duplicate: [year: number, month: number]
}>()

const { t, locale } = useI18n()
const monthStore = useMonthStore()
const { isMobileOrTablet } = useBreakpoint()

const isOpen = defineModel<boolean>({ default: false })
const formRef = ref()
const selectedMonth = ref(1)
const selectedYear = ref(new Date().getFullYear())

const monthOptions = computed(() => {
  const months = []
  
  for (let i = 1; i <= 12; i++) {
    const date = new Date(2000, i - 1)
    const label = date.toLocaleDateString(locale.value, { month: 'long' })
    
    months.push({ label, value: i })
  }
  
  return months
})

const yearOptions = computed(() => {
  const currentYear = new Date().getFullYear()
  const years = []
  
  for (let i = currentYear - 1; i <= currentYear + 3; i++) {
    years.push(i)
  }
  
  return years
})

const targetMonthExists = computed(() => {
  return monthStore.checkMonthExists(selectedYear.value, selectedMonth.value)
})

const rules = {
  required: (value: number) => !!value || t('common.required'),
}

watch(isOpen, (opened) => {
  if (!opened) return
  
  const nextMonth = props.currentMonth === 12 ? 1 : props.currentMonth + 1
  const nextYear = props.currentMonth === 12 ? props.currentYear + 1 : props.currentYear
  
  selectedMonth.value = nextMonth
  selectedYear.value = nextYear
})

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return
  
  emit('duplicate', selectedYear.value, selectedMonth.value)
  closeDialog()
}

function closeDialog(): void {
  isOpen.value = false
  formRef.value?.resetValidation()
}
</script>

<style>
.duplicate-month-dialog {
  max-width: calc(100vw - 32px) !important;
  margin: 16px !important;
}

.duplicate-month-dialog .v-card {
  max-width: 100% !important;
  overflow-x: hidden !important;
}

.duplicate-month-dialog .dialog-title {
  font-size: 1.1rem;
  font-weight: 600;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  max-width: calc(100% - 48px);
}

@media (min-width: 960px) {
  .duplicate-month-dialog {
    max-width: 500px !important;
    margin: auto !important;
  }
}
</style>
