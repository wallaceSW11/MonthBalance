<template>
  <div class="income-types-view">
    <v-app-bar elevation="0">
      <v-btn icon="mdi-arrow-left" @click="goBack" />
      <v-app-bar-title>{{ t('incomeTypes.title') }}</v-app-bar-title>
    </v-app-bar>

    <div class="cards-container">
      <v-container>
        <v-card
          v-for="incomeType in incomeTypes"
          :key="incomeType.id"
          class="mb-2"
        >
          <v-card-text class="d-flex align-center">
            <v-icon class="mr-4">mdi-cash</v-icon>

            <div class="flex-grow-1">
              <div class="text-subtitle-1">{{ incomeType.name }}</div>
              <div class="text-caption text-medium-emphasis">{{ getTypeLabel(incomeType.type) }}</div>
            </div>

            <div class="d-flex ga-2">
              <IconToolTip
                icon="mdi-pencil"
                :text="t('common.edit')"
                as-button
                @click="openEditModal(incomeType)"
              />
              <IconToolTip
                icon="mdi-delete"
                :text="t('common.delete')"
                as-button
                @click="handleDelete(incomeType.id)"
              />
            </div>
          </v-card-text>
        </v-card>

        <v-alert v-if="incomeTypes.length === 0" type="info" variant="tonal">
          {{ t('incomeTypes.noData') }}
        </v-alert>
      </v-container>
    </div>

    <v-btn
      color="primary"
      icon="mdi-plus"
      size="large"
      position="fixed"
      location="bottom center"
      class="add-button"
      @click="openAddModal"
    />

    <IncomeTypeFormModal
      v-model="modalOpen"
      :income-type="selectedIncomeType"
      :mode="modalMode"
      @saved="handleSaved"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { IconToolTip, confirm, notify, loading } from '@wallacesw11/base-lib';
import { incomeTypeService } from '@/services/incomeTypeService';
import type { IncomeTypeModel } from '@/models';
import { IncomeType, FormMode } from '@/models';
import IncomeTypeFormModal from '@/components/IncomeTypeFormModal.vue';

const emit = defineEmits<{
  toggleDrawer: []
}>();

const { t } = useI18n();
const router = useRouter();

const incomeTypes = ref<IncomeTypeModel[]>([]);
const modalOpen = ref(false);
const modalMode = ref<FormMode>(FormMode.ADD);
const selectedIncomeType = ref<IncomeTypeModel | null>(null);

function toggleDrawer(): void {
  emit('toggleDrawer');
}

function goBack(): void {
  router.back();
}

const getTypeLabel = (type: IncomeType): string => {
  const labels = {
    [IncomeType.PAYCHECK]: t('incomeTypes.typePaycheck'),
    [IncomeType.HOURLY]: t('incomeTypes.typeHourly'),
    [IncomeType.EXTRA]: t('incomeTypes.typeExtra')
  }

  return labels[type]
}

const loadIncomeTypes = async (): Promise<void> => {
  loading.show(t('common.loading'));

  try {
    incomeTypes.value = await incomeTypeService.getAll();
  } catch (error) {
    notify.error(t('messages.error'), t('incomeTypes.loadError'));
  } finally {
    loading.hide();
  }
};

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

const handleDelete = async (id: number): Promise<void> => {
  const confirmed = await confirm.show(
    t('common.delete'),
    t('incomeTypes.deleteConfirm')
  );

  if (!confirmed) return;

  loading.show(t('incomeTypes.deleting'));

  try {
    await incomeTypeService.remove(id);
    await loadIncomeTypes();
    notify.success(t('incomeTypes.deleted'), '');
  } catch (error) {
    notify.error(t('messages.error'), t('incomeTypes.deleteError'));
  } finally {
    loading.hide();
  }
};

const handleSaved = async (): Promise<void> => {
  await loadIncomeTypes()
}

onMounted(() => {
  loadIncomeTypes()
})
</script>

<style scoped>
.income-types-view {
  height: 100vh;
  overflow: hidden;
}

.cards-container {
  height: calc(100dvh - 120px);
  overflow-y: auto;
  padding-bottom: 100px;
}

.add-button {
  margin-bottom: 16px;
}
</style>
