<template>
  <v-dialog v-model="dialog" max-width="500">
    <template #activator="{ props }">
      <slot name="activator" :props="props" />
    </template>

    <v-card>
      <v-card-title>Enviar Feedback</v-card-title>

      <v-card-text>
        <v-form ref="formRef" @submit.prevent="handleSubmit">
          <v-text-field
            v-model="form.subject"
            label="Assunto"
            :rules="[rules.required]"
            density="comfortable"
            class="mb-3"
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
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn variant="text" @click="dialog = false">
          Cancelar
        </v-btn>
        <v-btn
          color="primary"
          variant="flat"
          :loading="loading"
          @click="handleSubmit"
        >
          Enviar
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { feedbackService } from '@/services/feedbackService';
import { useSnackbar } from '@/composables/useSnackbar';

const { showSuccess, showError } = useSnackbar();

const dialog = ref(false);
const loading = ref(false);
const formRef = ref();

const form = ref({
  subject: '',
  message: '',
  rating: 0
});

const rules = {
  required: (v: string) => !!v || 'Campo obrigatório'
};

async function handleSubmit() {
  const { valid } = await formRef.value.validate();
  if (!valid) return;

  loading.value = true;

  try {
    await feedbackService.create({
      subject: form.value.subject,
      message: form.value.message,
      rating: form.value.rating || undefined
    });

    showSuccess('Feedback enviado com sucesso!');
    dialog.value = false;
    resetForm();
  } catch (error: any) {
    showError(error.message);
  } finally {
    loading.value = false;
  }
}

function resetForm() {
  form.value = {
    subject: '',
    message: '',
    rating: 0
  };
  formRef.value?.reset();
}
</script>
