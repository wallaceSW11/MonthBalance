# 🎨 Code Style - Regras Inquebraveis

## ⚠️ ESTAS REGRAS SÃO LEI - NUNCA VIOLE

---

## 🔴 Regra #1: If - Guia Completo

### Princípio Geral
Evitar `if` sempre que possível. Preferir: **Ternário** (2 caminhos), **Early Return**, **Objetos de mapeamento**, **Guard clauses**.

---

### 1. Ternário: Quando Só Tem 2 Caminhos

**Regra:** Se só tem 2 caminhos de resposta, use ternário.

```javascript
// ✅ CERTO - Ternário para 2 caminhos
const message = user.isActive ? 'Ativo' : 'Inativo'
const value = hasPermission ? processData() : null
return status === 'approved' ? 'Aprovado' : 'Pendente'
```

---

### 2. Early Return: Validações e Guard Clauses

**Regra:** Use early return para validações. SEMPRE pulo de linha antes do `if`.

```javascript
// ✅ CERTO - Early return com pulo de linha
async function processUser(user) {
  const data = await fetchData()
  
  if (!user) return null
  
  if (!user.isActive) return null
  
  const result = processData(user)
  
  return result
}

// ❌ ERRADO - Sem pulo de linha
async function processUser(user) {
  const data = await fetchData()
  if (!user) return null  // ❌ Falta linha em branco antes
  const result = processData(user)
  return result  // ❌ Falta linha em branco antes
}
```

---

### 3. If de Uma Linha: SEM Chaves

**Regra:** If de uma linha NUNCA usa `{}`. Uma operação = Um ponto e vírgula (`;`).

```javascript
// ✅ CERTO - Uma operação, sem chaves
if (!user) return null

if (isValid) this.save()

if (condicao) return valorA
else return valorB

// ❌ ERRADO - Chaves desnecessárias
if (!user) { return null }

if (isValid) { this.save() }
```

---

### 4. Múltiplas Linhas mas UMA Operação: SEM Chaves

**Regra:** Se é UMA operação (um `;`), mesmo com múltiplas linhas, NÃO use chaves.

```javascript
// ✅ CERTO - Atribuição de objeto (uma operação)
if (this.itemEdicao.identificadorUnidade)
  this.unidadeProduto = {
    identificador: this.itemEdicao.identificadorUnidade,
    descricao: this.itemEdicao.nomeUnidade,
    sigla: this.itemEdicao.siglaUnidade
  };

// ✅ CERTO - Chamada de método (uma operação)
if (this.itemSelecionado.produto && !produtoNaoCadastrado)
  this.preencherDadosProduto(this.itemSelecionado.produto);
else if (produtoNaoCadastrado)
  this.preencherDadosProdutoNaoCadastrado();

// ❌ ERRADO - Múltiplas operações SEM chaves (BUG!)
if (!usuario.ativo)
  console.log('Inativo');
  return false;  // ❌ Sempre executa! Bug grave!

// ✅ CERTO - Múltiplas operações COM chaves
if (!usuario.ativo) {
  console.log('Inativo');
  return false;
}
```

---

### 5. Pulo de Linha: SEMPRE Antes de If e Return

**Regra:** SEMPRE linha em branco antes de `if`, `return`, blocos lógicos.

**Exceções:**
- Primeiro `if` da função (não precisa linha antes)
- `return` logo após `if` de uma linha (não precisa linha entre)

```javascript
// ✅ CERTO - Pulos de linha corretos
async function saveUser(userData) {
  // Primeiro if da função (sem linha antes)
  if (!userData) return null
  
  const user = await fetchUser(userData.id)
  
  if (!user.isActive) return null
  
  const validated = validateUser(user)
  
  if (!validated) return null
  
  const result = await api.save(user)
  
  return result
}

// ❌ ERRADO - Sem pulos de linha
async function saveUser(userData) {
  if (!userData) return null
  const user = await fetchUser(userData.id)
  if (!user.isActive) return null
  const validated = validateUser(user)
  if (!validated) return null
  const result = await api.save(user)
  return result
}
```

---

### 6. Exemplo Completo: Tudo Junto

