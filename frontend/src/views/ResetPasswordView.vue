<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card elevation="8" rounded="lg">
          <v-card-title class="text-h5 text-center pa-6">
            {{ $t('auth.resetPassword') }}
          </v-card-title>

          <v-card-text class="px-6 pb-6">
            <v-alert v-if="successMessage" type="success" class="mb-4">
              {{ successMessage }}
            </v-alert>

            <v-alert v-if="errorMessage" type="error" class="mb-4">
              {{ errorMessage }}
            </v-alert>

            <v-form v-if="!successMessage" @submit.prevent="handleSubmit">
              <v-text-field
                v-model="password"
                :label="$t('auth.newPassword')"
                :type="showPassword ? 'text' : 'password'"
                :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                :rules="[rules.required, rules.minLength]"
                variant="outlined"
                density="comfortable"
                class="mb-3"
                @click:append-inner="showPassword = !showPassword"
              />

              <v-text-field
                v-model="confirmPassword"
                :label="$t('auth.confirmPassword')"
                :type="showConfirmPassword ? 'text' : 'password'"
                :append-inner-icon="showConfirmPassword ? 'mdi-eye-off' : 'mdi-eye'"
                :rules="[rules.required, rules.match]"
                variant="outlined"
                density="comfortable"
                class="mb-4"
                @click:append-inner="showConfirmPassword = !showConfirmPassword"
              />

              <v-btn
                type="submit"
                color="primary"
                size="large"
                block
                :loading="loading"
                :disabled="!isFormValid"
              >
                {{ $t('auth.resetPasswordButton') }}
              </v-btn>
            </v-form>

            <div v-if="successMessage" class="text-center mt-4">
              <v-btn color="primary" :to="ROUTES.LOGIN" variant="text">
                {{ $t('auth.backToLogin') }}
              </v-btn>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { authService } from '@/services/authService';
import { ROUTES } from '@/constants/routes';

const route = useRoute();
const router = useRouter();
const { t } = useI18n();

const password = ref('');
const confirmPassword = ref('');
const showPassword = ref(false);
const showConfirmPassword = ref(false);
const loading = ref(false);
const errorMessage = ref('');
const successMessage = ref('');

const token = computed(() => route.query.token as string);

const rules = {
  required: (v: string) => !!v || t('validation.required'),
  minLength: (v: string) => v.length >= 6 || t('validation.minLength', { min: 6 }),
  match: (v: string) => v === password.value || t('validation.passwordMatch')
};

const isFormValid = computed(() => {
  return password.value.length >= 6 && password.value === confirmPassword.value;
});

async function handleSubmit() {
  if (!token.value) {
    errorMessage.value = t('auth.invalidToken');
    return;
  }

  if (!isFormValid.value) return;

  loading.value = true;
  errorMessage.value = '';

  try {
    await authService.resetPassword(token.value, password.value);
    successMessage.value = t('auth.passwordResetSuccess');
    
    setTimeout(() => {
      router.push(ROUTES.LOGIN);
    }, 2000);
  } catch (error: any) {
    errorMessage.value = error.message;
  } finally {
    loading.value = false;
  }
}
</script>
