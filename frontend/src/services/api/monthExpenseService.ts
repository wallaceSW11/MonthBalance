import httpClient from './httpClient'
import type { MonthExpense } from '@/models/MonthExpense'

interface CreateMonthExpenseData {
  expenseId: number
  value: number
}

export const monthExpenseService = {
  async getByMonth(year: number, month: number): Promise<MonthExpense[]> {
    const response = await httpClient.get<MonthExpense[]>(`/months/${year}/${month}/expenses`)
    
    return response.data
  },

  async create(year: number, month: number, data: CreateMonthExpenseData): Promise<MonthExpense> {
    const response = await httpClient.post<MonthExpense>(`/months/${year}/${month}/expenses`, data)
    
    return response.data
  },

  async update(year: number, month: number, id: number, data: Partial<CreateMonthExpenseData>): Promise<MonthExpense> {
    const response = await httpClient.put<MonthExpense>(`/months/${year}/${month}/expenses/${id}`, data)
    
    return response.data
  },

  async delete(year: number, month: number, id: number): Promise<void> {
    await httpClient.delete(`/months/${year}/${month}/expenses/${id}`)
  }
}
