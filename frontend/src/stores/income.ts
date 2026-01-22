import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { incomeService } from '@/services/api/incomeService'
import { monthDataService } from '@/services/api/monthDataService'
import { IncomeTypeEnum } from '@/models/IncomeType'
import type { Income, IncomeFormData } from '@/models/Income'

export const useIncomeStore = defineStore('income', () => {
  const incomes = ref<Income[]>([])
  const currentYear = ref<number>(new Date().getFullYear())
  const currentMonth = ref<number>(new Date().getMonth() + 1)
  const loading = ref<boolean>(false)

  const totalIncome = computed(() => {
    return incomes.value.reduce((sum, income) => {
      const value = calculateIncomeValue(income)
      
      return sum + value
    }, 0)
  })

  async function loadIncomes(year: number, month: number): Promise<void> {
    currentYear.value = year
    currentMonth.value = month
    loading.value = true
    
    try {
      await ensureMonthExists(year, month)
      incomes.value = await incomeService.getByMonth(year, month)
    } catch (error) {
      console.error('Error loading incomes:', error)
      incomes.value = []
    } finally {
      loading.value = false
    }
  }

  async function addIncome(income: IncomeFormData): Promise<void> {
    loading.value = true
    
    try {
      await ensureMonthExists(currentYear.value, currentMonth.value)
      const newIncome = await incomeService.create(currentYear.value, currentMonth.value, income)
      incomes.value.push(newIncome)
    } catch (error) {
      console.error('Error adding income:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  async function updateIncome(id: number, income: IncomeFormData): Promise<void> {
    loading.value = true
    
    try {
      const updated = await incomeService.update(
        currentYear.value,
        currentMonth.value,
        id,
        income
      )
      
      const index = incomes.value.findIndex((i) => i.id === id)
      
      if (index !== -1) {
        incomes.value[index] = updated
      }
    } catch (error) {
      console.error('Error updating income:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  async function deleteIncome(incomeId: number): Promise<void> {
    loading.value = true
    
    try {
      await incomeService.delete(currentYear.value, currentMonth.value, incomeId)
      incomes.value = incomes.value.filter((i) => i.id !== incomeId)
    } catch (error) {
      console.error('Error deleting income:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  function calculateIncomeValue(income: Income): number {
    if (income.incomeTypeType === IncomeTypeEnum.Manual) {
      return income.netValue || 0
    }
    
    if (income.incomeTypeType === IncomeTypeEnum.Hourly) {
      const hours = income.hours || 0
      const minutes = income.minutes || 0
      const hourlyRate = income.hourlyRate || 0
      
      return (hours + minutes / 60) * hourlyRate
    }
    
    return 0
  }

  async function ensureMonthExists(year: number, month: number): Promise<void> {
    const monthData = await monthDataService.getByYearAndMonth(year, month)
    
    if (!monthData) {
      await monthDataService.create({ year, month })
    }
  }

  return {
    incomes,
    currentYear,
    currentMonth,
    loading,
    totalIncome,
    loadIncomes,
    addIncome,
    updateIncome,
    deleteIncome,
    calculateIncomeValue,
  }
})
