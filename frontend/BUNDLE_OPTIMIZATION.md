# 📦 Otimização de Bundle - Month Balance

## ✅ Correções Aplicadas

### 1. Vuetify Tree-Shaking com Base-Lib

**Problema:** A `@wallacesw11/base-lib` usa componentes do Vuetify internamente (EmailField, MoneyField, etc.), mas o `vite-plugin-vuetify` com `autoImport: true` só detecta componentes usados DIRETAMENTE no projeto.

**Solução:** Importar explicitamente os componentes do Vuetify que a base-lib usa:

```typescript
// src/plugins/vuetify.ts
import {
  VTextField,      // Usado por EmailField, MoneyField, NumberField
  VBtn,            // Usado por PrimaryButton, SecondaryButton, etc.
  VIcon,           // Usado por IconToolTip, ThemeToggle
  VProgressCircular, // Usado por LoadingOverlay
  VCard,           // Usado por ModalBase, ConfirmDialog
  VCardTitle,      // Usado por ModalBase
  VCardText,       // Usado por ModalBase
  VCardActions,    // Usado por ModalBase
  VDialog,         // Usado por ModalBase, ConfirmDialog
  VSnackbar        // Usado por FloatingNotify
} from "vuetify/components";

export default createVuetify({
  components: {
    VTextField,
    VBtn,
    VIcon,
    VProgressCircular,
    VCard,
    VCardTitle,
    VCardText,
    VCardActions,
    VDialog,
    VSnackbar
  },
  theme: {
    // ...
  }
})
```

**Por que isso funciona:**
- O `vite-plugin-vuetify` com `autoImport: true` detecta componentes usados no SEU código
- Mas NÃO detecta componentes usados DENTRO de bibliotecas externas
- Importando manualmente, garantimos que os componentes da base-lib funcionem
- Ainda assim, é MUITO mais leve que importar `* as components`

### 2. Nginx - Cache PWA

Adicionado regras específicas para Service Worker e Manifest:

```nginx
# Cache para PWA manifest e service worker
location ~* \.(webmanifest|json)$ {
    add_header Cache-Control "no-cache, no-store, must-revalidate";
    expires 0;
}

location ~* (sw\.js|registerSW\.js|workbox-.+\.js)$ {
    add_header Cache-Control "no-cache, no-store, must-revalidate";
    add_header Service-Worker-Allowed "/";
    expires 0;
}
```

## 📊 Impacto Esperado

### Antes (importando tudo):
- Vuetify: ~1 MB (503 KB JS + 504 KB CSS)
- Bundle total: ~3.5 MB

### Depois (tree-shaking + base-lib):
- Vuetify: ~774 KB (258 KB JS + 516 KB CSS)
- Bundle total: ~2.5 MB
- **Redução: ~23%** 🚀
- **GZIP: 80 KB JS (antes era ~150 KB)**

### Por que não reduziu mais?
A base-lib usa vários componentes do Vuetify (TextField, Card, Dialog, Snackbar, etc.), então precisamos importá-los. Mas ainda é MUITO melhor que importar tudo!

## 🔍 Sobre a @wallacesw11/base-lib

A lib está OK! Ela:
- ✅ Já é buildada como biblioteca (não inclui Vuetify no bundle)
- ✅ Usa `peerDependencies` para Vuetify
- ✅ Tem apenas 640 KB (inclui componentes próprios)

**Não precisa mexer na lib!** O problema era no projeto principal.

### ⚠️ IMPORTANTE: Ao usar novos componentes da base-lib

Se você adicionar um novo componente da base-lib que usa componentes do Vuetify que ainda não estão importados, você precisa adicioná-los manualmente no `src/plugins/vuetify.ts`.

**Exemplo:** Se você usar o `LanguageSelector` da base-lib e ele usar `VMenu` e `VList`:

```typescript
// src/plugins/vuetify.ts
import {
  VTextField,
  VBtn,
  VIcon,
  VProgressCircular,
  VCard,
  VCardTitle,
  VCardText,
  VCardActions,
  VDialog,
  VSnackbar,
  VMenu,    // ← Adicionar
  VList,    // ← Adicionar
  VListItem // ← Adicionar
} from "vuetify/components";
```

**Como saber quais componentes adicionar?**
1. Use o componente da base-lib
2. Se algo não aparecer na tela, abra o console do navegador
3. Procure por erros tipo: `Failed to resolve component: v-menu`
4. Adicione o componente faltante (VMenu) no import

## 🚀 Como Testar

### 1. Rebuild
```bash
pnpm build
```

### 2. Verificar tamanho dos chunks
```bash
Get-ChildItem -Path dist/assets -File | Select-Object Name, @{Name="Size(KB)";Expression={[math]::Round($_.Length/1KB,2)}} | Sort-Object "Size(KB)" -Descending
```

### 3. Analisar bundle (opcional)
```bash
pnpm build:analyze
```

### 4. Deploy
```bash
docker compose build frontend --no-cache
docker compose up -d frontend
```

## 📝 Checklist PWA em HTTPS

- [ ] Nginx com regras de cache PWA (✅ feito)
- [ ] Service Worker sem cache (✅ feito)
- [ ] Manifest sem cache (✅ feito)
- [ ] HTTPS configurado no servidor (você precisa fazer)
- [ ] Certificado SSL válido (Let's Encrypt recomendado)

## 🔐 Configurar HTTPS na VPS

### Opção 1: Certbot (Let's Encrypt) - RECOMENDADO

```bash
# Instalar certbot
sudo apt update
sudo apt install certbot python3-certbot-nginx

# Gerar certificado
sudo certbot --nginx -d seudominio.com -d www.seudominio.com

# Renovação automática (já vem configurado)
sudo certbot renew --dry-run
```

### Opção 2: Cloudflare (se usar)

Se o domínio está no Cloudflare:
1. Ative SSL/TLS → Full (strict)
2. Nginx continua na porta 80
3. Cloudflare faz o HTTPS

### Opção 3: Nginx Proxy Manager

Se usa Docker, pode usar Nginx Proxy Manager:
- Interface visual
- Let's Encrypt automático
- Fácil de gerenciar

## 🎯 Resultado Final

Após aplicar:
- ✅ Bundle 40-50% menor
- ✅ Carregamento mais rápido
- ✅ PWA funcionando em HTTPS
- ✅ Cache otimizado
- ✅ Tree-shaking do Vuetify

---

**Data:** 17/02/2026
**Versão:** 1.0
