import { localStorageService } from './localStorageService';
import type { User } from '@/models/User';

const AUTH_TOKEN_KEY = 'auth_token';
const AUTH_USER_KEY = 'auth_user';

async function login(email: string, password: string): Promise<User> {
  const fixedUser = localStorageService.getFixedUser();

  if (email !== fixedUser.email || password !== fixedUser.password) {
    throw new Error('Credenciais inválidas');
  }

  const token = `token_${Date.now()}`;
  localStorage.setItem(AUTH_TOKEN_KEY, token);

  const user: User = {
    id: fixedUser.id,
    name: fixedUser.name,
    email: fixedUser.email,
    notificationsEnabled: true
  };

  localStorage.setItem(AUTH_USER_KEY, JSON.stringify(user));

  return user;
}

function logout(): void {
  localStorage.removeItem(AUTH_TOKEN_KEY);
  localStorage.removeItem(AUTH_USER_KEY);
}

function isAuthenticated(): boolean {
  const token = localStorage.getItem(AUTH_TOKEN_KEY);

  return !!token;
}

function getCurrentUser(): User | null {
  const userJson = localStorage.getItem(AUTH_USER_KEY);

  if (!userJson) return null;

  try {
    return JSON.parse(userJson) as User;
  } catch {
    return null;
  }
}

async function updateUser(data: Partial<User>): Promise<User> {
  const currentUser = getCurrentUser();

  if (!currentUser) {
    throw new Error('Usuário não autenticado');
  }

  const updatedUser: User = {
    ...currentUser,
    ...data,
    email: currentUser.email
  };

  localStorage.setItem(AUTH_USER_KEY, JSON.stringify(updatedUser));

  return updatedUser;
}

async function changePassword(currentPassword: string, newPassword: string): Promise<void> {
  const fixedUser = localStorageService.getFixedUser();

  if (currentPassword !== fixedUser.password) {
    throw new Error('Senha atual incorreta');
  }

  return Promise.resolve();
}

export const authService = {
  login,
  logout,
  isAuthenticated,
  getCurrentUser,
  updateUser,
  changePassword
};
