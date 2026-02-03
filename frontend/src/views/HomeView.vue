<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 class="text-h3 mb-6">{{ t('home.title') }}</h1>
        <p class="text-h6 mb-4">{{ t('home.subtitle') }}</p>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>{{ t('common.appName') }} Features</v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item prepend-icon="mdi-vuejs">
                <v-list-item-title>Vue 3 Composition API</v-list-item-title>
              </v-list-item>
              <v-list-item prepend-icon="mdi-language-typescript">
                <v-list-item-title>TypeScript</v-list-item-title>
              </v-list-item>
              <v-list-item prepend-icon="mdi-material-design">
                <v-list-item-title>Vuetify 3</v-list-item-title>
              </v-list-item>
              <v-list-item prepend-icon="mdi-routes">
                <v-list-item-title>Vue Router</v-list-item-title>
              </v-list-item>
              <v-list-item prepend-icon="mdi-store">
                <v-list-item-title>Pinia State Management</v-list-item-title>
              </v-list-item>
              <v-list-item prepend-icon="mdi-test-tube">
                <v-list-item-title>Vitest & Cypress Testing</v-list-item-title>
              </v-list-item>
              <v-list-item prepend-icon="mdi-translate">
                <v-list-item-title>Vue I18n Internationalization</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>Quick Start</v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item>
                <v-list-item-title class="text-caption">
                  <code>pnpm install</code> - Install dependencies
                </v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title class="text-caption">
                  <code>pnpm dev</code> - Start development server
                </v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title class="text-caption">
                  <code>pnpm build</code> - Build for production
                </v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title class="text-caption">
                  <code>pnpm test:unit</code> - Run unit tests
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="primary" to="/demo">
              View Demo
              <v-icon end>mdi-arrow-right</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>
