# 📋 Plano de Migração para BaseLib

## 🎯 Objetivo
Migrar todos os componentes customizados para usar os componentes da BaseLib, garantindo consistência e reduzindo código duplicado.

---

## ✅ Status Atual

### Já Implementado
- [x] FloatingNotify configurado no App.vue
- [x] LoadingOverlay configurado no App.vue
- [x] ConfirmDialog configurado no App.vue
- [x] MoneyField usado nos formulários
- [x] NumberField usado no IncomeFormDialog
- [x] useBreakpoint usado em alguns componentes
- [x] ThemeToggle e LanguageSelector disponíveis (não usados ainda)

### Uso Parcial
- [ ] confirm.show() usado no MonthNavigation (✅ correto)
- [ ] Modais ainda usando v-dialog nativo do Vuetify

---

## 🚀 Tarefas de Migração

### 1. Migrar Modais para ModalBase ⚠️ PRIORIDADE ALTA

#### 1.1 DuplicateMonthDialog.vue
**Atual:** v-dialog + v-card + v-card-actions  
**Migrar para:** ModalBase da BaseLib

**Mudanças:**
- Substituir `v-dialog` por `<ModalBase>`
- Remover `v-card`, `v-card-title`, `v-card-text`, `v-card-actions`
- Usar prop `title` do ModalBase
- Usar prop `actions` para definir botões
- Remover CSS customizado (ModalBase já é responsivo)
- Manter lógica de validação e submit

**Benefícios:**
- Responsividade automática (fullscreen mobile)
- Margens consistentes (16px)
- Animações suaves
- Menos código CSS

---

#### 1.2 ExpenseFormDialog.vue
**Atual:** v-dialog + v-card + v-card-actions  
**Migrar para:** ModalBase da BaseLib

**Mudanças:**
- Substituir `v-dialog` por `<ModalBase>`
- Remover `v-card`, `v-card-title`, `v-card-text`, `v-card-actions`
- Usar prop `title` dinâmico (computed)
- Usar prop `actions` para botões Salvar/Cancelar
- Manter MoneyField (já da BaseLib ✅)
- Remover botão X do título (ModalBase não tem por padrão)

**Benefícios:**
- Consistência visual com outros modais
- Menos código boilerplate
- Responsividade automática

---

#### 1.3 IncomeFormDialog.vue
**Atual:** v-dialog + v-card + v-card-actions  
**Migrar para:** ModalBase da BaseLib

**Mudanças:**
- Substituir `v-dialog` por `<ModalBase>`
- Remover `v-card`, `v-card-title`, `v-card-text`, `v-card-actions`
- Usar prop `title` dinâmico (computed)
- Usar prop `actions` para botões Salvar/Cancelar
- Manter MoneyField e NumberField (já da BaseLib ✅)
- Remover botão X do título

**Benefícios:**
- Consistência visual
- Menos código
- Responsividade automática

---

### 2. Migrar Botões para BaseLib 🔘

#### 2.1 DashboardView.vue
**Botões identificados:**
- Menu button (icon) → ✅ Manter v-btn (botão de ícone)
- FAB (icon, primary) → ✅ Manter v-btn (botão de ícone + posicionamento fixo)

**Ação:** Nenhuma mudança necessária (botões de ícone são exceção)

---

#### 2.2 Modais (após migração para ModalBase)
**Botões nos modais:**
- Botão "Cancelar" → Definido em `actions` do ModalBase
- Botão "Salvar/Duplicar" → Definido em `actions` do ModalBase

**Ação:** Configurar `actions` prop com cores corretas

**Exemplo:**
```typescript
const actions: ModalAction[] = [
  {
    text: t('common.cancel'),
    handler: () => isOpen.value = false,
    variant: 'text'
  },
  {
    text: t('common.save'),
    color: 'primary',
    handler: handleSubmit
  }
]
```

---

#### 2.3 MonthNavigation.vue
**Botões identificados:**
- Botões de navegação (icon) → ✅ Manter v-btn (botões de ícone)
- Botão do menu (text) → ✅ Manter v-btn (integrado com v-menu)

**Ação:** Nenhuma mudança necessária

---

#### 2.4 Resumo de Botões
**Conclusão:** Projeto já usa botões corretamente!
- Botões de ícone: v-btn (correto)
- Botões de ação: Serão migrados para ModalBase actions
- Não há botões genéricos que precisem migração para BaseLib

**Exceções válidas:**
- Botões de ícone (`icon`)
- Botões integrados com componentes Vuetify (v-menu, v-toolbar)
- FAB com posicionamento fixo

---

### 3. Ajustar Confirmações 🔔

#### 3.1 MonthNavigation.vue
**Status:** ✅ Já usa `confirm.show()` corretamente

