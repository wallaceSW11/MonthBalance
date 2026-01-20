<template>
  <v-container fluid class="incomes-view pa-0">
    <v-app-bar color="primary" density="compact">
      <v-btn icon @click="goBack">
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>

      <v-app-bar-title>{{ t('navigation.incomes') }}</v-app-bar-title>

      <v-btn icon>
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-app-bar>

    <div class="content-container">
      <v-card class="ma-4" elevation="0" color="surface">
        <v-card-text class="text-center pa-8">
          <v-icon size="64" color="primary" class="mb-4">
            mdi-cash-plus
          </v-icon>
          
          <h2 class="text-h6 mb-2">{{ t('incomes.title') }}</h2>
          <p class="text-body-2 text-medium-emphasis">
            {{ t('incomes.description') }}
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
.incomes-view {
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.content-container {
  flex: 1;
  overflow-y: auto;
}
</style>
