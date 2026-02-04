# 🗺️ ROADMAP - Month Balance

## 📱 Sobre o Projeto

Sistema de previsão financeira mensal (receitas e despesas).
- Mobile-first (iPhone 16 Pro Max)
- PWA (instalar como app)
- Whitelabel (tema claro/escuro)
- i18n (pt-BR, en-US)
- Usuário fixo inicial: wall@wall.com / senha

---

## 🎯 FASE 1: Limpeza e Estrutura Base

### 1.1 Limpar Projeto
- [ ] Remover views demo (DemoView.vue)
- [ ] Remover testes exemplo
- [ ] Remover docs desnecessários
- [ ] Manter: stores/locale.ts, plugins (i18n, vuetify), router

### 1.2 Estrutura de Pastas
```
src/
├── models/           # Interfaces e Enums
├── services/         # LocalStorage Service (simula API)
├── components/       # Componentes reutilizáveis
├── views/            # Páginas
└── utils/            # Helpers
```

### 1.3 Models (src/models/)
- [ ] `IncomeType.ts` - Enum: PAYCHECK, HOURLY, EXTRA
- [ ] `User.ts` - Interface User
- [ ] `IncomeTypeModel.ts` - Interface (id, userId, name, type)
- [ ] `ExpenseTypeModel.ts` - Interface (id, userId, name)
- [ ] `MonthData.ts` - Interface (id, userId, year, month, lastAccessed)
- [ ] `Income.ts` - Interface (id, monthDataId, incomeTypeId, grossValue?, netValue?, hourlyRate?, hours?, minutes?, calculatedValue)
- [ ] `Expense.ts` - Interface (id, monthDataId, expenseTypeId, value)

### 1.4 LocalStorage Service
- [ ] `src/services/localStorageService.ts`
  - Métodos genéricos: get, post, put, delete
  - Simular delay de API (opcional)
  - Usuário fixo: wall@wall.com / senha

### 1.5 Whitelabel + Theme + i18n
- [ ] Configurar `public/theme.json` (cores, logo)
- [ ] Adicionar ThemeToggle da lib no menu
- [ ] Adicionar LanguageSelector da lib no menu
- [ ] Configurar locales (pt-BR, en-US)
- [ ] Traduzir todas as strings da UI

---

## 🎯 FASE 2: CRUD Tipos de Receita

### 2.1 View `/income-types`
- [ ] Criar `src/views/IncomeTypesView.vue`
- [ ] Lista de tipos cadastrados (v-list do Vuetify)
- [ ] Botão flutuante (+) para adicionar
- [ ] Cada item com IconToolTip (editar/excluir)
- [ ] Usar `ref()` para gerenciar estado local
- [ ] Integrar com LocalStorage Service

### 2.2 Modal de Formulário
- [ ] Criar `src/components/IncomeTypeFormModal.vue`
- [ ] ModalBase da lib
- [ ] Campos: nome (v-text-field), tipo (v-select com enum)
- [ ] Validação
- [ ] Actions: Salvar, Cancelar
- [ ] **MODO ADICIONAR**: Após salvar, limpar campos e manter modal aberto
- [ ] **MODO EDITAR**: Após salvar, fechar modal

### 2.3 Rota
- [ ] Adicionar rota `/income-types` no router

---

## 🎯 FASE 3: CRUD Tipos de Despesa

### 3.1 View `/expense-types`
- [ ] Criar `src/views/ExpenseTypesView.vue`
- [ ] Lista de tipos cadastrados
- [ ] Botão flutuante (+) para adicionar
- [ ] Cada item com IconToolTip (editar/excluir)
- [ ] Usar `ref()` para gerenciar estado local
- [ ] Integrar com LocalStorage Service

### 3.2 Modal de Formulário
- [ ] Criar `src/components/ExpenseTypeFormModal.vue`
- [ ] ModalBase da lib
- [ ] Campo: nome (v-text-field)
- [ ] Validação
- [ ] Actions: Salvar, Cancelar
- [ ] **MODO ADICIONAR**: Após salvar, limpar campos e manter modal aberto
- [ ] **MODO EDITAR**: Após salvar, fechar modal

### 3.3 Rota
- [ ] Adicionar rota `/expense-types` no router

---

## 🎯 FASE 4: Tela Principal - Estrutura

### 4.1 View `/` (HomeView)
- [ ] Criar `src/views/HomeView.vue`
- [ ] Header fixo com blur (sticky-blur)
  - Menu hamburguer (dropdown: Duplicar mês, Limpar mês, Tipos de Receita, Tipos de Despesa, Theme, Language)
  - Navegação mês (chevron_left, "Outubro 2023", chevron_right)
  - Resumo (Receitas, Despesas, Saldo)
- [ ] Área scrollável
  - Seção Receitas (expansível/recolhível)
  - Seção Despesas (expansível/recolhível)
- [ ] Botão flutuante (+) para adicionar despesa

