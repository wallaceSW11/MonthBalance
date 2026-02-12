<template>
  <div>
    <AdminAppBar title="Dashboard Admin" @menu-click="$emit('toggle-drawer')" />
    
    <v-container fluid>
      <v-row v-if="loading">
        <v-col cols="12" class="text-center">
          <v-progress-circular indeterminate color="primary" />
        </v-col>
      </v-row>

    <template v-else-if="dashboard">
      <!-- Métricas Principais -->
      <v-row>
        <v-col cols="12" sm="6" md="3">
          <v-card>
            <v-card-text>
              <div class="text-overline mb-1">Total de Usuários</div>
              <div class="text-h4">{{ dashboard.totalUsers }}</div>
            </v-card-text>
          </v-card>
        </v-col>

        <v-col cols="12" sm="6" md="3">
          <v-card>
            <v-card-text>
              <div class="text-overline mb-1">Novos Hoje</div>
              <div class="text-h4">{{ dashboard.newUsersToday }}</div>
              <div class="text-caption text-medium-emphasis">
                {{ dashboard.newUsersThisWeek }} esta semana
              </div>
            </v-card-text>
          </v-card>
        </v-col>

        <v-col cols="12" sm="6" md="3">
          <v-card>
            <v-card-text>
              <div class="text-overline mb-1">Ativos Hoje</div>
              <div class="text-h4">{{ dashboard.activeUsersToday }}</div>
              <div class="text-caption text-medium-emphasis">
                {{ dashboard.activeUsersThisWeek }} esta semana
              </div>
            </v-card-text>
          </v-card>
        </v-col>

        <v-col cols="12" sm="6" md="3">
          <v-card>
            <v-card-text>
              <div class="text-overline mb-1">Feedbacks Não Lidos</div>
              <div class="text-h4">{{ dashboard.unreadFeedbacks }}</div>
              <v-btn
                v-if="dashboard.unreadFeedbacks > 0"
                size="small"
                variant="text"
                color="primary"
                :to="ROUTES.ADMIN_FEEDBACKS"
                class="mt-2"
              >
                Ver feedbacks
              </v-btn>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>

      <!-- Usuários Recentes -->
      <v-row>
        <v-col cols="12">
          <v-card>
            <v-card-title>Usuários Recentes</v-card-title>
            <v-card-text>
              <v-table>
                <thead>
                  <tr>
                    <th>Nome</th>
                    <th>Email</th>
                    <th>Cadastro</th>
                    <th>Último Acesso</th>
                    <th>Total Logins</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="user in dashboard.recentUsers" :key="user.id">
                    <td>{{ user.name }}</td>
                    <td>{{ user.email }}</td>
                    <td>{{ formatDate(user.createdAt) }}</td>
                    <td>{{ user.lastLoginAt ? formatDate(user.lastLoginAt) : 'Nunca' }}</td>
                    <td>{{ user.totalLogins }}</td>
                    <td>
                      <v-chip
                        :color="user.isActive ? 'success' : 'default'"
                        size="small"
                      >
                        {{ user.isActive ? 'Ativo' : 'Inativo' }}
                      </v-chip>
                    </td>
                  </tr>
                </tbody>
              </v-table>

              <div class="text-center mt-4">
                <v-btn :to="ROUTES.ADMIN_USERS" variant="text" color="primary">
                  Ver todos os usuários
                </v-btn>
              </div>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </template>
  </v-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { adminService, type AdminDashboard } from '@/services/adminService';
import { ROUTES } from '@/constants/routes';
import AdminAppBar from '@/components/AdminAppBar.vue';

const dashboard = ref<AdminDashboard | null>(null);
const loading = ref(true);

onMounted(async () => {
  try {
    dashboard.value = await adminService.getDashboard();
  } catch (error) {
    console.error('Erro ao carregar dashboard:', error);
  } finally {
    loading.value = false;
  }
});

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
