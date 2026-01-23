import { ref } from 'vue'
import { defineStore } from 'pinia'
import { incomeService } from '@/services/api/incomeService'
import type { Income, IncomeTypeEnum } from '@/models/Income'

export const useIncomeGlobalStore = defineStore('incomeGlobal', () => {
  const incomes = ref<Income[]>([])
  const loading = ref(false)

  async function loadIncomes(): Promise<void> {
    loading.value = true

    try {
      incomes.value = await incomeService.getAll()
    } finally {
      loading.value = false
    }
  }

  async function createIncome(description: string, type: IncomeTypeEnum): Promise<Income> {
    loading.value = true

    try {
      const income = await incomeService.create(description, type)
      
      incomes.value.push(income)
      
      return income
    } finally {
      loading.value = false
    }
  }

  async function updateIncome(id: number, description: string, type: IncomeTypeEnum): Promise<void> {
    loading.value = true

    try {
      const updated = await incomeService.update(id, description, type)
      
      const index = incomes.value.findIndex(i => i.id === id)
      
      if (index !== -1) {
        incomes.value[index] = updated
      }
    } finally {
      loading.value = false
    }
  }

  async function deleteIncome(id: number): Promise<void> {
    loading.value = true

    try {
      await incomeService.delete(id)
      
      incomes.value = incomes.value.filter(i => i.id !== id)
    } finally {
      loading.value = false
    }
  }

  return {
    incomes,
    loading,
    loadIncomes,
    createIncome,
    updateIncome,
    deleteIncome
  }
})