### 4.2 Componentes Base
- [ ] `src/components/MonthNavigator.vue` - Header com navegação
- [ ] `src/components/MonthSummary.vue` - Cards de resumo
- [ ] `src/components/IncomeList.vue` - Lista de receitas
- [ ] `src/components/ExpenseList.vue` - Lista de despesas
- [ ] `src/components/IncomeItem.vue` - Item individual de receita
- [ ] `src/components/ExpenseItem.vue` - Item individual de despesa

### 4.3 Lógica do Mês
- [ ] Usar `ref()` para: currentYear, currentMonth, incomes, expenses
- [ ] Usar `computed()` para: totalIncome, totalExpense, balance
- [ ] Funções: loadMonth, duplicateMonth, clearMonth, navigateMonth
- [ ] Salvar último mês acessado no localStorage

---

## 🎯 FASE 5: Funcionalidades de Receita

### 5.1 Adicionar Receita
- [ ] Criar `src/components/IncomeTypeSelectModal.vue`
  - Modal para selecionar tipo de receita
  - Lista dos tipos cadastrados
- [ ] Criar `src/components/IncomeFormModal.vue`
  - Formulário dinâmico baseado no tipo:
    - **PAYCHECK**: MoneyField (bruto), MoneyField (líquido)
    - **HOURLY**: MoneyField (valor/hora), NumberField (horas), NumberField (minutos)
    - **EXTRA**: MoneyField (valor)
  - Calcular valor final
  - **MODO ADICIONAR**: Após salvar, limpar campos e manter modal aberto
  - **MODO EDITAR**: Após salvar, fechar modal
- [ ] Botão (+) na linha de Receitas abre modal de seleção
- [ ] Salvar no LocalStorage
- [ ] Atualizar lista

### 5.2 Editar Receita
- [ ] Clicar no valor abre IncomeFormModal em modo edição
- [ ] Campos preenchidos com valores atuais
- [ ] NÃO permitir alterar o tipo
- [ ] Salvar alterações e fechar modal

### 5.3 Excluir Receita
- [ ] IconToolTip com ícone delete
- [ ] confirm.show() da lib
- [ ] Excluir do LocalStorage
- [ ] Atualizar lista

---

## 🎯 FASE 6: Funcionalidades de Despesa

### 6.1 Adicionar Despesa
- [ ] Criar `src/components/ExpenseTypeSelectModal.vue`
  - Modal para selecionar tipo de despesa
- [ ] Criar `src/components/ExpenseFormModal.vue`
  - MoneyField (valor)
  - **MODO ADICIONAR**: Após salvar, limpar campos e manter modal aberto
  - **MODO EDITAR**: Após salvar, fechar modal
- [ ] Botão flutuante (+) abre modal de seleção
- [ ] Salvar no LocalStorage
- [ ] Atualizar lista

### 6.2 Editar Despesa
- [ ] Clicar no valor abre ExpenseFormModal em modo edição
- [ ] Campo preenchido com valor atual
- [ ] NÃO permitir alterar o tipo
- [ ] Salvar alterações e fechar modal

### 6.3 Excluir Despesa
- [ ] IconToolTip com ícone delete
- [ ] confirm.show() da lib
- [ ] Excluir do LocalStorage
- [ ] Atualizar lista

---

## 🎯 FASE 7: Navegação entre Meses

### 7.1 Lógica de Navegação
- [ ] Permitir avançar até 5 meses à frente do último cadastrado
- [ ] Permitir voltar em todos os meses cadastrados
- [ ] Bloquear antes de janeiro/2026
- [ ] Desabilitar botões quando no limite
- [ ] Salvar último mês acessado no localStorage
- [ ] Ao abrir app, carregar último mês acessado

### 7.2 Duplicar Mês
- [ ] Ao avançar para mês não cadastrado:
  - Exibir confirm.show(): "Deseja copiar os dados de [mês anterior]?"
  - Sim: copiar tipos E valores
  - Não: criar mês zerado
- [ ] Opção manual no menu dropdown
  - Copiar mês atual para próximo mês
  - Validar se próximo mês já existe

### 7.3 Limpar Mês
- [ ] Opção no menu dropdown
- [ ] confirm.show() da lib: "Limpar todos os lançamentos do mês?"
- [ ] Remover TODOS os lançamentos (incomes e expenses)
- [ ] Manter MonthData (ano/mês)

---

## 🎯 FASE 8: Expansão/Recolhimento

### 8.1 Receitas
- [ ] Botão com ícone expand_more
- [ ] Rotacionar ícone ao recolher (transform: rotate(180deg))
- [ ] Recolhido: só título "RECEITAS" + linha divisória
- [ ] Expandido: mostra todos os lançamentos
- [ ] Salvar estado no localStorage (opcional)

### 8.2 Despesas
- [ ] Botão com ícone expand_more
- [ ] Rotacionar ícone ao recolher
- [ ] Recolhido: só título "DESPESAS" + linha divisória
- [ ] Expandido: mostra todos os lançamentos
- [ ] Salvar estado no localStorage (opcional)

---

## 🎯 FASE 9: PWA

