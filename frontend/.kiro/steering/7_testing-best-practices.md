# 🧪 Boas Práticas de Testes

## 🎯 Filosofia
> "Teste comportamento, não implementação. Teste o que o usuário vê."

### Princípios
1. Teste comportamento, não implementação
2. Teste o que o usuário vê e interage
3. Proteja contra regressões
4. Testes claros e autodocumentados
5. Testes independentes

---

## 🎭 Tipos de Teste

### 🔬 Testes Unitários (Lógica)
**Quando:** Lógica de negócio pura, transformações, computed com lógica, métodos sem DOM

```javascript
it('Deve formatar código com 6 dígitos quando receber número menor', () => {
  wrapper.setProps({ codigo: '123' });
  expect(wrapper.vm.codigoFormatado).toBe('000123');
});
```

### 🎨 Testes de Interface (DOM/Visual)
**Quando:** Elementos visíveis, interações (clicks, inputs), estilos, atributos HTML

```javascript
it('Deve exibir botão salvar visível quando formulário for válido', async () => {
  await wrapper.setData({ nome: 'João', email: 'joao@email.com' });
  
  const botao = wrapper.find('[data-testid="btn-salvar"]');
  expect(botao.isVisible()).toBe(true);
  expect(botao.attributes('disabled')).toBeUndefined();
});
```

### 🔄 Testes Híbridos (Lógica + Interface)
**Quando:** Lógica reflete na interface, fluxos completos

```javascript
it('Deve calcular total e exibir valor formatado na tela', async () => {
  await wrapper.setData({ quantidade: 5, preco: 10.50 });
  
  expect(wrapper.vm.total).toBe(52.50);
  
  const displayTotal = wrapper.find('[data-testid="display-total"]');
  expect(displayTotal.text()).toBe('R$ 52,50');
  expect(displayTotal.isVisible()).toBe(true);
});
```

---

## 🎯 Regra de Ouro: isVisible() vs exists()

### ✅ Use `isVisible()` para validar visualização
- Elemento visível para o usuário
- Não está com `display: none`, `visibility: hidden`, `opacity: 0`
- Testar show/hide

```javascript
// ✅ CORRETO
it('Deve exibir modal quando clicar no botão', async () => {
  await wrapper.find('[data-testid="btn-abrir"]').trigger('click');
  
  const modal = wrapper.findComponent(BaseModal);
  expect(modal.isVisible()).toBe(true);
});
```

### ⚠️ Use `exists()` apenas para validar presença no DOM
- Elemento foi renderizado (mesmo que oculto)
- Testar v-if (existe ou não existe)

```javascript
// ✅ CORRETO
it('Não deve renderizar botão excluir quando usuário não tiver permissão', () => {
  wrapper.setProps({ permissaoExcluir: false });
  
  const botao = wrapper.find('[data-testid="btn-excluir"]');
  expect(botao.exists()).toBe(false);
});
```

---

## 🎨 Testando pela Interface

### 🎯 Prioridade: Interface > VM

```javascript
// ✅ MELHOR - Testa pela interface
it('Deve aplicar largura de 80dvw no modal', async () => {
  await wrapper.vm.abrirModal();
  
  const modal = wrapper.findComponent(BaseModal);
  expect(modal.element.style.width).toBe('80dvw');
});

// 🎯 IDEAL - Testa ambos
it('Deve aplicar largura de 80dvw no modal', async () => {
  await wrapper.vm.abrirModal();
  
  expect(wrapper.vm.modalWidth).toBe('80dvw');
  
  const modal = wrapper.findComponent(BaseModal);
  expect(modal.element.style.width).toBe('80dvw');
});
```

---

## 🖱️ Testando Interações

### 🎯 Prioridade: Interação > Método Direto

```javascript
// ✅ MELHOR - Testa interação real
it('Deve salvar dados quando usuário clicar no botão', async () => {
  await wrapper.find('[data-testid="input-nome"]').setValue('João');
  await wrapper.find('[data-testid="btn-salvar"]').trigger('click');
  
  expect(mockApi.salvar).toHaveBeenCalledWith({ nome: 'João' });
});

// ✅ MELHOR - Testa interação completa
it('Deve validar email em tempo real quando usuário digitar', async () => {
  const input = wrapper.find('[data-testid="input-email"]');
  
  await input.setValue('joao');
  expect(wrapper.find('.error-message').isVisible()).toBe(true);
  
  await input.setValue('joao@email.com');
  expect(wrapper.find('.error-message').exists()).toBe(false);
});
```

