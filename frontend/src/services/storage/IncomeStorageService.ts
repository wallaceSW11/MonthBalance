import { StorageService } from './StorageService'
import type { Income } from '@/models/Income'

type IncomesByMonth = Record<string, Income[]>

export class IncomeStorageService extends StorageService<IncomesByMonth> {
  constructor() {
    super('monthbalance:incomes')
  }

  getIncomesByMonth(year: number, month: number): Income[] {
    const allIncomes = this.getFromStorage() || {}
    const monthKey = this.getMonthKey(year, month)
    
    return allIncomes[monthKey] || []
  }

  saveIncomesByMonth(year: number, month: number, incomes: Income[]): void {
    const allIncomes = this.getFromStorage() || {}
    const monthKey = this.getMonthKey(year, month)
    
    allIncomes[monthKey] = incomes
    this.saveToStorage(allIncomes)
  }

  addIncome(year: number, month: number, income: Income): void {
    const incomes = this.getIncomesByMonth(year, month)
    
    incomes.push(income)
    this.saveIncomesByMonth(year, month, incomes)
  }

  updateIncome(year: number, month: number, updatedIncome: Income): void {
    const incomes = this.getIncomesByMonth(year, month)
    const index = incomes.findIndex((i) => i.id === updatedIncome.id)
    
    if (index === -1) return
    
    incomes[index] = updatedIncome
    this.saveIncomesByMonth(year, month, incomes)
  }

  deleteIncome(year: number, month: number, incomeId: string): void {
    const incomes = this.getIncomesByMonth(year, month)
    const filtered = incomes.filter((i) => i.id !== incomeId)
    
    this.saveIncomesByMonth(year, month, filtered)
  }

  monthExists(year: number, month: number): boolean {
    const allIncomes = this.getFromStorage() || {}
    const monthKey = this.getMonthKey(year, month)
    
    return monthKey in allIncomes
  }

  private getMonthKey(year: number, month: number): string {
    const monthStr = month.toString().padStart(2, '0')
    
    return `${year}-${monthStr}`
  }
}

export const incomeStorageService = new IncomeStorageService()
