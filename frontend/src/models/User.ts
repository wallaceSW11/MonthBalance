export interface User {
  id: number;
  name: string;
  email: string;
  avatar: string | null;
  notificationsEnabled: boolean;
}
