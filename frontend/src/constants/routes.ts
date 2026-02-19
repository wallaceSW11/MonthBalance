export const ROUTES = {
  LOGIN: '/login',
  REGISTER: '/register',
  FORGOT_PASSWORD: '/forgot-password',
  RESET_PASSWORD: '/reset-password',
  PRIVACY_POLICY: '/privacy-policy',
  HOME: '/',
  INCOME_TYPES: '/income-types',
  EXPENSE_TYPES: '/expense-types',
  ACCOUNT: '/account',
  ADMIN: '/admin',
  ADMIN_DASHBOARD: '/admin/dashboard',
  ADMIN_USERS: '/admin/users',
  ADMIN_FEEDBACKS: '/admin/feedbacks'
} as const;

export type RouteKey = keyof typeof ROUTES;
export type RoutePath = typeof ROUTES[RouteKey];
