import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useIncomeStore } from './income'
import { useExpenseStore } from './expense'
import { incomeStorageService } from '@/services/storage/IncomeStorageService'
import { expenseStorageService } from '@/services/storage/ExpenseStorageService'

export const useMonthStore = defineStore('month', () => {
  const currentYear = ref<number>(new Date().getFullYear())
  const currentMonth = ref<number>(new Date().getMonth() + 1)

  const incomeStore = useIncomeStore()
  const expenseStore = useExpenseStore()

  const totalIncome = computed(() => incomeStore.totalIncome)
  const totalExpense = computed(() => expenseStore.totalExpense)
  const balance = computed(() => totalIncome.value - totalExpense.value)

  const monthName = computed(() => {
    const date = new Date(currentYear.value, currentMonth.value - 1)
    
    return date.toLocaleDateString('default', { month: 'long', year: 'numeric' })
  })

  function loadMonth(year: number, month: number): void {
    currentYear.value = year
    currentMonth.value = month
    
    incomeStore.loadIncomes(year, month)
    expenseStore.loadExpenses(year, month)
  }

  function goToPreviousMonth(): void {
    if (currentMonth.value === 1) {
      currentMonth.value = 12
      currentYear.value -= 1
    } else {
      currentMonth.value -= 1
    }
    
    loadMonth(currentYear.value, currentMonth.value)
  }

  function goToNextMonth(): boolean {
    const nextMonth = currentMonth.value === 12 ? 1 : currentMonth.value + 1
    const nextYear = currentMonth.value === 12 ? currentYear.value + 1 : currentYear.value
    
    const monthsAhead = calculateMonthsAhead(nextYear, nextMonth)
    
    if (monthsAhead > 3) return false
    
    const monthExists = checkMonthExists(nextYear, nextMonth)
    
    if (!monthExists) return false
    
    currentMonth.value = nextMonth
    currentYear.value = nextYear
    loadMonth(currentYear.value, currentMonth.value)
    
    return true
  }

  function checkMonthExists(year: number, month: number): boolean {
    const incomesExist = incomeStorageService.monthExists(year, month)
    const expensesExist = expenseStorageService.monthExists(year, month)
    
    return incomesExist || expensesExist
  }

  function canNavigateForward(): boolean {
    const nextMonth = currentMonth.value === 12 ? 1 : currentMonth.value + 1
    const nextYear = currentMonth.value === 12 ? currentYear.value + 1 : currentYear.value
    
    const monthsAhead = calculateMonthsAhead(nextYear, nextMonth)
    
    return monthsAhead <= 3
  }

  function duplicateCurrentMonth(): void {
    const nextMonth = currentMonth.value === 12 ? 1 : currentMonth.value + 1
    const nextYear = currentMonth.value === 12 ? currentYear.value + 1 : currentYear.value
    
    incomeStore.duplicateToMonth(nextYear, nextMonth)
    expenseStore.duplicateToMonth(nextYear, nextMonth)
    
    currentMonth.value = nextMonth
    currentYear.value = nextYear
    loadMonth(currentYear.value, currentMonth.value)
  }

  function calculateMonthsAhead(targetYear: number, targetMonth: number): number {
    const now = new Date()
    const currentDate = new Date(now.getFullYear(), now.getMonth())
    const targetDate = new Date(targetYear, targetMonth - 1)
    
    const diffTime = targetDate.getTime() - currentDate.getTime()
    const diffMonths = Math.round(diffTime / (1000 * 60 * 60 * 24 * 30))
    
    return diffMonths
  }

  function initializeCurrentMonth(): void {
    loadMonth(currentYear.value, currentMonth.value)
  }

  return {
    currentYear,
    currentMonth,
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
    initializeCurrentMonth,
  }
})
