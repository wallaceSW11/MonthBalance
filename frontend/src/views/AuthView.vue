<template>
  <v-container
    fluid
    class="auth-container"
  >
    <div class="auth-content">
      <div class="auth-header">
        <v-icon
          size="64"
          color="primary"
        >
          mdi-shield-lock
        </v-icon>
        <h1 class="auth-title">
          {{ t('auth.title') }}
        </h1>
        <p class="auth-subtitle">
          {{ authSubtitle }}
        </p>
      </div>

      <div
        v-if="!isRegistered"
        class="auth-setup"
      >
        <v-btn
          v-if="biometricAvailable"
          block
          size="large"
          color="primary"
          prepend-icon="mdi-fingerprint"
          :loading="loading"
          @click="handleBiometricSetup"
        >
          {{ t('auth.setupBiometric') }}
        </v-btn>

        <div class="divider-text">
          {{ t('auth.or') }}
        </div>

        <v-btn
          block
          size="large"
          variant="outlined"
          prepend-icon="mdi-lock"
          @click="showPinSetup = true"
        >
          {{ t('auth.setupPin') }}
        </v-btn>
      </div>

      <div
        v-else
        class="auth-login"
      >
        <v-btn
          v-if="biometricAvailable && !hasPinOnly"
          block
          size="large"
          color="primary"
          prepend-icon="mdi-fingerprint"
          :loading="loading"
          @click="handleBiometricAuth"
        >
          {{ t('auth.useBiometric') }}
        </v-btn>

        <div
          v-if="biometricAvailable && !hasPinOnly"
          class="divider-text"
        >
          {{ t('auth.or') }}
        </div>

        <v-btn
          v-if="hasPinOnly || biometricAvailable"
          block
          size="large"
          :variant="hasPinOnly ? 'flat' : 'outlined'"
          :color="hasPinOnly ? 'primary' : undefined"
          prepend-icon="mdi-lock"
          @click="showPinAuth = true"
        >
          {{ t('auth.usePin') }}
        </v-btn>
      </div>

      <v-dialog
        v-model="showPinSetup"
        max-width="400"
        persistent
      >
        <v-card>
          <v-card-title>{{ t('auth.createPin') }}</v-card-title>

          <v-card-text>
            <v-text-field
              v-model="pin"
              :label="t('auth.pinLabel')"
              type="password"
              inputmode="numeric"
              maxlength="6"
              :rules="[rules.required, rules.minLength]"
              autofocus
              @keyup.enter="handlePinSetup"
            />

            <v-text-field
              v-model="pinConfirm"
              :label="t('auth.confirmPin')"
              type="password"
              inputmode="numeric"
              maxlength="6"
              :rules="[rules.required, rules.match]"
              @keyup.enter="handlePinSetup"
            />
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              variant="text"
              @click="showPinSetup = false"
            >
              {{ t('common.cancel') }}
            </v-btn>
            <v-btn
              color="primary"
              variant="flat"
              :loading="loading"
              @click="handlePinSetup"
            >
              {{ t('common.confirm') }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-dialog
        v-model="showPinAuth"
        max-width="400"
        persistent
      >
        <v-card>
          <v-card-title>{{ t('auth.enterPin') }}</v-card-title>

          <v-card-text>
            <v-text-field
              v-model="pin"
              :label="t('auth.pinLabel')"
              type="password"
              inputmode="numeric"
              maxlength="6"
              :error="pinError"
              :error-messages="pinError ? t('auth.incorrectPin') : ''"
              autofocus
              @keyup.enter="handlePinAuth"
              @input="pinError = false"
            />
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              variant="text"
              @click="closePinAuth"
            >
              {{ t('common.cancel') }}
            </v-btn>
            <v-btn
              color="primary"
              variant="flat"
              :loading="loading"
              @click="handlePinAuth"
            >
              {{ t('common.confirm') }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { authService } from '@/services/AuthService'
import { notify } from '@wallacesw11/base-lib'

const { t } = useI18n()
const router = useRouter()

const loading = ref(false)
const biometricAvailable = ref(false)
const isRegistered = ref(false)
const hasPinOnly = ref(false)
const showPinSetup = ref(false)
const showPinAuth = ref(false)
const pin = ref('')
const pinConfirm = ref('')
const pinError = ref(false)

const authSubtitle = computed(() => {
  if (!isRegistered.value) return t('auth.setupSubtitle')
  
  return t('auth.loginSubtitle')
})

const rules = {
  required: (value: string) => !!value || t('common.required'),
  minLength: (value: string) => value.length >= 4 || t('auth.pinMinLength'),
  match: (value: string) => value === pin.value || t('auth.pinMismatch'),
}

async function handleBiometricSetup(): Promise<void> {
  loading.value = true
  
  const success = await authService.registerBiometric()
  
  loading.value = false
  
  if (!success) {
    notify.error(t('auth.error'), t('auth.biometricSetupFailed'))
    return
  }
  
  notify.success(t('auth.success'), t('auth.biometricSetupSuccess'))
  router.push('/dashboard')
}

async function handleBiometricAuth(): Promise<void> {
  loading.value = true
  
  const success = await authService.authenticateBiometric()
  
  loading.value = false
  
  if (!success) {
    notify.error(t('auth.error'), t('auth.biometricAuthFailed'))
    return
  }
  
  router.push('/dashboard')
}

async function handlePinSetup(): Promise<void> {
  if (pin.value.length < 4) {
    notify.warning(t('auth.warning'), t('auth.pinMinLength'))
    return
  }
  
  if (pin.value !== pinConfirm.value) {
    notify.warning(t('auth.warning'), t('auth.pinMismatch'))
    return
  }
  
  loading.value = true
  
  const success = await authService.registerPIN(pin.value)
  
  loading.value = false
  
  if (!success) {
    notify.error(t('auth.error'), t('auth.pinSetupFailed'))
    return
  }
  
  notify.success(t('auth.success'), t('auth.pinSetupSuccess'))
  showPinSetup.value = false
  router.push('/dashboard')
}

async function handlePinAuth(): Promise<void> {
  if (!pin.value) return
  
  loading.value = true
  
  const success = await authService.authenticatePIN(pin.value)
  
  loading.value = false
  
  if (!success) {
    pinError.value = true
    pin.value = ''
    return
  }
  
  router.push('/dashboard')
}

function closePinAuth(): void {
  showPinAuth.value = false
  pin.value = ''
  pinError.value = false
}

onMounted(async () => {
  biometricAvailable.value = await authService.isBiometricAvailable()
  isRegistered.value = authService.isRegistered()
  hasPinOnly.value = authService.hasPIN() && !biometricAvailable.value
})
</script>

<style scoped>
.auth-container {
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, rgba(var(--v-theme-primary), 0.1) 0%, rgba(var(--v-theme-surface), 1) 100%);
}

.auth-content {
  width: 100%;
  max-width: 400px;
  padding: 32px;
}

.auth-header {
  text-align: center;
  margin-bottom: 48px;
}

.auth-title {
  font-size: 28px;
  font-weight: 700;
  margin-top: 16px;
  margin-bottom: 8px;
}

.auth-subtitle {
  font-size: 14px;
  color: rgba(var(--v-theme-on-surface), 0.6);
}

.auth-setup,
.auth-login {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.divider-text {
  text-align: center;
  color: rgba(var(--v-theme-on-surface), 0.4);
  font-size: 12px;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  margin: 8px 0;
}
</style>
