<template>
  <v-navigation-drawer v-model="open" :temporary="isTemporary" :permanent="!isTemporary" width="280">
    <div class="drawer-content">
      <div class="profile-section">
        <v-avatar size="64" color="primary" class="profile-avatar">
          <v-icon size="40" color="white">mdi-account</v-icon>
        </v-avatar>

        <h2 class="profile-name">{{ userName }}</h2>
        <p class="profile-email">{{ userEmail }}</p>
      </div>

      <v-list class="navigation-list">
        <v-list-item
          v-for="item in navigationItems"
          :key="item.path"
          :prepend-icon="item.icon"
          :title="t(item.titleKey)"
          :to="item.path"
          :active="isActiveRoute(item.path)"
        />
      </v-list>

      <div class="drawer-footer">
        <div class="settings-section">
          <LanguageSelector />
          <ThemeToggle />
        </div>

        <FeedbackDialog>
          <template #activator="{ props }">
            <v-btn
              v-bind="props"
              block
              color="primary"
              variant="tonal"
              prepend-icon="mdi-message-text"
              class="mb-2"
            >
              {{ t('drawer.sendFeedback') }}
            </v-btn>
          </template>
        </FeedbackDialog>

        <v-btn
          block
          color="error"
          variant="tonal"
          prepend-icon="mdi-logout"
          class="logout-button"
          @click="handleLogout"
        >
          {{ t('drawer.logout') }}
        </v-btn>
      </div>
    </div>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { useDisplay } from 'vuetify';
import { ThemeToggle, LanguageSelector, confirm } from '@wallacesw11/base-lib';
import { useAuthStore } from '@/stores/auth';
import { isAdmin } from '@/utils/auth';
import { ROUTES } from '@/constants/routes';
import type { NavigationItem } from '@/types/navigation';
import FeedbackDialog from '@/components/FeedbackDialog.vue';

const open = defineModel<boolean>({ required: true });

const router = useRouter();
const route = useRoute();
const { t } = useI18n();
const authStore = useAuthStore();
const { width } = useDisplay();

const userIsAdmin = ref(isAdmin());

// Drawer é temporário apenas em telas menores que 960px
const isTemporary = computed(() => width.value < 960);

// Atualiza quando o usuário mudar
watch(() => authStore.user, () => {
  userIsAdmin.value = isAdmin();
}, { immediate: true });

const navigationItems = computed<NavigationItem[]>(() => {
  const items: NavigationItem[] = [
    {
      icon: 'mdi-home',
      titleKey: 'drawer.home',
      path: ROUTES.HOME
    },
    {
      icon: 'mdi-trending-up',
      titleKey: 'drawer.incomes',
      path: ROUTES.INCOME_TYPES
    },
    {
      icon: 'mdi-trending-down',
      titleKey: 'drawer.expenses',
      path: ROUTES.EXPENSE_TYPES
    },
    {
      icon: 'mdi-account',
      titleKey: 'drawer.myAccount',
      path: ROUTES.ACCOUNT
    }
  ];

  if (userIsAdmin.value) {
    items.push({
      icon: 'mdi-shield-crown',
      titleKey: 'drawer.admin',
      path: ROUTES.ADMIN_DASHBOARD
    });
  }

  return items;
});

const userName = computed(() => authStore.user?.name ?? t('common.appName'));

const userEmail = computed(() => authStore.user?.email ?? '');

function isActiveRoute(path: string): boolean {
  return route.path === path;
}

async function handleLogout(): Promise<void> {
  const confirmed = await confirm.show(
    t('drawer.logoutTitle'),
    t('drawer.logoutMessage'),
    {
      confirmText: t('drawer.logoutConfirm'),
      cancelText: t('common.cancel'),
      confirmColor: 'error'
    }
  );

  if (!confirmed) return;

  authStore.logout();
  await router.push(ROUTES.LOGIN);
}
</script>

<style scoped>
.drawer-content {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.profile-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 32px 24px;
  border-bottom: 1px solid rgba(var(--v-border-color), var(--v-border-opacity));
}

.profile-avatar {
  margin-bottom: 16px;
  border: 2px solid rgba(var(--v-theme-primary), 0.3);
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

.navigation-list {
  flex: 1;
  padding: 8px;
}

.drawer-footer {
  padding: 16px;
  border-top: 1px solid rgba(var(--v-border-color), var(--v-border-opacity));
}

.footer-divider {
  margin-bottom: 16px;
}

.settings-section {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 16px;
}

.logout-button {
  margin-top: 8px;
}
</style>
