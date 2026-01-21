import { startAuthentication, startRegistration } from '@simplewebauthn/browser'

interface PublicKeyCredentialCreationOptionsJSON {
  challenge: string
  rp: {
    name: string
    id: string
  }
  user: {
    id: string
    name: string
    displayName: string
  }
  pubKeyCredParams: Array<{ alg: number; type: 'public-key' }>
  timeout: number
  attestation: 'none' | 'indirect' | 'direct'
  authenticatorSelection: {
    authenticatorAttachment: 'platform' | 'cross-platform'
    userVerification: 'required' | 'preferred' | 'discouraged'
    requireResidentKey: boolean
  }
}

interface PublicKeyCredentialRequestOptionsJSON {
  challenge: string
  timeout: number
  userVerification: 'required' | 'preferred' | 'discouraged'
  rpId: string
}

const AUTH_KEY = 'mb_auth_registered'
const PIN_KEY = 'mb_pin_hash'
const LAST_AUTH_KEY = 'mb_last_auth'
const HIDDEN_AT_KEY = 'mb_hidden_at'

class AuthService {
  private readonly AUTH_TIMEOUT = 30 * 1000
  private isListenerSetup = false

  private get lastAuthTime(): number {
    return Number(localStorage.getItem(LAST_AUTH_KEY) || 0)
  }

  private set lastAuthTime(value: number) {
    localStorage.setItem(LAST_AUTH_KEY, String(value))
  }

  private get hiddenAt(): number {
    return Number(localStorage.getItem(HIDDEN_AT_KEY) || 0)
  }

  private set hiddenAt(value: number) {
    localStorage.setItem(HIDDEN_AT_KEY, String(value))
  }

  isDevMode(): boolean {
    return import.meta.env.DEV
  }

  isRegistered(): boolean {
    return localStorage.getItem(AUTH_KEY) === 'true'
  }

  hasPIN(): boolean {
    return !!localStorage.getItem(PIN_KEY)
  }

  setupLifecycleGuards(): void {
    if (this.isListenerSetup || typeof window === 'undefined') return
    
    this.isListenerSetup = true

    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'hidden') {
        this.hiddenAt = Date.now()
      }
    })

    window.addEventListener('pageshow', () => {
      this.checkResumeLock()
    })

    setInterval(() => {
      this.checkResumeLock()
    }, 1000)
  }

  private checkResumeLock(): void {
    if (!this.hiddenAt) return
    
    const diff = Date.now() - this.hiddenAt
    
    if (diff > this.AUTH_TIMEOUT) {
      this.forceLock()
    }
  }

  private forceLock(): void {
    localStorage.removeItem(LAST_AUTH_KEY)
    localStorage.removeItem(HIDDEN_AT_KEY)
  }

  isAuthRequired(): boolean {
    if (this.isDevMode()) return false
    
    const last = this.lastAuthTime
    
    if (!last) return true
    
    const diff = Date.now() - last
    
    return diff > this.AUTH_TIMEOUT
  }

  async isBiometricAvailable(): Promise<boolean> {
    if (!window.PublicKeyCredential) return false
    
    try {
      return await window.PublicKeyCredential.isUserVerifyingPlatformAuthenticatorAvailable()
    } catch {
      return false
    }
  }

  async registerBiometric(): Promise<boolean> {
    try {
      const options: PublicKeyCredentialCreationOptionsJSON = {
        challenge: this.generateChallenge(),
        rp: {
          name: 'MonthBalance',
          id: window.location.hostname,
        },
        user: {
          id: this.generateUserId(),
          name: 'user',
          displayName: 'MonthBalance User',
        },
        pubKeyCredParams: [
          { alg: -7, type: 'public-key' },
          { alg: -257, type: 'public-key' },
        ],
        timeout: 60000,
        attestation: 'none',
        authenticatorSelection: {
          authenticatorAttachment: 'platform',
          userVerification: 'required',
          requireResidentKey: false,
        },
      }

      const credential = await startRegistration({ optionsJSON: options })
      
      localStorage.setItem(AUTH_KEY, 'true')
      localStorage.setItem('mb_credential', JSON.stringify(credential))
      
      this.lastAuthTime = Date.now()
      
      return true
    } catch (err) {
      console.error('Biometric registration failed:', err)
      
      return false
    }
  }

  async authenticateBiometric(): Promise<boolean> {
    try {
      const options: PublicKeyCredentialRequestOptionsJSON = {
        challenge: this.generateChallenge(),
        timeout: 60000,
        userVerification: 'required',
        rpId: window.location.hostname,
      }

      await startAuthentication({ optionsJSON: options })
      
      this.lastAuthTime = Date.now()
      this.hiddenAt = 0
      
      return true
    } catch (err) {
      console.error('Biometric authentication failed:', err)
      
      return false
    }
  }

  async registerPIN(pin: string): Promise<boolean> {
    if (pin.length < 4) return false
    
    const hash = await this.hashPIN(pin)
    
    localStorage.setItem(AUTH_KEY, 'true')
    localStorage.setItem(PIN_KEY, hash)
    
    this.lastAuthTime = Date.now()
    this.hiddenAt = 0
    
    return true
  }

  async authenticatePIN(pin: string): Promise<boolean> {
    const storedHash = localStorage.getItem(PIN_KEY)
    
    if (!storedHash) return false
    
    const hash = await this.hashPIN(pin)
    
    if (hash !== storedHash) return false
    
    this.lastAuthTime = Date.now()
    this.hiddenAt = 0
    
    return true
  }

  private generateChallenge(): string {
    const array = new Uint8Array(32)
    crypto.getRandomValues(array)
    
    return btoa(String.fromCharCode(...array))
  }

  private generateUserId(): string {
    const array = new Uint8Array(16)
    crypto.getRandomValues(array)
    
    return btoa(String.fromCharCode(...array))
  }

  private async hashPIN(pin: string): Promise<string> {
    const encoder = new TextEncoder()
    const data = encoder.encode(pin + 'mb_salt_2026')
    const hashBuffer = await crypto.subtle.digest('SHA-256', data)
    const hashArray = Array.from(new Uint8Array(hashBuffer))
    
    return hashArray.map(b => b.toString(16).padStart(2, '0')).join('')
  }
}

export const authService = new AuthService()
