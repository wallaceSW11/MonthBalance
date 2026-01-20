# 📚 Referência do Projeto - TechCambio Frontend

## 🎯 Sobre o Projeto

Plataforma completa para gestão de operações de câmbio, desenvolvida para bancos de câmbio, correspondentes cambiais e lojas de câmbio. Controle total sobre operações cambiais, desde cotação até liquidação, com foco em compliance, rastreabilidade e eficiência.

---

## 🛠️ Stack Técnico

### Core
- **Vue.js**: 2.7.14 (Options API)
- **Vuex**: 3.6.2 (State Management)
- **Vue Router**: 3.2.0
- **Vuetify**: 2.7.1 (UI Framework)

### Build & Dev
- **Vite**: 6.3.5 (Build Tool)
- **Vitest**: 3.2.4 (Test Runner)
- **ESLint**: 7.5.0

### Testing
- **Vitest**: 3.2.4
- **@vue/test-utils**: 1.3.6
- **jsdom**: 26.1.0

### HTTP & API
- **Axios**: 0.21.1
- **Socket.io-client**: 4.8.1

### Utilities
- **Moment.js**: 2.29.1
- **Lodash**: 4.17.21
- **v-mask**: 2.2.3
- **v-money**: 0.8.1

### UI & Icons
- **Material Design Icons**: 5.0.1
- **Phosphor Vue**: 1.4.2
- **ApexCharts**: 5.3.1

### Monitoring
- **Sentry**: 7.18.0
- **Mixpanel**: 2.45.0

---

## 📜 Scripts

### Desenvolvimento
```bash
npm run dev          # Servidor desenvolvimento
npm run serve        # Alias
```

### Build
```bash
npm run build        # Build produção (Linux/Mac)
npm run build:windows # Build produção (Windows)
npm run preview      # Preview do build
```

### Testes
```bash
npm run test                    # Todos os testes
npm run test:unit              # Testes unitários
npm run test:unit:watch        # Modo watch
npm run test:ui                # UI de testes
npm run test:unit:coverage     # Com coverage
npm run test:unit:threshold    # Com validação threshold
```

### Qualidade
```bash
npm run lint         # ESLint com auto-fix
```

---

## 📂 Estrutura do Projeto

```
techcambio-frontend/
├── src/
│   ├── api/              # Chamadas HTTP por módulo
│   ├── assets/           # Recursos estáticos
│   ├── components/       # Componentes Vue reutilizáveis
│   ├── constants/        # Constantes e enums
│   ├── directives/       # Diretivas Vue customizadas
│   ├── mixin/            # Mixins Vue
│   ├── model/            # Classes de modelo de dados
│   ├── plugins/          # Plugins Vue
│   ├── router/           # Configuração de rotas
│   ├── services/         # Serviços (WebSocket, etc)
│   ├── utils/            # Funções utilitárias
│   ├── views/            # Páginas/Views da aplicação
│   └── vuex/             # State management
├── tests/                # Testes E2E
├── public/               # Arquivos públicos estáticos
├── .env                  # Variáveis de ambiente
├── vite.config.js        # Configuração Vite
└── vitest.config.js      # Configuração Vitest
```

---

## 🔧 Convenções de Nomenclatura

### Arquivos
| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Componentes Vue | PascalCase.vue | `CustomerForm.vue` |
| Views | PascalCase.vue | `Dashboard.vue` |
| APIs | kebab-case-api.js | `customer-api.js` |
| Models | kebab-case-model.js | `customer-model.js` |
| Constants | kebab-case-constants.js | `general-constants.js` |
| Mixins | mixin-kebab-case.js | `mixin-authorization.js` |
| Utils | kebab-case.js | `validate-cpf-or-cnpj.js` |
| Directives | kebab-case-directive.js | `access-directive.js` |
| Testes | ComponentName.spec.js | `CustomerForm.spec.js` |

### Código
| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Variáveis | camelCase | `userName`, `isActive` |
| Funções | camelCase | `fetchUserData()` |
| Classes | PascalCase | `CustomerModel` |
| Constantes | UPPER_SNAKE_CASE | `API_BASE_URL` |
| Componentes (template) | PascalCase | `<CustomerForm />` |

---

## 🎯 Regras de Organização

### 1. Máximo 2 Níveis de Aninhamento
```
✅ CORRETO: src/components/customers/CustomerForm.vue
❌ ERRADO: src/components/customers/forms/edit/CustomerEditForm.vue
```

### 2. Testes na Pasta `__tests__`
```
✅ CORRETO:
src/components/customers/
├── CustomerForm.vue
└── __tests__/
    └── CustomerForm.spec.js
```

### 3. Imports com Alias `@/`
```javascript
// ✅ CORRETO
import CustomerApi from '@/api/customer/customer-api';

// ❌ ERRADO
import CustomerApi from '../../../api/customer/customer-api';
```

### 4. Organização por Módulo de Negócio
Agrupar arquivos relacionados por funcionalidade

---

## 🏗️ Módulos Principais

### Gestão de Clientes (Customers)
Cadastro completo: PF/PJ, documentação KYC, endereços, contas bancárias, sócios, limites, spreads, histórico.

**Componentes:** `src/components/customers/`, `src/views/customers/`  
**APIs:** `src/api/customer/`

### Operações de Câmbio (Operations)
Ciclo completo: criação, cotação em tempo real, simulador, controle de status, documentos, liquidação, rastreabilidade.

**Componentes:** `src/components/form-operation/`, `src/views/operations/`  
**APIs:** `src/api/operation/`

### Bancos de Câmbio (Exchange Banks)
Gestão de bancos parceiros: cadastro, tipos, contatos, configurações, integração APIs.

**Componentes:** `src/components/exchange-bank/`  
**APIs:** `src/api/exchangeBank/`

### Correspondentes (Correspondents)
Gestão de corbans: cadastro, agentes, comissões, spreads, relatórios, controle de acesso.

**Componentes:** `src/components/form-correspondent/`  
**APIs:** `src/api/correspondent/`

### Controle de Acesso (Access)
Usuários, perfis, permissões granulares, auditoria, sessões.

**Componentes:** `src/views/access/`  
**APIs:** `src/api/access/`, `src/api/user/`

---

## 🎓 Glossário Essencial

- **Câmbio**: Troca de moedas
- **Spread**: Margem de lucro sobre cotação
- **IOF**: Imposto sobre Operações Financeiras
- **KYC**: Know Your Customer (Conheça seu Cliente)
- **Compliance**: Conformidade com regulamentações
- **Corban**: Correspondente Cambial
- **Liquidação**: Efetivação financeira da operação
- **BACEN**: Banco Central do Brasil
- **VET**: Valor Efetivo Total da operação

---

## ⚠️ Notas Importantes

### Versões Fixas
- Vue 2.7.14 (não atualizar para Vue 3 sem planejamento)
- Vuetify 2.7.1 (compatível com Vue 2)
- Vuex 3.6.2 (compatível com Vue 2)

### Dependências Críticas
- **Axios 0.21.1**: Versão antiga, considerar atualização por segurança
- **Moment.js**: Considerar migração para Day.js (mais leve)

### Build
- Vite usado ao invés de Webpack (mais rápido)
- Scripts de cópia pós-build para skins, langs e themes

---

**Versão:** 3.0 (Unificado)
