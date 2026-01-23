import httpClient from './httpClient'
import type { Income, IncomeTypeEnum } from '@/models/Income'

export const incomeService = {
  async getAll(): Promise<Income[]> {
    const response = await httpClient.get<Income[]>('/incomes')
    
    return response.data
  },

  async getById(id: number): Promise<Income> {
    const response = await httpClient.get<Income>(`/incomes/${id}`)
    
    return response.data
  },

  async create(description: string, type: IncomeTypeEnum): Promise<Income> {
    const response = await httpClient.post<Income>('/incomes', {
      description,
      type
    })
    
    return response.data
  },

  async update(id: number, description: string, type: IncomeTypeEnum): Promise<Income> {
    const response = await httpClient.put<Income>(`/incomes/${id}`, {
      description,
      type
    })
    
    return response.data
  },

  async delete(id: number): Promise<void> {
    await httpClient.delete(`/incomes/${id}`)
  }
}
