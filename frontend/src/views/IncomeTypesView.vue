<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <h1 class="text-h5 mb-4">{{ t('incomeTypes.title') }}</h1>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-list v-if="incomeTypes.length > 0">
          <v-list-item
            v-for="incomeType in incomeTypes"
            :key="incomeType.id"
            class="mb-2"
          >
            <template #prepend>
              <v-icon>mdi-cash</v-icon>
            </template>

            <v-list-item-title>{{ incomeType.name }}</v-list-item-title>
            <v-list-item-subtitle>{{ getTypeLabel(incomeType.type) }}</v-list-item-subtitle>

            <template #append>
              <div class="d-flex ga-2">
                <IconToolTip
                  icon="mdi-pencil"
                  :tooltip="t('common.edit')"
                  @click="openEditModal(incomeType)"
                />
                <IconToolTip
                  icon="mdi-delete"
                  :tooltip="t('common.delete')"
                  color="error"
                  @click="handleDelete(incomeType.id)"
                />
              </div>
            </template>
          </v-list-item>
        </v-list>

        <v-alert v-else type="info" variant="tonal">
          {{ t('incomeTypes.noData') }}
        </v-alert>
      </v-col>
    </v-row>

    <v-btn
      color="primary"
      icon="mdi-plus"
      size="large"
      position="fixed"
      location="bottom right"
      class="mb-4 mr-4"
      @click="openAddModal"
    />

    <IncomeTypeFormModal
      :model-value="modalOpen"
      :income-type="selectedIncomeType"
      :mode="modalMode"
      @update:model-value="handleModalClose"
      @saved="handleSaved"
    />
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { IconToolTip, confirm, notify, loading } from '@wallacesw11/base-lib'
import { localStorageService } from '@/services/localStorageService'
import type { IncomeTypeModel } from '@/models'
import { IncomeType, FormMode } from '@/models'
import IncomeTypeFormModal from '@/components/IncomeTypeFormModal.vue'

const { t } = useI18n()
const incomeTypes = ref<IncomeTypeModel[]>([])
const modalOpen = ref(false)
const modalMode = ref<FormMode>(FormMode.ADD)
const selectedIncomeType = ref<IncomeTypeModel | null>(null)

const getTypeLabel = (type: IncomeType): string => {
  const labels = {
    [IncomeType.PAYCHECK]: t('incomeTypes.typePaycheck'),
    [IncomeType.HOURLY]: t('incomeTypes.typeHourly'),
    [IncomeType.EXTRA]: t('incomeTypes.typeExtra')
  }

  return labels[type]
}

const loadIncomeTypes = async (): Promise<void> => {
  loading.show(t('common.loading'))

  try {
    const data = await localStorageService.get<IncomeTypeModel>('incomeTypes')
    incomeTypes.value = Array.isArray(data) ? data : []
  } catch (error) {
    notify.error(t('messages.error'), t('incomeTypes.loadError'))
  } finally {
    loading.hide()
  }
}

const openAddModal = (): void => {
  modalMode.value = FormMode.ADD
  selectedIncomeType.value = null
  modalOpen.value = true
}

const openEditModal = (incomeType: IncomeTypeModel): void => {
  modalMode.value = FormMode.EDIT
  selectedIncomeType.value = incomeType
  modalOpen.value = true
}

const handleModalClose = (value: boolean): void => {
  modalOpen.value = value
}

const handleDelete = async (id: string): Promise<void> => {
  const confirmed = await confirm.show(
    t('common.delete'),
    t('incomeTypes.deleteConfirm')
  )

  if (!confirmed) return

  loading.show(t('incomeTypes.deleting'))

  try {
    await localStorageService.delete('incomeTypes', id)
    await loadIncomeTypes()
    notify.success(t('messages.success'), t('incomeTypes.deleteSuccess'))
  } catch (error) {
    notify.error(t('messages.error'), t('incomeTypes.deleteError'))
  } finally {
    loading.hide()
  }
}

const handleSaved = async (): Promise<void> => {
  await loadIncomeTypes()
}

onMounted(() => {
  loadIncomeTypes()
})
</script>
