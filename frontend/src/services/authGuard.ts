/**
 * AuthGuard Service
 * 
 * iOS PWA-proof authentication guard with:
 * - Persistent state (localStorage, not memory)
 * - Watchdog timer (checks every 1s)
 * - Background lock (immediate re-auth on resume)
 * - WebAuthn ready (biometric support)
 */

const STORAGE_KEYS = {
  LAST_AUTH: 'mb_last_auth',
  WAS_BACKGROUND: 'mb_was_bg',
  HIDDEN_AT: 'mb_hidden_at',
  WEBAUTHN_ENABLED: 'mb_webauthn_enabled',
  WEBAUTHN_CREDENTIAL_ID: 'mb_webauthn_cred_id'
} as const;

const AUTH_TIMEOUT = 0; // 0 = immediate lock on background

class AuthGuardService {
  private isListenerSetup = false;
  private watchdogInterval: number | null = null;

  /**
   * Persistent lastAuthTime (localStorage, not memory)
   */
  private get lastAuthTime(): number {
    return Number(localStorage.getItem(STORAGE_KEYS.LAST_AUTH) || 0);
  }

  private set lastAuthTime(value: number) {
    localStorage.setItem(STORAGE_KEYS.LAST_AUTH, String(value));
  }

  /**
   * Persistent wasInBackground (localStorage, not memory)
   */
  private get wasInBackground(): boolean {
    return localStorage.getItem(STORAGE_KEYS.WAS_BACKGROUND) === 'true';
  }

  private set wasInBackground(value: boolean) {
    localStorage.setItem(STORAGE_KEYS.WAS_BACKGROUND, value ? 'true' : 'false');
  }

  /**
   * Check if WebAuthn is enabled for this user
   */
  get webAuthnEnabled(): boolean {
    return localStorage.getItem(STORAGE_KEYS.WEBAUTHN_ENABLED) === 'true';
  }

  /**
   * Check if running in dev mode (skip auth checks)
   */
  private isDevMode(): boolean {
    return import.meta.env.DEV && import.meta.env.VITE_SKIP_AUTH === 'true';
  }

