export enum IncomeTypeEnum {
  Manual = 0,
  Hourly = 1
}

export interface IncomeType {
  id: number
  name: string
  type: IncomeTypeEnum
}

export interface IncomeTypeFormData {
  name: string
  type: IncomeTypeEnum
}
