import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";
import { authService } from "@/services/AuthService";

const routes: RouteRecordRaw[] = [
  {
    path: "/",
    redirect: "/auth",
  },
  {
    path: "/auth",
    name: "Auth",
    component: () => import("@/views/AuthView.vue"),
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    component: () => import("@/views/DashboardView.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/incomes",
    name: "Incomes",
    component: () => import("@/views/IncomesView.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/expenses",
    name: "Expenses",
    component: () => import("@/views/ExpensesView.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/demo",
    name: "Demo",
    component: () => import("@/views/DemoView.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/debug",
    name: "Debug",
    component: () => import("@/views/DebugView.vue"),
    meta: { requiresAuth: true },
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, _from, next) => {
  if (to.meta.requiresAuth && authService.isAuthRequired()) {
    next('/auth')
    return
  }
  
  next()
})

export default router;
