import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { monthExpenseService } from '@/services/api/monthExpenseService'
import type { MonthExpense } from '@/models/MonthExpense'

export const useExpenseStore = defineStore('expense', () => {
  const monthExpenses = ref<MonthExpense[]>([])
  const loading = ref(false)

  const totalExpense = computed(() => 
    monthExpenses.value.reduce((sum, expense) => sum + expense.value, 0)
  )

  async function loadMonthExpenses(year: number, month: number): Promise<void> {
    loading.value = true

    try {
      monthExpenses.value = await monthExpenseService.getByMonth(year, month)
    } finally {
      loading.value = false
    }
  }

  async function createMonthExpense(year: number, month: number, data: {
    expenseId: number
    value: number
  }): Promise<MonthExpense> {
    loading.value = true

    try {
      const monthExpense = await monthExpenseService.create(year, month, data)
      
      monthExpenses.value.push(monthExpense)
      
      return monthExpense
    } finally {
      loading.value = false
    }
  }

  async function updateMonthExpense(year: number, month: number, id: number, data: {
    value: number
  }): Promise<void> {
    loading.value = true

    try {
      const updated = await monthExpenseService.update(year, month, id, data)
      
      const index = monthExpenses.value.findIndex(e => e.id === id)
      
      if (index !== -1) {
        monthExpenses.value[index] = updated
      }
    } finally {
      loading.value = false
    }
  }

  async function deleteMonthExpense(year: number, month: number, id: number): Promise<void> {
    loading.value = true

    try {
      await monthExpenseService.delete(year, month, id)
      
      monthExpenses.value = monthExpenses.value.filter(e => e.id !== id)
    } finally {
      loading.value = false
    }
  }

  return {
    monthExpenses,
    loading,
    totalExpense,
    loadMonthExpenses,
    createMonthExpense,
    updateMonthExpense,
    deleteMonthExpense
  }
})
