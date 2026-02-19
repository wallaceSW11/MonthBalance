<template>
  <v-app-bar color="primary" elevation="2">
    <v-app-bar-nav-icon @click="$emit('menu-click')" />
    <v-app-bar-title>{{ title }}</v-app-bar-title>
    
    <template #append>
      <v-tabs v-model="currentTab" color="white" align-tabs="end">
        <v-tab :to="ROUTES.ADMIN_DASHBOARD" value="dashboard">
          <v-icon start>mdi-view-dashboard</v-icon>
          Dashboard
        </v-tab>
        <v-tab :to="ROUTES.ADMIN_USERS" value="users">
          <v-icon start>mdi-account-group</v-icon>
          Usuários
        </v-tab>
        <v-tab :to="ROUTES.ADMIN_FEEDBACKS" value="feedbacks">
          <v-icon start>mdi-message-text</v-icon>
          Feedbacks
        </v-tab>
      </v-tabs>
    </template>
  </v-app-bar>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { ROUTES } from '@/constants/routes';

defineProps<{
  title: string;
}>();

defineEmits<{
  'menu-click': [];
}>();

const route = useRoute();
const currentTab = ref('dashboard');

// Atualiza a tab baseado na rota atual
watch(() => route.path, (newPath) => {
  if (newPath === ROUTES.ADMIN_DASHBOARD) {
    currentTab.value = 'dashboard';
  } else if (newPath === ROUTES.ADMIN_USERS) {
    currentTab.value = 'users';
  } else if (newPath === ROUTES.ADMIN_FEEDBACKS) {
    currentTab.value = 'feedbacks';
  }
}, { immediate: true });
</script>
