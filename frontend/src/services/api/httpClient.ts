import axios, { type AxiosInstance } from 'axios'

const httpClient: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5130/api',
  headers: {
    'Content-Type': 'application/json'
  }
})

httpClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('auth_token')
    
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('auth_token')
      localStorage.removeItem('auth_email')
      
      window.location.href = '/login'
    }
    
    return Promise.reject(error)
  }
)

export default httpClient
