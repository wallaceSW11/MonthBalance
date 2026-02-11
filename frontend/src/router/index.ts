import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import { authService } from '@/services/authService';
import { authGuard } from '@/services/authGuard';
import { ROUTES } from '@/constants/routes';

const routes: RouteRecordRaw[] = [
  {
    path: ROUTES.LOGIN,
    name: 'Login',
    component: () => import('@/views/LoginView.vue'),
    meta: { requiresAuth: false }
  },
  {
    path: ROUTES.REGISTER,
    name: 'Register',
    component: () => import('@/views/RegisterView.vue'),
    meta: { requiresAuth: false }
  },
  {
    path: ROUTES.FORGOT_PASSWORD,
    name: 'ForgotPassword',
    component: () => import('@/views/ForgotPasswordView.vue'),
    meta: { requiresAuth: false }
  },
  {
    path: ROUTES.PRIVACY_POLICY,
    name: 'PrivacyPolicy',
    component: () => import('@/views/PrivacyPolicyView.vue'),
    meta: { requiresAuth: false }
  },
  {
    path: ROUTES.HOME,
    name: 'Home',
    component: () => import('@/views/HomeView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: ROUTES.INCOME_TYPES,
    name: 'IncomeTypes',
    component: () => import('@/views/IncomeTypesView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: ROUTES.EXPENSE_TYPES,
    name: 'ExpenseTypes',
    component: () => import('@/views/ExpenseTypesView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: ROUTES.ACCOUNT,
    name: 'Account',
    component: () => import('@/views/AccountView.vue'),
    meta: { requiresAuth: true }
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

authGuard.setupLifecycleGuards();

router.beforeEach((to, _, next) => {
  const requiresAuth = to.meta.requiresAuth !== false;
  const authenticated = authService.isAuthenticated();

  if (requiresAuth && !authenticated) {
    next(ROUTES.LOGIN);

    return;
  }

  if (requiresAuth && authGuard.isAuthRequired()) {
    next(ROUTES.LOGIN);

    return;
  }

  if (to.path === ROUTES.LOGIN && authenticated && !authGuard.isAuthRequired()) {
    next(ROUTES.HOME);

    return;
  }

  next();
});

export default router;
