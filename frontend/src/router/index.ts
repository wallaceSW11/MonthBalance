import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

const routes: RouteRecordRaw[] = [
  {
    path: "/",
    name: "Home",
    component: () => import("@/views/HomeView.vue"),
  },
  {
    path: "/income-types",
    name: "IncomeTypes",
    component: () => import("@/views/IncomeTypesView.vue"),
  },
  {
    path: "/expense-types",
    name: "ExpenseTypes",
    component: () => import("@/views/ExpenseTypesView.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
