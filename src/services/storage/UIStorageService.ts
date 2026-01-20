import { StorageService } from './StorageService'

interface UIState {
  incomesCollapsed: boolean
  expensesCollapsed: boolean
}

export class UIStorageService extends StorageService<UIState> {
  constructor() {
    super('monthbalance:ui')
  }

  getUIState(): UIState {
    const state = this.getFromStorage()
    
    if (!state) {
      return this.getDefaultUIState()
    }
    
    return state
  }

  saveUIState(state: UIState): void {
    this.saveToStorage(state)
  }

  updateIncomesCollapsed(collapsed: boolean): void {
    const state = this.getUIState()
    
    state.incomesCollapsed = collapsed
    this.saveUIState(state)
  }

  updateExpensesCollapsed(collapsed: boolean): void {
    const state = this.getUIState()
    
    state.expensesCollapsed = collapsed
    this.saveUIState(state)
  }

  private getDefaultUIState(): UIState {
    return {
      incomesCollapsed: false,
      expensesCollapsed: false,
    }
  }
}

export const uiStorageService = new UIStorageService()
