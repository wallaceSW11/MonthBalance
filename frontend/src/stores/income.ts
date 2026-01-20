import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { incomeStorageService } from '@/services/storage/IncomeStorageService'
import type { Income } from '@/models/Income'
import { v4 as uuidv4 } from '@/utils/uuid'

export const useIncomeStore = defineStore('income', () => {
  const incomes = ref<Income[]>([])
  const currentYear = ref<number>(new Date().getFullYear())
  const currentMonth = ref<number>(new Date().getMonth() + 1)

  const totalIncome = computed(() => {
    return incomes.value.reduce((sum, income) => {
      const value = calculateIncomeValue(income)
      
      return sum + value
    }, 0)
  })

  function loadIncomes(year: number, month: number): void {
    currentYear.value = year
    currentMonth.value = month
    incomes.value = incomeStorageService.getIncomesByMonth(year, month)
  }

  function addIncome(income: Omit<Income, 'id'>): void {
    const newIncome: Income = {
      ...income,
      id: uuidv4(),
    }
    
    incomes.value.push(newIncome)
    incomeStorageService.addIncome(currentYear.value, currentMonth.value, newIncome)
  }

  function updateIncome(updatedIncome: Income): void {
    const index = incomes.value.findIndex((i) => i.id === updatedIncome.id)
    
    if (index === -1) return
    
    incomes.value[index] = updatedIncome
    incomeStorageService.updateIncome(currentYear.value, currentMonth.value, updatedIncome)
  }

  function deleteIncome(incomeId: string): void {
    incomes.value = incomes.value.filter((i) => i.id !== incomeId)
    incomeStorageService.deleteIncome(currentYear.value, currentMonth.value, incomeId)
  }

  function calculateIncomeValue(income: Income): number {
    if (income.type === 'manual') {
      return income.netValue || 0
    }
    
    if (income.type === 'hourly') {
      const hours = income.hours || 0
      const minutes = income.minutes || 0
      const hourlyRate = income.hourlyRate || 0
      
      return (hours + minutes / 60) * hourlyRate
    }
    
    return 0
  }

  function duplicateToMonth(targetYear: number, targetMonth: number): void {
    const duplicatedIncomes = incomes.value.map((income) => ({
      ...income,
      id: uuidv4(),
    }))
    
    incomeStorageService.saveIncomesByMonth(targetYear, targetMonth, duplicatedIncomes)
  }

  function clearMonth(year: number, month: number): void {
    incomeStorageService.saveIncomesByMonth(year, month, [])
    
    if (year === currentYear.value && month === currentMonth.value) {
      incomes.value = []
    }
  }

  return {
    incomes,
    currentYear,
    currentMonth,
    totalIncome,
    loadIncomes,
    addIncome,
    updateIncome,
    deleteIncome,
    calculateIncomeValue,
    duplicateToMonth,
    clearMonth,
  }
})
