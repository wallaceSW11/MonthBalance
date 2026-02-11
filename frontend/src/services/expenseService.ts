import { api } from './api';
import type { Expense } from '@/models/Expense';

interface CreateExpenseRequest {
  monthDataId: number;
  expenseTypeId: number;
  value: number;
}

interface UpdateExpenseRequest {
  value: number;
}

async function getByMonth(monthDataId: number): Promise<Expense[]> {
  try {
    const response = await api.get<Expense[]>(`/expenses/month/${monthDataId}`);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao carregar despesas';

    throw new Error(message);
  }
}

async function getById(id: number): Promise<Expense> {
  try {
    const response = await api.get<Expense>(`/expenses/${id}`);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar despesa';

    throw new Error(message);
  }
}

async function create(monthDataId: number, expenseTypeId: number, value: number): Promise<Expense> {
  try {
    const payload: CreateExpenseRequest = { monthDataId, expenseTypeId, value };
    const response = await api.post<Expense>('/expenses', payload);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao criar despesa';

    throw new Error(message);
  }
}

async function update(id: number, value: number): Promise<Expense> {
  try {
    const payload: UpdateExpenseRequest = { value };
    const response = await api.put<Expense>(`/expenses/${id}`, payload);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao atualizar despesa';

    throw new Error(message);
  }
}

async function remove(id: number): Promise<void> {
  try {
    await api.delete(`/expenses/${id}`);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao excluir despesa';

    throw new Error(message);
  }
}

export const expenseService = {
  getByMonth,
  getById,
  create,
  update,
  remove
};
