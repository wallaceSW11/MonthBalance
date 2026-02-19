import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import { authService } from '@/services/authService';
import { authGuard } from '@/services/authGuard';
import { isAdmin } from '@/utils/auth';
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
    path: ROUTES.RESET_PASSWORD,
    name: 'ResetPassword',
    component: () => import('@/views/ResetPasswordView.vue'),
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
  },
  {
    path: ROUTES.ADMIN_DASHBOARD,
    name: 'AdminDashboard',
    component: () => import('@/views/AdminDashboardView.vue'),
    meta: { requiresAuth: true, requiresAdmin: true }
  },
  {
    path: ROUTES.ADMIN_USERS,
    name: 'AdminUsers',
    component: () => import('@/views/AdminUsersView.vue'),
    meta: { requiresAuth: true, requiresAdmin: true }
  },
  {
    path: ROUTES.ADMIN_FEEDBACKS,
    name: 'AdminFeedbacks',
    component: () => import('@/views/AdminFeedbacksView.vue'),
    meta: { requiresAuth: true, requiresAdmin: true }
  },
  {
    path: ROUTES.ADMIN,
    redirect: ROUTES.ADMIN_DASHBOARD
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

authGuard.setupLifecycleGuards();

router.beforeEach((to, _, next) => {
  const requiresAuth = to.meta.requiresAuth !== false;
  const requiresAdmin = to.meta.requiresAdmin === true;
  const authenticated = authService.isAuthenticated();
  const sessionExpired = authenticated && authGuard.isAuthRequired();

  if (requiresAuth && (!authenticated || sessionExpired)) {
    next(ROUTES.LOGIN);

    return;
  }

  if (requiresAdmin && !isAdmin()) {
    next(ROUTES.HOME);

    return;
  }

  if (to.path === ROUTES.LOGIN && authenticated && !sessionExpired) {
    next(ROUTES.HOME);

    return;
  }

  next();
});

export default router;
