import { defineConfig } from "vitest/config";
import vue from "@vitejs/plugin-vue";
import vuetify from "vite-plugin-vuetify";
import { fileURLToPath } from "node:url";

export default defineConfig({
  plugins: [
    vue(),
    vuetify({
      autoImport: true,
      styles: "none",
    }),
  ],
  test: {
    globals: true,
    environment: "jsdom",
    setupFiles: ["./tests/unit/setup.ts"],
    coverage: {
      provider: "v8",
      reporter: ["text", "json", "html"],
    },
    server: {
      deps: {
        inline: ["vuetify"],
      },
    },
  },
  ssr: {
    noExternal: ["vuetify"],
  },
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
});