  /**
   * Setup lifecycle guards (visibility, pageshow, watchdog)
   * iOS PWA-proof: doesn't rely only on events
   */
  setupLifecycleGuards(): void {
    if (this.isListenerSetup || typeof window === 'undefined') return;

    this.isListenerSetup = true;

    // Detect when app goes to background
    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'hidden') {
        this.wasInBackground = true;
        localStorage.setItem(STORAGE_KEYS.HIDDEN_AT, String(Date.now()));
      }
    });

    // Detect when app comes back from background
    window.addEventListener('pageshow', () => {
      this.checkResumeLock();
    });

    // Watchdog: check every 1s (iOS PWA doesn't always fire events)
    this.watchdogInterval = window.setInterval(() => {
      this.checkResumeLock();
    }, 1000);
  }

  /**
   * Stop watchdog (cleanup)
   */
  stopLifecycleGuards(): void {
    if (this.watchdogInterval) {
      clearInterval(this.watchdogInterval);
      this.watchdogInterval = null;
    }

    this.isListenerSetup = false;
  }

  /**
   * Check if app resumed from background and needs lock
   */
  private checkResumeLock(): void {
    const hiddenAt = Number(localStorage.getItem(STORAGE_KEYS.HIDDEN_AT) || 0);

    if (!hiddenAt) return;

    const diff = Date.now() - hiddenAt;

    // Immediate lock (AUTH_TIMEOUT = 0)
    if (diff > AUTH_TIMEOUT) {
      this.forceLock();
    }
  }

  /**
   * Force lock (clear auth state)
   */
  private forceLock(): void {
    this.wasInBackground = false;
    localStorage.removeItem(STORAGE_KEYS.LAST_AUTH);
    localStorage.removeItem(STORAGE_KEYS.HIDDEN_AT);
  }

  /**
   * Check if authentication is required
   * iOS PWA-proof: deterministic, doesn't rely on events
   */
  isAuthRequired(): boolean {
    if (this.isDevMode()) return false;

    const last = this.lastAuthTime;

    if (!last) return true;

    const diff = Date.now() - last;

    return diff > AUTH_TIMEOUT;
  }

  /**
   * Mark user as authenticated (update lastAuthTime)
   */
  markAuthenticated(): void {
    this.lastAuthTime = Date.now();
    this.wasInBackground = false;
    localStorage.removeItem(STORAGE_KEYS.HIDDEN_AT);
  }

  /**
   * Clear auth state (logout)
   */
  clearAuthState(): void {
    localStorage.removeItem(STORAGE_KEYS.LAST_AUTH);
    localStorage.removeItem(STORAGE_KEYS.WAS_BACKGROUND);
    localStorage.removeItem(STORAGE_KEYS.HIDDEN_AT);
  }

  /**
   * WebAuthn: Check if browser supports it
   */
  isWebAuthnSupported(): boolean {
    return (
      typeof window !== 'undefined' &&
      window.PublicKeyCredential !== undefined &&
      typeof window.PublicKeyCredential === 'function'
    );
  }

  /**
   * WebAuthn: Register biometric credential
   * Backend needs to implement /auth/webauthn/register
   */
  async registerWebAuthn(_userId: number, userName: string): Promise<boolean> {
    if (!this.isWebAuthnSupported()) {
      throw new Error('WebAuthn not supported');
    }

    try {
      const { authService } = await import('./authService');
      const challengeData = await authService.registerWebAuthnChallenge();

      const publicKeyCredentialCreationOptions: PublicKeyCredentialCreationOptions = {
        challenge: Uint8Array.from(atob(challengeData.challenge), c => c.charCodeAt(0)),
        rp: {
          name: challengeData.rpName,
          id: challengeData.rpId
        },
        user: {
          id: Uint8Array.from(atob(challengeData.userId), c => c.charCodeAt(0)),
          name: userName,
          displayName: userName
        },
        pubKeyCredParams: [
          { alg: -7, type: 'public-key' },
          { alg: -257, type: 'public-key' }
        ],
        authenticatorSelection: {
          authenticatorAttachment: 'platform',
          userVerification: 'required'
        },
        timeout: challengeData.timeout,
        attestation: 'none'
      };

      const credential = (await navigator.credentials.create({
        publicKey: publicKeyCredentialCreationOptions
      })) as PublicKeyCredential;

      if (!credential) {
        throw new Error('Failed to create credential');
      }

      const response = credential.response as AuthenticatorAttestationResponse;

      const credentialData = {
        credentialId: btoa(String.fromCharCode(...new Uint8Array(credential.rawId))),
        publicKey: btoa(
          String.fromCharCode(...new Uint8Array(response.getPublicKey() || new ArrayBuffer(0)))
        ),
        transports: response.getTransports ? response.getTransports() : ['internal'],
        counter: 0
      };

      await authService.registerWebAuthnCredential(credentialData);

      localStorage.setItem(STORAGE_KEYS.WEBAUTHN_ENABLED, 'true');
      localStorage.setItem(STORAGE_KEYS.WEBAUTHN_CREDENTIAL_ID, credential.id);

      return true;
    } catch (error) {
      console.error('WebAuthn registration failed:', error);

      return false;
    }
  }

  /**
   * WebAuthn: Authenticate with biometric
   * Backend needs to implement /auth/webauthn/authenticate
   */
  async authenticateWithWebAuthn(email: string): Promise<boolean> {
    if (!this.isWebAuthnSupported() || !this.webAuthnEnabled) {
      return false;
    }

    try {
      const { authService } = await import('./authService');
      const challengeData = await authService.authenticateWebAuthnChallenge(email);

      const allowCredentials = challengeData.allowCredentials.map((cred: any) => ({
        id: Uint8Array.from(atob(cred.id), c => c.charCodeAt(0)),
        type: cred.type as PublicKeyCredentialType,
        transports: cred.transports as AuthenticatorTransport[]
      }));

      const publicKeyCredentialRequestOptions: PublicKeyCredentialRequestOptions = {
        challenge: Uint8Array.from(atob(challengeData.challenge), c => c.charCodeAt(0)),
        timeout: challengeData.timeout,
        rpId: challengeData.rpId,
        allowCredentials,
        userVerification: 'required'
      };

      const credential = (await navigator.credentials.get({
        publicKey: publicKeyCredentialRequestOptions
      })) as PublicKeyCredential;

      if (!credential) {
        throw new Error('Failed to get credential');
      }

      const response = credential.response as AuthenticatorAssertionResponse;

      const credentialData = {
        credentialId: btoa(String.fromCharCode(...new Uint8Array(credential.rawId))),
        authenticatorData: btoa(
          String.fromCharCode(...new Uint8Array(response.authenticatorData))
        ),
        clientDataJSON: btoa(String.fromCharCode(...new Uint8Array(response.clientDataJSON))),
        signature: btoa(String.fromCharCode(...new Uint8Array(response.signature)))
      };

      await authService.authenticateWebAuthn(credentialData);

      this.markAuthenticated();

      return true;
    } catch (error) {
      console.error('WebAuthn authentication failed:', error);

      return false;
    }
  }

  /**
   * Disable WebAuthn for this user
   */
  disableWebAuthn(): void {
    localStorage.removeItem(STORAGE_KEYS.WEBAUTHN_ENABLED);
    localStorage.removeItem(STORAGE_KEYS.WEBAUTHN_CREDENTIAL_ID);
  }
}

export const authGuard = new AuthGuardService();