### 9.1 Configuração
- [ ] Instalar `vite-plugin-pwa`
- [ ] Configurar `vite.config.ts`
- [ ] Criar ícones (192x192, 512x512)
- [ ] Configurar `manifest.json`
- [ ] Testar instalação no iPhone 16 Pro Max

### 9.2 Service Worker
- [ ] Estratégia de cache
- [ ] Offline fallback (opcional, pois precisa de backend)

---

## 🎯 FASE 10: Preparação para Backend

### 10.1 API Service
- [ ] Criar `src/services/apiService.ts`
- [ ] Usar `api` da lib (@wallacesw11/base-lib)
- [ ] Configurar baseURL
- [ ] Endpoints:
  - Auth: POST /login, POST /register, POST /forgot-password
  - IncomeTypes: GET, POST, PUT, DELETE /income-types
  - ExpenseTypes: GET, POST, PUT, DELETE /expense-types
  - MonthData: GET, POST /month-data
  - Incomes: GET, POST, PUT, DELETE /incomes
  - Expenses: GET, POST, PUT, DELETE /expenses

### 10.2 Substituir LocalStorage
- [ ] Trocar chamadas de LocalStorage por API
- [ ] Manter estrutura de código
- [ ] Adicionar loading.show() / loading.hide()
- [ ] Adicionar tratamento de erros com notify.error()

---

## 🎯 FASE 11: Autenticação (Futuro)

### 11.1 Tela de Login
- [ ] Criar `src/views/LoginView.vue`
- [ ] EmailField da lib
- [ ] v-text-field para senha
- [ ] PrimaryButton "Entrar"
- [ ] Link "Esqueci minha senha"
- [ ] Link "Criar conta"

### 11.2 Tela de Cadastro
- [ ] Criar `src/views/RegisterView.vue`
- [ ] Campos: apelido, email, senha, confirmar senha
- [ ] Validação

### 11.3 Esqueci Senha
- [ ] Criar `src/views/ForgotPasswordView.vue`
- [ ] EmailField
- [ ] Chamar API para enviar email

### 11.4 Guards
- [ ] Criar `src/router/guards.ts`
- [ ] Verificar token JWT
- [ ] Redirecionar para /login se não autenticado

---

## 🎯 FASE 12: Docker (Futuro)

### 12.1 Frontend
- [ ] Criar `Dockerfile`
- [ ] Build otimizado para produção
- [ ] Nginx para servir arquivos estáticos

### 12.2 Docker Compose
- [ ] Criar `docker-compose.yml`
- [ ] Serviços: frontend, backend, postgres
- [ ] Volumes e networks
- [ ] Variáveis de ambiente

---

## 📋 Checklist de Qualidade (SEMPRE)

- [ ] Code Style seguido à risca (1_code-style.md)
- [ ] Código em inglês (UI em português)
- [ ] Early returns
- [ ] Async/await (não .then())
- [ ] Optional chaining (?.)
- [ ] Zero lógica no template
- [ ] Componentes PascalCase no template
- [ ] TypeScript strict
- [ ] Usar componentes da lib: ModalBase, MoneyField, NumberField, EmailField, IconToolTip, PrimaryButton, SecondaryButton
- [ ] Usar utils da lib: notify, confirm, loading, api
- [ ] Sem comentários no código
- [ ] Sem stores desnecessárias (usar ref/computed)

---

## 🔧 Stack Técnica

- Vue 3.5+ (Composition API, `<script setup>`)
- TypeScript (strict mode)
- Pinia (apenas para locale - já existe)
- Vuetify 3
- Vue Router
- @wallacesw11/base-lib
- Vite
- PWA (vite-plugin-pwa)

---

## 📝 Convenções

### Nomenclatura
- Variáveis booleanas: `loading`, `valid`, `active` (não `isLoading`, `isValid`)
- Componentes: PascalCase (IncomeList, ExpenseItem)
- Arquivos: PascalCase para componentes, camelCase para services
- Funções: camelCase (loadMonth, duplicateMonth)

### Estrutura de Componente
```vue
<script setup lang="ts">
// 1. Imports
// 2. Props & Emits
// 3. Refs
// 4. Computed
// 5. Functions
// 6. Lifecycle
</script>

<template>
  <!-- UI -->
</template>

<style scoped>
  /* Estilos */
</style>
```

---

## 🚀 Ordem de Execução

1. ✅ Fase 1: Limpeza e Estrutura Base
2. ✅ Fase 2: CRUD Tipos de Receita
3. ✅ Fase 3: CRUD Tipos de Despesa
4. ✅ Fase 4: Tela Principal - Estrutura
5. ✅ Fase 5: Funcionalidades de Receita
6. ✅ Fase 6: Funcionalidades de Despesa
7. ✅ Fase 7: Navegação entre Meses
8. ✅ Fase 8: Expansão/Recolhimento
9. ⏳ Fase 9: PWA
10. ⏳ Fase 10: Preparação para Backend
11. ⏳ Fase 11: Autenticação
12. ⏳ Fase 12: Docker

---

**Versão:** 1.0  
**Data:** 03/02/2026  
**Projeto:** Month Balance - Previsão Financeira Mensal
