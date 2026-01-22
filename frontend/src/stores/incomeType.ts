import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { incomeTypeService } from '@/services/api/incomeTypeService'
import type { IncomeType, IncomeTypeFormData } from '@/models/IncomeType'
import { IncomeTypeEnum } from '@/models/IncomeType'

export const useIncomeTypeStore = defineStore('incomeType', () => {
  const incomeTypes = ref<IncomeType[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const manualTypes = computed(() => 
    incomeTypes.value.filter(t => t.type === IncomeTypeEnum.Manual)
  )

  const hourlyTypes = computed(() => 
    incomeTypes.value.filter(t => t.type === IncomeTypeEnum.Hourly)
  )

  async function loadIncomeTypes() {
    loading.value = true
    error.value = null

    try {
      incomeTypes.value = await incomeTypeService.getAll()
    } catch (err) {
      error.value = 'Erro ao carregar tipos de receita'
      console.error(err)
    } finally {
      loading.value = false
    }
  }

  async function createIncomeType(data: IncomeTypeFormData) {
    loading.value = true
    error.value = null

    try {
      const created = await incomeTypeService.create(data)
      incomeTypes.value.push(created)
      return created
    } catch (err) {
      error.value = 'Erro ao criar tipo de receita'
      console.error(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateIncomeType(id: number, data: IncomeTypeFormData) {
    loading.value = true
    error.value = null

    try {
      const updated = await incomeTypeService.update(id, data)
      const index = incomeTypes.value.findIndex(t => t.id === id)
      
      if (index !== -1) {
        incomeTypes.value[index] = updated
      }
      
      return updated
    } catch (err) {
      error.value = 'Erro ao atualizar tipo de receita'
      console.error(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteIncomeType(id: number) {
    loading.value = true
    error.value = null

    try {
      await incomeTypeService.delete(id)
      incomeTypes.value = incomeTypes.value.filter(t => t.id !== id)
    } catch (err) {
      error.value = 'Erro ao deletar tipo de receita'
      console.error(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  function getIncomeTypeById(id: number) {
    return incomeTypes.value.find(t => t.id === id)
  }

  return {
    incomeTypes,
    manualTypes,
    hourlyTypes,
    loading,
    error,
    loadIncomeTypes,
    createIncomeType,
    updateIncomeType,
    deleteIncomeType,
    getIncomeTypeById
  }
})
