<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 class="text-h4 mb-4">Receitas Globais</h1>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-btn
          color="primary"
          @click="openCreateDialog"
        >
          <v-icon left>mdi-plus</v-icon>
          Nova Receita
        </v-btn>
      </v-col>
    </v-row>

    <v-row v-if="loading">
      <v-col cols="12" class="text-center">
        <v-progress-circular indeterminate color="primary" />
      </v-col>
    </v-row>

    <v-row v-else>
      <v-col cols="12">
        <v-card>
          <v-list>
            <v-list-item
              v-for="income in incomes"
              :key="income.id"
            >
              <v-list-item-title>{{ income.description }}</v-list-item-title>
              <v-list-item-subtitle>
                {{ incomeTypeLabel(income.type) }}
              </v-list-item-subtitle>

              <template #append>
                <v-btn
                  icon="mdi-pencil"
                  size="small"
                  @click="openEditDialog(income)"
                />
                <v-btn
                  icon="mdi-delete"
                  size="small"
                  color="error"
                  @click="handleDelete(income.id)"
                />
              </template>
            </v-list-item>
          </v-list>
        </v-card>
      </v-col>
    </v-row>

    <IncomeGlobalFormDialog
      v-model="dialogOpen"
      :income="selectedIncome"
      @save="handleSave"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useIncomeGlobalStore } from '@/stores/incomeGlobal'
import { IncomeTypeEnum, type Income } from '@/models/Income'
import IncomeGlobalFormDialog from '@/components/IncomeGlobalFormDialog.vue'

const incomeGlobalStore = useIncomeGlobalStore()

const dialogOpen = ref(false)
const selectedIncome = ref<Income | null>(null)

const incomes = computed(() => incomeGlobalStore.incomes)
const loading = computed(() => incomeGlobalStore.loading)

function incomeTypeLabel(type: IncomeTypeEnum): string {
  return type === IncomeTypeEnum.Manual ? 'Manual' : 'Por Hora'
}

function openCreateDialog(): void {
  selectedIncome.value = null
  dialogOpen.value = true
}

function openEditDialog(income: Income): void {
  selectedIncome.value = income
  dialogOpen.value = true
}

async function handleSave(): Promise<void> {
  dialogOpen.value = false
  selectedIncome.value = null
  
  await incomeGlobalStore.loadIncomes()
}

async function handleDelete(id: number): Promise<void> {
  if (!confirm('Tem certeza que deseja excluir esta receita?')) return

  try {
    await incomeGlobalStore.deleteIncome(id)
  } catch (error: any) {
    alert(error.response?.data?.message || 'Erro ao excluir receita')
  }
}

onMounted(() => {
  incomeGlobalStore.loadIncomes()
})
</script>
