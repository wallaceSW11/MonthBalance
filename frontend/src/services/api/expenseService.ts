import httpClient from './httpClient'
import type { Expense, ExpenseFormData } from '@/models/Expense'

export const expenseService = {
  async getByMonth(year: number, month: number): Promise<Expense[]> {
    const response = await httpClient.get<Expense[]>(`/months/${year}/${month}/expenses`)
    
    return response.data
  },

  async getById(year: number, month: number, id: number): Promise<Expense> {
    const response = await httpClient.get<Expense>(`/months/${year}/${month}/expenses/${id}`)
    
    return response.data
  },

  async create(year: number, month: number, data: ExpenseFormData): Promise<Expense> {
    const response = await httpClient.post<Expense>(`/months/${year}/${month}/expenses`, data)
    
    return response.data
  },

  async update(year: number, month: number, id: number, data: ExpenseFormData): Promise<Expense> {
    const response = await httpClient.put<Expense>(`/months/${year}/${month}/expenses/${id}`, data)
    
    return response.data
  },

  async delete(year: number, month: number, id: number): Promise<void> {
    await httpClient.delete(`/months/${year}/${month}/expenses/${id}`)
  },
}
