<template>
  <v-dialog
    :model-value="modelValue"
    max-width="600"
    @update:model-value="$emit('update:modelValue', $event)"
  >
    <v-card>
      <v-card-title>Selecionar Receita</v-card-title>

      <v-card-text>
        <v-list v-if="!loading">
          <v-list-item
            v-for="income in availableIncomes"
            :key="income.id"
            @click="handleSelect(income)"
          >
            <v-list-item-title>{{ income.description }}</v-list-item-title>
            <v-list-item-subtitle>
              {{ incomeTypeLabel(income.type) }}
            </v-list-item-subtitle>
          </v-list-item>

          <v-list-item v-if="availableIncomes.length === 0">
            <v-list-item-title class="text-center text-grey">
              Nenhuma receita disponível
            </v-list-item-title>
          </v-list-item>
        </v-list>

        <div v-else class="text-center pa-4">
          <v-progress-circular indeterminate color="primary" />
        </div>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          @click="$emit('update:modelValue', false)"
        >
          Cancelar
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useIncomeGlobalStore } from '@/stores/incomeGlobal'
import { useIncomeStore } from '@/stores/income'
import { IncomeTypeEnum, type Income } from '@/models/Income'

defineProps<{
  modelValue: boolean
}>()

const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  select: [income: Income]
}>()

const incomeGlobalStore = useIncomeGlobalStore()
const incomeStore = useIncomeStore()

const loading = computed(() => incomeGlobalStore.loading)

const availableIncomes = computed(() => {
  const linkedIds = incomeStore.monthIncomes.map(mi => mi.incomeId)
  
  return incomeGlobalStore.incomes.filter(income => !linkedIds.includes(income.id))
})

function incomeTypeLabel(type: IncomeTypeEnum): string {
  return type === IncomeTypeEnum.Manual ? 'Manual' : 'Por Hora'
}

function handleSelect(income: Income): void {
  emit('select', income)
  emit('update:modelValue', false)
}
</script>