```javascript
// ✅ EXEMPLO COMPLETO - Todas as regras aplicadas
async function processOrder(order) {
  // Early return (primeiro if, sem linha antes)
  if (!order) return null
  
  // Ternário para 2 caminhos
  const status = order.isPaid ? 'paid' : 'pending'
  
  const customer = await fetchCustomer(order.customerId)
  
  // Early return com pulo de linha
  if (!customer) return null
  
  // If de uma linha sem chaves
  if (!customer.isActive) return null
  
  // Ternário inline
  const discount = customer.isPremium ? 0.1 : 0
  
  const total = calculateTotal(order, discount)
  
  // If com múltiplas linhas mas uma operação (sem chaves)
  if (order.items && order.items.length > 0)
    this.orderDetails = {
      total: total,
      discount: discount,
      status: status,
      customerId: customer.id
    };
  
  const result = await api.saveOrder(this.orderDetails)
  
  // Return com pulo de linha
  return result
}
```

---

### 7. Quando NÃO Usar If

**Use objetos de mapeamento ao invés de múltiplos if/else:**

```javascript
// ❌ ERRADO - Múltiplos if/else
let message
if (status === 'success') message = 'Sucesso'
else if (status === 'error') message = 'Erro'
else if (status === 'pending') message = 'Pendente'
else message = 'Desconhecido'

// ✅ CERTO - Objeto de mapeamento
const messages = {
  success: 'Sucesso',
  error: 'Erro',
  pending: 'Pendente',
  default: 'Desconhecido'
}
const message = messages[status] || messages.default
```

---

### Resumo: Decisão Rápida

| Situação | Solução |
|----------|---------|
| 2 caminhos simples | **Ternário** |
| Validação/Guard | **Early return** (com pulo de linha) |
| 1 operação | **Sem chaves** |
| Múltiplas operações | **Com chaves** |
| Múltiplos if/else | **Objeto de mapeamento** |
| Antes de if/return | **Pulo de linha** (exceto primeiro if) |

---

## 🔴 Regra #2: Switch/Case é Proibido
- NUNCA usar `switch/case`
- Sempre usar objetos de mapeamento

```javascript
// ✅ CERTO
const typeLabels = {
  admin: 'Administrador',
  user: 'Usuário',
  default: 'Desconhecido'
}
return typeLabels[type] || typeLabels.default
```

---

## 🔴 Regra #3: Async/Await SEMPRE
- `.then()` é PROIBIDO
- Sempre `async/await` com `try/catch`

```javascript
// ✅ CERTO
try {
  this.user = await api.getUser()
} catch (error) {
  console.error(error)
}
```

---

## 🔴 Regra #4: ZERO Lógica no Template
- Template é APENAS renderização
- Toda lógica vai para `computed` ou `methods`
- Proibido: ternários, `.map()`, `.filter()`, operações matemáticas

```vue
<!-- ❌ ERRADO -->
<div>{{ user.name ? user.name.toUpperCase() : 'Sem nome' }}</div>
<span>{{ item.indicator ? 'Indicador' : 'Corban' }}</span>

<!-- ✅ CERTO -->
<div>{{ displayName }}</div>
<span>{{ getTypeLabel(item.indicator) }}</span>

<script>
computed: {
  displayName() {
    return this.user.name ? this.user.name.toUpperCase() : 'Sem nome'
  }
},
methods: {
  getTypeLabel(isIndicator) {
    return isIndicator ? 'Indicador' : 'Corban'
  }
}
</script>
```

---

## 🔴 Regra #5: ZERO Style Inline
- NUNCA `style=""`
- Sempre classes CSS ou `<style scoped>`

---

## 🔴 Regra #6: !important é PROIBIDO
- NUNCA usar `!important`
- Solução: Aumentar especificidade, usar classes mais específicas

---

## 🔴 Regra #7: Componentes em PascalCase
```vue
<!-- ✅ CERTO -->
<ModalPadrao />
<FormUsuario />

<!-- ❌ ERRADO -->
<modal-padrao />
<form-usuario />
```

---

## 🔴 Regra #8: Máximo 200 Linhas por Componente
- Componente > 200 linhas DEVE ser quebrado
- Extrair lógica para composables ou subcomponentes

---

## 🔴 Regra #9: Comentários no Código são PROIBIDOS
- ZERO comentários no código
- Código deve ser autoexplicativo
- Solução: Nomes claros, funções pequenas

