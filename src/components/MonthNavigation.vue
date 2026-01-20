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

    <h2 class="month-title">{{ formattedMonth }}</h2>

    <v-btn
      icon
      size="small"
      variant="text"
      :disabled="!canGoForward"
      @click="handleNext"
    >
      <v-icon>mdi-chevron-right</v-icon>
    </v-btn>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useMonthStore } from '@/stores/month'
import { useConfirmStore } from '@wallacesw11/base-lib'

const { t, locale } = useI18n()
const monthStore = useMonthStore()
const confirmStore = useConfirmStore()

const formattedMonth = computed(() => {
  const date = new Date(monthStore.currentYear, monthStore.currentMonth - 1)
  
  return date.toLocaleDateString(locale.value, { month: 'long', year: 'numeric' })
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
    const confirmed = await confirmStore.confirm(
      t('dashboard.duplicateMonth.title'),
      t('dashboard.duplicateMonth.message')
    )
    
    if (confirmed) {
      monthStore.duplicateCurrentMonth()
    }
    
    return
  }
  
  monthStore.goToNextMonth()
}
</script>

<style scoped>
.month-navigation {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.month-title {
  font-size: 18px;
  font-weight: 700;
  letter-spacing: -0.02em;
  white-space: nowrap;
  text-transform: capitalize;
}
</style>
