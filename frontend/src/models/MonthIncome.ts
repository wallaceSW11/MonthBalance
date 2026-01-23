import type { IncomeTypeEnum } from './Income'

export interface MonthIncome {
  id: number
  incomeId: number
  incomeDescription: string
  incomeType: IncomeTypeEnum
  grossValue: number | null
  netValue: number | null
  hourlyRate: number | null
  hours: number | null
  minutes: number | null
}
