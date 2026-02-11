# Guia de Internacionalização (i18n)

Este projeto está totalmente configurado com Vue I18n para suporte multi-idioma.

## 📋 Idiomas Suportados

- 🇧🇷 **Português (Brasil)** - `pt-BR` (padrão)
- 🇺🇸 **English (US)** - `en-US`

## 🎯 Como Usar

### 1. Em Templates Vue

```vue
<template>
  <!-- Tradução simples -->
  <h1>{{ $t("home.title") }}</h1>

  <!-- Tradução com parâmetros -->
  <p>{{ $t("demo.buttons.clicked", { type: "Primary" }) }}</p>

  <!-- Tradução em atributos -->
  <input :placeholder="$t('common.search')" />
</template>
```

### 2. Em Script Setup

```vue
<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n();

// Usar em funções
function showMessage() {
  alert(t("messages.success"));
}

// Com parâmetros
const message = t("demo.buttons.clicked", { type: "Primary" });
</script>
```

### 3. Usando o Composable Helper

```typescript
import { useI18nHelpers } from "@/composables";

const { t, changeLocale, currentLocale } = useI18nHelpers();

// Traduzir
const text = t("common.save");

// Trocar idioma programaticamente
changeLocale("en-US");

// Obter idioma atual
console.log(currentLocale()); // 'pt-BR' ou 'en-US'
```

## 📝 Estrutura de Traduções

Os arquivos de tradução estão em `src/locales/`:

```
locales/
├── index.ts        # Configuração e exports
├── pt-BR.ts        # Traduções em português
├── en-US.ts        # Traduções em inglês
└── _template.ts    # Template para novos idiomas
```

### Estrutura de um arquivo de tradução:

```typescript
export default {
  common: {
    appName: "Nome do App",
    save: "Salvar",
    cancel: "Cancelar",
    // ... outras traduções comuns
  },
  navigation: {
    home: "Início",
    demo: "Demo",
  },
  home: {
    title: "Título da Home",
    subtitle: "Subtítulo",
  },
  // ... outras seções
};
```

## ➕ Adicionar Novo Idioma

### Passo 1: Criar arquivo de tradução

Copie `_template.ts` e renomeie para o código do locale (ex: `es-ES.ts`):

```bash
cp src/locales/_template.ts src/locales/es-ES.ts
```

Traduza todos os textos no novo arquivo.

### Passo 2: Registrar o idioma

Em `src/locales/index.ts`:

```typescript
import ptBR from "@/locales/pt-BR";
import enUS from "@/locales/en-US";
import esES from "@/locales/es-ES"; // Novo idioma

export const messages = {
  "pt-BR": ptBR,
  "en-US": enUS,
  "es-ES": esES, // Adicionar aqui
};

export const availableLocales = [
  { code: "pt-BR", name: "Português (Brasil)", countryCode: "BR" },
  { code: "en-US", name: "English (US)", countryCode: "US" },
  { code: "es-ES", name: "Español", countryCode: "ES" }, // Adicionar aqui
] as const;

// Atualizar o tipo
export type LocaleCode = "pt-BR" | "en-US" | "es-ES";
```

### Passo 3: Atualizar a store de locale

Em `src/stores/locale.ts`, atualizar a função `detectBrowserLocale` e `loadSavedLocaleOrDetect` para incluir o novo idioma:

```typescript
function loadSavedLocaleOrDetect(): LocaleCode {
  const savedLocale = localStorage.getItem(
    LOCALE_STORAGE_KEY
  ) as LocaleCode | null;

  if (savedLocale && ["pt-BR", "en-US", "es-ES"].includes(savedLocale)) {
    return savedLocale;
  }

  return detectBrowserLocale();
}
```

## 🔄 Troca de Idioma

### Automática

O componente `LanguageSelector` da BaseLib já está configurado no header do App. Os usuários podem trocar o idioma clicando na bandeira.

### Programática

```typescript
import { useI18nHelpers } from "@/composables";

const { changeLocale } = useI18nHelpers();

// Trocar para inglês
changeLocale("en-US");

// Trocar para português
changeLocale("pt-BR");
```

## 💾 Persistência

O idioma selecionado é automaticamente:

- Salvo no `localStorage` com a chave `"locale"`
- Restaurado quando o usuário volta ao site
- Sincronizado com o componente LanguageSelector da BaseLib

## 🌐 Detecção de Idioma do Navegador

Na primeira visita, o app detecta o idioma do navegador:

- Se for português (`pt-*`), usa `pt-BR`
- Caso contrário, usa `en-US` como padrão

## 📖 Exemplos Práticos

### Notificação com tradução

```typescript
import { notify } from "@wallacesw11/base-lib";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

function saveData() {
  // ... lógica de salvar
  notify("success", t("messages.success"), "");
}
```

### Diálogo de confirmação com tradução

```typescript
import { confirm } from "@wallacesw11/base-lib";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

async function deleteItem() {
  const confirmed = await confirm(
    t("common.delete"),
    t("messages.deleteConfirm")
  );

  if (confirmed) {
    // Deletar item
  }
}
```

### Formulário com validação traduzida

```vue
<template>
  <v-form>
    <v-text-field
      v-model="name"
      :label="$t('common.name')"
      :rules="[rules.required]"
    />
    <v-btn @click="save">{{ $t("common.save") }}</v-btn>
  </v-form>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const name = ref("");

const rules = {
  required: (v: string) => !!v || t("validation.required"),
};

function save() {
  // Lógica de salvar
}
</script>
```

## 🔍 Boas Práticas

1. **Organize as chaves logicamente**: Use categorias (`common`, `navigation`, `demo`, etc.)
2. **Use nomes descritivos**: `home.title` em vez de `h1`
3. **Evite textos fixos**: Sempre use `$t()` ou `t()` para textos
4. **Parâmetros dinâmicos**: Use `{ variavel }` para interpolação
5. **Mantenha consistência**: Mesma estrutura em todos os arquivos de locale
6. **Documente**: Adicione comentários para contexto se necessário

## 🚨 Troubleshooting

### Tradução não aparece

- Verifique se a chave existe em TODOS os arquivos de locale
- Confirme que o arquivo está importado em `locales/index.ts`
- Verifique erros no console do navegador

### Idioma não troca

- Limpe o localStorage: `localStorage.clear()`
- Verifique se o locale code está correto
- Confirme que o locale está registrado em `availableLocales`

### LanguageSelector não aparece

- Verifique se está importado no App.vue
- Confirme que `availableLocales` está sendo passado como prop

## 📚 Recursos

- [Vue I18n Docs](https://vue-i18n.intlify.dev/)
- [BaseLib LanguageSelector](https://github.com/wallaceSW11/BaseLib)