---

## 📋 O Que Testar vs O Que NÃO Testar

### ✅ SEMPRE Teste
- Lógica de negócio (transformações, cálculos, validações)
- Comportamento de métodos (API calls, validações, side effects)
- Computed properties com lógica
- Interface e interações (visibilidade, clicks, inputs, valores exibidos)
- Classes CSS dinâmicas, atributos HTML
- Integrações críticas (API, Pinia)

### ❌ NUNCA Teste
- Framework (Vue, Vuetify)
- Props que apenas passam valores
- Reatividade do Vue
- Métodos privados
- Testes superficiais (`exists()` sozinho, `toBeDefined()` sem contexto)

---

## 🏗️ Estrutura de Testes

### Organização
```
src/components/
  ExpenseList.vue
  ExpenseList.spec.ts
```

### Anatomia
```typescript
import { describe, it, expect, beforeEach, vi } from 'vitest'
import { mount } from '@vue/test-utils'
import ExpenseList from './ExpenseList.vue'

describe('ExpenseList.vue', () => {
  let wrapper

  beforeEach(() => {
    vi.clearAllMocks()
    wrapper = mount(ExpenseList, {
      props: { /* props */ }
    })
  })

  describe('deleteExpense', () => {
    it('Deve chamar API com ID correto quando usuário confirmar exclusão', async () => {
      const mockDelete = vi.fn()
      wrapper.vm.expenseService = { delete: mockDelete }
      
      await wrapper.vm.deleteExpense(123)
      
      expect(mockDelete).toHaveBeenCalledWith(123)
    })
  })
})
```

---

## 📝 Nomenclatura

### Padrão: "Deve... quando..."
- Sempre letra maiúscula
- "Deve" para comportamentos esperados
- "Não deve" para comportamentos que não devem ocorrer
- Incluir contexto: "quando", "com", "para"
- SEMPRE incluir "quando" no nome do teste (não no describe)

```javascript
// ✅ CORRETO
describe('codigoFormatado', () => {
  it('Deve formatar código com 6 dígitos quando receber número menor', () => {});
  it('Deve retornar "-" quando código for null', () => {});
});

describe('Renderização do botão', () => {
  it('Deve desabilitar botão quando não houver identificador da pessoa', () => {});
  it('Deve habilitar botão quando houver identificador da pessoa', () => {});
});

// ❌ INCORRETO
describe('Quando não houver identificador', () => {
  it('Deve desabilitar botão', () => {});
});
```

---

## 🎨 Padrões por Tipo

### 1. Computed Properties
Teste apenas se houver lógica

```javascript
// ✅ CORRETO
describe('codigoFormatado', () => {
  it('Deve formatar código com 6 dígitos preenchendo com zeros', () => {
    wrapper.setProps({ codigo: '123' });
    expect(wrapper.vm.codigoFormatado).toBe('000123');
  });
});
```

### 2. Métodos
Prefira testar via interação

```javascript
// ✅ MELHOR
it('Deve salvar dados quando usuário clicar no botão', async () => {
  await wrapper.find('[data-testid="btn-salvar"]').trigger('click');
  expect(mockApi.salvar).toHaveBeenCalled();
});
```

### 3. Renderização Condicional
Teste visibilidade

```typescript
// ✅ CORRETO
it('Deve desabilitar botão quando não houver ID da despesa', async () => {
  await wrapper.setProps({ expenseId: null })
  
  const button = wrapper.find('[data-testid="btn-delete"]')
  expect(button.attributes('disabled')).toBeDefined()
  expect(button.isVisible()).toBe(true)
})
```

### 4. Props
Teste comportamento, não existência

```typescript
// ✅ CORRETO
it('Deve usar ID customizado no elemento', async () => {
  await wrapper.setProps({ id: 'expense-123' })
  
  const element = wrapper.find('[data-testid="expense-item"]')
  expect(element.attributes('id')).toBe('expense-123')
})
```

### 5. Componentes Filhos
Teste integração, não existência

```typescript
// ✅ CORRETO
it('Deve passar dados corretos para o modal', () => {
  const modal = wrapper.findComponent(ExpenseFormDialog)
  
  expect(modal.props()).toMatchObject({
    expenseId: 123,
    year: 2026,
    month: 1
  })
})
```

---

## 🔍 Validação de Objetos

### .toEqual() vs .toMatchObject()

**Use `.toEqual()`:** Adapters, transformações, contrato completo crítico

