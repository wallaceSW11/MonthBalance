import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { monthIncomeService } from '@/services/api/monthIncomeService'
import type { MonthIncome } from '@/models/MonthIncome'

export const useIncomeStore = defineStore('income', () => {
  const monthIncomes = ref<MonthIncome[]>([])
  const loading = ref(false)

  const totalIncome = computed(() => 
    monthIncomes.value.reduce((sum, income) => sum + (income.netValue ?? 0), 0)
  )

  async function loadMonthIncomes(year: number, month: number): Promise<void> {
    loading.value = true

    try {
      monthIncomes.value = await monthIncomeService.getByMonth(year, month)
    } finally {
      loading.value = false
    }
  }

  async function createMonthIncome(year: number, month: number, data: {
    incomeId: number
    grossValue?: number | null
    netValue?: number | null
    hourlyRate?: number | null
    hours?: number | null
    minutes?: number | null
  }): Promise<MonthIncome> {
    loading.value = true

    try {
      const monthIncome = await monthIncomeService.create(year, month, data)
      
      monthIncomes.value.push(monthIncome)
      
      return monthIncome
    } finally {
      loading.value = false
    }
  }

  async function updateMonthIncome(year: number, month: number, id: number, data: {
    grossValue?: number | null
    netValue?: number | null
    hourlyRate?: number | null
    hours?: number | null
    minutes?: number | null
  }): Promise<void> {
    loading.value = true

    try {
      const updated = await monthIncomeService.update(year, month, id, data)
      
      const index = monthIncomes.value.findIndex(i => i.id === id)
      
      if (index !== -1) {
        monthIncomes.value[index] = updated
      }
    } finally {
      loading.value = false
    }
  }

  async function deleteMonthIncome(year: number, month: number, id: number): Promise<void> {
    loading.value = true

    try {
      await monthIncomeService.delete(year, month, id)
      
      monthIncomes.value = monthIncomes.value.filter(i => i.id !== id)
    } finally {
      loading.value = false
    }
  }

  return {
    monthIncomes,
    loading,
    totalIncome,
    loadMonthIncomes,
    createMonthIncome,
    updateMonthIncome,
    deleteMonthIncome
  }
})
