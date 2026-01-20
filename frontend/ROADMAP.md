# 💰 MonthBalance - Roadmap

## 📋 Project Overview

Mobile-first web app for monthly financial forecast management. Track income and expenses with inline editing, month navigation, and localStorage persistence.

**Stack:** Vue 3, TypeScript, Pinia, Vuetify, Vue Router, Vitest, Cypress, i18n (pt-BR/en-US)

---

## 🎯 Phase 1: Main Dashboard (Route "/")

**Goal:** Implement the main screen showing monthly financial overview with inline editing

### Tasks:
- [ ] 1.1 Setup project structure and dependencies
  - [ ] Review and update package.json dependencies
  - [ ] Configure Pinia store structure
  - [ ] Setup i18n with pt-BR and en-US locales
  - [ ] Configure Vuetify theme (dark mode with custom colors)

- [ ] 1.2 Create localStorage service layer
  - [ ] Create `services/storage/` folder structure
  - [ ] Implement `StorageService` base class
  - [ ] Implement `IncomeStorageService` (CRUD operations)
  - [ ] Implement `ExpenseStorageService` (CRUD operations)
  - [ ] Implement `SettingsStorageService` (theme, locale)
  - [ ] Implement `MonthDataStorageService` (month-specific data)
  - [ ] Add TypeScript interfaces for all data models

- [ ] 1.3 Create Pinia stores
  - [ ] Create `stores/income.ts` (income management)
  - [ ] Create `stores/expense.ts` (expense management)
  - [ ] Create `stores/settings.ts` (theme, locale)
  - [ ] Create `stores/month.ts` (current month, navigation, totals)

- [ ] 1.4 Implement data models
  - [ ] Create `models/Income.ts` (id, name, type, grossValue, netValue, hourlyRate, hours, minutes)
  - [ ] Create `models/Expense.ts` (id, name, value)
  - [ ] Create `models/MonthData.ts` (year, month, incomes, expenses)
  - [ ] Create `models/Settings.ts` (theme, locale)

- [ ] 1.5 Build main dashboard layout
  - [ ] Create `views/DashboardView.vue`
  - [ ] Implement sticky header with blur effect
  - [ ] Add hamburger menu button (left)
  - [ ] Add month navigation (center: < October 2023 >)
  - [ ] Add summary cards (Receitas, Despesas, Saldo)

- [ ] 1.6 Implement income section
  - [ ] Create `components/IncomeList.vue`
  - [ ] Add collapsible section header
  - [ ] Implement inline editable input fields
  - [ ] Add currency formatting (pt-BR: 1.000,00 / en-US: 1,000.00)
  - [ ] Connect to Pinia store

- [ ] 1.7 Implement expense section
  - [ ] Create `components/ExpenseList.vue`
  - [ ] Add collapsible section header
  - [ ] Implement inline editable input fields
  - [ ] Add currency formatting
  - [ ] Connect to Pinia store

- [ ] 1.8 Implement month navigation logic
  - [ ] Add previous/next month handlers
  - [ ] Implement "duplicate to next month" dialog
  - [ ] Add 3-month forward limit validation
  - [ ] Auto-calculate totals on month change

- [ ] 1.9 Add floating action button (FAB)
  - [ ] Create FAB component (bottom center)
  - [ ] Add click handler (placeholder for future features)

- [ ] 1.10 Implement auto-save functionality
  - [ ] Debounce input changes (500ms)
  - [ ] Save to localStorage on value change
  - [ ] Update totals in real-time

---

## 🎯 Phase 2: Navigation & Routes

**Goal:** Create navigation drawer and additional pages

### Tasks:
- [ ] 2.1 Setup Vue Router
  - [ ] Configure routes: `/`, `/incomes`, `/expenses`, `/settings`
  - [ ] Add route guards if needed
  - [ ] Configure route transitions

