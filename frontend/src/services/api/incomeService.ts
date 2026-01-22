import httpClient from './httpClient'
import type { Income, IncomeFormData } from '@/models/Income'

export const incomeService = {
  async getByMonth(year: number, month: number): Promise<Income[]> {
    const response = await httpClient.get<Income[]>(`/months/${year}/${month}/incomes`)
    
    return response.data
  },

  async getById(year: number, month: number, id: number): Promise<Income> {
    const response = await httpClient.get<Income>(`/months/${year}/${month}/incomes/${id}`)
    
    return response.data
  },

  async create(year: number, month: number, data: IncomeFormData): Promise<Income> {
    const response = await httpClient.post<Income>(`/months/${year}/${month}/incomes`, data)
    
    return response.data
  },

  async update(year: number, month: number, id: number, data: IncomeFormData): Promise<Income> {
    const response = await httpClient.put<Income>(`/months/${year}/${month}/incomes/${id}`, data)
    
    return response.data
  },

  async delete(year: number, month: number, id: number): Promise<void> {
    await httpClient.delete(`/months/${year}/${month}/incomes/${id}`)
  },
}
