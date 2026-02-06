export const ROUTES = {
  LOGIN: '/login',
  REGISTER: '/register',
  FORGOT_PASSWORD: '/forgot-password',
  HOME: '/',
  INCOME_TYPES: '/income-types',
  EXPENSE_TYPES: '/expense-types',
  ACCOUNT: '/account'
} as const;

export type RouteKey = keyof typeof ROUTES;
export type RoutePath = typeof ROUTES[RouteKey];
