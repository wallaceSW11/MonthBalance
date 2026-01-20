<template>
  <v-container fluid class="settings-view pa-0">
    <v-app-bar color="primary" density="compact">
      <v-btn icon @click="goBack">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-app-bar-title>{{ t('navigation.settings') }}</v-app-bar-title>
    </v-app-bar>

    <div class="content-container">
      <v-list>
        <v-list-subheader>{{ t('settings.appearance') }}</v-list-subheader>

        <v-list-item>
          <template #prepend>
            <v-icon>mdi-theme-light-dark</v-icon>
          </template>

          <v-list-item-title>{{ t('settings.theme.title') }}</v-list-item-title>
          <v-list-item-subtitle>{{ t('settings.theme.description') }}</v-list-item-subtitle>

          <template #append>
            <v-btn-toggle
              v-model="selectedTheme"
              mandatory
              density="compact"
              @update:model-value="handleThemeChange"
            >
              <v-btn value="light" size="small">
                <v-icon>mdi-white-balance-sunny</v-icon>
              </v-btn>
              <v-btn value="dark" size="small">
                <v-icon>mdi-moon-waning-crescent</v-icon>
              </v-btn>
            </v-btn-toggle>
          </template>
        </v-list-item>

        <v-divider class="my-2" />

        <v-list-subheader>{{ t('settings.language.title') }}</v-list-subheader>

        <v-list-item>
          <template #prepend>
            <v-icon>mdi-translate</v-icon>
          </template>

          <v-list-item-title>{{ t('settings.language.title') }}</v-list-item-title>
          <v-list-item-subtitle>{{ t('settings.language.description') }}</v-list-item-subtitle>

          <template #append>
            <v-select
              v-model="selectedLocale"
              :items="localeOptions"
              item-title="name"
              item-value="code"
              density="compact"
              variant="outlined"
              hide-details
              style="width: 180px"
              @update:model-value="handleLocaleChange"
            />
          </template>
        </v-list-item>

        <v-divider class="my-2" />

        <v-list-subheader>{{ t('settings.about') }}</v-list-subheader>

        <v-list-item>
          <template #prepend>
            <v-icon>mdi-information</v-icon>
          </template>

          <v-list-item-title>{{ t('common.appName') }}</v-list-item-title>
          <v-list-item-subtitle>{{ t('settings.version') }}: 1.0.0</v-list-item-subtitle>
        </v-list-item>
      </v-list>
    </div>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { useTheme } from 'vuetify'
import { useLocaleStore } from '@/stores/locale'
import { availableLocales } from '@/locales'

const { t } = useI18n()
const router = useRouter()
const theme = useTheme()
const localeStore = useLocaleStore()

const selectedTheme = ref<'light' | 'dark'>('dark')
const selectedLocale = ref<string>('pt-BR')

const localeOptions = availableLocales.map((locale) => ({
  code: locale.code,
  name: locale.name,
}))

function goBack(): void {
  router.push('/')
}

function handleThemeChange(themeName: 'light' | 'dark'): void {
  theme.global.name.value = themeName
}

function handleLocaleChange(locale: string): void {
  localeStore.setLocale(locale as 'pt-BR' | 'en-US')
}

onMounted(() => {
  selectedTheme.value = theme.global.name.value as 'light' | 'dark'
  selectedLocale.value = localeStore.currentLocale
})
</script>

<style scoped>
.settings-view {
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.content-container {
  flex: 1;
  overflow-y: auto;
}
</style>
