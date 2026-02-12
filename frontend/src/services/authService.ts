import { api } from './api';
import type { User } from '@/models/User';

const AUTH_TOKEN_KEY = 'auth_token';
const AUTH_USER_KEY = 'auth_user';

interface AuthResponse {
  token: string;
  user: User;
}

interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

interface LoginRequest {
  email: string;
  password: string;
}

interface UpdateUserRequest {
  name?: string;
  avatar?: string | null;
  notificationsEnabled?: boolean;
}

interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

async function register(name: string, email: string, password: string): Promise<User> {
  try {
    const payload: RegisterRequest = { name, email, password };
    const response = await api.post<AuthResponse>('/auth/register', payload);
    const { token, user } = response.data;

    localStorage.setItem(AUTH_TOKEN_KEY, token);
    localStorage.setItem(AUTH_USER_KEY, JSON.stringify(user));

    return user;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao criar conta';

    throw new Error(message);
  }
}

async function login(email: string, password: string): Promise<User> {
  try {
    const payload: LoginRequest = { email, password };
    const response = await api.post<AuthResponse>('/auth/login', payload);
    const { token, user } = response.data;

    localStorage.setItem(AUTH_TOKEN_KEY, token);
    localStorage.setItem(AUTH_USER_KEY, JSON.stringify(user));

    return user;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Credenciais inválidas';

    throw new Error(message);
  }
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

async function fetchCurrentUser(): Promise<User> {
  const response = await api.get<User>('/auth/me');
  const user = response.data;

  localStorage.setItem(AUTH_USER_KEY, JSON.stringify(user));

  return user;
}

async function updateUser(data: UpdateUserRequest): Promise<User> {
  try {
    const response = await api.put<User>('/auth/me', data);
    const user = response.data;

    localStorage.setItem(AUTH_USER_KEY, JSON.stringify(user));

    return user;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao atualizar usuário';

    throw new Error(message);
  }
}

async function changePassword(currentPassword: string, newPassword: string): Promise<void> {
  try {
    const payload: ChangePasswordRequest = { currentPassword, newPassword };

    await api.post('/auth/change-password', payload);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao alterar senha';

    throw new Error(message);
  }
}

async function registerWebAuthnChallenge(): Promise<any> {
  try {
    const response = await api.post('/auth/webauthn/register/challenge');

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao gerar challenge';

    throw new Error(message);
  }
}

async function registerWebAuthnCredential(credential: any): Promise<void> {
  try {
    await api.post('/auth/webauthn/register', credential);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao registrar credencial';

    throw new Error(message);
  }
}

async function authenticateWebAuthnChallenge(email: string): Promise<any> {
  try {
    const response = await api.post('/auth/webauthn/authenticate/challenge', { email });

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao gerar challenge';

    throw new Error(message);
  }
}

async function authenticateWebAuthn(credential: any): Promise<User> {
  try {
    const response = await api.post<AuthResponse>('/auth/webauthn/authenticate', credential);
    const { token, user } = response.data;

    localStorage.setItem(AUTH_TOKEN_KEY, token);
    localStorage.setItem(AUTH_USER_KEY, JSON.stringify(user));

    return user;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao autenticar';

    throw new Error(message);
  }
}

async function deleteAccount(): Promise<void> {
  try {
    await api.delete('/auth/me');
    logout();
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao excluir conta';

    throw new Error(message);
  }
}

async function forgotPassword(email: string): Promise<void> {
  try {
    await api.post('/auth/forgot-password', { email });
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao solicitar recuperação de senha';

    throw new Error(message);
  }
}

async function resetPassword(token: string, newPassword: string): Promise<void> {
  try {
    await api.post('/auth/reset-password', { token, newPassword });
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao redefinir senha';

    throw new Error(message);
  }
}

export const authService = {
  register,
  login,
  logout,
  isAuthenticated,
  getCurrentUser,
  fetchCurrentUser,
  updateUser,
  changePassword,
  registerWebAuthnChallenge,
  registerWebAuthnCredential,
  authenticateWebAuthnChallenge,
  authenticateWebAuthn,
  deleteAccount,
  forgotPassword,
  resetPassword
};
