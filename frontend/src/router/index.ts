import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";
import { authService } from "@/services/AuthService";

const routes: RouteRecordRaw[] = [
  {
    path: "/auth",
    name: "Auth",
    component: () => import("@/views/AuthView.vue"),
    meta: { public: true },
  },
  {
    path: "/",
    redirect: "/dashboard",
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
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, _from, next) => {
  const isPublicRoute = to.meta.public === true
  
  if (isPublicRoute) {
    next()
    return
  }
  
  if (authService.isAuthRequired()) {
    next('/auth')
    return
  }
  
  next()
})

export default router;
