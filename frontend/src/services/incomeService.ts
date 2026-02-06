import { api } from './api';
import type { Income } from '@/models/Income';

interface CreateIncomeRequest {
  monthDataId: number;
  incomeTypeId: number;
  grossValue: number | null;
  netValue: number | null;
  hourlyRate: number | null;
  hours: number | null;
  minutes: number | null;
}

interface UpdateIncomeRequest {
  grossValue?: number | null;
  netValue?: number | null;
  hourlyRate?: number | null;
  hours?: number | null;
  minutes?: number | null;
}

async function getByMonth(monthDataId: number): Promise<Income[]> {
  try {
    const response = await api.get<Income[]>(`/incomes/month/${monthDataId}`);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao carregar receitas';

    throw new Error(message);
  }
}

async function getById(id: number): Promise<Income> {
  try {
    const response = await api.get<Income>(`/incomes/${id}`);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar receita';

    throw new Error(message);
  }
}

async function create(data: CreateIncomeRequest): Promise<Income> {
  try {
    const response = await api.post<Income>('/incomes', data);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao criar receita';

    throw new Error(message);
  }
}

async function update(id: number, data: UpdateIncomeRequest): Promise<Income> {
  try {
    const response = await api.put<Income>(`/incomes/${id}`, data);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao atualizar receita';

    throw new Error(message);
  }
}

async function remove(id: number): Promise<void> {
  try {
    await api.delete(`/incomes/${id}`);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao excluir receita';

    throw new Error(message);
  }
}

export const incomeService = {
  getByMonth,
  getById,
  create,
  update,
  remove
};
