<template>
  <ModalBase v-model="open" :title="t('auth.changePassword')" :actions="actions" :max-width="500">
    <v-form ref="formRef" @submit.prevent="handleSubmit">
      <v-text-field
        v-model="form.currentPassword"
        :label="t('auth.currentPassword')"
        type="password"
        :rules="[validateRequired]"
        class="mb-4"
      />

      <v-text-field
        v-model="form.newPassword"
        :label="t('auth.newPassword')"
        type="password"
        :rules="[validateRequired, validateMinLength]"
        class="mb-4"
      />

      <v-text-field
        v-model="form.confirmPassword"
        :label="t('auth.confirmNewPassword')"
        type="password"
        :rules="[validateRequired, validatePasswordMatch]"
      />
    </v-form>
  </ModalBase>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { ModalBase, notify, loading } from '@wallacesw11/base-lib';
import type { ModalAction } from '@wallacesw11/base-lib/components';
import { useAuthStore } from '@/stores/auth';

const open = defineModel<boolean>({ required: true });

const { t } = useI18n();
const authStore = useAuthStore();
const formRef = ref();

const form = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
});

const actions = computed<ModalAction[]>(() => [
  {
    text: t('common.cancel'),
    color: 'secondary',
    variant: 'outlined',
    handler: () => {
      open.value = false;
    }
  },
  {
    text: t('common.confirm'),
    color: 'primary',
    variant: 'elevated',
    handler: handleSubmit
  }
]);

function validateRequired(value: string): boolean | string {
  return !!value || t('auth.requiredField');
}

function validateMinLength(value: string): boolean | string {
  return value.length >= 6 || t('auth.passwordMinLengthRule');
}

function validatePasswordMatch(value: string): boolean | string {
  return value === form.value.newPassword || t('auth.passwordsDoNotMatch');
}

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate();

  if (!valid) return;

  loading.show(t('auth.changingPassword'));

  try {
    await authStore.changePassword(form.value.currentPassword, form.value.newPassword);
    notify.success(t('common.confirm'), t('auth.passwordChanged'));
    open.value = false;
    resetForm();
  } catch (error) {
    notify.error(t('messages.error'), error instanceof Error ? error.message : t('auth.passwordChangeError'));
  } finally {
    loading.hide();
  }
}

function resetForm(): void {
  form.value = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };
  formRef.value?.reset();
}
</script>
