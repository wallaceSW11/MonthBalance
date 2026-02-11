import { computed } from 'vue';
import { authGuard } from '@/services/authGuard';

export function useAuthGuard() {
  const webAuthnSupported = computed(() => authGuard.isWebAuthnSupported());
  const webAuthnEnabled = computed(() => authGuard.webAuthnEnabled);

  async function enableBiometric(userId: number, userName: string): Promise<boolean> {
    try {
      return await authGuard.registerWebAuthn(userId, userName);
    } catch (error) {
      return false;
    }
  }

  async function authenticateWithBiometric(email: string): Promise<boolean> {
    try {
      return await authGuard.authenticateWithWebAuthn(email);
    } catch (error) {
      return false;
    }
  }

  function disableBiometric(): void {
    authGuard.disableWebAuthn();
  }

  return {
    webAuthnSupported,
    webAuthnEnabled,
    enableBiometric,
    authenticateWithBiometric,
    disableBiometric
  };
}
