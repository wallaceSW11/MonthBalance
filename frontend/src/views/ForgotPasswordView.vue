<template>
  <div class="forgot-password-container">
    <div class="forgot-password-content">
      <div class="header-nav">
        <v-btn
          icon
          variant="text"
          size="small"
          @click="goToLogin"
        >
          <v-icon>mdi-arrow-left</v-icon>
        </v-btn>
        <h2 class="header-title">{{ t('auth.recoverPassword') }}</h2>
        <div class="header-spacer" />
      </div>

      <div class="main-content">
        <div class="icon-section">
          <div class="icon-wrapper">
            <v-icon size="40" color="primary">mdi-lock-reset</v-icon>
          </div>
        </div>

        <h3 class="page-title">{{ t('auth.recoverPassword') }}</h3>
        <p class="page-description">
          {{ t('auth.forgotPasswordInstruction') }}
        </p>

        <v-form ref="formRef" class="forgot-form" @submit.prevent="handleForgotPassword">
          <div class="form-field">
            <label class="field-label">{{ t('auth.email').toUpperCase() }}</label>
            <EmailField
              v-model="form.email"
              :placeholder="t('auth.emailExample')"
              required
              @valid="v => emailValid = v"
            >
              <template #prepend-inner>
                <v-icon size="20" class="field-icon">mdi-email</v-icon>
              </template>
            </EmailField>
          </div>

          <div class="action-button-wrapper">
            <PrimaryButton
              :text="t('auth.sendEmail')"
              type="submit"
              block
              size="large"
            />
          </div>
        </v-form>

        <div class="footer-link-wrapper">
          <a href="#" class="back-link" @click.prevent="goToLogin">
            <v-icon size="16">mdi-arrow-left</v-icon>
            <span>{{ t('auth.backToLogin') }}</span>
          </a>
        </div>
      </div>

      <div class="safe-area-spacer" />
    </div>

    <div class="background-decoration">
      <div class="decoration-circle decoration-bottom-right" />
      <div class="decoration-circle decoration-top-left" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { EmailField, PrimaryButton } from '@wallacesw11/base-lib';

const router = useRouter();
const { t } = useI18n();

const formRef = ref();
const emailValid = ref(false);
const form = ref({
  email: ''
});

async function handleForgotPassword(): Promise<void> {
  const { valid } = await formRef.value.validate();

  if (!valid || !emailValid.value) return;

  console.log('Forgot Password:', form.value);
}

function goToLogin(): void {
  router.push('/login');
}
</script>

<style scoped>
.forgot-password-container {
  position: relative;
  min-height: 100dvh;
  display: flex;
  flex-direction: column;
  background: rgb(var(--v-theme-surface));
  max-width: 430px;
  margin: 0 auto;
  border-left: 1px solid rgba(var(--v-theme-on-surface), 0.05);
  border-right: 1px solid rgba(var(--v-theme-on-surface), 0.05);
  overflow: hidden;
}

.forgot-password-content {
  position: relative;
  z-index: 1;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.header-nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px;
  padding-bottom: 8px;
}

.header-title {
  font-size: 18px;
  font-weight: 700;
  letter-spacing: -0.015em;
  flex: 1;
  text-align: center;
  padding-right: 48px;
}

.header-spacer {
  width: 48px;
}

.main-content {
  flex: 1;
  padding: 40px 24px 24px;
  display: flex;
  flex-direction: column;
}

.icon-section {
  margin-bottom: 32px;
  display: flex;
  justify-content: flex-start;
}

.icon-wrapper {
  width: 64px;
  height: 64px;
  background: rgba(var(--v-theme-primary), 0.1);
  border-radius: 12px;
  border: 1px solid rgba(var(--v-theme-primary), 0.2);
  display: flex;
  align-items: center;
  justify-content: center;
}

.page-title {
  font-size: 28px;
  font-weight: 700;
  letter-spacing: -0.02em;
  margin: 0 0 8px 0;
}

.page-description {
  font-size: 16px;
  line-height: 1.5;
  opacity: 0.7;
  margin: 0 0 32px 0;
}

.forgot-form {
  width: 100%;
}

.form-field {
  margin-bottom: 24px;
}

.field-label {
  display: block;
  font-size: 12px;
  font-weight: 500;
  letter-spacing: 0.05em;
  margin-bottom: 8px;
  padding-left: 4px;
  opacity: 0.8;
}

.field-icon {
  opacity: 0.5;
  margin-right: 8px;
}

.action-button-wrapper {
  margin-bottom: 48px;
}

.footer-link-wrapper {
  margin-top: auto;
  text-align: center;
}

.back-link {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  color: rgb(var(--v-theme-primary));
  font-weight: 600;
  text-decoration: none;
  transition: opacity 0.2s;
}

.back-link:hover {
  opacity: 0.8;
}

.safe-area-spacer {
  height: 40px;
}

.background-decoration {
  position: absolute;
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
  background: rgba(var(--v-theme-primary), 0.08);
  filter: blur(80px);
}

.decoration-bottom-right {
  bottom: -80px;
  right: -80px;
  width: 256px;
  height: 256px;
}

.decoration-top-left {
  top: 80px;
  left: -80px;
  width: 192px;
  height: 192px;
  background: rgba(var(--v-theme-primary), 0.05);
  filter: blur(60px);
}
</style>
