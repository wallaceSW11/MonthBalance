import { api } from './api';
import type { MonthData } from '@/models/MonthData';

interface CreateMonthDataRequest {
  year: number;
  month: number;
}

async function getAll(): Promise<MonthData[]> {
  try {
    const response = await api.get<MonthData[]>('/month-data');

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao carregar meses';

    throw new Error(message);
  }
}

async function getByYearMonth(year: number, month: number): Promise<MonthData> {
  try {
    const response = await api.get<MonthData>(`/month-data/${year}/${month}`);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar mês';

    throw new Error(message);
  }
}

async function create(year: number, month: number): Promise<MonthData> {
  try {
    const payload: CreateMonthDataRequest = { year, month };
    const response = await api.post<MonthData>('/month-data', payload);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao criar mês';

    throw new Error(message);
  }
}

async function updateLastAccessed(id: number): Promise<void> {
  try {
    await api.put(`/month-data/${id}/last-accessed`);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao atualizar último acesso';

    throw new Error(message);
  }
}

async function remove(id: number): Promise<void> {
  try {
    await api.delete(`/month-data/${id}`);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao excluir mês';

    throw new Error(message);
  }
}

export const monthDataService = {
  getAll,
  getByYearMonth,
  create,
  updateLastAccessed,
  remove
};
