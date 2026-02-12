<template>
  <div>
    <AdminAppBar title="Feedbacks" @menu-click="$emit('toggle-drawer')" />
    
    <v-container fluid>
      <v-row>
        <v-col cols="12">
          <v-card>
          <v-card-title>
            <v-row align="center">
              <v-col cols="12" md="4">
                <v-select
                  v-model="filter"
                  :items="filterOptions"
                  label="Filtrar por"
                  variant="outlined"
                  density="compact"
                  @update:model-value="loadFeedbacks"
                />
              </v-col>
              <v-col cols="12" md="8" class="d-flex justify-end">
                <v-btn
                  color="primary"
                  prepend-icon="mdi-refresh"
                  @click="loadFeedbacks"
                  :loading="loading"
                >
                  Atualizar
                </v-btn>
              </v-col>
            </v-row>
          </v-card-title>

          <v-card-text>
            <v-progress-linear v-if="loading" indeterminate color="primary" />

            <v-list v-else>
              <v-list-item
                v-for="feedback in feedbacks"
                :key="feedback.id"
                class="mb-2"
                @click="openFeedback(feedback)"
              >
                <template #prepend>
                  <v-avatar :color="feedback.isRead ? 'grey' : 'primary'">
                    <v-icon>{{ feedback.isRead ? 'mdi-email-open' : 'mdi-email' }}</v-icon>
                  </v-avatar>
                </template>

                <v-list-item-title>
                  {{ feedback.subject }}
                  <v-chip v-if="feedback.rating" size="x-small" class="ml-2">
                    ⭐ {{ feedback.rating }}/5
                  </v-chip>
                </v-list-item-title>

                <v-list-item-subtitle>
                  {{ feedback.userName }} ({{ feedback.email }}) - {{ formatDate(feedback.createdAt) }}
                </v-list-item-subtitle>

                <template #append>
                  <v-chip
                    :color="feedback.isRead ? 'default' : 'primary'"
                    size="small"
                  >
                    {{ feedback.isRead ? 'Lido' : 'Não lido' }}
                  </v-chip>
                </template>
              </v-list-item>

              <v-list-item v-if="feedbacks.length === 0">
                <v-list-item-title class="text-center text-medium-emphasis">
                  Nenhum feedback encontrado
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Dialog de Detalhes -->
    <v-dialog v-model="dialog" max-width="600">
      <v-card v-if="selectedFeedback">
        <v-card-title>
          {{ selectedFeedback.subject }}
          <v-chip v-if="selectedFeedback.rating" size="small" class="ml-2">
            ⭐ {{ selectedFeedback.rating }}/5
          </v-chip>
        </v-card-title>

        <v-card-text>
          <div class="mb-4">
            <div class="text-caption text-medium-emphasis">De:</div>
            <div>{{ selectedFeedback.userName }} ({{ selectedFeedback.email }})</div>
          </div>

          <div class="mb-4">
            <div class="text-caption text-medium-emphasis">Data:</div>
            <div>{{ formatDate(selectedFeedback.createdAt) }}</div>
          </div>

          <div class="mb-4">
            <div class="text-caption text-medium-emphasis">Mensagem:</div>
            <div class="mt-2" style="white-space: pre-wrap;">{{ selectedFeedback.message }}</div>
          </div>

          <v-chip
            :color="selectedFeedback.isRead ? 'default' : 'primary'"
            size="small"
          >
            {{ selectedFeedback.isRead ? 'Lido' : 'Não lido' }}
          </v-chip>
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn
            v-if="!selectedFeedback.isRead"
            color="primary"
            variant="text"
            @click="markAsRead"
          >
            Marcar como lido
          </v-btn>
          <v-btn variant="text" @click="dialog = false">
            Fechar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { feedbackService, type Feedback } from '@/services/feedbackService';
import AdminAppBar from '@/components/AdminAppBar.vue';

const feedbacks = ref<Feedback[]>([]);
const loading = ref(true);
const filter = ref<'all' | 'unread' | 'read'>('all');
const dialog = ref(false);
const selectedFeedback = ref<Feedback | null>(null);

const filterOptions = [
  { title: 'Todos', value: 'all' },
  { title: 'Não lidos', value: 'unread' },
  { title: 'Lidos', value: 'read' }
];

onMounted(() => {
  loadFeedbacks();
});

async function loadFeedbacks() {
  loading.value = true;
  try {
    const isRead = filter.value === 'all' ? undefined : filter.value === 'read';
    feedbacks.value = await feedbackService.getAll(isRead);
  } catch (error) {
    console.error('Erro ao carregar feedbacks:', error);
  } finally {
    loading.value = false;
  }
}

function openFeedback(feedback: Feedback) {
  selectedFeedback.value = feedback;
  dialog.value = true;
}

async function markAsRead() {
  if (!selectedFeedback.value) return;

  try {
    await feedbackService.markAsRead(selectedFeedback.value.id);
    selectedFeedback.value.isRead = true;
    
    const index = feedbacks.value.findIndex(f => f.id === selectedFeedback.value!.id);
    if (index !== -1) {
      feedbacks.value[index].isRead = true;
    }
  } catch (error) {
    console.error('Erro ao marcar como lido:', error);
  }
}

function formatDate(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
}
</script>
