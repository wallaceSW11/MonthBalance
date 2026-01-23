import { ref } from 'vue'
import { defineStore } from 'pinia'
import { expenseService } from '@/services/api/expenseService'
import type { Expense } from '@/models/Expense'

export const useExpenseGlobalStore = defineStore('expenseGlobal', () => {
  const expenses = ref<Expense[]>([])
  const loading = ref(false)

  async function loadExpenses(): Promise<void> {
    loading.value = true

    try {
      expenses.value = await expenseService.getAll()
    } finally {
      loading.value = false
    }
  }

  async function createExpense(description: string): Promise<Expense> {
    loading.value = true

    try {
      const expense = await expenseService.create(description)
      
      expenses.value.push(expense)
      
      return expense
    } finally {
      loading.value = false
    }
  }

  async function updateExpense(id: number, description: string): Promise<void> {
    loading.value = true

    try {
      const updated = await expenseService.update(id, description)
      
      const index = expenses.value.findIndex(e => e.id === id)
      
      if (index !== -1) {
        expenses.value[index] = updated
      }
    } finally {
      loading.value = false
    }
  }

  async function deleteExpense(id: number): Promise<void> {
    loading.value = true

    try {
      await expenseService.delete(id)
      
      expenses.value = expenses.value.filter(e => e.id !== id)
    } finally {
      loading.value = false
    }
  }

  return {
    expenses,
    loading,
    loadExpenses,
    createExpense,
    updateExpense,
    deleteExpense
  }
})
