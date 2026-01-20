import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "@/App.vue";
import router from "@/router";
import vuetify from "@/plugins/vuetify";
import i18n from "@/plugins/i18n";
import { setupLib, useThemeStore } from "@wallacesw11/base-lib";
import "@wallacesw11/base-lib/style.css";
import "@/styles/main.css";

const app = createApp(App);
const pinia = createPinia();

function registerPlugins(app: ReturnType<typeof createApp>) {
  app.use(pinia);
  app.use(router);
  app.use(vuetify);
  app.use(i18n);
  setupLib(app);
}

async function initializeAndMountApp() {
  const themeStore = useThemeStore();
  await themeStore.loadTheme();
  app.mount("#app");
}

registerPlugins(app);
initializeAndMountApp();