```javascript
it('Deve transformar filtro em modelo esperado pelo backend', () => {
  const resultado = adaptarFiltroParaBackend(filtro);
  
  expect(resultado).toEqual({
    statusRequisicao: 'ATIVO',
    tipoRequisicao: 'MATERIAL',
    pagina: 1
  });
});
```

**Use `.toMatchObject()`:** Validar apenas campos críticos, objetos grandes

```javascript
it('Deve incluir dados obrigatórios no payload', () => {
  const payload = criarPayload(dados);
  
  expect(payload).toMatchObject({
    identificador: expect.any(String),
    status: 'PENDENTE'
  });
});
```

### Reutilize objetos - Evite duplicação

```javascript
// ✅ CORRETO
it('Deve preencher itemSelecionado com dados do produto', () => {
  const produtoPadrao = {
    identificador: 999,
    codigo: 'PADRAO',
    unidade: { identificador: 1, descricao: 'UN' }
  };
  
  wrapper.vm.aoObterProdutoPadrao(produtoPadrao);
  
  expect(wrapper.vm.itemSelecionado).toMatchObject({
    produto: produtoPadrao,  // ✅ Reutiliza
    identificadorProduto: produtoPadrao.identificador
  });
});
```

### Booleanos: toBe(true/false) vs toBeTruthy/toBeFalsy

**REGRA:** Variáveis booleanas usam `.toBe(true)` ou `.toBe(false)`. NUNCA `.toBeTruthy()` ou `.toBeFalsy()`.

```javascript
// ✅ CORRETO
expect(botao.props('desabilitar')).toBe(true);
expect(botao.props('desabilitar')).toBe(false);

// ❌ INCORRETO
expect(botao.props('desabilitar')).toBeTruthy();
```

---

## 🔁 DRY: Use it.each

```javascript
// ✅ CORRETO
const casosDeFormatacao = [
  { descricao: 'Deve retornar "-" quando código for null', entrada: null, esperado: '-' },
  { descricao: 'Deve formatar "1" como "000001"', entrada: '1', esperado: '000001' }
];

it.each(casosDeFormatacao)('$descricao', async ({ entrada, esperado }) => {
  await wrapper.setProps({ codigo: entrada });
  expect(wrapper.vm.codigoFormatado).toBe(esperado);
});
```

---

## 🎭 Stubs

```javascript
// ✅ CORRETO - Array para stubs simples
wrapper = montarComponente(MeuComponente, {
  stubs: ['tabela-generica', 'item-requisicao']
});

// ✅ CORRETO - Objeto quando precisar customizar
wrapper = montarComponente(MeuComponente, {
  stubs: {
    'tabela-generica': { template: '<div><slot /></div>' }
  }
});
```

---

## ✅ Checklist

- [ ] Teste quebra se regra mudar?
- [ ] Teste sobrevive a refatorações?
- [ ] Nome do teste é claro?
- [ ] Teste tem um único motivo para falhar?
- [ ] Teste é necessário (não testa framework)?

---

## 🚫 Anti-Padrões

### 1. Testar Existência Sem Comportamento
```javascript
// ❌ ERRADO
it('Deve inicializar componente', () => {
  expect(wrapper.exists()).toBe(true);
});
```

### 2. Testar Framework
```javascript
// ❌ ERRADO
it('Deve controlar modal via v-model', async () => {
  expect(modal.props('value')).toBe(false);
  await wrapper.setData({ exibirModal: true });
  expect(modal.props('value')).toBe(true);
});
```

### 3. Testar Props Sem Comportamento
```javascript
// ❌ ERRADO
it('Deve validar valores padrão das props', () => {
  expect(wrapper.props().id).toEqual('dados-empresa');
});
```

### 4. Testar Configuração
```javascript
// ❌ ERRADO
it('Deve exibir botão com ícone mdi-domain', () => {
  expect(botao.props('icone')).toBe('mdi-domain');
});
```

---

## 📚 Resumo

**Teste:**
✅ Lógica de negócio, métodos, computed com lógica, integrações, interface, interações, visibilidade, valores exibidos

**Não teste:**
❌ Framework, implementação interna, existência sem comportamento, props sem efeito, configuração estática

**Nomenclatura:**
"Deve... quando..." com artigos e contexto completo

**Qualidade:**
`it.each` para repetitivos, `.toEqual()` para contratos, `.toMatchObject()` para parciais, `.toBe(true/false)` para booleanos, `.isVisible()` para visualização

**Prioridades:**
1. Teste pela interface
2. Teste pela lógica
3. Teste ambos (ideal)

---

**Versão:** 2.1 (Otimizada)
