import httpClient from './httpClient'
import type { MonthData } from '@/models/MonthData'

export const monthDataService = {
  async getOrCreate(year: number, month: number): Promise<MonthData> {
    const response = await httpClient.get<MonthData>(`/months/${year}/${month}`)
    
    return response.data
  },

  async getAll(): Promise<MonthData[]> {
    const response = await httpClient.get<MonthData[]>('/months')
    
    return response.data
  },

  async duplicate(sourceYear: number, sourceMonth: number, targetYear: number, targetMonth: number): Promise<MonthData> {
    const response = await httpClient.post<MonthData>('/months/duplicate', {
      sourceYear,
      sourceMonth,
      targetYear,
      targetMonth
    })
    
    return response.data
  },

  async delete(year: number, month: number): Promise<void> {
    await httpClient.delete(`/months/${year}/${month}`)
  }
}
