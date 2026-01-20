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
        name: "MB - Month Balance",
        short_name: "MB",
        description: "Monthly financial forecast management",
        theme_color: "#1c1c22",
        background_color: "#1c1c22",
        display: "standalone",
        orientation: "portrait",
        icons: [
          {
            src: "pwa-192x192.png",
            sizes: "192x192",
            type: "image/png",
            purpose: "any maskable",
          },
          {
            src: "pwa-512x512.png",
            sizes: "512x512",
            type: "image/png",
            purpose: "any maskable",
          },
          {
            src: "apple-touch-icon.png",
            sizes: "180x180",
            type: "image/png",
          },
        ],
      },
      workbox: {
        globPatterns: ["**/*.{js,css,html,ico,png,svg,woff2}"],
        runtimeCaching: [
          {
            urlPattern: /^https:\/\/fonts\.googleapis\.com\/.*/i,
            handler: "CacheFirst",
            options: {
              cacheName: "google-fonts-cache",
              expiration: {
                maxEntries: 10,
                maxAgeSeconds: 60 * 60 * 24 * 365,
              },
              cacheableResponse: {
                statuses: [0, 200],
              },
            },
          },
        ],
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
            if (id.includes("@mdi/font")) {
              return "mdi";
            }
            if (id.includes("vue-router") || id.includes("pinia")) {
              return "vue-vendor";
            }
            if (id.includes("/vue/") || id.includes("/vue@")) {
              return "vue-vendor";
            }
            if (id.includes("vue-i18n")) {
              return "i18n";
            }
            if (id.includes("@wallacesw11/base-lib")) {
              return "base-lib";
            }
            if (id.includes("axios")) {
              return "axios";
            }
            if (id.includes("workbox")) {
              return "pwa";
            }
            return "vendor";
          }
        },
      },
    },
    chunkSizeWarningLimit: 700,
    cssCodeSplit: true,
    minify: "terser",
    terserOptions: {
      compress: {
        drop_console: true,
        drop_debugger: true,
        passes: 2,
        pure_funcs: ['console.log', 'console.info', 'console.debug'],
      },
      mangle: {
        safari10: true,
      },
      format: {
        comments: false,
      },
    },
    reportCompressedSize: true,
    sourcemap: false,
    assetsInlineLimit: 4096,
  },
  optimizeDeps: {
    include: ['vue', 'vue-router', 'pinia', 'vuetify', 'vue-i18n'],
    exclude: ['@wallacesw11/base-lib', '@mdi/font'],
  },
});
