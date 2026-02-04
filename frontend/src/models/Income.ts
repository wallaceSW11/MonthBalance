export interface Income {
  id: string
  monthDataId: string
  incomeTypeId: string
  grossValue?: number
  netValue?: number
  hourlyRate?: number
  hours?: number
  minutes?: number
  calculatedValue: number
}
