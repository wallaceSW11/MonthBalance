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

## 🎯 FASE 2: CRUD Tipos de Receita ✅ CONCLUÍDO

### 2.1 View `/income-types` ✅
- [x] Criar `src/views/IncomeTypesView.vue`
- [x] Lista de tipos cadastrados (v-card com scroll)
- [x] Botão flutuante (+) centralizado para adicionar
- [x] Cada item com IconToolTip (editar/excluir)
- [x] Usar `ref()` para gerenciar estado local
- [x] Integrar com LocalStorage Service
- [x] Layout: Título fixo, scroll apenas na lista de cards
- [x] Espaçamento entre cards (mb-2)
- [x] Altura dinâmica com `calc(100dvh - 200px)`

### 2.2 Modal de Formulário ✅
- [x] Criar `src/components/IncomeTypeFormModal.vue`
- [x] ModalBase da lib
- [x] Campos: nome (v-text-field), tipo (v-select com enum)
- [x] Validação
- [x] Actions: [Salvar (primary), Cancelar (secondary)] - Botão primário sempre primeiro
- [x] **MODO ADICIONAR**: Após salvar, limpar campos, resetar validação e manter modal aberto
- [x] **MODO EDITAR**: Após salvar, fechar modal
- [x] Foco automático no primeiro campo ao abrir (apenas modo ADD)
- [x] Espaçamento entre campos (mb-4)
- [x] Mensagens de sucesso sem "com sucesso" (ex: "Receita cadastrada", "Receita atualizada")

### 2.3 Rota ✅
- [x] Adicionar rota `/income-types` no router

### 2.4 Ajustes na BaseLib ✅
- [x] ModalBase: Controle manual de fechamento (não fecha automaticamente)
- [x] IconToolTip: Corrigido evento @click com handleClick
- [x] Select: Menu aparece por baixo do modal (z-index ajustado)

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
- [ ] Actions: [Salvar (primary), Cancelar (secondary)] - Botão primário sempre primeiro
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
  - Actions: [Salvar (primary), Cancelar (secondary)] - Botão primário sempre primeiro
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

## 🎯 FASE 6: Funcionalidades de Despesa ✅ CONCLUÍDO

### 6.1 Adicionar Despesa ✅
- [x] Criar `src/components/ExpenseTypeSelectModal.vue`
  - Modal para selecionar tipo de despesa
- [x] Criar `src/components/ExpenseFormModal.vue`
  - MoneyField (valor)
  - Actions: [Salvar (primary), Cancelar (secondary)] - Botão primário sempre primeiro
  - **MODO ADICIONAR**: Após salvar, limpar campos e fechar modal
  - **MODO EDITAR**: Após salvar, fechar modal
- [x] Botão flutuante (+) abre modal de seleção
- [x] Salvar no LocalStorage
- [x] Atualizar lista

### 6.2 Editar Despesa ✅
- [x] Clicar no valor abre ExpenseFormModal em modo edição
- [x] Campo preenchido com valor atual
- [x] NÃO permitir alterar o tipo
- [x] Salvar alterações e fechar modal

### 6.3 Excluir Despesa ✅
- [x] IconToolTip com ícone delete
- [x] confirm.show() da lib
- [x] Excluir do LocalStorage
- [x] Atualizar lista

---

## 🎯 FASE 7: Navegação entre Meses ✅ CONCLUÍDO

### 7.1 Lógica de Navegação ✅
- [x] Permitir avançar até 5 meses à frente do último cadastrado
- [x] Permitir voltar em todos os meses cadastrados
- [x] Bloquear antes de janeiro/2026
- [x] Desabilitar botões quando no limite
- [x] Salvar último mês acessado no localStorage
- [x] Ao abrir app, carregar último mês acessado

### 7.2 Duplicar Mês ✅
- [x] Ao avançar para mês não cadastrado:
  - Exibir confirm.show(): "Deseja copiar os dados de [mês anterior]?"
  - Sim: copiar tipos E valores
  - Não: criar mês zerado
- [x] Opção manual no menu dropdown
  - Copiar mês atual para próximo mês
  - Validar se próximo mês já existe

