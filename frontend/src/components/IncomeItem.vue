<template>
  <div class="income-item">
    <IconToolTip
      icon="mdi-delete"
      tooltip="Excluir"
      color="error"
      size="x-small"
      @click="emit('delete')"
    />

    <div class="item-name">{{ name }}</div>

    <div class="item-value" @click="emit('edit')">{{ formattedValue }}</div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { IconToolTip } from '@wallacesw11/base-lib';

const props = defineProps<{
  name: string;
  type: string;
  value: number;
}>();

const emit = defineEmits<{
  edit: [];
  delete: [];
}>();

const formattedValue = computed(() => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(props.value);
});
</script>

<style scoped>
.income-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding-top: 4px;
  padding-bottom: 8px;
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.12);
}

.income-item:last-child {
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
  color: rgb(var(--v-theme-success));
  cursor: pointer;
  transition: opacity 0.2s;
  white-space: nowrap;
}

.item-value:hover {
  opacity: 0.7;
}
</style>
