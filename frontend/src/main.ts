import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "@/App.vue";
import router from "@/router";
import vuetify from "@/plugins/vuetify";
import i18n from "@/plugins/i18n";
import { setupLib, useThemeStore } from "@wallacesw11/base-lib";
import { useAuthStore } from "@/stores/auth";
import "@wallacesw11/base-lib/style.css";
import "@/styles/main.css";

const app = createApp(App);
const pinia = createPinia();

async function initializeAndMountApp() {
  app.use(pinia);
  
  const themeStore = useThemeStore();
  await themeStore.loadTheme();
  
  if (!localStorage.getItem('theme'))
    themeStore.setTheme('dark');

  const authStore = useAuthStore();
  authStore.initializeAuth();
  
  app.use(router);
  app.use(vuetify);
  app.use(i18n);
  setupLib(app);
  
  app.mount("#app");
}

initializeAndMountApp();
