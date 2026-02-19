import { api } from './api';

export interface UserSummary {
  id: number;
  name: string;
  email: string;
  createdAt: string;
  lastLoginAt?: string;
  totalLogins: number;
  isActive: boolean;
}

export interface UserListResponse {
  users: UserSummary[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface AdminDashboard {
  totalUsers: number;
  newUsersToday: number;
  newUsersThisWeek: number;
  newUsersThisMonth: number;
  activeUsersToday: number;
  activeUsersThisWeek: number;
  activeUsersThisMonth: number;
  unreadFeedbacks: number;
  recentUsers: UserSummary[];
}

async function getDashboard(): Promise<AdminDashboard> {
  try {
    const response = await api.get<AdminDashboard>('/admin/dashboard');
    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar dashboard';
    throw new Error(message);
  }
}

async function getUsers(search?: string, page = 1, pageSize = 20): Promise<UserListResponse> {
  try {
    const params: any = { page, pageSize };
    if (search) params.search = search;
    
    const response = await api.get<UserListResponse>('/admin/users', { params });
    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar usuários';
    throw new Error(message);
  }
}

async function getUserById(id: number): Promise<UserSummary> {
  try {
    const response = await api.get<UserSummary>(`/admin/users/${id}`);
    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar usuário';
    throw new Error(message);
  }
}

export const adminService = {
  getDashboard,
  getUsers,
  getUserById
};
