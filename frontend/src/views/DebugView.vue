<template>
  <v-container fluid>
    <v-card>
      <v-card-title>Debug - LocalStorage</v-card-title>

      <v-card-text>
        <v-btn
          color="primary"
          class="mb-4"
          @click="loadData"
        >
          Recarregar Dados
        </v-btn>

        <v-expansion-panels>
          <v-expansion-panel
            v-for="(value, key) in storageData"
            :key="key"
          >
            <v-expansion-panel-title>
              {{ key }}
            </v-expansion-panel-title>

            <v-expansion-panel-text>
              <pre class="debug-content">{{ formatValue(value) }}</pre>

              <v-btn
                size="small"
                color="error"
                class="mt-2"
                @click="deleteKey(key)"
              >
                Deletar
              </v-btn>
            </v-expansion-panel-text>
          </v-expansion-panel>
        </v-expansion-panels>

        <v-alert
          v-if="Object.keys(storageData).length === 0"
          type="info"
          class="mt-4"
        >
          Nenhum dado no localStorage
        </v-alert>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'

const storageData = ref<Record<string, string>>({})

function loadData(): void {
  storageData.value = {}
  
  for (let i = 0; i < localStorage.length; i++) {
    const key = localStorage.key(i)
    
    if (!key) continue
    
    const value = localStorage.getItem(key)
    
    if (value) {
      storageData.value[key] = value
    }
  }
}

function formatValue(value: string): string {
  try {
    const parsed = JSON.parse(value)
    
    return JSON.stringify(parsed, null, 2)
  } catch {
    return value
  }
}

function deleteKey(key: string): void {
  if (!confirm(`Deletar ${key}?`)) return
  
  localStorage.removeItem(key)
  loadData()
}

onMounted(() => {
  loadData()
})
</script>

<style scoped>
.debug-content {
  background: rgba(var(--v-theme-on-surface), 0.05);
  padding: 16px;
  border-radius: 4px;
  overflow-x: auto;
  font-size: 12px;
  max-height: 400px;
  overflow-y: auto;
}
</style>
