import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { authService } from '@/services/authService';
import { authGuard } from '@/services/authGuard';
import type { User } from '@/models/User';

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null);
  const loading = ref<boolean>(false);

  const authenticated = computed(() => !!user.value);

  async function register(name: string, email: string, password: string): Promise<void> {
    loading.value = true;

    try {
      user.value = await authService.register(name, email, password);
      authGuard.markAuthenticated();
    } finally {
      loading.value = false;
    }
  }

  async function login(email: string, password: string): Promise<void> {
    loading.value = true;

    try {
      user.value = await authService.login(email, password);
      authGuard.markAuthenticated();
    } finally {
      loading.value = false;
    }
  }

  function logout(): void {
    authService.logout();
    authGuard.clearAuthState();
    user.value = null;
  }

  function initializeAuth(): void {
    if (!authService.isAuthenticated()) return;

    user.value = authService.getCurrentUser();
    authGuard.markAuthenticated();
  }

  async function updateUserData(data: Partial<User>): Promise<void> {
    loading.value = true;

    try {
      user.value = await authService.updateUser(data);
    } finally {
      loading.value = false;
    }
  }

  async function changePassword(currentPassword: string, newPassword: string): Promise<void> {
    loading.value = true;

    try {
      await authService.changePassword(currentPassword, newPassword);
    } finally {
      loading.value = false;
    }
  }

  return {
    user,
    loading,
    authenticated,
    register,
    login,
    logout,
    initializeAuth,
    updateUserData,
    changePassword
  };
});
