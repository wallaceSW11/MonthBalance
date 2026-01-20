export abstract class StorageService<T> {
  protected storageKey: string

  constructor(storageKey: string) {
    this.storageKey = storageKey
  }

  protected getFromStorage(): T | null {
    try {
      const data = localStorage.getItem(this.storageKey)
      
      if (!data) return null
      
      return JSON.parse(data) as T
    } catch (error) {
      console.error(`Error reading from localStorage (${this.storageKey}):`, error)
      
      return null
    }
  }

  protected saveToStorage(data: T): void {
    try {
      localStorage.setItem(this.storageKey, JSON.stringify(data))
    } catch (error) {
      console.error(`Error saving to localStorage (${this.storageKey}):`, error)
    }
  }

  protected removeFromStorage(): void {
    try {
      localStorage.removeItem(this.storageKey)
    } catch (error) {
      console.error(`Error removing from localStorage (${this.storageKey}):`, error)
    }
  }

  clear(): void {
    this.removeFromStorage()
  }
}
