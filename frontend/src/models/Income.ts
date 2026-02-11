export interface Income {
  id: number;
  monthDataId: number;
  incomeTypeId: number;
  grossValue: number | null;
  netValue: number | null;
  hourlyRate: number | null;
  hours: number | null;
  minutes: number | null;
  calculatedValue: number;
}