- [ ] 2.2 Create navigation drawer
  - [ ] Create `components/NavigationDrawer.vue`
  - [ ] Add menu items: Dashboard, Receitas, Despesas, Configurações
  - [ ] Add icons for each menu item
  - [ ] Implement active route highlighting
  - [ ] Connect hamburger button to drawer

- [ ] 2.3 Create placeholder pages
  - [ ] Create `views/IncomesView.vue` (empty state)
  - [ ] Create `views/ExpensesView.vue` (empty state)
  - [ ] Create `views/SettingsView.vue` (empty state)

- [ ] 2.4 Implement drawer behavior
  - [ ] Add open/close animation
  - [ ] Add overlay/backdrop
  - [ ] Close on route change
  - [ ] Close on outside click

---

## 🎯 Phase 3: Income Management (CRUD)

**Goal:** Full CRUD for income entries with manual and hourly types

### Tasks:
- [ ] 3.1 Design income form
  - [ ] Create `components/IncomeForm.vue`
  - [ ] Add form fields: name, type (manual/hourly)
  - [ ] Conditional fields based on type:
    - Manual: grossValue, netValue
    - Hourly: hourlyRate, hours:minutes (masked input)
  - [ ] Add form validation

- [ ] 3.2 Implement income list page
  - [ ] Update `views/IncomesView.vue`
  - [ ] Display all income entries
  - [ ] Add edit/delete actions
  - [ ] Add empty state message

- [ ] 3.3 Implement hourly calculation
  - [ ] Create utility function: `calculateHourlyIncome(hourlyRate, hours, minutes)`
  - [ ] Formula: `(hours + (minutes / 60)) * hourlyRate`
  - [ ] Auto-calculate on input change

- [ ] 3.4 Add income CRUD operations
  - [ ] Create new income (modal/dialog)
  - [ ] Edit existing income
  - [ ] Delete income (with confirmation)
  - [ ] Persist to localStorage

- [ ] 3.5 Connect to dashboard
  - [ ] Update dashboard to show income entries
  - [ ] Sync changes between pages
  - [ ] Update totals automatically

---

## 🎯 Phase 4: Expense Management (CRUD)

**Goal:** Full CRUD for expense entries

### Tasks:
- [ ] 4.1 Design expense form
  - [ ] Create `components/ExpenseForm.vue`
  - [ ] Add form fields: name, value
  - [ ] Add form validation

- [ ] 4.2 Implement expense list page
  - [ ] Update `views/ExpensesView.vue`
  - [ ] Display all expense entries
  - [ ] Add edit/delete actions
  - [ ] Add empty state message

- [ ] 4.3 Add expense CRUD operations
  - [ ] Create new expense (modal/dialog)
  - [ ] Edit existing expense
  - [ ] Delete expense (with confirmation)
  - [ ] Persist to localStorage

- [ ] 4.4 Connect to dashboard
  - [ ] Update dashboard to show expense entries
  - [ ] Sync changes between pages
  - [ ] Update totals automatically

---

## 🎯 Phase 5: Settings Page

**Goal:** Configure theme and language preferences

### Tasks:
- [ ] 5.1 Implement settings page
  - [ ] Update `views/SettingsView.vue`
  - [ ] Add theme selector (light/dark)
  - [ ] Add language selector (pt-BR/en-US)
  - [ ] Add section headers and descriptions

- [ ] 5.2 Implement theme switching
  - [ ] Connect to Vuetify theme system
  - [ ] Persist theme preference to localStorage
  - [ ] Apply theme on app load

- [ ] 5.3 Implement language switching
  - [ ] Connect to i18n plugin
  - [ ] Persist locale preference to localStorage
  - [ ] Apply locale on app load
  - [ ] Update all UI text dynamically

- [ ] 5.4 Add settings to Pinia store
  - [ ] Create actions: `setTheme()`, `setLocale()`
  - [ ] Load settings on app initialization

---

## 🧪 Phase 6: Testing & Quality

**Goal:** Ensure code quality and functionality

