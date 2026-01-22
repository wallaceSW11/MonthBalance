import httpClient from './httpClient'
import type { IncomeType, IncomeTypeFormData } from '@/models/IncomeType'

export const incomeTypeService = {
  async getAll(): Promise<IncomeType[]> {
    const response = await httpClient.get('/incometypes')
    return response.data
  },

  async getById(id: number): Promise<IncomeType> {
    const response = await httpClient.get(`/incometypes/${id}`)
    return response.data
  },

  async create(data: IncomeTypeFormData): Promise<IncomeType> {
    const response = await httpClient.post('/incometypes', data)
    return response.data
  },

  async update(id: number, data: IncomeTypeFormData): Promise<IncomeType> {
    const response = await httpClient.put(`/incometypes/${id}`, data)
    return response.data
  },

  async delete(id: number): Promise<void> {
    await httpClient.delete(`/incometypes/${id}`)
  }
}
