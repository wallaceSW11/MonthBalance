import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "@/App.vue";
import router from "@/router";
import vuetify from "@/plugins/vuetify";
import i18n from "@/plugins/i18n";
import { setupLib } from "@wallacesw11/base-lib";
import { useThemeStore } from "@wallacesw11/base-lib/stores";
import { settingsStorageService } from "@/services/storage/SettingsStorageService";
import "@wallacesw11/base-lib/style.css";
import "@/styles/main.css";

const app = createApp(App);
const pinia = createPinia();

app.use(pinia);
app.use(router);
app.use(vuetify);
app.use(i18n);

setupLib(app);

async function initializeAndMountApp() {
  const settings = settingsStorageService.getSettings()
  i18n.global.locale.value = settings.locale

  const themeStore = useThemeStore();
  await themeStore.loadTheme();

  app.mount("#app");
}

initializeAndMountApp();
