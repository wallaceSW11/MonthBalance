<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 class="text-h3 mb-6">
          {{ $t('demo.title') }}
        </h1>
        <p class="text-subtitle-1 mb-8">
          {{ $t('demo.subtitle') }}
        </p>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card class="mb-6">
          <v-card-title>{{ $t('demo.buttons.title') }}</v-card-title>
          <v-card-text>
            <div class="d-flex flex-wrap ga-4">
              <PrimaryButton
                :text="$t('demo.buttons.primary')"
                prepend-icon="mdi-check"
                @click="handleButtonClick('Primary')"
              />
              <SecondaryButton
                :text="$t('demo.buttons.secondary')"
                prepend-icon="mdi-information"
                @click="handleButtonClick('Secondary')"
              />
              <TertiaryButton
                :text="$t('demo.buttons.tertiary')"
                prepend-icon="mdi-star"
                @click="handleButtonClick('Tertiary')"
              />
              <QuartenaryButton
                :text="$t('demo.buttons.quartenary')"
                prepend-icon="mdi-alert"
                @click="handleButtonClick('Quartenary')"
              />
              <PrimaryButton
                :text="$t('demo.buttons.disabled')"
                :disabled="true"
              />
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card class="mb-6">
          <v-card-title>{{ $t('demo.notifications.title') }}</v-card-title>
          <v-card-text>
            <div class="d-flex flex-wrap">
              <v-btn
                class="mr-4 mb-2"
                color="success"
                @click="showNotification('success')"
              >
                {{ $t('demo.notifications.success') }}
              </v-btn>
              <v-btn
                class="mr-4 mb-2"
                color="error"
                @click="showNotification('error')"
              >
                {{ $t('demo.notifications.error') }}
              </v-btn>
              <v-btn
                class="mr-4 mb-2"
                color="warning"
                @click="showNotification('warning')"
              >
                {{ $t('demo.notifications.warning') }}
              </v-btn>
              <v-btn
                class="mb-2"
                color="info"
                @click="showNotification('info')"
              >
                {{ $t('demo.notifications.info') }}
              </v-btn>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card class="mb-6">
          <v-card-title>{{ $t('demo.theme.title') }}</v-card-title>
          <v-card-text>
            <div class="mb-4">
              <p class="text-subtitle-2 mb-2">
                {{ $t('demo.theme.currentTheme') }}: <strong>{{ currentTheme }}</strong>
              </p>
              <p class="text-subtitle-2 mb-4">
                {{ $t('demo.theme.appName') }}: <strong>{{ themeStore.appName }}</strong>
              </p>
            </div>

            <div class="mb-4">
              <h4 class="mb-2">
                {{ $t('demo.theme.themeColors') }}:
              </h4>
              <div class="d-flex flex-wrap">
                <v-chip
                  v-for="(value, name) in themeStore.currentColors"
                  :key="name"
                  :color="String(name)"
                  class="mr-2 mb-2"
                  label
                >
                  {{ name }}: {{ value }}
                </v-chip>
              </div>
            </div>

            <v-btn
              color="primary"
              @click="themeStore.toggleTheme()"
            >
              {{ $t('demo.theme.toggleTheme') }}
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card class="mb-6">
          <v-card-title>{{ $t('demo.loading.title') }}</v-card-title>
          <v-card-text>
            <v-btn
              color="primary"
              @click="showLoading"
            >
              {{ $t('demo.loading.showButton') }}
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card class="mb-6">
          <v-card-title>{{ $t('demo.confirm.title') }}</v-card-title>
          <v-card-text>
            <v-btn
              color="primary"
              @click="showConfirm"
            >
              {{ $t('demo.confirm.showButton') }}
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card class="mb-6">
          <v-card-title>{{ $t('demo.iconTooltip.title') }}</v-card-title>
          <v-card-text>
            <div class="d-flex align-center">
              <IconToolTip
                class="mr-4"
                icon="mdi-help-circle"
                :text="$t('demo.iconTooltip.help')"
              />
              <IconToolTip
                class="mr-4"
                icon="mdi-information"
                :text="$t('demo.iconTooltip.info')"
              />
              <IconToolTip
                class="mr-4"
                icon="mdi-delete"
                :text="$t('demo.iconTooltip.delete')"
                as-button
              />
              <IconToolTip
                icon="mdi-pencil"
                :text="$t('demo.iconTooltip.edit')"
                :as-button="true"
              />
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>{{ $t('demo.store.title') }}</v-card-title>
          <v-card-text>
            <p class="mb-4">
              {{ $t('demo.store.counter') }}: {{ appStore.counter }} ({{ $t('demo.store.double') }}: {{
                appStore.doubleCounter }})
            </p>
            <v-btn
              color="primary"
              @click="appStore.increment()"
            >
              {{ $t('demo.store.increment') }}
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useAppStore } from '@/store'
import {
  useThemeStore,
  notify,
  loading,
  confirm,
  IconToolTip,
  PrimaryButton,
  SecondaryButton,
  TertiaryButton,
  QuartenaryButton
} from '@wallacesw11/base-lib'

const { t } = useI18n()
const appStore = useAppStore()
const themeStore = useThemeStore()

const currentTheme = computed(() => themeStore.currentMode)

const handleButtonClick = (buttonType: string) => {
  notify.info(t('demo.buttons.clicked', { type: buttonType }), '')
}

const showNotification = (type: 'success' | 'error' | 'warning' | 'info') => {
  const messages = {
    success: { title: t('demo.notifications.successTitle'), message: t('demo.notifications.successMessage') },
    error: { title: t('demo.notifications.errorTitle'), message: t('demo.notifications.errorMessage') },
    warning: { title: t('demo.notifications.warningTitle'), message: t('demo.notifications.warningMessage') },
    info: { title: t('demo.notifications.infoTitle'), message: t('demo.notifications.infoMessage') },
  }

  const { title, message } = messages[type]
  notify[type](title, message)
}

const showLoading = () => {
  loading.show(t('demo.loading.message'))
  setTimeout(() => {
    loading.hide()
    notify.success(t('demo.loading.doneTitle'), t('demo.loading.doneMessage'))
  }, 3000)
}

const showConfirm = async () => {
  const confirmed = await confirm.show(t('demo.confirm.dialogTitle'), t('demo.confirm.dialogMessage'))

  if (confirmed) {
    notify.success(t('demo.confirm.confirmedTitle'), t('demo.confirm.confirmedMessage'))
  } else {
    notify.error(t('demo.confirm.cancelledTitle'), t('demo.confirm.cancelledMessage'))
  }
};
</script>
