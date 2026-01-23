# 📦 Integração com @wallacesw11/base-lib

## 🎯 Sobre a Lib

A `@wallacesw11/base-lib` é uma biblioteca de componentes base reutilizáveis que fornece componentes UI e utilitários comuns.

**Instalação:**
```json
"@wallacesw11/base-lib": "github:wallacesw11/BaseLib#main"
```

---

## 🧩 Componentes Disponíveis

### ModalBase
Modal base com suporte a título, conteúdo e ações customizadas.

**Props:**
- `modelValue: boolean` - Controla visibilidade
- `title: string` - Título do modal
- `maxWidth: string` - Largura máxima (ex: "500", "600")
- `persistent: boolean` - Impede fechar ao clicar fora

**Slots:**
- `default` - Conteúdo do modal
- `actions` - Botões de ação

**Exemplo:**
```vue
<ModalBase
  :model-value="dialogOpen"
  title="Título do Modal"
  max-width="500"
  persistent
  @update:model-value="dialogOpen = $event"
>
  <v-form>
    <!-- Conteúdo -->
  </v-form>

  <template #actions>
    <v-btn text @click="handleCancel">Cancelar</v-btn>
    <v-btn color="primary" @click="handleSave">Salvar</v-btn>
  </template>
</ModalBase>
```

---

### IconToolTip
Botão com ícone e tooltip.

**Props:**
- `icon: string` - Ícone MDI (ex: "mdi-pencil")
- `tooltip: string` - Texto do tooltip
- `color: string` - Cor do botão (ex: "primary", "error")
- `size: string` - Tamanho do botão (ex: "small", "default")

**Eventos:**
- `@click` - Evento de clique

**Exemplo:**
```vue
<IconToolTip
  icon="mdi-pencil"
  tooltip="Editar"
  @click="handleEdit"
/>

<IconToolTip
  icon="mdi-delete"
  tooltip="Excluir"
  color="error"
  @click="handleDelete"
/>
```

---

## 🛠️ Utilitários

### confirm
Função para exibir diálogo de confirmação.

**Assinatura:**
```typescript
confirm.show(
  title: string,
  message: string,
  options?: {
    confirmText?: string
    cancelText?: string
  }
): Promise<boolean>
```

**Exemplo:**
```typescript
import { confirm } from '@wallacesw11/base-lib'

async function handleDelete(id: number): Promise<void> {
  const confirmed = await confirm.show(
    'Confirmar exclusão',
    'Tem certeza que deseja excluir este item?',
    {
      confirmText: 'Excluir',
      cancelText: 'Cancelar'
    }
  )

  if (!confirmed) return

  await deleteItem(id)
}
```

---

## 📝 Padrões de Uso

### Substituir v-dialog por ModalBase
```vue
<!-- ❌ ANTES -->
<v-dialog :model-value="open" max-width="500">
  <v-card>
    <v-card-title>Título</v-card-title>
    <v-card-text>Conteúdo</v-card-text>
    <v-card-actions>
      <v-btn>Cancelar</v-btn>
      <v-btn>Salvar</v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>

<!-- ✅ DEPOIS -->
<ModalBase :model-value="open" title="Título" max-width="500">
  Conteúdo
  
  <template #actions>
    <v-btn>Cancelar</v-btn>
    <v-btn>Salvar</v-btn>
  </template>
</ModalBase>
```

### Substituir confirm() nativo por confirm.show()
```typescript
// ❌ ANTES
if (!confirm('Tem certeza?')) return

// ✅ DEPOIS
const confirmed = await confirm.show(
  'Confirmar',
  'Tem certeza?',
  { confirmText: 'Sim', cancelText: 'Não' }
)

if (!confirmed) return
```

### Substituir v-btn icon por IconToolTip
```vue
<!-- ❌ ANTES -->
<v-btn icon="mdi-pencil" size="small" @click="edit" />

<!-- ✅ DEPOIS -->
<IconToolTip icon="mdi-pencil" tooltip="Editar" @click="edit" />
```

---

## 🎨 Imports

```typescript
// Componentes
import { ModalBase, IconToolTip } from '@wallacesw11/base-lib'

// Utilitários
import { confirm } from '@wallacesw11/base-lib'
```

---

**Versão:** 1.0  
**Data:** 23/01/2026