### Tasks:
- [ ] 6.1 Unit tests
  - [ ] Test Pinia stores (income, expense, settings, month)
  - [ ] Test utility functions (currency format, hourly calculation)
  - [ ] Test storage services

- [ ] 6.2 Component tests
  - [ ] Test IncomeList component
  - [ ] Test ExpenseList component
  - [ ] Test IncomeForm component
  - [ ] Test ExpenseForm component
  - [ ] Test NavigationDrawer component

- [ ] 6.3 E2E tests (Cypress)
  - [ ] Test main dashboard flow
  - [ ] Test month navigation
  - [ ] Test duplicate month dialog
  - [ ] Test CRUD operations (income/expense)
  - [ ] Test settings changes

- [ ] 6.4 Code quality
  - [ ] Run ESLint and fix issues
  - [ ] Ensure TypeScript strict mode compliance
  - [ ] Review and refactor code
  - [ ] Add JSDoc comments where needed

---

## 🚀 Phase 7: PWA & Polish

**Goal:** Make app installable and production-ready

### Tasks:
- [ ] 7.1 PWA configuration
  - [ ] Configure service worker
  - [ ] Add manifest.json
  - [ ] Add app icons (multiple sizes)
  - [ ] Test offline functionality

- [ ] 7.2 Performance optimization
  - [ ] Lazy load routes
  - [ ] Optimize bundle size
  - [ ] Add loading states
  - [ ] Optimize re-renders

- [ ] 7.3 UX improvements
  - [ ] Add loading spinners
  - [ ] Add success/error toasts
  - [ ] Add animations/transitions
  - [ ] Add empty states
  - [ ] Add confirmation dialogs

- [ ] 7.4 Accessibility
  - [ ] Add ARIA labels
  - [ ] Test keyboard navigation
  - [ ] Test screen reader compatibility
  - [ ] Ensure color contrast

- [ ] 7.5 Final polish
  - [ ] Review all i18n translations
  - [ ] Test on iPhone 16 Pro Max
  - [ ] Test dark/light themes
  - [ ] Fix any remaining bugs

---

## 📝 Notes

### Data Structure (localStorage)

```typescript
// Key: "month-balance-incomes"
{
  "2024-01": [
    { id: "uuid", name: "Salary", type: "manual", grossValue: 10000, netValue: 8000 },
    { id: "uuid", name: "Freelance", type: "hourly", hourlyRate: 30, hours: 10, minutes: 30 }
  ]
}

// Key: "month-balance-expenses"
{
  "2024-01": [
    { id: "uuid", name: "Rent", value: 3200 },
    { id: "uuid", name: "Groceries", value: 1200 }
  ]
}

// Key: "month-balance-settings"
{
  theme: "dark",
  locale: "pt-BR"
}
```

### Design Tokens (from theme.json)

```typescript
colors: {
  primary: "#00aab2",
  success: "#5EC77E",   // Income/positive (green)
  error: "#DC465D",     // Expense/negative (red)
  background: "#1c1c22",
  surface: "#2E2E33",
  divider: "#3F3F46"
}

fonts: {
  primary: "Space Grotesk, sans-serif"
}
```

### Month Navigation Rules

1. Current month is default on first load
2. Can navigate backward unlimited
3. Can navigate forward max 3 months from current
4. If navigating to non-existent future month, show dialog: "Duplicate data from previous month?"
5. Auto-save all changes immediately (debounced)

---

## ✅ Definition of Done

Each phase is complete when:
- [ ] All tasks are implemented
- [ ] Code follows project style guide (English code, i18n for UI)
- [ ] Unit tests written and passing
- [ ] No TypeScript errors
- [ ] No ESLint warnings
- [ ] Tested on mobile viewport (iPhone 16 Pro Max)
- [ ] Dark theme working correctly
- [ ] Both languages (pt-BR/en-US) working

---

**Last Updated:** January 2026
**Status:** Planning Phase
