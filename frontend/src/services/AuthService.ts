const AUTH_KEY = 'mb_auth_registered'
const PIN_KEY = 'mb_pin_hash'
const SESSION_AUTH_KEY = 'mb_session_authenticated'

class AuthService {
  private listenerSetup = false

  isDevMode(): boolean {
    return import.meta.env.DEV
  }

  isRegistered(): boolean {
    return localStorage.getItem(AUTH_KEY) === 'true'
  }

  hasPIN(): boolean {
    return !!localStorage.getItem(PIN_KEY)
  }

  isSessionAuthenticated(): boolean {
    return sessionStorage.getItem(SESSION_AUTH_KEY) === 'true'
  }

  private setSessionAuthenticated(value: boolean): void {
    if (value) {
      sessionStorage.setItem(SESSION_AUTH_KEY, 'true')
      return
    }

    sessionStorage.removeItem(SESSION_AUTH_KEY)
  }

  setupVisibilityGuard(onLockRequired: () => void): void {
    if (this.listenerSetup || typeof window === 'undefined') return

    this.listenerSetup = true

    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'hidden') {
        this.setSessionAuthenticated(false)
        return
      }

      if (document.visibilityState === 'visible' && !this.isDevMode()) {
        onLockRequired()
      }
    })

    window.addEventListener('pageshow', (event) => {
      if (event.persisted && !this.isDevMode()) {
        this.setSessionAuthenticated(false)
        onLockRequired()
      }
    })

    window.addEventListener('focus', () => {
      if (!this.isSessionAuthenticated() && !this.isDevMode()) {
        onLockRequired()
      }
    })
  }

  isAuthRequired(): boolean {
    if (this.isDevMode()) return false

    return !this.isSessionAuthenticated()
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
      const { startRegistration } = await import('@simplewebauthn/browser')

      const options = {
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
          { alg: -7, type: 'public-key' as const },
          { alg: -257, type: 'public-key' as const },
        ],
        timeout: 60000,
        attestation: 'none' as const,
        authenticatorSelection: {
          authenticatorAttachment: 'platform' as const,
          userVerification: 'required' as const,
          requireResidentKey: false,
        },
      }

      const credential = await startRegistration({ optionsJSON: options })

      localStorage.setItem(AUTH_KEY, 'true')
      localStorage.setItem('mb_credential', JSON.stringify(credential))
      this.setSessionAuthenticated(true)

      return true
    } catch (err) {
      console.error('Biometric registration failed:', err)

      return false
    }
  }

  async authenticateBiometric(): Promise<boolean> {
    try {
      const { startAuthentication } = await import('@simplewebauthn/browser')

      const options = {
        challenge: this.generateChallenge(),
        timeout: 60000,
        userVerification: 'required' as const,
        rpId: window.location.hostname,
      }

      await startAuthentication({ optionsJSON: options })
      this.setSessionAuthenticated(true)

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
    this.setSessionAuthenticated(true)

    return true
  }

  async authenticatePIN(pin: string): Promise<boolean> {
    const storedHash = localStorage.getItem(PIN_KEY)

    if (!storedHash) return false

    const hash = await this.hashPIN(pin)

    if (hash !== storedHash) return false

    this.setSessionAuthenticated(true)

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
