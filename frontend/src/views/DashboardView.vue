<template>
  <v-container
    fluid
    class="dashboard-container pa-0"
  >
    <NavigationDrawer v-model="drawerOpen" />

    <div class="sticky-header">
      <div class="header-top">
        <v-btn
          icon
          size="small"
          variant="text"
          class="menu-button"
          @click="toggleDrawer"
        >
          <v-icon>mdi-menu</v-icon>
        </v-btn>

        <MonthNavigation />

        <div class="spacer" />
      </div>

      <MonthSummary />
    </div>

    <div class="content-scroll">
      <IncomeList />
      <ExpenseList />
    </div>

    <v-btn
      icon
      size="large"
      color="primary"
      class="fab"
      elevation="8"
    >
      <v-icon size="28">
        mdi-plus
      </v-icon>
    </v-btn>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useMonthStore } from '@/stores/month'
import NavigationDrawer from '@/components/NavigationDrawer.vue'
import MonthNavigation from '@/components/MonthNavigation.vue'
import MonthSummary from '@/components/MonthSummary.vue'
import IncomeList from '@/components/IncomeList.vue'
import ExpenseList from '@/components/ExpenseList.vue'

const monthStore = useMonthStore()
const drawerOpen = ref(false)

function toggleDrawer(): void {
  drawerOpen.value = !drawerOpen.value
}

onMounted(() => {
  monthStore.initializeCurrentMonth()
})
</script>

<style scoped>
.dashboard-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.sticky-header {
  position: sticky;
  top: 0;
  z-index: 10;
  background: rgba(var(--v-theme-surface), 0.95);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border-bottom: 1px solid rgba(var(--v-theme-on-surface), 0.08);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.header-top {
  display: grid;
  grid-template-columns: 1fr 3fr 1fr;
  align-items: center;
  padding: 12px 16px;
}

.menu-button {
  justify-self: start;
}

.spacer {
  justify-self: end;
}

.content-scroll {
  flex: 1;
  overflow-y: auto;
  width: 100%;
  max-width: 448px;
  margin: 0 auto;
}

.fab {
  position: fixed;
  bottom: 32px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 20;
  border: 2px solid rgba(var(--v-theme-surface), 1);
}
</style>
