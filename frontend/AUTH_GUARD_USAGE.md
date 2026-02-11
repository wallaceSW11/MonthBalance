# 🔐 AuthGuard - Guia de Uso

## 📋 O que foi implementado?

Sistema de segurança **iOS PWA-proof** que:

1. ✅ **Lock imediato ao minimizar** (swipe pra Home → volta → pede senha)
2. ✅ **Persistência em localStorage** (não depende de memória)
3. ✅ **Watchdog timer** (checa a cada 1s, não depende de eventos)
4. ✅ **Preparado pra WebAuthn** (Face ID / Touch ID)

---

## 🚀 Como Funciona

### 1. Lifecycle Guards (Automático)

O `authGuard` é inicializado automaticamente no router:

```typescript
// src/router/index.ts
authGuard.setupLifecycleGuards();
```

Isso ativa:
- Listener de `visibilitychange` (detecta quando app vai pra background)
- Listener de `pageshow` (detecta quando app volta)
- Watchdog (timer de 1s que checa se precisa lock)

### 2. Router Guard (Automático)

O router checa automaticamente se precisa re-autenticar:

```typescript
router.beforeEach((to, _, next) => {
  const requiresAuth = to.meta.requiresAuth !== false;
  const authenticated = authService.isAuthenticated();

  if (requiresAuth && !authenticated) {
    next(ROUTES.LOGIN);
    return;
  }

  // 🔥 NOVO: Checa se precisa re-autenticar
  if (requiresAuth && authGuard.isAuthRequired()) {
    next(ROUTES.LOGIN);
    return;
  }

  // ...
});
```

### 3. AuthStore (Automático)

A store marca o usuário como autenticado após login:

```typescript
async function login(email: string, password: string): Promise<void> {
  loading.value = true;

  try {
    user.value = await authService.login(email, password);
    authGuard.markAuthenticated(); // 🔥 NOVO
  } finally {
    loading.value = false;
  }
}
```

---

## 🧪 Testando

### Teste 1: Lock ao Minimizar (iOS PWA)

1. Abra o app no iPhone (instalado como PWA)
2. Faça login
3. Swipe pra Home (minimizar)
4. Volte pro app
5. ✅ **Deve pedir senha novamente**

### Teste 2: Lock ao Fechar Aba (Desktop)

1. Abra o app no navegador
2. Faça login
3. Feche a aba
4. Abra novamente
5. ✅ **Deve pedir senha novamente**

### Teste 3: Sem Lock se Não Minimizar

1. Abra o app
2. Faça login
3. Use normalmente (sem minimizar)
4. ✅ **Não deve pedir senha**

---

## 🔧 Configuração

### Timeout de Inatividade

Por padrão, o lock é **imediato** (0ms). Para mudar:

```typescript
// src/services/authGuard.ts
const AUTH_TIMEOUT = 0; // 0 = imediato

// Exemplos:
const AUTH_TIMEOUT = 30000; // 30 segundos
const AUTH_TIMEOUT = 120000; // 2 minutos
```

### Dev Mode (Skip Auth)

Para pular autenticação em dev:

```env
# .env.local
VITE_SKIP_AUTH=true
```

---

## 🎯 Próximos Passos (WebAuthn)

### 1. Backend

Implementar endpoints conforme `BACKEND_WEBAUTHN_SPEC.md`:

- `POST /auth/webauthn/register/challenge`
- `POST /auth/webauthn/register`
- `POST /auth/webauthn/authenticate/challenge`
- `POST /auth/webauthn/authenticate`

### 2. Frontend - Tela de Setup

Adicionar na `AccountView.vue`:

```vue
<script setup lang="ts">
import { useAuthGuard } from '@/composables';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();
const { webAuthnSupported, webAuthnEnabled, enableBiometric, disableBiometric } = useAuthGuard();

async function handleEnableBiometric() {
  if (!authStore.user) return;

  const success = await enableBiometric(authStore.user.id, authStore.user.name);

  if (success) {
    // Mostrar sucesso
  } else {
    // Mostrar erro
  }
}
</script>

<template>
  <v-card v-if="webAuthnSupported">
    <v-card-title>Autenticação Biométrica</v-card-title>
    <v-card-text>
      <v-switch
        :model-value="webAuthnEnabled"
        label="Face ID / Touch ID"
        @update:model-value="webAuthnEnabled ? disableBiometric() : handleEnableBiometric()"
      />
    </v-card-text>
  </v-card>
</template>
```

### 3. Frontend - Login com Biometria

Atualizar `LoginView.vue`:

```vue
<script setup lang="ts">
import { useAuthGuard } from '@/composables';

const { webAuthnEnabled, authenticateWithBiometric } = useAuthGuard();

async function handleBiometricLogin() {
  const success = await authenticateWithBiometric();

  if (success) {
    router.push(ROUTES.HOME);
  } else {
    // Fallback pra senha
  }
}

onMounted(() => {
  if (webAuthnEnabled.value) {
    handleBiometricLogin();
  }
});
</script>

<template>
  <v-btn
    v-if="webAuthnEnabled"
    prepend-icon="mdi-fingerprint"
    @click="handleBiometricLogin"
  >
    Usar Face ID
  </v-btn>
</template>
```

---

## 🐛 Troubleshooting

### Lock não funciona no iOS PWA

**Causa:** Eventos não disparam no iOS  
**Solução:** Já implementado! O watchdog checa a cada 1s

### Lock funciona no desktop mas não no iOS

**Causa:** `visibilitychange` não dispara  
**Solução:** Já implementado! O watchdog não depende de eventos

### WebAuthn não funciona

**Causa:** Precisa HTTPS (ou localhost)  
**Solução:** Deploy em HTTPS ou teste em localhost

### Face ID não aparece

**Causa:** Backend não implementado  
**Solução:** Implementar endpoints conforme `BACKEND_WEBAUTHN_SPEC.md`

---

## 📚 Arquivos Criados/Modificados

### Criados
- ✅ `src/services/authGuard.ts` - Lógica principal
- ✅ `src/composables/useAuthGuard.ts` - Composable
- ✅ `BACKEND_WEBAUTHN_SPEC.md` - Spec pro backend
- ✅ `AUTH_GUARD_USAGE.md` - Este arquivo

### Modificados
- ✅ `src/stores/auth.ts` - Integração com authGuard
- ✅ `src/router/index.ts` - Guard no router
- ✅ `src/composables/index.ts` - Export do composable

---

## 🎉 Pronto!

O sistema de lock já está funcionando! 

**Teste agora no iPhone PWA:**
1. Minimize o app
2. Volte pro app
3. Deve pedir senha ✅

**Próximo passo:** Implementar WebAuthn no backend pra ter Face ID 🚀

---

**Versão:** 1.0  
**Data:** 06/02/2026
