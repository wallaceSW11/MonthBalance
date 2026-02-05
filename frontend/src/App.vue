<template>
  <v-app>
    <router-view />

    <FloatingNotify ref="floatingNotifyRef" />
    <LoadingOverlay ref="loadingOverlayRef" />
    <ConfirmDialog ref="confirmDialogRef" />
  </v-app>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import {
  FloatingNotify,
  LoadingOverlay,
  ConfirmDialog,
  useThemeSync,
  useNotifyStore,
  useLoadingStore,
  useConfirmStore
} from '@wallacesw11/base-lib';
import { useLocaleStore } from '@/stores/locale';

const floatingNotifyRef = ref();
const loadingOverlayRef = ref();
const confirmDialogRef = ref();

const localeStore = useLocaleStore();
const notifyStore = useNotifyStore();
const loadingStore = useLoadingStore();
const confirmStore = useConfirmStore();

useThemeSync();
localeStore.initializeLocale();

function registerGlobalComponentRefs(): void {
  if (floatingNotifyRef.value) {
    notifyStore.setNotifyRef(floatingNotifyRef.value);
  }

  if (loadingOverlayRef.value) {
    loadingStore.setLoadingRef(loadingOverlayRef.value);
  }

  if (confirmDialogRef.value) {
    confirmStore.setConfirmRef(confirmDialogRef.value);
  }
}

onMounted(registerGlobalComponentRefs);
</script>
