import { StorageService } from './StorageService'
import type { Settings } from '@/models/Settings'

export class SettingsStorageService extends StorageService<Settings> {
  constructor() {
    super('month-balance-settings')
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

  private getDefaultSettings(): Settings {
    return {
      theme: 'dark',
      locale: 'pt-BR',
    }
  }
}

export const settingsStorageService = new SettingsStorageService()
