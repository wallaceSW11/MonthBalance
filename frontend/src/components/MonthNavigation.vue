<template>
  <div class="month-navigation">
    <v-btn
      icon
      size="small"
      variant="text"
      @click="handlePrevious"
    >
      <v-icon>mdi-chevron-left</v-icon>
    </v-btn>

    <v-menu offset-y>
      <template #activator="{ props }">
        <v-btn
          variant="text"
          class="month-title-button"
          v-bind="props"
        >
          <h2 class="month-title">
            {{ formattedMonth }}
          </h2>
          <v-icon size="small" class="ml-1">
            mdi-chevron-down
          </v-icon>
        </v-btn>
      </template>

      <v-list density="compact">
        <v-list-item @click="openDuplicateDialog">
          <template #prepend>
            <v-icon>mdi-content-copy</v-icon>
          </template>
          <v-list-item-title>{{ t('dashboard.monthMenu.duplicate') }}</v-list-item-title>
        </v-list-item>

        <v-list-item @click="handleClearMonth">
          <template #prepend>
            <v-icon>mdi-delete-outline</v-icon>
          </template>
          <v-list-item-title>{{ t('dashboard.monthMenu.clear') }}</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>

    <v-btn
      icon
      size="small"
      variant="text"
      :disabled="!canGoForward"
      @click="handleNext"
    >
      <v-icon>mdi-chevron-right</v-icon>
    </v-btn>

    <DuplicateMonthDialog
      v-model="duplicateDialogOpen"
      :current-year="monthStore.currentYear"
      :current-month="monthStore.currentMonth"
      @duplicate="handleDuplicate"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useMonthStore } from '@/stores/month'
import { confirm } from '@wallacesw11/base-lib'
import DuplicateMonthDialog from './DuplicateMonthDialog.vue'

const { t, locale } = useI18n()
const monthStore = useMonthStore()

const duplicateDialogOpen = ref(false)

const formattedMonth = computed(() => {
  const date = new Date(monthStore.currentYear, monthStore.currentMonth - 1)
  const currentYear = new Date().getFullYear()
  const monthName = date.toLocaleDateString(locale.value, { month: 'long' })
  
  return monthStore.currentYear === currentYear ? monthName : `${monthName}/${monthStore.currentYear}`
})

const canGoForward = computed(() => monthStore.canNavigateForward())

function handlePrevious(): void {
  monthStore.goToPreviousMonth()
}

async function handleNext(): Promise<void> {
  const nextMonth = monthStore.currentMonth === 12 ? 1 : monthStore.currentMonth + 1
  const nextYear = monthStore.currentMonth === 12 ? monthStore.currentYear + 1 : monthStore.currentYear
  
  const monthExists = monthStore.checkMonthExists(nextYear, nextMonth)
  
  if (!monthExists) {
    const confirmed = await confirm.show(
      t('dashboard.duplicateMonth.title'),
      t('dashboard.duplicateMonth.message'),
      {
        confirmText: t('common.yes'),
        cancelText: t('common.no'),
        confirmColor: 'primary'
      }
    )
    
    if (confirmed) {
      monthStore.duplicateCurrentMonth()
      return
    }
    
    monthStore.goToNextMonth()
    return
  }
  
  monthStore.goToNextMonth()
}

function openDuplicateDialog(): void {
  duplicateDialogOpen.value = true
}

async function handleClearMonth(): Promise<void> {
  const confirmed = await confirm.show(
    t('dashboard.clearMonth.title'),
    t('dashboard.clearMonth.message'),
    {
      confirmText: t('common.yes'),
      cancelText: t('common.no'),
      confirmColor: 'error',
      persistent: true
    }
  )
  
  if (!confirmed) return
  
  monthStore.clearCurrentMonth()
}

function handleDuplicate(targetYear: number, targetMonth: number): void {
  monthStore.duplicateToMonth(targetYear, targetMonth)
}
</script>

<style scoped>
.month-navigation {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.month-title-button {
  text-transform: none;
  letter-spacing: normal;
  height: auto;
  padding: 4px 8px;
}

.month-title {
  font-size: 18px;
  font-weight: 700;
  letter-spacing: -0.02em;
  white-space: nowrap;
  text-transform: capitalize;
}
</style>