```javascript
// ❌ ERRADO
// Verifica se o usuário tem permissão
if (user.role === 'admin' || user.permissions.includes('write'))

// ✅ CERTO
const hasWritePermission = user.role === 'admin' || user.permissions.includes('write')
if (hasWritePermission)
```

---

## 🔴 Regra #10: Props Booleanas sem Valor
```vue
<!-- ✅ CERTO -->
<BaseModal hide-primary-button />
<v-btn disabled />

<!-- ❌ ERRADO -->
<BaseModal :hide-primary-button="true" />
<v-btn :disabled="true" />
```

---

## 🔴 Regra #11: v-row Sempre com no-gutters
- SEMPRE `v-row no-gutters`
- Espaçamento nos componentes filhos

```vue
<!-- ✅ CERTO -->
<v-row no-gutters>
  <v-col cols="12" md="6" class="pa-2">
    <v-text-field />
  </v-col>
</v-row>
```

---

## 🔴 Regra #12: Props Down, Events Up
- Props: pai → filho
- Events: filho → pai
- Evitar: Computed com `get/set`, Watch desnecessários, Mutação de props

```vue
<!-- ✅ CERTO -->
<input :value="value" @input="$emit('input', $event.target.value)" />

<!-- ❌ ERRADO -->
<input v-model="localValue" />
<script>
computed: {
  localValue: {
    get() { return this.value },
    set(val) { this.$emit('input', val) }
  }
}
</script>
```

---

## 🔴 Regra #13: Evitar Watch ao Máximo
- Watch deve ser evitado
- Preferir: computed properties ou usar props diretamente
- Watch aceitável: Side effects (API calls), lógica complexa sem alternativa

---

## 🔴 Regra #14: v-col Dentro de v-col é PROIBIDO
- NUNCA `v-col` dentro de `v-col`
- Estrutura: `v-row` → `v-col` → (conteúdo ou novo `v-row`)

```vue
<!-- ✅ CERTO -->
<v-row>
  <v-col cols="9">
    <v-row>
      <v-col cols="6">Conteúdo 1</v-col>
      <v-col cols="6">Conteúdo 2</v-col>
    </v-row>
  </v-col>
</v-row>
```

---

## 🔴 Regra #15: Imports de Imagens - SEMPRE import
```javascript
// ✅ CERTO
import imagemSemFoto from '@/static/img/sem-imagem.jpg';
<img :src="imagemSemFoto" />

// ❌ ERRADO
<img :src="require('@/static/img/sem-imagem.jpg')" />
```

---

## 🔴 Regra #16: Variáveis Booleanas - Formato de Pergunta SEM Auxiliar
- Sem "eh", "esta", "tem", "possui"
- Formato: pergunta direta

```javascript
// ✅ CERTO
const usuarioAtivo = true;
const produtoNaoCadastrado = false;
const dadosValidos = false;

// ❌ ERRADO
const ehUsuarioAtivo = true;
const estaAtivo = true;
const temPermissao = true;
```

---

## 🔴 Regra #17: Organização de Imports
- Imports organizados por categoria com comentários
- SEMPRE usar `@/` ao invés de `../../`

```javascript
// APIs
import CustomerApi from '@/api/customer/customer-api';

// Models
import CustomerModel from '@/model/customer-model';

// Mixins
import mixinMessage from '@/mixin/mixin-message';

// Components
import BaseModal from '@/components/comum/BaseModal.vue';

// Utils
import dateUtils from '@common/utils/date';

// Constants
import { PAGINATION_DEFAULT } from '@/constants/general-constants';
```

---

## 🔴 Regra #18: Abreviação de Componentes Comuns
```javascript
// ✅ CERTO
import IconTooltip from '@/components/comum/IconButtonWithTooltip.vue';

// ❌ ERRADO
import IconButtonWithTooltip from '@/components/comum/IconButtonWithTooltip.vue';
```

---

## 🔴 Regra #19: Formatação de Colunas de Tabelas
- Uma coluna por linha

```javascript
// ✅ CERTO
columns: [
  {
    text: 'Código',
    value: 'id',
    align: 'left',
    sortable: true
  },
  {
    text: 'Nome',
    value: 'name',
    align: 'left'
  }
]
```

---

