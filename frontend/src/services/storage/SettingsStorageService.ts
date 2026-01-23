interface Settings {
  theme: 'light' | 'dark' | 'system'
  locale: string
}

const SETTINGS_KEY = 'settings'

const defaultSettings: Settings = {
  theme: 'system',
  locale: 'pt-BR'
}

export const settingsStorageService = {
  getSettings(): Settings {
    const stored = localStorage.getItem(SETTINGS_KEY)
    
    if (!stored) return defaultSettings
    
    try {
      return { ...defaultSettings, ...JSON.parse(stored) }
    } catch {
      return defaultSettings
    }
  },

  saveSettings(settings: Partial<Settings>): void {
    const current = this.getSettings()
    const updated = { ...current, ...settings }
    
    localStorage.setItem(SETTINGS_KEY, JSON.stringify(updated))
  },

  setTheme(theme: 'light' | 'dark' | 'system'): void {
    this.saveSettings({ theme })
  },

  setLocale(locale: string): void {
    this.saveSettings({ locale })
  }
}
