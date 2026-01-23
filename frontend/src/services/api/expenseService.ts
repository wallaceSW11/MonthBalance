import httpClient from './httpClient'
import type { Expense } from '@/models/Expense'

export const expenseService = {
  async getAll(): Promise<Expense[]> {
    const response = await httpClient.get<Expense[]>('/expenses')
    
    return response.data
  },

  async getById(id: number): Promise<Expense> {
    const response = await httpClient.get<Expense>(`/expenses/${id}`)
    
    return response.data
  },

  async create(description: string): Promise<Expense> {
    const response = await httpClient.post<Expense>('/expenses', {
      description
    })
    
    return response.data
  },

  async update(id: number, description: string): Promise<Expense> {
    const response = await httpClient.put<Expense>(`/expenses/${id}`, {
      description
    })
    
    return response.data
  },

  async delete(id: number): Promise<void> {
    await httpClient.delete(`/expenses/${id}`)
  }
}
