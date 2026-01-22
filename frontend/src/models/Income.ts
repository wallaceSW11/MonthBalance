import type { IncomeTypeEnum } from './IncomeType'

export interface Income {
  id: number
  incomeTypeId: number
  incomeTypeName: string
  incomeTypeType: IncomeTypeEnum
  grossValue?: number
  netValue?: number
  hourlyRate?: number
  hours?: number
  minutes?: number
  monthDataId: number
}

export interface IncomeFormData {
  incomeTypeId: number
  grossValue?: number
  netValue?: number
  hourlyRate?: number
  hours?: number
  minutes?: number
}
