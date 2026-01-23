import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { monthDataService } from '@/services/api/monthDataService'
import { useIncomeStore } from './income'
import { useExpenseStore } from './expense'
import type { MonthData } from '@/models/MonthData'

export const useMonthStore = defineStore('month', () => {
  const currentYear = ref(new Date().getFullYear())
  const currentMonth = ref(new Date().getMonth() + 1)
  const monthData = ref<MonthData | null>(null)
  const loading = ref(false)

  const incomeStore = useIncomeStore()
  const expenseStore = useExpenseStore()

  const totalIncome = computed(() => incomeStore.totalIncome)
  const totalExpense = computed(() => expenseStore.totalExpense)
  const balance = computed(() => totalIncome.value - totalExpense.value)

  async function loadMonthData(year: number, month: number): Promise<void> {
    loading.value = true
    currentYear.value = year
    currentMonth.value = month

    try {
      monthData.value = await monthDataService.getOrCreate(year, month)
      
      await Promise.all([
        incomeStore.loadMonthIncomes(year, month),
        expenseStore.loadMonthExpenses(year, month)
      ])
    } finally {
      loading.value = false
    }
  }

  async function duplicateMonth(sourceYear: number, sourceMonth: number, targetYear: number, targetMonth: number): Promise<void> {
    loading.value = true

    try {
      await monthDataService.duplicate(sourceYear, sourceMonth, targetYear, targetMonth)
      
      await loadMonthData(targetYear, targetMonth)
    } finally {
      loading.value = false
    }
  }

  async function deleteMonth(year: number, month: number): Promise<void> {
    loading.value = true

    try {
      await monthDataService.delete(year, month)
      
      if (year === currentYear.value && month === currentMonth.value) {
        await loadMonthData(year, month)
      }
    } finally {
      loading.value = false
    }
  }

  return {
    currentYear,
    currentMonth,
    monthData,
    loading,
    totalIncome,
    totalExpense,
    balance,
    loadMonthData,
    duplicateMonth,
    deleteMonth
  }
})
