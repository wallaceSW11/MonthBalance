<template>
  <v-container
    fluid
    class="settings-view pa-0"
  >
    <v-app-bar
      color="primary"
      density="compact"
    >
      <v-btn
        icon
        @click="goBack"
      >
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-app-bar-title>{{ t('navigation.settings') }}</v-app-bar-title>
    </v-app-bar>

    <div class="content-container">
      <v-card
        class="ma-4"
        elevation="0"
        color="surface"
      >
        <v-card-text class="text-center pa-8">
          <v-icon
            size="64"
            color="primary"
            class="mb-4"
          >
            mdi-cog
          </v-icon>
          
          <h2 class="text-h6 mb-2">
            {{ t('settings.title') }}
          </h2>
          <p class="text-body-2 text-medium-emphasis">
            {{ t('settings.description') }}
          </p>
        </v-card-text>
      </v-card>
    </div>
  </v-container>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

const { t } = useI18n()
const router = useRouter()

function goBack(): void {
  router.push('/')
}
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
