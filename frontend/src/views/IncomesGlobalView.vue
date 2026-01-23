<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <div class="d-flex align-center mb-4">
          <v-btn
            icon="mdi-arrow-left"
            variant="text"
            @click="goBack"
          />
          <h1 class="text-h4 ml-2">Receitas</h1>
        </div>
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
      <v-col
        v-for="income in incomes"
        :key="income.id"
        cols="12"
      >
        <v-card>
          <v-card-text>
            <div class="d-flex justify-space-between align-center">
              <div>
                <div class="text-h6">{{ income.description }}</div>
                <div class="text-caption text-grey">
                  {{ incomeTypeLabel(income.type) }}
                </div>
              </div>
              <div class="d-flex gap-2">
                <IconToolTip
                  icon="mdi-pencil"
                  tooltip="Editar"
                  @click="openEditDialog(income)"
                />
                <IconToolTip
                  icon="mdi-delete"
                  tooltip="Excluir"
                  color="error"
                  @click="handleDelete(income.id)"
                />
              </div>
            </div>
          </v-card-text>
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
import { useRouter } from 'vue-router'
import { useIncomeGlobalStore } from '@/stores/incomeGlobal'
import { IncomeTypeEnum, type Income } from '@/models/Income'
import IncomeGlobalFormDialog from '@/components/IncomeGlobalFormDialog.vue'
import { IconToolTip, confirm } from '@wallacesw11/base-lib'

const router = useRouter()
const incomeGlobalStore = useIncomeGlobalStore()

const dialogOpen = ref(false)
const selectedIncome = ref<Income | null>(null)

const incomes = computed(() => incomeGlobalStore.incomes)
const loading = computed(() => incomeGlobalStore.loading)

function goBack(): void {
  router.back()
}

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
  const confirmed = await confirm.show(
    'Confirmar exclusão',
    'Tem certeza que deseja excluir esta receita?',
    {
      confirmText: 'Excluir',
      cancelText: 'Cancelar'
    }
  )

  if (!confirmed) return

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