**Melhorias:**
- Adicionar `confirmColor` e `cancelColor` nas confirmações
- Usar `persistent: true` para ações críticas (limpar mês)

**Exemplo:**
```typescript
const confirmed = await confirm.show(
  t('dashboard.clearMonth.title'),
  t('dashboard.clearMonth.message'),
  {
    confirmText: t('common.yes'),
    cancelText: t('common.no'),
    confirmColor: 'error',
    persistent: true
  }
)
```

---

#### 3.2 ExpenseList.vue e IncomeList.vue
**Status:** ✅ Não possuem funcionalidade de delete

**Observação:**
- ExpenseList: Apenas edita valores inline
- IncomeList: Apenas abre modal de edição

**Ação:** Nenhuma mudança necessária (não há confirmações)

---

### 4. Adicionar ThemeToggle e LanguageSelector 🎨

#### 4.1 Criar Componente de Settings (se não existir)
**Ou adicionar em:** NavigationDrawer.vue ou DashboardView.vue

**Componentes a usar:**
```vue
<ThemeToggle />
<LanguageSelector />
```

**Benefícios:**
- Toggle de tema visual e funcional
- Seletor de idioma com bandeiras
- Sincronização automática com stores

---

### 5. Revisar Uso de useBreakpoint 📱

#### 5.1 Componentes que já usam
- DuplicateMonthDialog.vue ✅

#### 5.2 Componentes que podem usar
- ExpenseFormDialog.vue
- IncomeFormDialog.vue
- Outros componentes com lógica mobile/desktop

**Padrão:**
```typescript
import { useBreakpoint } from '@wallacesw11/base-lib'

const { isMobile, isMobileOrTablet } = useBreakpoint()
```

---

## 📝 Ordem de Execução Recomendada

### Fase 1: Modais (1-2 horas)
1. DuplicateMonthDialog.vue
2. ExpenseFormDialog.vue
3. IncomeFormDialog.vue

### Fase 2: Confirmações (15 min)
1. Melhorar MonthNavigation.vue (adicionar cores e persistent)

### Fase 3: Botões (1 hora)
1. Identificar todos os botões
2. Migrar para PrimaryButton, SecondaryButton, etc
3. Testar interações

### Fase 4: UI/UX (30 min)
1. Adicionar ThemeToggle
2. Adicionar LanguageSelector
3. Testar responsividade

---

## ⚠️ Pontos de Atenção

### ModalBase
- **NÃO fecha automaticamente** - sempre setar `isOpen.value = false` nos handlers
- Usar `max-width` para controlar largura
- Usar `fullscreen` para mobile (via `isMobileOrTablet`)

### Botões
- Preferir BaseLib, mas não forçar em casos específicos
- Manter `v-btn icon` para botões de ícone
- Usar `text` prop ao invés de slot quando possível

### Confirmações
- Sempre usar `confirm.show()` para ações destrutivas
- Adicionar cores (`confirmColor`, `cancelColor`)
- Usar `persistent: true` para ações críticas

### Responsividade
- Usar `useBreakpoint()` ao invés de `$vuetify.display`
- Preferir classes responsivas do Vuetify
- Testar em mobile e desktop

---

## 🧪 Checklist de Testes

### Após cada migração:
- [ ] Modal abre e fecha corretamente
- [ ] Validações funcionam
- [ ] Submit funciona
- [ ] Responsivo (mobile e desktop)
- [ ] Traduções corretas (pt-BR e en-US)
- [ ] Sem erros no console
- [ ] Sem warnings de lint

### Testes gerais:
- [ ] Todos os modais funcionam
- [ ] Todas as confirmações funcionam
- [ ] Botões têm visual consistente
- [ ] ThemeToggle funciona
- [ ] LanguageSelector funciona
- [ ] App funciona em mobile
- [ ] App funciona em desktop

---

## 📚 Referências

### Documentação BaseLib
- ModalBase: Usar `actions` prop, não fecha automaticamente
- Buttons: PrimaryButton, SecondaryButton, TertiaryButton, QuartenaryButton
- confirm.show(): Retorna Promise<boolean>
- useBreakpoint(): isMobile, isMobileOrTablet

### Padrões do Projeto
- Code Style: 1_code-style.md
- Testing: 7_testing-best-practices.md
- Kiro Guide: 2_kiro-guide.md

---

## 🎯 Resultado Esperado

### Antes:
- Modais customizados com v-dialog
- Botões v-btn genéricos
- Confirmações inconsistentes
- CSS duplicado

### Depois:
- Todos os modais usando ModalBase
- Botões da BaseLib (Primary, Secondary, etc)
- Confirmações padronizadas com confirm.show()
- ThemeToggle e LanguageSelector integrados
- Menos código, mais consistência
- Responsividade automática

---

**Versão:** 1.0  
**Data:** 2026-01-20  
**Status:** 📋 Planejamento Completo
