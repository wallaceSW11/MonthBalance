---
inclusion: always
priority: normal
---
# @wallacesw11/base-lib - AI Reference

## Install
```bash
pnpm add github:wallacesw11/BaseLib#main
```

## Setup (ORDER CRITICAL)
```typescript
// main.ts
import { createPinia } from 'pinia'
import { createVuetify } from 'vuetify'
import { createI18n } from 'vue-i18n'
import { setupLib, defaultMessages, defaultLocale } from '@wallacesw11/base-lib'
import '@wallacesw11/base-lib/style.css'

app.use(createPinia())
app.use(createVuetify({ theme: { defaultTheme: 'light' } }))
app.use(createI18n({ legacy: false, locale: defaultLocale, messages: defaultMessages }))
setupLib(app) // MUST BE LAST
```

```vue
<!-- App.vue - Required -->
<template>
  <v-app>
    <router-view />
    <FloatingNotify ref="notifyRef" />
    <LoadingOverlay ref="loadingRef" />
    <ConfirmDialog ref="confirmRef" />
  </v-app>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { FloatingNotify, LoadingOverlay, ConfirmDialog } from '@wallacesw11/base-lib'
import { useNotifyStore, useLoadingStore, useConfirmStore } from '@wallacesw11/base-lib/stores'

const notifyRef = ref()
const loadingRef = ref()
const confirmRef = ref()

onMounted(() => {
  useNotifyStore().setNotifyRef(notifyRef.value)
  useLoadingStore().setLoadingRef(loadingRef.value)
  useConfirmStore().setConfirmRef(confirmRef.value)
})
</script>
```

## Components

### Buttons
```typescript
import { PrimaryButton, SecondaryButton, TertiaryButton, QuartenaryButton } from '@wallacesw11/base-lib'
```
Props: `text`, `prependIcon`, `appendIcon`, `disabled`, `loading`, `block`, `size`, `color`, `variant`

### ModalBase
```vue
<script setup lang="ts">
import { ModalBase } from '@wallacesw11/base-lib'
import type { ModalAction } from '@wallacesw11/base-lib/components'

const open = ref(false)
const actions: ModalAction[] = [
  { text: 'Save', color: 'primary', handler: () => { save(); open.value = false } },
  { text: 'Cancel', handler: () => open.value = false }
]
</script>

<template>
  <ModalBase v-model="open" title="Title" :actions="actions" :max-width="600">
    Content
  </ModalBase>
</template>
```
**CRITICAL**: Modal does NOT auto-close. Set `open.value = false` in handlers.

Props: `modelValue`, `title`, `actions`, `maxWidth`, `persistent`, `fullscreen`

### IconToolTip
```vue
<IconToolTip icon="mdi-pencil" tooltip="Edit" @click="edit" />
<IconToolTip icon="mdi-delete" tooltip="Delete" color="error" @click="del" />
```
Props: `icon`, `tooltip`, `color`, `size`

### Input Fields

#### MoneyField
```vue
<MoneyField v-model="amount" label="Amount" currency="BRL" locale="pt-BR" />
```
Props: `modelValue`, `label`, `rules`, `disabled`, `hint`, `persistentHint`, `currency` (BRL/USD/EUR/GBP), `locale`
Features: Auto-format, mask visible, negative support

#### EmailField
```vue
<EmailField v-model="email" label="Email" required @valid="v => isValid = v" />
```
Props: `modelValue`, `label`, `rules`, `disabled`, `hint`, `persistentHint`, `required`, `validateOnBlur`
Events: `@valid` (boolean)

#### NumberField
```vue
<NumberField v-model="qty" label="Quantity" :decimal-places="0" />
<NumberField v-model="price" label="Price" :decimal-places="2" />
```
Props: `modelValue`, `label`, `rules`, `disabled`, `hint`, `persistentHint`, `decimalPlaces` (default: 0), `locale`, `allowNegative`

### Theme
```vue
<ThemeToggle />
```
```typescript
import { useThemeStore } from '@wallacesw11/base-lib/stores'
const themeStore = useThemeStore()
themeStore.toggleTheme()
themeStore.setTheme('dark')
```

### Language
```vue
<LanguageSelector />
```
Supported: pt-BR (default), en-US

## Utils

### notify
```typescript
import { notify } from '@wallacesw11/base-lib'

notify.success('Title', 'Message')
notify.error('Title', 'Message')
notify.warning('Title', 'Message')
notify.info('Title', 'Message')
```

### confirm
```typescript
import { confirm } from '@wallacesw11/base-lib'

// Default (Yes/No)
const result = await confirm.show('Title', 'Message')
if (result) { /* Yes */ }

// Custom
const deleted = await confirm.show('Delete', 'Cannot undo', {
  confirmText: 'Delete',
  cancelText: 'Cancel',
  confirmColor: 'error'
})

// OK dialog
await confirm.show('Info', 'Done!', { confirmText: 'OK', cancelText: 'Close' })
```
Options: `persistent`, `confirmText`, `cancelText`, `confirmColor`, `cancelColor`

### loading
```typescript
import { loading } from '@wallacesw11/base-lib'

loading.show('Loading...')
await asyncOp()
loading.hide()
```

