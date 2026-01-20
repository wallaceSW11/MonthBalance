import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { expenseStorageService } from '@/services/storage/ExpenseStorageService'
import type { Expense } from '@/models/Expense'
import { v4 as uuidv4 } from '@/utils/uuid'

export const useExpenseStore = defineStore('expense', () => {
  const expenses = ref<Expense[]>([])
  const currentYear = ref<number>(new Date().getFullYear())
  const currentMonth = ref<number>(new Date().getMonth() + 1)

  const totalExpense = computed(() => {
    return expenses.value.reduce((sum, expense) => sum + expense.value, 0)
  })

  function loadExpenses(year: number, month: number): void {
    currentYear.value = year
    currentMonth.value = month
    expenses.value = expenseStorageService.getExpensesByMonth(year, month)
  }

  function addExpense(expense: Omit<Expense, 'id'>): void {
    const newExpense: Expense = {
      ...expense,
      id: uuidv4(),
    }
    
    expenses.value.push(newExpense)
    expenseStorageService.addExpense(currentYear.value, currentMonth.value, newExpense)
  }

  function updateExpense(updatedExpense: Expense): void {
    const index = expenses.value.findIndex((e) => e.id === updatedExpense.id)
    
    if (index === -1) return
    
    expenses.value[index] = updatedExpense
    expenseStorageService.updateExpense(currentYear.value, currentMonth.value, updatedExpense)
  }

  function deleteExpense(expenseId: string): void {
    expenses.value = expenses.value.filter((e) => e.id !== expenseId)
    expenseStorageService.deleteExpense(currentYear.value, currentMonth.value, expenseId)
  }

  function duplicateToMonth(targetYear: number, targetMonth: number): void {
    const duplicatedExpenses = expenses.value.map((expense) => ({
      ...expense,
      id: uuidv4(),
    }))
    
    expenseStorageService.saveExpensesByMonth(targetYear, targetMonth, duplicatedExpenses)
  }

  function clearMonth(year: number, month: number): void {
    expenseStorageService.saveExpensesByMonth(year, month, [])
    
    if (year === currentYear.value && month === currentMonth.value) {
      expenses.value = []
    }
  }

  return {
    expenses,
    currentYear,
    currentMonth,
    totalExpense,
    loadExpenses,
    addExpense,
    updateExpense,
    deleteExpense,
    duplicateToMonth,
    clearMonth,
  }
})
