<template>
  <v-card class="mb-2">
    <v-card-text class="d-flex align-center">
      <div class="flex-grow-1">
        <div class="item-name">{{ name }}</div>
        <div class="item-value">{{ formattedValue }}</div>
      </div>

      <div class="d-flex gap-2">
        <IconToolTip icon="mdi-pencil" tooltip="Editar" @click="emit('edit')" />
        <IconToolTip icon="mdi-delete" tooltip="Excluir" color="error" @click="emit('delete')" />
      </div>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { IconToolTip } from '@wallacesw11/base-lib';

const props = defineProps<{
  name: string;
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
.item-name {
  font-weight: 500;
  font-size: 0.875rem;
  opacity: 0.7;
}

.item-value {
  font-weight: 700;
  font-size: 1.125rem;
  color: rgb(var(--v-theme-error));
}

.gap-2 {
  gap: 8px;
}
</style>