### api
```typescript
import { api, configureApi } from '@wallacesw11/base-lib'

// Config (optional)
configureApi({
  baseURL: 'https://api.example.com',
  authTokenKey: 'auth_token',
  onUnauthorized: () => router.push('/login')
})

// Use
const users = await api.get('/users')
await api.post('/users', { name: 'John' })
await api.put('/users/1', { name: 'Jane' })
await api.delete('/users/1')
```
Auth: Auto-reads token from `localStorage.getItem('auth_token')`

## Composables

### useBreakpoint
```typescript
import { useBreakpoint } from '@wallacesw11/base-lib'

const { isMobile, isMobileOrTablet } = useBreakpoint()
// isMobile.value -> xs (< 600px)
// isMobileOrTablet.value -> xs/sm (< 960px)
```

## All Imports
```typescript
// Components
import {
  PrimaryButton, SecondaryButton, TertiaryButton, QuartenaryButton, BaseButton,
  IconToolTip, ModalBase, ConfirmDialog, FloatingNotify, LoadingOverlay,
  ThemeToggle, LanguageSelector, MoneyField, EmailField, NumberField
} from '@wallacesw11/base-lib'

// Utils
import { notify, loading, confirm, api, configureApi } from '@wallacesw11/base-lib'

// Stores
import { useThemeStore, useNotifyStore, useLoadingStore, useConfirmStore } from '@wallacesw11/base-lib/stores'

// Composables
import { useBreakpoint, useGlobals, useLocale, useThemeSync } from '@wallacesw11/base-lib/composables'

// Locales
import { defaultMessages, defaultLocale, defaultAvailableLocales } from '@wallacesw11/base-lib/locales'

// Types
import type { ModalAction } from '@wallacesw11/base-lib/components'
import type { NotifyType, LoadingComponentRef, ConfirmComponentRef, ApiConfig } from '@wallacesw11/base-lib/utils'
```

## Common Patterns

### Form + validation
```vue
<script setup lang="ts">
import { ref } from 'vue'
import { MoneyField, EmailField, NumberField } from '@wallacesw11/base-lib'
import { notify, loading, api } from '@wallacesw11/base-lib'

const form = ref({ email: '', amount: 0, quantity: 0 })
const isEmailValid = ref(false)

async function submit() {
  if (!isEmailValid.value) {
    notify.error('Invalid', 'Check email')
    return
  }
  
  loading.show('Saving...')
  
  try {
    await api.post('/submit', form.value)
    notify.success('Success', 'Saved!')
  } catch (error) {
    notify.error('Error', 'Failed to save')
  } finally {
    loading.hide()
  }
}
</script>

<template>
  <v-form @submit.prevent="submit">
    <EmailField v-model="form.email" required @valid="v => isEmailValid = v" />
    <MoneyField v-model="form.amount" label="Amount" />
    <NumberField v-model="form.quantity" label="Quantity" :decimal-places="0" />
    <PrimaryButton text="Submit" type="submit" />
  </v-form>
</template>
```

### Delete confirm
```typescript
import { confirm, notify, loading, api } from '@wallacesw11/base-lib'

async function deleteItem(id: number) {
  const confirmed = await confirm.show('Delete', 'Cannot undo', {
    confirmText: 'Delete',
    cancelText: 'Cancel',
    confirmColor: 'error'
  })
  
  if (!confirmed) return
  
  loading.show('Deleting...')
  
  try {
    await api.delete(`/items/${id}`)
    notify.success('Deleted', 'Item removed')
  } catch (error) {
    notify.error('Error', 'Failed to delete')
  } finally {
    loading.hide()
  }
}
```

## Migration Patterns

### v-dialog → ModalBase
```vue
<!-- BEFORE -->
<v-dialog :model-value="open" max-width="500">
  <v-card>
    <v-card-title>Title</v-card-title>
    <v-card-text>Content</v-card-text>
    <v-card-actions>
      <v-btn>Cancel</v-btn>
      <v-btn>Save</v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>

<!-- AFTER -->
<ModalBase v-model="open" title="Title" :actions="actions" max-width="500">
  Content
</ModalBase>
```

### confirm() → confirm.show()
```typescript
// BEFORE
if (!confirm('Sure?')) return

// AFTER
const confirmed = await confirm.show('Confirm', 'Sure?', { confirmText: 'Yes', cancelText: 'No' })
if (!confirmed) return
```

### v-btn icon → IconToolTip
```vue
<!-- BEFORE -->
<v-btn icon="mdi-pencil" size="small" @click="edit" />

<!-- AFTER -->
<IconToolTip icon="mdi-pencil" tooltip="Edit" @click="edit" />
```

## Troubleshooting
| Issue | Fix |
|-------|-----|
| `Failed to resolve v-card-title` | Register Vuetify BEFORE setupLib() |
| Notifications not showing | Add FloatingNotify to App.vue + register ref |
| Theme not loading | Create public/theme.json |
| API auth missing | `localStorage.setItem('auth_token', token)` |
| Modal not closing | Set `open.value = false` in handler |
| Input mask disappearing | Use MoneyField/NumberField instead of v-text-field |

## Update
```bash
pnpm update @wallacesw11/base-lib
# or force
pnpm add github:wallacesw11/BaseLib#main --force
```

---
**v2.0** | 31/01/2026 | AI-optimized