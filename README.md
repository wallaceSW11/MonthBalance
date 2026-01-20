# 💰 Month Balance

> A simple and efficient Progressive Web App (PWA) for monthly financial control

<div align="center">

![Vue.js](https://img.shields.io/badge/Vue.js-3.5-4FC08D?style=flat&logo=vue.js&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-5.7-3178C6?style=flat&logo=typescript&logoColor=white)
![Vuetify](https://img.shields.io/badge/Vuetify-3.7-1867C0?style=flat&logo=vuetify&logoColor=white)
![PWA](https://img.shields.io/badge/PWA-Ready-5A0FC8?style=flat&logo=pwa&logoColor=white)

</div>

## 📋 About the Project

**Month Balance** is a Progressive Web App (PWA) designed to simplify monthly financial management. With a clean and intuitive interface, you can manage your income and expenses month by month, keeping your finances organized and under control.

### ✨ Key Features

- 📊 **Monthly Control**: View income and expenses organized by month
- 💵 **Income Management**: Add manual income or hourly-based earnings
- 💸 **Expense Management**: Record and track all your expenses
- 📱 **PWA**: Install on your device and use as a native app
- 🌓 **Light/Dark Theme**: Switch between themes according to your preference
- 🌍 **Multilingual**: Support for Portuguese (BR) and English (US)
- 💾 **Local Storage**: Your data is saved in the browser
- 🎨 **White Label**: Customize colors and branding via JSON

## 🚀 Tech Stack

### Frontend

- **[Vue 3](https://vuejs.org/)** - Progressive JavaScript framework with Composition API
- **[TypeScript](https://www.typescriptlang.org/)** - JavaScript superset with static typing
- **[Vuetify 3](https://vuetifyjs.com/)** - Material Design component framework
- **[Pinia](https://pinia.vuejs.org/)** - Official Vue state management
- **[Vue Router](https://router.vuejs.org/)** - Official Vue routing
- **[Vue I18n](https://vue-i18n.intlify.dev/)** - Internationalization
- **[Vite](https://vitejs.dev/)** - Lightning-fast build tool
- **[Vitest](https://vitest.dev/)** - Unit testing framework
- **[Cypress](https://www.cypress.io/)** - E2E testing framework

### Tools

- **PNPM** - Efficient package manager
- **ESLint** - Code quality linter
- **Vite PWA Plugin** - Progressive Web App support

## 📦 Installation

### Prerequisites

- Node.js >= 20.0.0
- PNPM >= 9.0.0

### Installing PNPM

```bash
npm install -g pnpm
```

### Installing Dependencies

```bash
cd frontend
pnpm install
```

## 🎮 Available Commands

```bash
# Start development server
pnpm dev

# Build for production
pnpm build

# Preview production build
pnpm preview

# Run unit tests
pnpm test:unit

# Run unit tests in watch mode
pnpm test:unit:watch

# Run E2E tests
pnpm test:e2e

# Open Cypress interface
pnpm test:e2e:open

# Lint and auto-fix
pnpm lint
```

## 🎨 Customization

### Theme and Branding

Edit the `frontend/public/theme.json` file to customize:

- Light and dark theme colors
- Logos for each theme
- Application name and metadata

```json
{
  "customization": {
    "appName": "Month Balance",
    "appDescription": "Monthly financial control",
    "copyrightText": "© 2025 Month Balance"
  },
  "theme": {
    "light": {
      "primary": "#1976D2",
      "secondary": "#424242"
    },
    "dark": {
      "primary": "#2196F3",
      "secondary": "#616161"
    }
  }
}
```

### Translations

Add or edit translations in the files:

- `frontend/src/locales/pt-BR.ts` - Portuguese (Brazil)
- `frontend/src/locales/en-US.ts` - English (US)

## 📱 PWA - Progressive Web App

Month Balance can be installed as a native application on mobile and desktop devices:

1. Access the app in your browser
2. Look for the "Install" or "Add to Home Screen" option
3. Use it as a native app with its own icon

## 📁 Project Structure

```
frontend/
├── public/              # Static files
│   ├── theme.json      # Theme configuration
│   └── ...
├── src/
│   ├── components/     # Reusable Vue components
│   ├── composables/    # Vue composables
│   ├── locales/        # Translation files (i18n)
│   ├── models/         # TypeScript interfaces and types
│   ├── plugins/        # Plugins (Vuetify, i18n)
│   ├── router/         # Route configuration
│   ├── services/       # Services (storage, API)
│   ├── stores/         # Pinia stores
│   ├── styles/         # Global styles
│   ├── utils/          # Utilities
│   ├── views/          # Pages/Views
│   ├── App.vue         # Root component
│   └── main.ts         # Entry point
├── tests/              # Unit and E2E tests
└── package.json
```

## 🤝 Contributing

Contributions are welcome! Feel free to:

1. Fork the project
2. Create a branch for your feature (`git checkout -b feature/MyFeature`)
3. Commit your changes (`git commit -m 'Add MyFeature'`)
4. Push to the branch (`git push origin feature/MyFeature`)
5. Open a Pull Request

## 📄 License

This project is under the MIT license.

## 🙏 Acknowledgments

- [BaseLib](https://github.com/wallaceSW11/BaseLib) - Reusable components and utilities library
- Vue.js Community
- All contributors

---

<div align="center">

Built with ❤️ using Vue 3 and TypeScript

</div>
