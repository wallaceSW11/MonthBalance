export type IncomeType = 'manual' | 'hourly'

export interface Income {
  id: string
  name: string
  type: IncomeType
  grossValue?: number
  netValue?: number
  hourlyRate?: number
  hours?: number
  minutes?: number
}

export interface IncomeFormData {
  name: string
  type: IncomeType
  grossValue: number
  netValue: number
  hourlyRate: number
  hours: number
  minutes: number
}
