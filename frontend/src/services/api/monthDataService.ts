import httpClient from './httpClient'
import type { MonthData } from '@/models/MonthData'

interface CreateMonthDataDto {
  year: number
  month: number
}

interface DuplicateMonthDto {
  sourceYear: number
  sourceMonth: number
  targetYear: number
  targetMonth: number
}

export const monthDataService = {
  async getAll(): Promise<MonthData[]> {
    const response = await httpClient.get<MonthData[]>('/monthdata')
    
    return response.data
  },

  async getById(id: number): Promise<MonthData> {
    const response = await httpClient.get<MonthData>(`/monthdata/${id}`)
    
    return response.data
  },

  async getByYearAndMonth(year: number, month: number): Promise<MonthData | null> {
    try {
      const response = await httpClient.get<MonthData>(`/monthdata/${year}/${month}`)
      
      return response.data
    } catch (error: any) {
      if (error.response?.status === 404) return null
      
      throw error
    }
  },

  async create(data: CreateMonthDataDto): Promise<MonthData> {
    const response = await httpClient.post<MonthData>('/monthdata', data)
    
    return response.data
  },

  async delete(id: number): Promise<void> {
    await httpClient.delete(`/monthdata/${id}`)
  },

  async duplicate(data: DuplicateMonthDto): Promise<MonthData> {
    const response = await httpClient.post<MonthData>('/monthdata/duplicate', data)
    
    return response.data
  },
}
