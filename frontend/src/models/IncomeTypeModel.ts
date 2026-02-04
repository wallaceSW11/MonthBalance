import type { IncomeType } from './IncomeType'

export interface IncomeTypeModel {
  id: string
  userId: string
  name: string
  type: IncomeType
}
