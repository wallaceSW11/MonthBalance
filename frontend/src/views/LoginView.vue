<template>
  <div class="login-container">
    <div class="login-content">
      <div class="logo-section">
        <div class="logo-wrapper">
          <v-icon size="48" color="primary">mdi-wallet</v-icon>
        </div>
        <h1 class="app-title">Month Balance</h1>
        <p class="app-subtitle">{{ t('auth.tagline') }}</p>
      </div>

      <v-form ref="formRef" class="login-form" @submit.prevent="handleLogin">
        <div class="form-field">
          <EmailField
            v-model="form.email"
            :placeholder="t('auth.emailPlaceholder')"
            required
            @valid="v => emailValid = v"
          />
        </div>

        <div class="form-field">
          <v-text-field
            v-model="form.password"
            :placeholder="t('auth.passwordPlaceholder')"
            :rules="[validateRequired]"
            :type="showPassword ? 'text' : 'password'"
            variant="outlined"
            density="comfortable"
          >
            <template #append-inner>
              <v-btn
                icon
                variant="text"
                size="small"
                @click="showPassword = !showPassword"
              >
                <v-icon size="20">
                  {{ showPassword ? 'mdi-eye-off' : 'mdi-eye' }}
                </v-icon>
              </v-btn>
            </template>
          </v-text-field>
        </div>

        <div class="forgot-password-wrapper">
          <a href="#" class="forgot-password-link" @click.prevent="goToForgotPassword">
            {{ t('auth.forgotPassword') }}
          </a>
        </div>

        <div class="action-button-wrapper">
          <PrimaryButton
            :text="t('auth.login')"
            type="submit"
            block
            size="large"
          />
        </div>
      </v-form>

      <div class="footer-section">
        <p class="footer-text">
          {{ t('auth.noAccount') }}
          <a href="#" class="footer-link" @click.prevent="goToRegister">
            {{ t('auth.createAccountLink') }}
          </a>
        </p>
      </div>

      <div class="safe-area-spacer" />
    </div>

    <div class="background-decoration">
      <div class="decoration-circle decoration-top" />
      <div class="decoration-circle decoration-bottom" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { EmailField, PrimaryButton, notify, loading } from '@wallacesw11/base-lib';
import { useAuthStore } from '@/stores/auth';
import { ROUTES } from '@/constants/routes';

const router = useRouter();
const { t } = useI18n();
const authStore = useAuthStore();

const formRef = ref();
const emailValid = ref(false);
const showPassword = ref(false);
const form = ref({
  email: '',
  password: ''
});

const validateRequired = (value: string): boolean | string => {
  return !!value || t('auth.requiredField');
};

async function handleLogin(): Promise<void> {
  const { valid } = await formRef.value.validate();

  if (!valid || !emailValid.value) return;

  loading.show(t('auth.loggingIn'));

  try {
    await authStore.login(form.value.email, form.value.password);
    await router.push(ROUTES.HOME);
  } catch (error) {
    notify.error(t('auth.loginError'), error instanceof Error ? error.message : 'Erro ao fazer login');
  } finally {
    loading.hide();
  }
}

function goToRegister(): void {
  router.push(ROUTES.REGISTER);
}

function goToForgotPassword(): void {
  router.push(ROUTES.FORGOT_PASSWORD);
}
</script>

<style scoped>
.login-container {
  position: relative;
  min-height: 100dvh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
  background: rgb(var(--v-theme-surface));
  overflow: hidden;
}

.login-content {
  width: 100%;
  max-width: 400px;
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  z-index: 1;
}

.logo-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 48px;
}

.logo-wrapper {
  width: 80px;
  height: 80px;
  background: rgba(var(--v-theme-primary), 0.1);
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 24px;
}

.app-title {
  font-size: 28px;
  font-weight: 700;
  letter-spacing: -0.02em;
  margin: 0 0 8px 0;
}

.app-subtitle {
  font-size: 14px;
  opacity: 0.6;
  margin: 0;
}

.login-form {
  width: 100%;
}

.form-field {
  margin-bottom: 16px;
}

.field-label {
  display: block;
  font-size: 14px;
  font-weight: 500;
  margin-bottom: 8px;
  padding-left: 4px;
}

.forgot-password-wrapper {
  display: flex;
  justify-content: flex-end;
  margin-top: 4px;
  margin-bottom: 16px;
}

.forgot-password-link {
  font-size: 14px;
  font-weight: 500;
  color: rgb(var(--v-theme-primary));
  text-decoration: none;
  transition: opacity 0.2s;
}

.forgot-password-link:hover {
  opacity: 0.8;
}

.action-button-wrapper {
  margin-top: 24px;
}

.footer-section {
  margin-top: 48px;
  text-align: center;
}

.footer-text {
  font-size: 14px;
  opacity: 0.6;
  margin: 0;
}

.footer-link {
  color: rgb(var(--v-theme-primary));
  font-weight: 700;
  text-decoration: none;
  margin-left: 4px;
}

.footer-link:hover {
  text-decoration: underline;
}

.safe-area-spacer {
  height: 32px;
}

.background-decoration {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: 0;
  pointer-events: none;
  overflow: hidden;
}

.decoration-circle {
  position: absolute;
  border-radius: 50%;
  background: rgba(var(--v-theme-primary), 0.05);
  filter: blur(120px);
}

.decoration-top {
  top: -10%;
  right: -10%;
  width: 50%;
  height: 50%;
}

.decoration-bottom {
  bottom: -10%;
  left: -10%;
  width: 50%;
  height: 50%;
  background: rgba(var(--v-theme-primary), 0.08);
}
</style>
