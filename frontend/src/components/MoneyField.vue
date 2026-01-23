<template>
  <input
    ref="inputRef"
    :value="displayValue"
    type="text"
    class="money-field"
    @focus="handleFocus"
    @blur="handleBlur"
    @input="handleInput"
    @keydown.enter="handleEnter"
  >
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'

const props = defineProps<{
  modelValue: number
}>()

const emit = defineEmits<{
  'update:modelValue': [value: number]
  blur: []
}>()

const inputRef = ref<HTMLInputElement>()
const focused = ref(false)
const internalValue = ref('')

const displayValue = computed(() => {
  if (focused.value) return internalValue.value

  return formatCurrency(props.modelValue)
})

watch(() => props.modelValue, (value) => {
  if (!focused.value) {
    internalValue.value = value.toFixed(2)
  }
}, { immediate: true })

function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value)
}

function handleFocus(): void {
  focused.value = true
  internalValue.value = props.modelValue.toFixed(2)
  
  setTimeout(() => {
    inputRef.value?.select()
  }, 0)
}

function handleInput(event: Event): void {
  const input = event.target as HTMLInputElement
  let value = input.value.replace(/[^\d]/g, '')

  if (value.length === 0) {
    value = '000'
  }

  const numValue = parseInt(value, 10) / 100
  
  internalValue.value = numValue.toFixed(2)
  
  const cursorPosition = input.selectionStart ?? 0
  
  input.value = internalValue.value
  input.setSelectionRange(cursorPosition, cursorPosition)
}

function handleBlur(): void {
  focused.value = false
  
  const numValue = parseFloat(internalValue.value)
  
  if (!isNaN(numValue)) {
    emit('update:modelValue', numValue)
  }
  
  emit('blur')
}

function handleEnter(): void {
  inputRef.value?.blur()
}
</script>

<style scoped>
.money-field {
  width: 100%;
  background: transparent;
  text-align: right;
  font-size: 16px;
  font-weight: 500;
  color: rgba(var(--v-theme-on-surface), 1);
  border: none;
  padding: 4px 8px;
  border-radius: 4px;
  transition: background-color 0.2s, color 0.2s;
  font-variant-numeric: tabular-nums;
  cursor: pointer;
}

.money-field:hover {
  background-color: rgba(var(--v-theme-on-surface), 0.04);
}

.money-field:focus {
  outline: none;
  background-color: rgba(var(--v-theme-on-surface), 0.08);
  color: rgb(var(--v-theme-primary));
  cursor: text;
}
</style>
