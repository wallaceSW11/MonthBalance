<template>
  <v-app>
    <v-app-bar color="primary" elevation="0">
      <v-btn icon="mdi-arrow-left" @click="goBack" />
      <v-toolbar-title>{{ t('privacy.title') }}</v-toolbar-title>
    </v-app-bar>

    <v-main>
      <v-container class="py-8">
        <v-card elevation="0">
          <v-card-text class="text-body-1">
            <div class="privacy-content">
              <p class="text-h6 mb-4">{{ t('privacy.lastUpdate') }}</p>

              <section class="mb-6">
                <h2 class="text-h5 mb-3">{{ t('privacy.section1.title') }}</h2>
                <p>{{ t('privacy.section1.content') }}</p>
              </section>

              <section class="mb-6">
                <h2 class="text-h5 mb-3">{{ t('privacy.section2.title') }}</h2>
                <p class="mb-2">{{ t('privacy.section2.intro') }}</p>
                <ul class="ml-6">
                  <li>{{ t('privacy.section2.item1') }}</li>
                  <li>{{ t('privacy.section2.item2') }}</li>
                  <li>{{ t('privacy.section2.item3') }}</li>
                </ul>
              </section>

              <section class="mb-6">
                <h2 class="text-h5 mb-3">{{ t('privacy.section3.title') }}</h2>
                <p class="mb-2">{{ t('privacy.section3.intro') }}</p>
                <ul class="ml-6">
                  <li>{{ t('privacy.section3.item1') }}</li>
                  <li>{{ t('privacy.section3.item2') }}</li>
                  <li>{{ t('privacy.section3.item3') }}</li>
                </ul>
              </section>

              <section class="mb-6">
                <h2 class="text-h5 mb-3">{{ t('privacy.section4.title') }}</h2>
                <p>{{ t('privacy.section4.content') }}</p>
              </section>

              <section class="mb-6">
                <h2 class="text-h5 mb-3">{{ t('privacy.section5.title') }}</h2>
                <p>{{ t('privacy.section5.content') }}</p>
              </section>

              <section class="mb-6">
                <h2 class="text-h5 mb-3">{{ t('privacy.section6.title') }}</h2>
                <p>{{ t('privacy.section6.content') }}</p>
              </section>

              <section class="mb-6">
                <h2 class="text-h5 mb-3">{{ t('privacy.section7.title') }}</h2>
                <p>{{ t('privacy.section7.content') }}</p>
              </section>
            </div>
          </v-card-text>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';

const router = useRouter();
const { t } = useI18n();

function goBack(): void {
  router.back();
}
</script>

<style scoped>
.privacy-content {
  max-width: 800px;
  margin: 0 auto;
}

.privacy-content h2 {
  color: rgb(var(--v-theme-primary));
}

.privacy-content p {
  line-height: 1.8;
  margin-bottom: 1rem;
}

.privacy-content ul {
  line-height: 1.8;
}

.privacy-content li {
  margin-bottom: 0.5rem;
}
</style>
