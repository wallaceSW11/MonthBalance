export enum IncomeTypeEnum {
  Manual = 0,
  Hourly = 1
}

export interface Income {
  id: number
  description: string
  type: IncomeTypeEnum
}
