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

class AuthService {
  private lastAuthTime = 0
  private readonly AUTH_TIMEOUT = 30 * 1000 // 30 segundos
  private isListenerSetup = false
  private wasInBackground = false

  isDevMode(): boolean {
    return import.meta.env.DEV
  }

  isAuthRequired(): boolean {
    if (this.isDevMode()) return false
    
    if (this.lastAuthTime === 0) return true
    
    if (this.wasInBackground) {
      this.wasInBackground = false
      return true
    }
    
    const now = Date.now()
    const timeSinceAuth = now - this.lastAuthTime
    
    return timeSinceAuth > this.AUTH_TIMEOUT
  }

  isRegistered(): boolean {
    return localStorage.getItem(AUTH_KEY) === 'true'
  }

  setupPageShowListener(): void {
    if (this.isListenerSetup || typeof window === 'undefined') return
    
    this.isListenerSetup = true
    
    window.addEventListener('pageshow', (event) => {
      if (event.persisted) {
        this.wasInBackground = true
      }
    })
    
    document.addEventListener('visibilitychange', () => {
      if (document.hidden) {
        this.wasInBackground = true
      }
    })
  }

  async isBiometricAvailable(): Promise<boolean> {
    if (!window.PublicKeyCredential) return false
    
    try {
      const available = await window.PublicKeyCredential.isUserVerifyingPlatformAuthenticatorAvailable()
      
      return available
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
    } catch (error) {
      console.error('Biometric registration failed:', error)
      
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
      
      return true
    } catch (error) {
      console.error('Biometric authentication failed:', error)
      
      return false
    }
  }

  async registerPIN(pin: string): Promise<boolean> {
    if (pin.length < 4) return false
    
    const hash = await this.hashPIN(pin)
    
    localStorage.setItem(AUTH_KEY, 'true')
    localStorage.setItem(PIN_KEY, hash)
    
    this.lastAuthTime = Date.now()
    
    return true
  }

  async authenticatePIN(pin: string): Promise<boolean> {
    const storedHash = localStorage.getItem(PIN_KEY)
    
    if (!storedHash) return false
    
    const hash = await this.hashPIN(pin)
    
    if (hash !== storedHash) return false
    
    this.lastAuthTime = Date.now()
    
    return true
  }

  hasPIN(): boolean {
    return !!localStorage.getItem(PIN_KEY)
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
