import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/dashboard'
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('@/views/LoginView.vue'),
    meta: { requiresAuth: false }
  },
  {
    path: '/register',
    name: 'register',
    component: () => import('@/views/RegisterView.vue'),
    meta: { requiresAuth: false }
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: () => import('@/views/DashboardView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/incomes',
    name: 'incomes',
    component: () => import('@/views/IncomesGlobalView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/expenses',
    name: 'expenses',
    component: () => import('@/views/ExpensesGlobalView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/demo',
    name: 'demo',
    component: () => import('@/views/DemoView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/debug',
    name: 'debug',
    component: () => import('@/views/DebugView.vue'),
    meta: { requiresAuth: true }
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore()
  const requiresAuth = to.meta.requiresAuth !== false

  if (requiresAuth && !authStore.authenticated) {
    next({ name: 'login' })
    return
  }

  if (!requiresAuth && authStore.authenticated) {
    next({ name: 'dashboard' })
    return
  }

  next()
})

export default router;
