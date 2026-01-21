import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

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
  },
  {
    path: "/incomes",
    name: "Incomes",
    component: () => import("@/views/IncomesView.vue"),
  },
  {
    path: "/expenses",
    name: "Expenses",
    component: () => import("@/views/ExpensesView.vue"),
  },
  {
    path: "/demo",
    name: "Demo",
    component: () => import("@/views/DemoView.vue"),
  },
  {
    path: "/debug",
    name: "Debug",
    component: () => import("@/views/DebugView.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
