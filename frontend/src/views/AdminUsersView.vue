<template>
  <div>
    <AdminAppBar title="Usuários" @menu-click="$emit('toggle-drawer')" />
    
    <v-container fluid>
      <v-row>
        <v-col cols="12">
          <v-card>
          <v-card-title>
            <v-row>
              <v-col cols="12" md="6">
                <v-text-field
                  v-model="search"
                  label="Buscar por nome ou email"
                  prepend-inner-icon="mdi-magnify"
                  variant="outlined"
                  density="compact"
                  clearable
                  @update:model-value="handleSearch"
                />
              </v-col>
            </v-row>
          </v-card-title>

          <v-card-text>
            <v-progress-linear v-if="loading" indeterminate color="primary" />

            <v-table v-else>
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
                <tr v-for="user in users" :key="user.id">
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

            <div v-if="totalPages > 1" class="text-center mt-4">
              <v-pagination
                v-model="currentPage"
                :length="totalPages"
                @update:model-value="loadUsers"
              />
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { adminService, type UserSummary } from '@/services/adminService';
import AdminAppBar from '@/components/AdminAppBar.vue';

const users = ref<UserSummary[]>([]);
const loading = ref(true);
const search = ref('');
const currentPage = ref(1);
const pageSize = ref(20);
const totalCount = ref(0);

const totalPages = computed(() => Math.ceil(totalCount.value / pageSize.value));

let searchTimeout: ReturnType<typeof setTimeout>;

onMounted(() => {
  loadUsers();
});

async function loadUsers() {
  loading.value = true;
  try {
    const response = await adminService.getUsers(search.value || undefined, currentPage.value, pageSize.value);
    users.value = response.users;
    totalCount.value = response.totalCount;
  } catch (error) {
    console.error('Erro ao carregar usuários:', error);
  } finally {
    loading.value = false;
  }
}

function handleSearch() {
  clearTimeout(searchTimeout);
  searchTimeout = setTimeout(() => {
    currentPage.value = 1;
    loadUsers();
  }, 500);
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
