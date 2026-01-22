import axios, { type AxiosInstance } from 'axios'

const baseURL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5150/api'

const httpClient: AxiosInstance = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
})

httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    const status = error.response?.status
    
    if (status !== 404 && status !== 400) {
      console.error('API Error:', error)
    }
    
    return Promise.reject(error)
  }
)

export default httpClient
