const FIXED_USER_ID = 'user-test'
const FIXED_USER_EMAIL = 'teste@teste.com'
const FIXED_USER_PASSWORD = 'senha'
const FIXED_USER_NAME = 'Usuário Teste'


const generateId = (): string => {
  return `${Date.now()}-${Math.random().toString(36).substring(2, 9)}`
}

const delay = (ms: number = 300): Promise<void> => {
  return new Promise(resolve => setTimeout(resolve, ms))
}

const getStorageKey = (resource: string): string => {
  return `monthbalance_${resource}`
}

const getAll = <T>(resource: string): T[] => {
  const key = getStorageKey(resource)
  const data = localStorage.getItem(key)
  
  if (!data) return []
  
  return JSON.parse(data) as T[]
}

const saveAll = <T>(resource: string, data: T[]): void => {
  const key = getStorageKey(resource)
  localStorage.setItem(key, JSON.stringify(data))
}

export const localStorageService = {
  async get<T>(resource: string, id?: string): Promise<T | T[] | null> {
    await delay()
    
    const items = getAll<T & { id: string }>(resource)
    
    if (!id) return items
    
    const item = items.find(i => i.id === id)
    
    return item ?? null
  },

  async post<T extends { id?: string; userId?: string }>(resource: string, data: Omit<T, 'id' | 'userId'> & { id?: string; userId?: string }): Promise<T> {
    await delay()
    
    const items = getAll<T & { id: string }>(resource)
    const newItem = {
      ...data,
      id: data.id ?? generateId(),
      userId: data.userId ?? FIXED_USER_ID
    } as T & { id: string }
    
    items.push(newItem)
    saveAll(resource, items)
    
    return newItem
  },

  async put<T extends { id: string }>(resource: string, id: string, data: Partial<T>): Promise<T | null> {
    await delay()
    
    const items = getAll<T & { id: string }>(resource)
    const index = items.findIndex(i => i.id === id)
    
    if (index === -1) return null
    
    items[index] = { ...items[index], ...data }
    saveAll(resource, items)
    
    return items[index]
  },

  async delete(resource: string, id: string): Promise<boolean> {
    await delay()
    
    const items = getAll<{ id: string }>(resource)
    const filtered = items.filter(i => i.id !== id)
    
    if (filtered.length === items.length) return false
    
    saveAll(resource, filtered)
    
    return true
  },

  getFixedUser: () => ({
    id: FIXED_USER_ID,
    name: FIXED_USER_NAME,
    email: FIXED_USER_EMAIL,
    password: FIXED_USER_PASSWORD
  }),

  clearAll: () => {
    const keys = Object.keys(localStorage).filter(key => key.startsWith('monthbalance_'))
    keys.forEach(key => localStorage.removeItem(key))
  }
}
