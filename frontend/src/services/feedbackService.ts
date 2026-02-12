import { api } from './api';

export interface CreateFeedbackRequest {
  subject: string;
  message: string;
  rating?: number;
}

export interface Feedback {
  id: number;
  userId?: number;
  userName: string;
  email: string;
  subject: string;
  message: string;
  rating?: number;
  createdAt: string;
  isRead: boolean;
  adminNotes?: string;
}

async function create(data: CreateFeedbackRequest): Promise<Feedback> {
  try {
    const response = await api.post<Feedback>('/feedback', data);
    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao enviar feedback';
    throw new Error(message);
  }
}

async function getAll(isRead?: boolean, page = 1, pageSize = 20): Promise<Feedback[]> {
  try {
    const params: any = { page, pageSize };
    if (isRead !== undefined) params.isRead = isRead;
    
    const response = await api.get<Feedback[]>('/feedback', { params });
    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar feedbacks';
    throw new Error(message);
  }
}

async function getById(id: number): Promise<Feedback> {
  try {
    const response = await api.get<Feedback>(`/feedback/${id}`);
    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar feedback';
    throw new Error(message);
  }
}

async function markAsRead(id: number): Promise<void> {
  try {
    await api.put(`/feedback/${id}/mark-read`);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao marcar como lido';
    throw new Error(message);
  }
}

async function getUnreadCount(): Promise<number> {
  try {
    const response = await api.get<{ count: number }>('/feedback/unread-count');
    return response.data.count;
  } catch (error: any) {
    return 0;
  }
}

export const feedbackService = {
  create,
  getAll,
  getById,
  markAsRead,
  getUnreadCount
};
