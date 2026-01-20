<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <v-app-bar-title>{{ appName }}</v-app-bar-title>

      <v-spacer />

      <v-btn icon to="/">
        <v-icon>mdi-home</v-icon>
      </v-btn>

      <v-btn icon to="/demo">
        <v-icon>mdi-test-tube</v-icon>
      </v-btn>

      <LanguageSelector :available-locales="availableLocales" />

      <ThemeToggle />
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>

    <FloatingNotify ref="floatingNotifyRef" />
    <LoadingOverlay ref="loadingOverlayRef" />
    <ConfirmDialog ref="confirmDialogRef" />
  </v-app>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import {
  FloatingNotify,
  LoadingOverlay,
  ConfirmDialog,
  LanguageSelector,
  ThemeToggle,
  useThemeSync,
  useThemeStore,
  useNotifyStore,
  useLoadingStore,
  useConfirmStore
} from '@wallacesw11/base-lib'
import { useLocaleStore } from '@/stores/locale'
import { availableLocales } from '@/locales'

const floatingNotifyRef = ref()
const loadingOverlayRef = ref()
const confirmDialogRef = ref()

const themeStore = useThemeStore()
const localeStore = useLocaleStore()
const notifyStore = useNotifyStore()
const loadingStore = useLoadingStore()
const confirmStore = useConfirmStore()

const appName = computed(() => themeStore.appName)

useThemeSync()
localeStore.initializeLocale()

function registerGlobalComponentRefs() {
  // Registrar as refs dos componentes globais nas stores
  if (floatingNotifyRef.value) {
    notifyStore.setNotifyRef(floatingNotifyRef.value)
  }

  if (loadingOverlayRef.value) {
    loadingStore.setLoadingRef(loadingOverlayRef.value)
  }

  if (confirmDialogRef.value) {
    confirmStore.setConfirmRef(confirmDialogRef.value)
  }
}

onMounted(registerGlobalComponentRefs)
</script>
