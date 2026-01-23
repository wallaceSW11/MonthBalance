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
          <v-icon
            size="small"
            class="ml-1"
          >
            mdi-chevron-down
          </v-icon>
        </v-btn>
      </template>

      <v-list density="compact">
        <v-list-item @click="openDuplicateDialog">
          <template #prepend>
            <v-icon>mdi-content-copy</v-icon>
          </template>
          <v-list-item-title>Duplicar Mês</v-list-item-title>
        </v-list-item>

        <v-list-item @click="handleClearMonth">
          <template #prepend>
            <v-icon>mdi-delete-outline</v-icon>
          </template>
          <v-list-item-title>Limpar Mês</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>

    <v-btn
      icon
      size="small"
      variant="text"
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
import { useMonthStore } from '@/stores/month'
import DuplicateMonthDialog from './DuplicateMonthDialog.vue'

const monthStore = useMonthStore()

const duplicateDialogOpen = ref(false)

const formattedMonth = computed(() => {
  const date = new Date(monthStore.currentYear, monthStore.currentMonth - 1)
  const currentYear = new Date().getFullYear()
  const monthName = date.toLocaleDateString('pt-BR', { month: 'long' })
  
  return monthStore.currentYear === currentYear ? monthName : `${monthName}/${monthStore.currentYear}`
})

async function handlePrevious(): Promise<void> {
  const prevMonth = monthStore.currentMonth === 1 ? 12 : monthStore.currentMonth - 1
  const prevYear = monthStore.currentMonth === 1 ? monthStore.currentYear - 1 : monthStore.currentYear
  
  await monthStore.loadMonthData(prevYear, prevMonth)
}

async function handleNext(): Promise<void> {
  const nextMonth = monthStore.currentMonth === 12 ? 1 : monthStore.currentMonth + 1
  const nextYear = monthStore.currentMonth === 12 ? monthStore.currentYear + 1 : monthStore.currentYear
  
  await monthStore.loadMonthData(nextYear, nextMonth)
}

function openDuplicateDialog(): void {
  duplicateDialogOpen.value = true
}

async function handleClearMonth(): Promise<void> {
  if (!confirm('Tem certeza que deseja limpar este mês?')) return
  
  await monthStore.deleteMonth(monthStore.currentYear, monthStore.currentMonth)
}

async function handleDuplicate(targetYear: number, targetMonth: number): Promise<void> {
  await monthStore.duplicateMonth(
    monthStore.currentYear,
    monthStore.currentMonth,
    targetYear,
    targetMonth
  )
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
