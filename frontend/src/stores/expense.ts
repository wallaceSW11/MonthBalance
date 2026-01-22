import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { expenseService } from '@/services/api/expenseService'
import { monthDataService } from '@/services/api/monthDataService'
import type { Expense, ExpenseFormData } from '@/models/Expense'

export const useExpenseStore = defineStore('expense', () => {
  const expenses = ref<Expense[]>([])
  const currentYear = ref<number>(new Date().getFullYear())
  const currentMonth = ref<number>(new Date().getMonth() + 1)
  const loading = ref<boolean>(false)

  const totalExpense = computed(() => {
    return expenses.value.reduce((sum, expense) => sum + expense.value, 0)
  })

  async function loadExpenses(year: number, month: number): Promise<void> {
    currentYear.value = year
    currentMonth.value = month
    loading.value = true
    
    try {
      await ensureMonthExists(year, month)
      expenses.value = await expenseService.getByMonth(year, month)
    } catch (error) {
      console.error('Error loading expenses:', error)
      expenses.value = []
    } finally {
      loading.value = false
    }
  }

  async function addExpense(expense: ExpenseFormData): Promise<void> {
    loading.value = true
    
    try {
      await ensureMonthExists(currentYear.value, currentMonth.value)
      const newExpense = await expenseService.create(currentYear.value, currentMonth.value, expense)
      expenses.value.push(newExpense)
    } catch (error) {
      console.error('Error adding expense:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  async function updateExpense(updatedExpense: Expense): Promise<void> {
    loading.value = true
    
    try {
      const updated = await expenseService.update(
        currentYear.value,
        currentMonth.value,
        updatedExpense.id,
        updatedExpense
      )
      
      const index = expenses.value.findIndex((e) => e.id === updatedExpense.id)
      
      if (index !== -1) {
        expenses.value[index] = updated
      }
    } catch (error) {
      console.error('Error updating expense:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  async function deleteExpense(expenseId: number): Promise<void> {
    loading.value = true
    
    try {
      await expenseService.delete(currentYear.value, currentMonth.value, expenseId)
      expenses.value = expenses.value.filter((e) => e.id !== expenseId)
    } catch (error) {
      console.error('Error deleting expense:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  async function ensureMonthExists(year: number, month: number): Promise<void> {
    const monthData = await monthDataService.getByYearAndMonth(year, month)
    
    if (!monthData) {
      await monthDataService.create({ year, month })
    }
  }

  return {
    expenses,
    currentYear,
    currentMonth,
    loading,
    totalExpense,
    loadExpenses,
    addExpense,
    updateExpense,
    deleteExpense,
  }
})
