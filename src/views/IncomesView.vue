<template>
  <v-container fluid class="incomes-view pa-0">
    <v-app-bar color="primary" density="compact">
      <v-btn icon @click="goBack">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-app-bar-title>{{ t('navigation.incomes') }}</v-app-bar-title>

      <v-btn icon @click="openAddDialog">
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-app-bar>

    <div class="content-container">
      <v-list v-if="incomes.length > 0" lines="two">
        <v-list-item
          v-for="income in incomes"
          :key="income.id"
          @click="openEditDialog(income)"
        >
          <template #prepend>
            <v-icon :color="income.type === 'manual' ? 'success' : 'info'">
              {{ income.type === 'manual' ? 'mdi-cash' : 'mdi-clock-outline' }}
            </v-icon>
          </template>

          <v-list-item-title>{{ income.name }}</v-list-item-title>
          <v-list-item-subtitle>
            {{ getIncomeTypeLabel(income.type) }} • {{ formatIncomeValue(income) }}
          </v-list-item-subtitle>

          <template #append>
            <v-btn
              icon
              size="small"
              variant="text"
              @click.stop="confirmDelete(income)"
            >
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-list-item>
      </v-list>

      <v-card v-else class="ma-4" elevation="0" color="surface">
        <v-card-text class="text-center pa-8">
          <v-icon size="64" color="primary" class="mb-4">
            mdi-cash-plus
          </v-icon>
          
          <h2 class="text-h6 mb-2">{{ t('incomes.emptyTitle') }}</h2>
          <p class="text-body-2 text-medium-emphasis mb-4">
            {{ t('incomes.emptyDescription') }}
          </p>

          <v-btn color="primary" @click="openAddDialog">
            <v-icon start>mdi-plus</v-icon>
            {{ t('incomes.addIncome') }}
          </v-btn>
        </v-card-text>
      </v-card>
    </div>

    <IncomeFormDialog
      v-model="dialogOpen"
      :income="selectedIncome"
      @save="handleSave"
      @close="closeDialog"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { useIncomeStore } from '@/stores/income'
import { useMonthStore } from '@/stores/month'
import { formatCurrency } from '@/utils/currency'
import type { Income } from '@/models/Income'
import IncomeFormDialog from '@/components/IncomeFormDialog.vue'
import { useConfirmStore } from '@wallacesw11/base-lib'

const { t, locale } = useI18n()
const router = useRouter()
const incomeStore = useIncomeStore()
const monthStore = useMonthStore()
const confirmStore = useConfirmStore()

const dialogOpen = ref(false)
const selectedIncome = ref<Income | null>(null)

const incomes = computed(() => incomeStore.incomes)

function goBack(): void {
  router.push('/')
}

function openAddDialog(): void {
  selectedIncome.value = null
  dialogOpen.value = true
}

function openEditDialog(income: Income): void {
  selectedIncome.value = income
  dialogOpen.value = true
}

function closeDialog(): void {
  selectedIncome.value = null
  dialogOpen.value = false
}

function handleSave(incomeData: Omit<Income, 'id'>): void {
  if (selectedIncome.value) {
    incomeStore.updateIncome({
      ...incomeData,
      id: selectedIncome.value.id,
    })
  } else {
    incomeStore.addIncome(incomeData)
  }
}

async function confirmDelete(income: Income): Promise<void> {
  const confirmRef = confirmStore.confirmRef
  
  if (!confirmRef || !confirmRef.$confirm) return
  
  const confirmed = await confirmRef.$confirm.show(
    t('incomes.deleteTitle'),
    t('incomes.deleteMessage', { name: income.name })
  )
  
  if (confirmed) {
    incomeStore.deleteIncome(income.id)
  }
}

function getIncomeTypeLabel(type: 'manual' | 'hourly'): string {
  return type === 'manual' ? t('incomes.types.manual') : t('incomes.types.hourly')
}

function formatIncomeValue(income: Income): string {
  const value = incomeStore.calculateIncomeValue(income)
  
  return formatCurrency(value, locale.value)
}

onMounted(() => {
  incomeStore.loadIncomes(monthStore.currentYear, monthStore.currentMonth)
})
</script>

<style scoped>
.incomes-view {
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.content-container {
  flex: 1;
  overflow-y: auto;
}
</style>