### 7.3 Limpar Mês ✅
- [x] Opção no menu dropdown
- [x] confirm.show() da lib: "Limpar todos os lançamentos do mês?"
- [x] Remover TODOS os lançamentos (incomes e expenses)
- [x] Manter MonthData (ano/mês)

---

## 🎯 FASE 8: Expansão/Recolhimento ✅ CONCLUÍDO

### 8.1 Receitas ✅
- [x] Botão com ícone expand_more
- [x] Rotacionar ícone ao recolher (transform: rotate(180deg))
- [x] Recolhido: só título "RECEITAS" + linha divisória
- [x] Expandido: mostra todos os lançamentos
- [x] Salvar estado no localStorage

### 8.2 Despesas ✅
- [x] Botão com ícone expand_more
- [x] Rotacionar ícone ao recolher
- [x] Recolhido: só título "DESPESAS" + linha divisória
- [x] Expandido: mostra todos os lançamentos
- [x] Salvar estado no localStorage

---

## 🎯 FASE 9: PWA ✅ CONCLUÍDO

### 9.1 Configuração ✅
- [x] Instalar `vite-plugin-pwa`
- [x] Configurar `vite.config.ts`
- [x] Criar ícones (192x192, 512x512)
- [x] Configurar `manifest.json`
- [x] Testar instalação no iPhone 16 Pro Max

### 9.2 Service Worker ✅
- [x] Estratégia de cache
- [x] Offline fallback (opcional, pois precisa de backend)

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

### 11.1 Tela de Login ✅
- [x] Criar `src/views/LoginView.vue`
- [x] EmailField da lib
- [x] v-text-field para senha
- [x] PrimaryButton "Entrar"
- [x] Link "Esqueci minha senha"
- [x] Link "Criar conta"
- [x] Design adaptado do Stitch

### 11.2 Tela de Cadastro ✅
- [x] Criar `src/views/RegisterView.vue`
- [x] Campos: nome, email, senha, confirmar senha
- [x] Validação
- [x] Design adaptado do Stitch

### 11.3 Esqueci Senha ✅
- [x] Criar `src/views/ForgotPasswordView.vue`
- [x] EmailField
- [x] Chamar API para enviar email
- [x] Design adaptado do Stitch

### 11.4 Integração com Backend
- [ ] Implementar chamadas de API (login, register, forgot-password)
- [ ] Armazenar token JWT no localStorage
- [ ] Configurar interceptors do axios

### 11.5 Guards
- [ ] Criar `src/router/guards.ts`
- [ ] Verificar token JWT
- [ ] Redirecionar para /login se não autenticado

### 11.6 **🔐 SEGURANÇA E PRIVACIDADE** (IMPORTANTE!)
**Problema**: Dados financeiros sensíveis (salários, gastos) armazenados no backend podem ser acessados pelo administrador.

**Opções a considerar:**
1. **Criptografia End-to-End**
   - Criptografar valores no frontend antes de enviar
   - Usuário tem a chave (senha)
   - Mais seguro, mas perde dados se esquecer senha

2. **Criptografia no Backend**
   - Criptografar com chave mestra do servidor
   - Admin não vê texto plano facilmente
   - Balanceado entre segurança e recuperação

3. **Dados Locais (Offline-First)**
   - Armazenar tudo no localStorage/IndexedDB
   - Backend só para sync opcional
   - Dados nunca saem do dispositivo

4. **Transparência + Criptografia Básica**
   - Criptografia no backend
   - Política de Privacidade clara
   - Modelo usado por Nubank, Guiabolso, etc.

**Decisão**: Avaliar antes de lançar em produção. Para MVP, considerar opção 4.

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
- [ ] **ModalBase: SEMPRE definir `color` nos botões** (primary/secondary/error) - Ver 10_modal-buttons.md
- [ ] **Fórmula receita por hora**: `(hours + (minutes / 60)) * hourlyRate`

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
9. ✅ Fase 9: PWA
10. ⏳ Fase 10: Preparação para Backend
11. ⏳ Fase 11: Autenticação
12. ⏳ Fase 12: Docker

---

**Versão:** 1.0  
**Data:** 03/02/2026  
**Projeto:** Month Balance - Previsão Financeira Mensal
