<template>
  <div class="expense-item">
    <IconToolTip
      icon="mdi-delete"
      tooltip="Excluir"
      color="error"
      size="x-small"
      @click="emit('delete')"
    />

    <div class="item-name">{{ name }}</div>

    <div v-if="!editing" class="item-value" @click="startEdit">{{ formattedValue }}</div>

    <MoneyField
      v-if="editing"
      v-model="editValue"
      class="edit-field"
      density="compact"
      hide-details
      @blur="saveEdit"
      @keyup.enter="saveEdit"
      @keyup.esc="cancelEdit"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, nextTick } from 'vue';
import { IconToolTip, MoneyField } from '@wallacesw11/base-lib';

const props = defineProps<{
  id: string;
  name: string;
  value: number;
}>();

const emit = defineEmits<{
  edit: [id: string, value: number];
  delete: [];
}>();

const editing = ref<boolean>(false);
const editValue = ref<number>(0);

const formattedValue = computed(() => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(props.value);
});

function startEdit(): void {
  editValue.value = props.value;
  editing.value = true;

  nextTick(() => {
    const input = document.querySelector('.edit-field input') as HTMLInputElement;
    if (input) input.focus();
  });
}

function saveEdit(): void {
  if (editValue.value !== props.value) {
    emit('edit', props.id, editValue.value);
  }

  editing.value = false;
}

function cancelEdit(): void {
  editing.value = false;
}
</script>

<style scoped>
.expense-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding-top: 4px;
  padding-bottom: 8px;
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.12);
}

.expense-item:last-child {
  border-bottom: none;
}

.item-name {
  flex: 1;
  font-weight: 500;
  font-size: 0.875rem;
  opacity: 0.9;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.item-value {
  font-weight: 700;
  font-size: 1rem;
  color: rgb(var(--v-theme-error));
  cursor: pointer;
  transition: opacity 0.2s;
  white-space: nowrap;
}

.item-value:hover {
  opacity: 0.7;
}

.edit-field {
  width: 140px;
}
</style>
