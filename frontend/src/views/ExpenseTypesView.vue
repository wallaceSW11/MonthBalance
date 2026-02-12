<template>
  <div class="expense-types-view">
    <v-app-bar elevation="0">
      <v-btn icon="mdi-arrow-left" @click="goBack" />
      <v-app-bar-title>{{ t('expenseTypes.title') }}</v-app-bar-title>
    </v-app-bar>

    <div class="cards-container">
      <v-container>
        <v-card
          v-for="expenseType in expenseTypes"
          :key="expenseType.id"
          class="mb-2"
        >
          <v-card-text class="d-flex align-center">
            <v-icon class="mr-4">mdi-cart</v-icon>

            <div class="flex-grow-1">
              <div class="text-subtitle-1">{{ expenseType.name }}</div>
            </div>

            <div class="d-flex ga-2">
              <IconToolTip
                icon="mdi-pencil"
                :text="t('common.edit')"
                @click="openEditModal(expenseType)"
              />
              <IconToolTip
                icon="mdi-delete"
                :text="t('common.delete')"
                @click="handleDelete(expenseType.id)"
              />
            </div>
          </v-card-text>
        </v-card>

        <v-alert v-if="expenseTypes.length === 0" type="info" variant="tonal">
          {{ t('expenseTypes.noData') }}
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

    <ExpenseTypeFormModal
      v-model="modalOpen"
      :expense-type="selectedExpenseType"
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
import { expenseTypeService } from '@/services/expenseTypeService';
import type { ExpenseTypeModel } from '@/models';
import { FormMode } from '@/models';
import ExpenseTypeFormModal from '@/components/ExpenseTypeFormModal.vue';

const emit = defineEmits<{
  'toggle-drawer': [];
}>();

const { t } = useI18n();
const router = useRouter();

const expenseTypes = ref<ExpenseTypeModel[]>([]);
const modalOpen = ref(false);
const modalMode = ref<FormMode>(FormMode.ADD);
const selectedExpenseType = ref<ExpenseTypeModel | null>(null);

function goBack(): void {
  router.back();
}

const loadExpenseTypes = async (): Promise<void> => {
  loading.show(t('common.loading'));

  try {
    expenseTypes.value = await expenseTypeService.getAll();
  } catch (error) {
    notify.error(t('messages.error'), t('expenseTypes.loadError'));
  } finally {
    loading.hide();
  }
};

const openAddModal = (): void => {
  modalMode.value = FormMode.ADD;
  selectedExpenseType.value = null;
  modalOpen.value = true;
};

const openEditModal = (expenseType: ExpenseTypeModel): void => {
  modalMode.value = FormMode.EDIT;
  selectedExpenseType.value = expenseType;
  modalOpen.value = true;
};

const handleDelete = async (id: number): Promise<void> => {
  const confirmed = await confirm.show(
    t('common.delete'),
    t('expenseTypes.deleteConfirm')
  );

  if (!confirmed) return;

  loading.show(t('expenseTypes.deleting'));

  try {
    await expenseTypeService.remove(id);
    await loadExpenseTypes();
    notify.success(t('expenseTypes.deleted'), '');
  } catch (error) {
    notify.error(t('messages.error'), t('expenseTypes.deleteError'));
  } finally {
    loading.hide();
  }
};

const handleSaved = async (): Promise<void> => {
  await loadExpenseTypes();
};

onMounted(() => {
  loadExpenseTypes();
});
</script>

<style scoped>
.expense-types-view {
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
