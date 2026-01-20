<template>
  <v-navigation-drawer
    v-model="isOpen"
    temporary
    location="left"
    width="280"
  >
    <div class="drawer-header">
      <div class="app-logo">
        <v-icon
          size="32"
          color="primary"
        >
          mdi-chart-line
        </v-icon>
        <span class="app-name">{{ t('common.appName') }}</span>
      </div>
    </div>

    <v-list
      nav
      density="comfortable"
    >
      <v-list-item
        v-for="item in menuItems"
        :key="item.route"
        :to="item.route"
        :prepend-icon="item.icon"
        :title="item.title"
        :active="isActiveRoute(item.route)"
        @click="closeDrawer"
      />
    </v-list>

    <template #append>
      <div class="drawer-footer">
        <v-divider class="mb-4" />
        
        <div class="footer-actions">
          <LanguageSelector :available-locales="availableLocales" />
          <ThemeToggle />
        </div>
      </div>
    </template>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRoute } from 'vue-router'
import { LanguageSelector, ThemeToggle } from '@wallacesw11/base-lib'
import { availableLocales } from '@/locales'

const { t } = useI18n()
const route = useRoute()

const isOpen = defineModel<boolean>({ default: false })

const menuItems = computed(() => [
  {
    title: t('navigation.dashboard'),
    icon: 'mdi-view-dashboard',
    route: '/',
  },
  {
    title: t('navigation.incomes'),
    icon: 'mdi-cash-plus',
    route: '/incomes',
  },
  {
    title: t('navigation.expenses'),
    icon: 'mdi-cash-minus',
    route: '/expenses',
  },
])

function isActiveRoute(routePath: string): boolean {
  return route.path === routePath
}

function closeDrawer(): void {
  isOpen.value = false
}
</script>

<style scoped>
.drawer-header {
  padding: 24px 16px;
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.12);
}

.app-logo {
  display: flex;
  align-items: center;
  gap: 12px;
}

.app-name {
  font-size: 20px;
  font-weight: 700;
  color: rgba(var(--v-theme-on-surface), 0.87);
}

.drawer-footer {
  padding: 16px;
}

.footer-actions {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 8px;
}
</style>
