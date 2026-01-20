import { StorageService } from './StorageService'
import type { Expense } from '@/models/Expense'

type ExpensesByMonth = Record<string, Expense[]>

export class ExpenseStorageService extends StorageService<ExpensesByMonth> {
  constructor() {
    super('monthbalance:expenses')
  }

  getExpensesByMonth(year: number, month: number): Expense[] {
    const allExpenses = this.getFromStorage() || {}
    const monthKey = this.getMonthKey(year, month)
    
    return allExpenses[monthKey] || []
  }

  saveExpensesByMonth(year: number, month: number, expenses: Expense[]): void {
    const allExpenses = this.getFromStorage() || {}
    const monthKey = this.getMonthKey(year, month)
    
    allExpenses[monthKey] = expenses
    this.saveToStorage(allExpenses)
  }

  addExpense(year: number, month: number, expense: Expense): void {
    const expenses = this.getExpensesByMonth(year, month)
    
    expenses.push(expense)
    this.saveExpensesByMonth(year, month, expenses)
  }

  updateExpense(year: number, month: number, updatedExpense: Expense): void {
    const expenses = this.getExpensesByMonth(year, month)
    const index = expenses.findIndex((e) => e.id === updatedExpense.id)
    
    if (index === -1) return
    
    expenses[index] = updatedExpense
    this.saveExpensesByMonth(year, month, expenses)
  }

  deleteExpense(year: number, month: number, expenseId: string): void {
    const expenses = this.getExpensesByMonth(year, month)
    const filtered = expenses.filter((e) => e.id !== expenseId)
    
    this.saveExpensesByMonth(year, month, filtered)
  }

  monthExists(year: number, month: number): boolean {
    const allExpenses = this.getFromStorage() || {}
    const monthKey = this.getMonthKey(year, month)
    
    return monthKey in allExpenses
  }

  private getMonthKey(year: number, month: number): string {
    const monthStr = month.toString().padStart(2, '0')
    
    return `${year}-${monthStr}`
  }
}

export const expenseStorageService = new ExpenseStorageService()
