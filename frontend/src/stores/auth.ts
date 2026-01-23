import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { authService } from '@/services/api/authService'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('auth_token'))
  const email = ref<string | null>(localStorage.getItem('auth_email'))

  const authenticated = computed(() => !!token.value)

  async function login(emailValue: string, password: string): Promise<void> {
    const response = await authService.login(emailValue, password)
    
    token.value = response.token
    email.value = response.email
    
    localStorage.setItem('auth_token', response.token)
    localStorage.setItem('auth_email', response.email)
  }

  async function register(emailValue: string, password: string, confirmPassword: string): Promise<void> {
    const response = await authService.register(emailValue, password, confirmPassword)
    
    token.value = response.token
    email.value = response.email
    
    localStorage.setItem('auth_token', response.token)
    localStorage.setItem('auth_email', response.email)
  }

  function logout(): void {
    authService.logout()
    
    token.value = null
    email.value = null
  }

  function checkAuth(): boolean {
    return authenticated.value
  }

  return {
    token,
    email,
    authenticated,
    login,
    register,
    logout,
    checkAuth
  }
})
