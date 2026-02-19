<template>
  <v-app>
    <AppDrawer v-if="authenticated" v-model="drawerOpen" />

    <v-main>
      <router-view @toggle-drawer="drawerOpen = !drawerOpen" />
    </v-main>

    <FloatingNotify ref="floatingNotifyRef" />
    <LoadingOverlay ref="loadingOverlayRef" />
    <ConfirmDialog ref="confirmDialogRef" />
  </v-app>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useDisplay } from 'vuetify';
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
import { useAuthStore } from '@/stores/auth';
import AppDrawer from '@/components/AppDrawer.vue';

const floatingNotifyRef = ref();
const loadingOverlayRef = ref();
const confirmDialogRef = ref();
const drawerOpen = ref<boolean>(false);
const { width } = useDisplay();

const localeStore = useLocaleStore();
const notifyStore = useNotifyStore();
const loadingStore = useLoadingStore();
const confirmStore = useConfirmStore();
const authStore = useAuthStore();

const authenticated = computed(() => authStore.authenticated);

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

// Abre o drawer automaticamente em telas maiores que 960px
watch(width, (newWidth) => {
  if (newWidth >= 960) {
    drawerOpen.value = true;
  }
}, { immediate: true });

onMounted(registerGlobalComponentRefs);
</script>
