import httpClient from './httpClient'
import type { MonthIncome } from '@/models/MonthIncome'

interface CreateMonthIncomeData {
  incomeId: number
  grossValue?: number | null
  netValue?: number | null
  hourlyRate?: number | null
  hours?: number | null
  minutes?: number | null
}

export const monthIncomeService = {
  async getByMonth(year: number, month: number): Promise<MonthIncome[]> {
    const response = await httpClient.get<MonthIncome[]>(`/months/${year}/${month}/incomes`)
    
    return response.data
  },

  async create(year: number, month: number, data: CreateMonthIncomeData): Promise<MonthIncome> {
    const response = await httpClient.post<MonthIncome>(`/months/${year}/${month}/incomes`, data)
    
    return response.data
  },

  async update(year: number, month: number, id: number, data: Partial<CreateMonthIncomeData>): Promise<MonthIncome> {
    const response = await httpClient.put<MonthIncome>(`/months/${year}/${month}/incomes/${id}`, data)
    
    return response.data
  },

  async delete(year: number, month: number, id: number): Promise<void> {
    await httpClient.delete(`/months/${year}/${month}/incomes/${id}`)
  }
}
