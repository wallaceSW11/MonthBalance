<template>
  <div class="account-view">
    <v-app-bar elevation="0" class="account-header">
      <v-app-bar-nav-icon @click="toggleDrawer" />
      <v-app-bar-title>{{ t('account.title') }}</v-app-bar-title>
    </v-app-bar>

    <div class="account-content">
      <div class="profile-section">
        <div class="avatar-container">
          <v-avatar size="128" color="primary" class="profile-avatar">
            <v-icon size="64" color="white">mdi-account</v-icon>
          </v-avatar>

          <v-btn
            icon="mdi-pencil"
            size="small"
            color="primary"
            class="edit-avatar-button"
            elevation="2"
          />
        </div>

        <h2 class="profile-name">{{ form.name }}</h2>
        <p class="profile-email">{{ form.email }}</p>
      </div>

      <v-form ref="formRef" class="account-form">
        <v-text-field
          v-model="form.name"
          :label="t('account.fullName')"
          variant="outlined"
          :rules="[validateRequired]"
          class="form-field"
        />

        <v-text-field
          v-model="form.email"
          :label="t('account.email')"
          variant="outlined"
          readonly
          disabled
          class="form-field"
        />

        <div class="section-divider">
          <h3 class="section-title">{{ t('account.security') }}</h3>
        </div>

        <v-card class="action-card" @click="openChangePassword">
          <v-card-text class="action-card-content">
            <div class="action-info">
              <v-avatar size="40" color="primary" variant="tonal">
                <v-icon>mdi-lock</v-icon>
              </v-avatar>
              <span class="action-text">{{ t('account.changePassword') }}</span>
            </div>
            <v-icon>mdi-chevron-right</v-icon>
          </v-card-text>
        </v-card>

        <v-card v-if="webAuthnSupported" class="action-card">
          <v-card-text class="action-card-content">
            <div class="action-info">
              <v-avatar size="40" color="primary" variant="tonal">
                <v-icon>mdi-fingerprint</v-icon>
              </v-avatar>
              <span class="action-text">{{ t('account.biometric') }}</span>
            </div>
            <v-switch
              :model-value="webAuthnEnabled"
              color="primary"
              hide-details
              density="compact"
              @update:model-value="handleToggleBiometric"
            />
          </v-card-text>
        </v-card>

        <v-card class="action-card">
          <v-card-text class="action-card-content">
            <div class="action-info">
              <v-avatar size="40" color="primary" variant="tonal">
                <v-icon>mdi-bell</v-icon>
              </v-avatar>
              <span class="action-text">{{ t('account.notifications') }}</span>
            </div>
            <v-switch
              v-model="form.notificationsEnabled"
              color="primary"
              hide-details
              density="compact"
            />
          </v-card-text>
        </v-card>
      </v-form>
    </div>

    <div class="account-footer">
      <PrimaryButton :text="t('account.saveChanges')" block @click="handleSave" />

      <v-btn
        variant="text"
        color="error"
        class="logout-link"
        @click="handleCloseAccount"
      >
        {{ t('account.closeAccount') }}
      </v-btn>
    </div>

    <ChangePasswordModal v-model="changePasswordOpen" />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { PrimaryButton, notify, loading, confirm } from '@wallacesw11/base-lib';
import { useAuthStore } from '@/stores/auth';
import { useAuthGuard } from '@/composables';
import { ROUTES } from '@/constants/routes';
import ChangePasswordModal from '@/components/ChangePasswordModal.vue';

const emit = defineEmits<{
  toggleDrawer: []
}>();

const router = useRouter();
const { t } = useI18n();
const authStore = useAuthStore();
const { webAuthnSupported, webAuthnEnabled, enableBiometric, disableBiometric } = useAuthGuard();
const formRef = ref();
const changePasswordOpen = ref<boolean>(false);

const form = ref({
  name: '',
  email: '',
  notificationsEnabled: true
});

function toggleDrawer(): void {
  emit('toggleDrawer');
}

function validateRequired(value: string): boolean | string {
  return !!value || t('auth.requiredField');
}