## 📱 Responsividade

## 🔴 Regra #20: DRY no Layout
- NUNCA duplique estruturas HTML
- Use classes responsivas do Vuetify

```vue
<!-- ✅ CERTO -->
<v-card>
  <v-card-title class="text-body-2 text-md-h6">Título</v-card-title>
  <v-card-text class="pa-2 pa-md-4">Conteúdo</v-card-text>
</v-card>
```

---

## 🔴 Regra #21: Breakpoint Padrão
- `smAndDown` = `tamanhoMobileETablet` (mobile + tablet)
- `mdAndUp` = desktop (960px+)

---

## 🔴 Regra #22: Classes Responsivas - SEMPRE Use Classes do Vuetify
- NUNCA `:class` com ternários
- SEMPRE classes responsivas

```vue
<!-- ✅ CERTO -->
<div class="pr-2 pl-md-3">Conteúdo</div>
<div class="pa-2 pa-md-4">Padding responsivo</div>
<div class="d-none d-md-flex">Esconde mobile, mostra desktop</div>

<!-- ❌ ERRADO -->
<div :class="tamanhoMobileETablet ? 'pr-2' : 'pl-3'">Conteúdo</div>
```

**Sufixos:** sem sufixo (xs+), `sm` (600px+), `md` (960px+), `lg` (1264px+), `xl` (1904px+)

---

## 🔴 Regra #23: Grid Responsivo
- NUNCA `:cols` com ternários
- SEMPRE props responsivas

```vue
<!-- ✅ CERTO -->
<v-col cols="6" md="12">6 mobile, 12 desktop</v-col>
<v-col cols="12" sm="6" md="4">12 mobile, 6 tablet, 4 desktop</v-col>

<!-- ❌ ERRADO -->
<v-col :cols="tamanhoMobileETablet ? 6 : 12">Conteúdo</v-col>
```

---

## 🔴 Regra #24: Estrutura Vuetify
- `v-app` APENAS no `App.vue` (uma vez)
- `v-container` nas Views como elemento raiz
- `v-main` apenas no `App.vue` envolvendo `<router-view />`

---

## 🔴 Regra #25: Comentários no Template são PROIBIDOS
- Evite comentários no template
- Código deve ser autoexplicativo

---

## 🔴 Regra #26: Ordem de Código Novo no Final
- Novos `computed`, `methods`, `watch` vão NO FINAL do bloco
- Código legado fica no topo

---

## 🎯 Checklist Rápido

### Formatação
- [ ] Linha em branco antes de `if`, `return`
- [ ] If de uma linha sem chaves
- [ ] Imports organizados por categoria

### Estruturas
- [ ] Early return ao invés de if/else
- [ ] Objetos de mapeamento ao invés de switch/case
- [ ] Async/await ao invés de .then()

### Vue.js
- [ ] Lógica em computed/methods (não no template)
- [ ] Componentes em PascalCase
- [ ] Props Down, Events Up
- [ ] Evitar Watch
- [ ] Imports com `@/`

### Nomenclatura
- [ ] Booleanas sem "eh", "esta", "tem"
- [ ] camelCase para variáveis/funções
- [ ] UPPER_SNAKE_CASE para constantes

### CSS/Responsividade
- [ ] Sem `!important`
- [ ] Sem style inline
- [ ] Classes responsivas do Vuetify
- [ ] Props responsivas no grid
- [ ] `v-row no-gutters`

### Organização
- [ ] Componente < 200 linhas
- [ ] Métodos < 20 linhas
- [ ] Sem comentários
- [ ] Código novo no final

---

## 🚫 Proibições Absolutas

**NUNCA:**
`!important`, `var`, `.then()`, `switch/case`, lógica no template, style inline, v-if para responsividade, if/else aninhados, `require()` para imagens, `v-col` dentro de `v-col`, ternários para responsividade, comentários, variáveis com "eh/esta/tem", caminhos relativos (`../../`)

**SEMPRE:**
Optional chaining (`?.`), early return, async/await, classes responsivas, Props Down Events Up, `import` para imagens, formato de pergunta para booleanas, aliases `@/`, imports organizados, uma coluna por linha em tabelas

---

**Versão:** 3.1 (Otimizada)
**Status:** INQUEBRÁVEL 🔒
