import { api } from './api';
import type { ExpenseTypeModel } from '@/models/ExpenseTypeModel';

interface CreateExpenseTypeRequest {
  name: string;
}

interface UpdateExpenseTypeRequest {
  name: string;
}

async function getAll(): Promise<ExpenseTypeModel[]> {
  try {
    const response = await api.get<ExpenseTypeModel[]>('/expense-types');

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao carregar tipos de despesa';

    throw new Error(message);
  }
}

async function getById(id: number): Promise<ExpenseTypeModel> {
  try {
    const response = await api.get<ExpenseTypeModel>(`/expense-types/${id}`);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar tipo de despesa';

    throw new Error(message);
  }
}

async function create(name: string): Promise<ExpenseTypeModel> {
  try {
    const payload: CreateExpenseTypeRequest = { name };
    const response = await api.post<ExpenseTypeModel>('/expense-types', payload);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao criar tipo de despesa';

    throw new Error(message);
  }
}

async function update(id: number, name: string): Promise<ExpenseTypeModel> {
  try {
    const payload: UpdateExpenseTypeRequest = { name };
    const response = await api.put<ExpenseTypeModel>(`/expense-types/${id}`, payload);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao atualizar tipo de despesa';

    throw new Error(message);
  }
}

async function remove(id: number): Promise<void> {
  try {
    await api.delete(`/expense-types/${id}`);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao excluir tipo de despesa';

    throw new Error(message);
  }
}

export const expenseTypeService = {
  getAll,
  getById,
  create,
  update,
  remove
};
