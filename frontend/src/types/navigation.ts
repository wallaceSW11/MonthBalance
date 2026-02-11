import type { RoutePath } from '@/constants/routes';

export interface NavigationItem {
  icon: string;
  titleKey: string;
  path: RoutePath;
}
