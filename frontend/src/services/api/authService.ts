import httpClient from './httpClient'
import type { AuthResponse, LoginRequest, RegisterRequest } from '@/models/Auth'

export const authService = {
  async login(email: string, password: string): Promise<AuthResponse> {
    const response = await httpClient.post<AuthResponse>('/auth/login', {
      email,
      password
    } as LoginRequest)
    
    return response.data
  },

  async register(email: string, password: string, confirmPassword: string): Promise<AuthResponse> {
    const response = await httpClient.post<AuthResponse>('/auth/register', {
      email,
      password,
      confirmPassword
    } as RegisterRequest)
    
    return response.data
  },

  async checkEmail(email: string): Promise<boolean> {
    const response = await httpClient.get<boolean>(`/auth/check-email/${email}`)
    
    return response.data
  },

  logout(): void {
    localStorage.removeItem('auth_token')
    localStorage.removeItem('auth_email')
  }
}
