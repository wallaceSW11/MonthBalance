import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useIncomeStore } from './income'
import { useExpenseStore } from './expense'
import { monthDataService } from '@/services/api/monthDataService'

export const useMonthStore = defineStore('month', () => {
  const currentYear = ref<number>(new Date().getFullYear())
  const currentMonth = ref<number>(new Date().getMonth() + 1)
  const loading = ref<boolean>(false)

  const incomeStore = useIncomeStore()
  const expenseStore = useExpenseStore()

  const totalIncome = computed(() => incomeStore.totalIncome)
  const totalExpense = computed(() => expenseStore.totalExpense)
  const balance = computed(() => totalIncome.value - totalExpense.value)

  const monthName = computed(() => {
    const date = new Date(currentYear.value, currentMonth.value - 1)
    
    return date.toLocaleDateString('default', { month: 'long', year: 'numeric' })
  })

  async function loadMonth(year: number, month: number): Promise<void> {
    currentYear.value = year
    currentMonth.value = month
    loading.value = true
    
    try {
      await Promise.all([
        incomeStore.loadIncomes(year, month),
        expenseStore.loadExpenses(year, month)
      ])
    } catch (error) {
      console.error('Error loading month:', error)
    } finally {
      loading.value = false
    }
  }

  async function goToPreviousMonth(): Promise<void> {
    if (currentMonth.value === 1) {
      currentMonth.value = 12
      currentYear.value -= 1
    } else {
      currentMonth.value -= 1
    }
    
    await loadMonth(currentYear.value, currentMonth.value)
  }

  async function goToNextMonth(): Promise<boolean> {
    const nextMonth = currentMonth.value === 12 ? 1 : currentMonth.value + 1
    const nextYear = currentMonth.value === 12 ? currentYear.value + 1 : currentYear.value
    
    const monthsAhead = calculateMonthsAhead(nextYear, nextMonth)
    
    if (monthsAhead > 3) return false
    
    currentMonth.value = nextMonth
    currentYear.value = nextYear
    await loadMonth(currentYear.value, currentMonth.value)
    
    return true
  }

  async function checkMonthExists(year: number, month: number): Promise<boolean> {
    try {
      const monthData = await monthDataService.getByYearAndMonth(year, month)
      
      return monthData !== null
    } catch (error) {
      return false
    }
  }

  function canNavigateForward(): boolean {
    const nextMonth = currentMonth.value === 12 ? 1 : currentMonth.value + 1
    const nextYear = currentMonth.value === 12 ? currentYear.value + 1 : currentYear.value
    
    const monthsAhead = calculateMonthsAhead(nextYear, nextMonth)
    
    return monthsAhead <= 3
  }

  async function duplicateCurrentMonth(): Promise<void> {
    const nextMonth = currentMonth.value === 12 ? 1 : currentMonth.value + 1
    const nextYear = currentMonth.value === 12 ? currentYear.value + 1 : currentYear.value
    
    loading.value = true
    
    try {
      await monthDataService.duplicate({
        sourceYear: currentYear.value,
        sourceMonth: currentMonth.value,
        targetYear: nextYear,
        targetMonth: nextMonth
      })
      
      currentMonth.value = nextMonth
      currentYear.value = nextYear
      await loadMonth(currentYear.value, currentMonth.value)
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || 'Erro ao duplicar mês'
      console.error('Error duplicating month:', errorMessage)
      throw new Error(errorMessage)
    } finally {
      loading.value = false
    }
  }

  async function duplicateToMonth(targetYear: number, targetMonth: number): Promise<void> {
    loading.value = true
    
    try {
      await monthDataService.duplicate({
        sourceYear: currentYear.value,
        sourceMonth: currentMonth.value,
        targetYear,
        targetMonth
      })
      
      currentMonth.value = targetMonth
      currentYear.value = targetYear
      await loadMonth(currentYear.value, currentMonth.value)
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || 'Erro ao duplicar mês'
      console.error('Error duplicating to month:', errorMessage)
      throw new Error(errorMessage)
    } finally {
      loading.value = false
    }
  }

  async function clearCurrentMonth(): Promise<void> {
    loading.value = true
    
    try {
      const monthData = await monthDataService.getByYearAndMonth(currentYear.value, currentMonth.value)
      
      if (monthData) {
        await monthDataService.delete(monthData.id)
        await monthDataService.create({ year: currentYear.value, month: currentMonth.value })
      }
      
      await loadMonth(currentYear.value, currentMonth.value)
    } catch (error) {
      console.error('Error clearing month:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  function calculateMonthsAhead(targetYear: number, targetMonth: number): number {
    const now = new Date()
    const currentDate = new Date(now.getFullYear(), now.getMonth())
    const targetDate = new Date(targetYear, targetMonth - 1)
    
    const diffTime = targetDate.getTime() - currentDate.getTime()
    const diffMonths = Math.round(diffTime / (1000 * 60 * 60 * 24 * 30))
    
    return diffMonths
  }

  async function initializeCurrentMonth(): Promise<void> {
    await loadMonth(currentYear.value, currentMonth.value)
  }

  return {
    currentYear,
    currentMonth,
    loading,
    totalIncome,
    totalExpense,
    balance,
    monthName,
    loadMonth,
    goToPreviousMonth,
    goToNextMonth,
    checkMonthExists,
    canNavigateForward,
    duplicateCurrentMonth,
    duplicateToMonth,
    clearCurrentMonth,
    initializeCurrentMonth,
  }
})
