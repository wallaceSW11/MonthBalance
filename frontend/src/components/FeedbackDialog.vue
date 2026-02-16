<template>
  <slot name="activator" :open="openDialog" />

  <ModalBase v-model="dialog" title="Enviar Feedback" :actions="actions" :max-width="500">
    <v-form ref="formRef" @submit.prevent="handleSubmit">
      <v-text-field
        ref="subjectFieldRef"
        v-model="form.subject"
        label="Assunto"
        :rules="[rules.required]"
        density="comfortable"
        class="mb-3"
        autofocus
      />

      <v-textarea
        v-model="form.message"
        label="Mensagem"
        :rules="[rules.required]"
        density="comfortable"
        rows="5"
        class="mb-3"
      />

      <v-rating
        v-model="form.rating"
        label="Avaliação (opcional)"
        color="yellow-darken-2"
        hover
        class="mb-3"
      />
    </v-form>
  </ModalBase>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { ModalBase, notify, loading as loadingUtil } from '@wallacesw11/base-lib';
import type { ModalAction } from '@wallacesw11/base-lib/components';
import { feedbackService } from '@/services/feedbackService';

const { t } = useI18n();

const dialog = ref(false);
const formRef = ref();
const subjectFieldRef = ref();

const form = ref({
  subject: '',
  message: '',
  rating: 0
});

const rules = {
  required: (v: string) => !!v || 'Campo obrigatório'
};

const actions = computed<ModalAction[]>(() => [
  {
    text: t('common.cancel'),
    color: 'secondary',
    variant: 'outlined',
    handler: handleCancel
  },
  {
    text: 'Enviar',
    color: 'primary',
    variant: 'elevated',
    handler: handleSubmit
  }
]);

function openDialog(): void {
  dialog.value = true;
}

function handleCancel(): void {
  dialog.value = false;
}

async function handleSubmit(): Promise<void> {
  const { valid } = await formRef.value.validate();

  if (!valid) return;

  loadingUtil.show(t('common.loading'));

  try {
    await feedbackService.create({
      subject: form.value.subject,
      message: form.value.message,
      rating: form.value.rating || undefined
    });

    notify.success('Feedback enviado', 'Obrigado pelo seu feedback!');
    dialog.value = false;
    resetForm();
  } catch (error: any) {
    notify.error(t('messages.error'), error.message);
  } finally {
    loadingUtil.hide();
  }
}

function resetForm(): void {
  form.value = {
    subject: '',
    message: '',
    rating: 0
  };

  formRef.value?.reset();
}
</script>
