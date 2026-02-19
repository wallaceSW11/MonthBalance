<template>
  <div class="register-container">
    <div class="register-content">
      <div class="header-section">
        <div class="logo-wrapper">
          <v-icon size="40" color="primary">mdi-wallet</v-icon>
        </div>
        <h1 class="app-title">Month Balance</h1>
        <p class="app-subtitle">{{ t('auth.tagline') }}</p>
      </div>

      <div class="form-section">
        <h2 class="form-title">{{ t('auth.createNewAccount') }}</h2>

        <v-form ref="formRef" class="register-form" @submit.prevent="handleRegister">
          <div class="form-field">
            <label class="field-label">{{ t('auth.fullName') }}</label>
            <v-text-field
              v-model="form.name"
              :placeholder="t('auth.fullNamePlaceholder')"
              :rules="[validateRequired]"
              density="comfortable"
            />
          </div>

          <div class="form-field">
            <label class="field-label">{{ t('auth.email') }}</label>
            <EmailField
              v-model="form.email"
              :placeholder="t('auth.emailPlaceholder')"
              required
              @valid="v => emailValid = v"
            />
          </div>

          <div class="form-field">
            <label class="field-label">{{ t('auth.password') }}</label>
            <v-text-field
              v-model="form.password"
              :placeholder="t('auth.passwordCreatePlaceholder')"
              :rules="[validateRequired, validatePasswordLength]"
              :type="showPassword ? 'text' : 'password'"
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

          <div class="form-field">
            <label class="field-label">{{ t('auth.confirmPassword') }}</label>
            <v-text-field
              v-model="form.confirmPassword"
              :placeholder="t('auth.confirmPasswordPlaceholder')"
              :rules="[validateRequired, validatePasswordMatch]"
              :type="showConfirmPassword ? 'text' : 'password'"
              density="comfortable"
            >
              <template #append-inner>
                <v-btn
                  icon
                  variant="text"
                  size="small"
                  @click="showConfirmPassword = !showConfirmPassword"
                >
                  <v-icon size="20">
                    {{ showConfirmPassword ? 'mdi-eye-off' : 'mdi-eye' }}
                  </v-icon>
                </v-btn>
              </template>
            </v-text-field>
          </div>

          <div class="terms-wrapper">
            <v-checkbox
              v-model="acceptedTerms"
              density="compact"
              hide-details
            />

            <span class="terms-text">
              {{ t('auth.termsAgreement') }}
              <a
                href="#"
                class="privacy-link"
                @click.prevent.stop="goToPrivacyPolicy"
              >
                {{ t('auth.privacyPolicy') }}
              </a>
            </span>
          </div>

          <div class="action-button-wrapper">
            <PrimaryButton
              :text="t('auth.register')"
              type="submit"
              block
              size="large"
            />
          </div>
        </v-form>
      </div>

      <div class="footer-section">
        <div class="footer-text">
          <span>{{ t('auth.alreadyHaveAccount') }}</span>
          <a href="#" class="footer-link" @click.prevent="goToLogin">
            {{ t('auth.backToLogin') }}
          </a>
        </div>
        <div class="footer-accent" />
      </div>

      <div class="safe-area-spacer" />
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
const showConfirmPassword = ref(false);
const acceptedTerms = ref(false);
const form = ref({
  name: '',
  email: '',
  password: '',
  confirmPassword: ''
});

const validateRequired = (value: string): boolean | string => {
  return !!value || t('auth.requiredField');
};

const validatePasswordLength = (value: string): boolean | string => {
  return value.length >= 6 || t('auth.passwordMinLength');
};

const validatePasswordMatch = (value: string): boolean | string => {
  return value === form.value.password || t('auth.passwordMismatch');
};

async function handleRegister(): Promise<void> {
  const { valid } = await formRef.value.validate();

  if (!valid || !emailValid.value) return;

  if (!acceptedTerms.value) {
    notify.warning(t('auth.termsRequired'), t('auth.termsRequiredMessage'));

    return;
  }

  loading.show(t('auth.creatingAccount'));

  try {
    await authStore.register(form.value.name, form.value.email, form.value.password);
    notify.success(t('auth.accountCreated'), t('auth.accountCreatedMessage'));
    await router.push(ROUTES.HOME);
  } catch (error) {
    notify.error(t('auth.registerError'), error instanceof Error ? error.message : 'Erro ao criar conta');
  } finally {
    loading.hide();
  }
}

function goToLogin(): void {
  router.push(ROUTES.LOGIN);
}

function goToPrivacyPolicy(): void {
  router.push(ROUTES.PRIVACY_POLICY);
}
</script>

<style scoped>
.register-container {
  min-height: 100dvh;
  display: flex;
  flex-direction: column;
  background: rgb(var(--v-theme-surface));
  max-width: 430px;
  margin: 0 auto;
  border-left: 1px solid rgba(var(--v-theme-on-surface), 0.08);
  border-right: 1px solid rgba(var(--v-theme-on-surface), 0.08);
}

.register-content {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.header-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 48px 24px 32px;
}

.logo-wrapper {
  width: 64px;
  height: 64px;
  background: rgba(var(--v-theme-primary), 0.1);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 16px;
  border: 1px solid rgba(var(--v-theme-primary), 0.2);
}

.app-title {
  font-size: 24px;
  font-weight: 700;
  letter-spacing: -0.02em;
  margin: 0 0 4px 0;
}

.app-subtitle {
  font-size: 14px;
  opacity: 0.6;
  margin: 0;
}

.form-section {
  flex: 1;
  padding: 0 24px;
}

.form-title {
  font-size: 20px;
  font-weight: 600;
  margin: 0 0 24px 0;
}

.register-form {
  width: 100%;
}

.form-field {
  margin-bottom: 20px;
}

.field-label {
  display: block;
  font-size: 14px;
  font-weight: 500;
  margin-bottom: 6px;
  opacity: 0.9;
}

.terms-wrapper {
  margin-top: 8px;
  margin-bottom: 16px;
  display: flex;
  align-items: flex-start;
  gap: 8px;
}

.terms-text {
  font-size: 12px;
  opacity: 0.7;
  line-height: 1.4;
  flex: 1;
  padding-top: 2px;
}

.privacy-link {
  color: rgb(var(--v-theme-primary));
  font-weight: 600;
  text-decoration: none;
  margin-left: 4px;
}

.privacy-link:hover {
  text-decoration: underline;
}

.action-button-wrapper {
  margin-top: 16px;
}

.footer-section {
  padding: 32px 24px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;
}

.footer-text {
  font-size: 14px;
  opacity: 0.7;
  display: flex;
  align-items: center;
  gap: 4px;
}

.footer-link {
  color: rgb(var(--v-theme-primary));
  font-weight: 600;
  text-decoration: none;
}

.footer-link:hover {
  text-decoration: underline;
}

.footer-accent {
  width: 96px;
  height: 4px;
  background: rgba(var(--v-theme-on-surface), 0.1);
  border-radius: 2px;
}

.safe-area-spacer {
  height: 32px;
}
</style>
