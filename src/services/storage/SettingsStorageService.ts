import { StorageService } from './StorageService'
import type { Settings } from '@/models/Settings'

export class SettingsStorageService extends StorageService<Settings> {
  constructor() {
    super('monthbalance:settings')
  }

  getSettings(): Settings {
    const settings = this.getFromStorage()
    
    if (!settings) {
      return this.getDefaultSettings()
    }
    
    return settings
  }

  saveSettings(settings: Settings): void {
    this.saveToStorage(settings)
  }

  updateTheme(theme: 'light' | 'dark'): void {
    const settings = this.getSettings()
    
    settings.theme = theme
    this.saveSettings(settings)
  }

  updateLocale(locale: 'pt-BR' | 'en-US'): void {
    const settings = this.getSettings()
    
    settings.locale = locale
    this.saveSettings(settings)
  }

  updateIncomesCollapsed(collapsed: boolean): void {
    const settings = this.getSettings()
    
    settings.incomesCollapsed = collapsed
    this.saveSettings(settings)
  }

  updateExpensesCollapsed(collapsed: boolean): void {
    const settings = this.getSettings()
    
    settings.expensesCollapsed = collapsed
    this.saveSettings(settings)
  }

  private getDefaultSettings(): Settings {
    const browserLang = navigator.language.toLowerCase()
    const defaultLocale = browserLang.startsWith('pt') ? 'pt-BR' : 'en-US'
    
    return {
      theme: 'dark',
      locale: defaultLocale as 'pt-BR' | 'en-US',
      incomesCollapsed: false,
      expensesCollapsed: false,
    }
  }
}

export const settingsStorageService = new SettingsStorageService()
