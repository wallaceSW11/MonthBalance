<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card>
          <v-card-title class="text-h5 text-center">
            Criar Conta
          </v-card-title>

          <v-card-text>
            <v-form ref="formRef" @submit.prevent="handleRegister">
              <v-text-field
                v-model="email"
                label="Email"
                type="email"
                :rules="emailRules"
                :disabled="loading"
                required
              />

              <v-text-field
                v-model="password"
                label="Senha"
                type="password"
                :rules="passwordRules"
                :disabled="loading"
                required
              />

              <v-text-field
                v-model="confirmPassword"
                label="Confirmar Senha"
                type="password"
                :rules="confirmPasswordRules"
                :disabled="loading"
                required
              />

              <v-alert
                v-if="errorMessage"
                type="error"
                class="mb-4"
              >
                {{ errorMessage }}
              </v-alert>

              <v-btn
                type="submit"
                color="primary"
                block
                :loading="loading"
              >
                Criar Conta
              </v-btn>
            </v-form>
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              text
              :to="{ name: 'login' }"
              :disabled="loading"
            >
              Já tenho conta
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const formRef = ref()
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const loading = ref(false)
const errorMessage = ref('')

const emailRules = [
  (v: string) => !!v || 'Email é obrigatório',
  (v: string) => /.+@.+\..+/.test(v) || 'Email inválido'
]

const passwordRules = [
  (v: string) => !!v || 'Senha é obrigatória',
  (v: string) => v.length >= 6 || 'Senha deve ter no mínimo 6 caracteres'
]

const confirmPasswordRules = computed(() => [
  (v: string) => !!v || 'Confirmação de senha é obrigatória',
  (v: string) => v === password.value || 'Senhas não conferem'
])

async function handleRegister() {
  const { valid } = await formRef.value.validate()
  
  if (!valid) return

  loading.value = true
  errorMessage.value = ''

  try {
    await authStore.register(email.value, password.value, confirmPassword.value)
    
    await router.push({ name: 'dashboard' })
  } catch (error: any) {
    errorMessage.value = error.response?.data?.message || 'Erro ao criar conta'
  } finally {
    loading.value = false
  }
}
</script>
