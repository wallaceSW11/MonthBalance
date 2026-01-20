# Vue 3 + TypeScript Base Project

A professional starter kit for scalable Vue 3 applications with TypeScript, Vuetify 3, and comprehensive testing setup, powered by [BaseLib](https://github.com/wallaceSW11/BaseLib).

## 🚀 Features

- ✅ **Vue 3** with Composition API
- ✅ **TypeScript** for type safety
- ✅ **Vuetify 3** with Material Design Icons
- ✅ **White Label Theme System** - Customize colors and branding via JSON
- ✅ **Light/Dark Theme Support** - Automatic theme switching with persistence
- ✅ **Vue Router** for navigation
- ✅ **Pinia** for state management
- ✅ **Vue I18n** for internationalization
- ✅ **Axios** with pre-configured interceptors
- ✅ **BaseLib** integration - Reusable components and utilities
- ✅ **Vitest** for unit testing
- ✅ **Cypress** for E2E testing
- ✅ **Vite** for blazing fast builds
- ✅ **PWA Support** ready

## 📦 Installation

**This project requires PNPM for optimal performance:**

```bash
# Install pnpm globally (if you haven't already)
npm install -g pnpm

# Install dependencies
pnpm install
```

## �️ Development

```bash
# Start development server
pnpm dev

# Build for production
pnpm build

# Preview production build
pnpm preview
```

## 🧪 Testing

### Unit Tests

```bash
npm run test:unit          # Run once
npm run test:unit:watch    # Watch mode
```

### E2E Tests

```bash
npm run test:e2e           # Run headless
npm run test:e2e:open      # Open Cypress UI
```

## 🎨 White Label Customization

Easily customize the application's branding without code changes:

1. Edit `public/theme.json` to configure:

   - Brand colors for light and dark themes
   - Logo paths for different themes
   - Application name and metadata

2. Toggle between light and dark themes using the theme switcher in the header

See the demo page (`/demo`) for live examples of theme customization.

## 📦 BaseLib Integration

This project comes pre-configured with **BaseLib**, a comprehensive library of reusable components and utilities.

### Available Components

- **Buttons**: `PrimaryButton`, `SecondaryButton`, `TertiaryButton`, `QuartenaryButton`
- **LanguageSelector**: Language selector with country flags
- **ThemeToggle**: Light/dark theme switcher
- **LoadingOverlay**: Full-screen loading indicator
- **FloatingNotify**: Toast notifications
- **ConfirmDialog**: Confirmation dialogs
- **ModalBase**: Customizable modal
- **IconToolTip**: Icon with optional tooltip

### Using BaseLib Components

```vue
<script setup lang="ts">
import {
  PrimaryButton,
  LanguageSelector,
  ThemeToggle,
} from "@wallacesw11/base-lib";
</script>

<template>
  <PrimaryButton
    text="Click me"
    prepend-icon="mdi-check"
    @click="handleClick"
  />
  <LanguageSelector :available-locales="locales" />
  <ThemeToggle />
</template>
```

### Using BaseLib Utilities

```typescript
import { notify, loading, confirm } from "@wallacesw11/base-lib";

// Notifications
notify("success", "Success!", "Operation completed successfully");
notify("error", "Error!", "Something went wrong");
notify("warning", "Warning!", "Please be careful");
notify("info", "Info", "This is an information message");

// Loading overlay
loading(true, "Processing your request...");
// ... perform async operation
loading(false);

// Confirmation dialog
const confirmed = await confirm(
  "Confirm Action",
  "Are you sure you want to proceed?"
);
if (confirmed) {
  // User clicked "Yes"
} else {
  // User clicked "No"
}
```

### Using BaseLib Composables

```typescript
import { useThemeSync, useThemeStore } from "@wallacesw11/base-lib";

const themeStore = useThemeStore();

// Sync theme with Vuetify
useThemeSync();

// Toggle theme
themeStore.toggleTheme();

// Get current theme
console.log(themeStore.currentMode); // 'light' or 'dark'
```

### API Client

BaseLib provides a pre-configured Axios instance with:

- Automatic Bearer token injection
- Loading states on mutations (POST, PUT, DELETE, PATCH)
- Error notification handling

```typescript
import { api } from "@wallacesw11/base-lib";

// GET request
const { data } = await api.get("/users");

// POST request (automatically shows loading)
const newUser = await api.post("/users", { name: "John" });
```

### Updating BaseLib

```bash
pnpm update @wallacesw11/base-lib
```

## 📁 Project Structure

```
src/
├── assets/         # Static assets
├── locales/        # i18n translations
│   ├── en-US.ts
│   ├── pt-BR.ts
│   └── index.ts
├── plugins/        # Plugin configuration (Vuetify, i18n)
│   ├── vuetify.ts
│   └── i18n.ts
├── router/         # Vue Router config
│   └── index.ts
├── stores/         # Pinia stores
│   └── locale.ts
├── styles/         # Global styles
│   ├── main.css
│   └── settings.scss
├── views/          # Page components
│   ├── HomeView.vue
│   └── DemoView.vue
├── App.vue         # Root component
└── main.ts         # Application entry point
```

## � Internationalization (i18n)

This project is fully configured with Vue I18n for multi-language support. The language selector in the header allows users to switch between languages seamlessly.

### Supported Languages

- 🇧🇷 Portuguese (Brazil) - `pt-BR`
- 🇺🇸 English (US) - `en-US`

### Using Translations in Components

```vue
<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n();
</script>

<template>
  <!-- Using translation in template -->
  <h1>{{ $t("home.title") }}</h1>

  <!-- Using translation with parameters -->
  <p>{{ $t("demo.buttons.clicked", { type: "Primary" }) }}</p>

  <!-- Using translation in script -->
  <button @click="notify('success', t('messages.success'), '')">
    {{ $t("common.save") }}
  </button>
</template>
```

### Using the i18n Helper Composable

```typescript
import { useI18nHelpers } from "@/composables";

const { t, changeLocale, currentLocale } = useI18nHelpers();

// Translate with parameters
const message = t("demo.buttons.clicked", { type: "Primary" });

// Change language programmatically
changeLocale("en-US");

// Get current language
console.log(currentLocale()); // 'pt-BR' or 'en-US'
```

### Adding New Languages

1. Create a new file in `src/locales/` (e.g., `es-ES.ts`)
2. Add translations following the existing structure
3. Import and add to `src/locales/index.ts`:

```typescript
import esES from "@/locales/es-ES";

export const messages = {
  "pt-BR": ptBR,
  "en-US": enUS,
  "es-ES": esES, // New language
};

export const availableLocales = [
  { code: "pt-BR", name: "Português (Brasil)", countryCode: "BR" },
  { code: "en-US", name: "English (US)", countryCode: "US" },
  { code: "es-ES", name: "Español", countryCode: "ES" }, // New language
];
```

4. Update the type in `src/locales/index.ts`:

```typescript
export type LocaleCode = "pt-BR" | "en-US" | "es-ES";
```

### Language Persistence

The selected language is automatically saved to `localStorage` and restored on page reload. The app also detects the browser's language on first visit.

## �🎯 Using as a Base Project

1. Clone or download this project
2. Rename the folder and update `package.json` (name, version, etc.)
3. Remove example code from `DemoView.vue` if not needed
4. Run `pnpm install`
5. Start developing!

## 📝 Demo

Visit `/demo` route to see live examples of all BaseLib components and utilities in action.

The `DemoView.vue` contains practical examples of:

- All button variants
- Notification system
- Theme configuration
- Loading overlay
- Confirmation dialogs
- Icon tooltips
- Pinia store integration

## 📚 Documentation

- [BaseLib Documentation](https://github.com/wallaceSW11/BaseLib)
- [Vue 3 Documentation](https://vuejs.org/)
- [Vuetify 3 Documentation](https://vuetifyjs.com/)
- [Pinia Documentation](https://pinia.vuejs.org/)
- [Vue Router Documentation](https://router.vuejs.org/)

## 📝 License

MIT

---

Built with ❤️ using Vue 3, TypeScript, and Vuetify
