import type { MonthIncome } from './MonthIncome'
import type { MonthExpense } from './MonthExpense'

export interface MonthData {
  id: number
  year: number
  month: number
  incomes: MonthIncome[]
  expenses: MonthExpense[]
  totalIncome: number
  totalExpense: number
  balance: number
}
