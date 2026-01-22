import type { Income } from './Income'
import type { Expense } from './Expense'

export interface MonthData {
  id: number
  year: number
  month: number
  incomes: Income[]
  expenses: Expense[]
  totalIncome: number
  totalExpense: number
  balance: number
}

export interface MonthKey {
  year: number
  month: number
}

export interface MonthTotals {
  totalIncome: number
  totalExpense: number
  balance: number
}
