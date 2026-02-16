---
inclusion: always
---

# Prettier - Regras de Formatação

## Configuração

```json
{
  "semi": true,
  "singleQuote": true,
  "trailingComma": "none",
  "printWidth": 100,
  "tabWidth": 2,
  "useTabs": false,
  "endOfLine": "lf",
  "arrowParens": "avoid",
  "vueIndentScriptAndStyle": false,
  "htmlWhitespaceSensitivity": "ignore",
  "singleAttributePerLine": false
}
```

## Regras Principais

### 1. Ponto e Vírgula Obrigatório
```typescript
// ✅ CERTO
const name = 'John';
const age = 30;

// ❌ ERRADO
const name = 'John'
const age = 30
```

### 2. Template Vue - Quebra de Linha com 3+ Props
```vue
<!-- ✅ CERTO - 3 ou mais props -->
<v-text-field
  v-model="form.name"
  :label="t('name')"
  :rules="[validateRequired]"
  variant="underlined"
/>

<!-- ✅ CERTO - 2 props ou menos -->
<v-btn color="primary" @click="save">Save</v-btn>

<!-- ❌ ERRADO - 3+ props na mesma linha -->
<v-text-field v-model="form.name" :label="t('name')" :rules="[validateRequired]" />
```

### 3. Aspas Simples
```typescript
// ✅ CERTO
const message = 'Hello World';

// ❌ ERRADO
const message = "Hello World";
```

### 4. Sem Vírgula Final
```typescript
// ✅ CERTO
const obj = {
  name: 'John',
  age: 30
};

// ❌ ERRADO
const obj = {
  name: 'John',
  age: 30,
};
```

### 5. Largura Máxima de 100 Caracteres
Linhas devem ter no máximo 100 caracteres antes de quebrar.

---

**Versão:** 1.0  
**Data:** 04/02/2026
