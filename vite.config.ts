import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import vuetify from "vite-plugin-vuetify";
import { VitePWA } from "vite-plugin-pwa";
import { fileURLToPath, URL } from "node:url";

export default defineConfig({
  plugins: [
    vue(),
    vuetify({
      autoImport: true,
    }),
    VitePWA({
      registerType: "autoUpdate",
      includeAssets: ["favicon.ico", "robots.txt", "apple-touch-icon.png"],
      manifest: {
        name: "Vue 3 TypeScript Base",
        short_name: "Vue3Base",
        description: "A professional Vue 3 + TypeScript starter kit",
        theme_color: "#1867C0",
        icons: [
          {
            src: "pwa-192x192.png",
            sizes: "192x192",
            type: "image/png",
          },
          {
            src: "pwa-512x512.png",
            sizes: "512x512",
            type: "image/png",
          },
        ],
      },
      workbox: {
        globPatterns: ["**/*.{js,css,html,ico,png,svg,woff2}"],
      },
    }),
  ],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  build: {
    rollupOptions: {
      output: {
        manualChunks: (id) => {
          if (id.includes("node_modules")) {
            if (id.includes("vuetify")) {
              return "vuetify";
            }
            if (id.includes("vue-router") || id.includes("pinia")) {
              return "vue-vendor";
            }
            if (id.includes("vue-i18n")) {
              return "i18n";
            }
            if (id.includes("vue") || id.includes("@vue")) {
              return "vue";
            }
            return "vendor";
          }
        },
      },
    },
    chunkSizeWarningLimit: 650,
  },
});
