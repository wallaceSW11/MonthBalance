import "vuetify/styles";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import "@mdi/font/css/materialdesignicons.css";

const lightColors = {
  primary: "#00aab2",
  secondary: "#5CBBF6",
  accent: "#005CAF",
  error: "#DC465D",
  info: "#2196F3",
  success: "#5EC77E",
  warning: "#FB8C00",
  background: "#fafafa",
  surface: "#FFFFFF",
};

const darkColors = {
  primary: "#00aab2",
  secondary: "#424242",
  accent: "#82B1FF",
  error: "#DC465D",
  info: "#2196F3",
  success: "#5EC77E",
  warning: "#FFA726",
  background: "#1c1c22",
  surface: "#2E2E33",
};

export default createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: "dark",
    themes: {
      light: {
        dark: false,
        colors: lightColors,
      },
      dark: {
        dark: true,
        colors: darkColors,
      },
    },
  },
  icons: {
    defaultSet: "mdi",
  },
});