function loadUserData(): void {
  if (!authStore.user) return;

  form.value = {
    name: authStore.user.name,
    email: authStore.user.email,
    notificationsEnabled: authStore.user.notificationsEnabled
  };
}

async function handleSave(): Promise<void> {
  const { valid } = await formRef.value.validate();

  if (!valid) return;

  loading.show(t('account.saving'));

  try {
    await authStore.updateUserData({
      name: form.value.name,
      notificationsEnabled: form.value.notificationsEnabled
    });

    notify.success(t('messages.success'), t('account.saved'));
  } catch (error) {
    notify.error(t('messages.error'), t('account.saveError'));
  } finally {
    loading.hide();
  }
}

function openChangePassword(): void {
  changePasswordOpen.value = true;
}

async function handleCloseAccount(): Promise<void> {
  const confirmed = await confirm.show(
    t('account.closeAccountTitle'),
    t('account.closeAccountMessage'),
    {
      confirmText: t('account.closeAccountConfirm'),
      cancelText: t('common.cancel'),
      confirmColor: 'error'
    }
  );

  if (!confirmed) return;

  loading.show(t('account.closingAccount'));

  try {
    authStore.logout();
    notify.success(t('account.closeAccountTitle'), t('account.accountClosed'));
    await router.push(ROUTES.LOGIN);
  } catch (error) {
    notify.error(t('messages.error'), t('account.closeAccountError'));
  } finally {
    loading.hide();
  }
}

async function handleToggleBiometric(value: boolean): Promise<void> {
  if (!authStore.user) return;

  if (value) {
    loading.show(t('account.enablingBiometric'));

    try {
      const success = await enableBiometric(authStore.user.id, authStore.user.name);

      if (success) {
        notify.success(t('messages.success'), t('account.biometricEnabled'));
      } else {
        notify.error(t('messages.error'), t('account.biometricError'));
      }
    } catch (error) {
      notify.error(t('messages.error'), t('account.biometricError'));
    } finally {
      loading.hide();
    }
  } else {
    const confirmed = await confirm.show(
      t('account.disableBiometricTitle'),
      t('account.disableBiometricMessage'),
      {
        confirmText: t('common.confirm'),
        cancelText: t('common.cancel'),
        confirmColor: 'error'
      }
    );

    if (!confirmed) return;

    disableBiometric();
    notify.success(t('messages.success'), t('account.biometricDisabled'));
  }
}

onMounted(() => {
  loadUserData();
});
</script>

<style scoped>
.account-view {
  display: flex;
  flex-direction: column;
  height: 100dvh;
  overflow: hidden;
}

.account-header {
  border-bottom: 1px solid rgba(var(--v-border-color), var(--v-border-opacity));
}

.account-content {
  flex: 1;
  overflow-y: auto;
  padding-bottom: 180px;
}

.profile-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 32px 24px;
}

.avatar-container {
  position: relative;
  margin-bottom: 16px;
}

.profile-avatar {
  border: 4px solid rgba(var(--v-theme-primary), 0.2);
}

.edit-avatar-button {
  position: absolute;
  bottom: 0;
  right: 0;
}

.profile-name {
  font-size: 1.25rem;
  font-weight: 700;
  text-align: center;
  margin-bottom: 4px;
}

.profile-email {
  font-size: 0.875rem;
  opacity: 0.6;
  text-align: center;
}

.account-form {
  padding: 0 24px;
}

.form-field {
  margin-bottom: 16px;
}

.section-divider {
  margin: 24px 0 16px;
}

.section-title {
  font-size: 0.75rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  opacity: 0.6;
}

.action-card {
  margin-bottom: 12px;
  cursor: pointer;
}

.action-card-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px;
}

.action-info {
  display: flex;
  align-items: center;
  gap: 16px;
}

.action-text {
  font-weight: 500;
}

.account-footer {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 24px;
  background: linear-gradient(
    to top,
    rgb(var(--v-theme-surface)) 70%,
    transparent
  );
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.logout-link {
  text-transform: none;
  font-weight: 600;
}
</style>
