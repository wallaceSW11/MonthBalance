<template>
  <v-app>
    <v-main>
      <router-view />
    </v-main>

    <FloatingNotify ref="floatingNotifyRef" />
    <LoadingOverlay ref="loadingOverlayRef" />
    <ConfirmDialog ref="confirmDialogRef" />
  </v-app>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import {
  FloatingNotify,
  LoadingOverlay,
  ConfirmDialog,
  useNotifyStore,
  useLoadingStore,
  useConfirmStore
} from '@wallacesw11/base-lib'
import { useThemeStore } from '@wallacesw11/base-lib/stores'
import { useThemeSync } from '@wallacesw11/base-lib/composables'
import { useLocaleStore } from '@/stores/locale'
import { settingsStorageService } from '@/services/storage/SettingsStorageService'
import { authService } from '@/services/AuthService'

const floatingNotifyRef = ref()
const loadingOverlayRef = ref()
const confirmDialogRef = ref()

const localeStore = useLocaleStore()
const notifyStore = useNotifyStore()
const loadingStore = useLoadingStore()
const confirmStore = useConfirmStore()
const themeStore = useThemeStore()

const { syncTheme } = useThemeSync()

localeStore.initializeLocale()

async function initializeApp() {
  if (floatingNotifyRef.value) {
    notifyStore.setNotifyRef(floatingNotifyRef.value)
  }

  if (loadingOverlayRef.value) {
    loadingStore.setLoadingRef(loadingOverlayRef.value)
  }

  if (confirmDialogRef.value) {
    confirmStore.setConfirmRef(confirmDialogRef.value)
  }

  const settings = settingsStorageService.getSettings()
  themeStore.setTheme(settings.theme)
  
  await themeStore.loadTheme()
  syncTheme()
  
  authService.setupVisibilityListener()
}

onMounted(initializeApp)
</script>
