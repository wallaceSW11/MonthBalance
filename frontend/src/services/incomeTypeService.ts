import { api } from './api';
import type { IncomeTypeModel } from '@/models/IncomeTypeModel';

interface CreateIncomeTypeRequest {
  name: string;
  type: 'paycheck' | 'hourly' | 'extra';
}

interface UpdateIncomeTypeRequest {
  name: string;
}

async function getAll(): Promise<IncomeTypeModel[]> {
  try {
    const response = await api.get<IncomeTypeModel[]>('/income-types');

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao carregar tipos de receita';

    throw new Error(message);
  }
}

async function getById(id: number): Promise<IncomeTypeModel> {
  try {
    const response = await api.get<IncomeTypeModel>(`/income-types/${id}`);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao buscar tipo de receita';

    throw new Error(message);
  }
}

async function create(name: string, type: 'paycheck' | 'hourly' | 'extra'): Promise<IncomeTypeModel> {
  try {
    const payload: CreateIncomeTypeRequest = { name, type };
    const response = await api.post<IncomeTypeModel>('/income-types', payload);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao criar tipo de receita';

    throw new Error(message);
  }
}

async function update(id: number, name: string): Promise<IncomeTypeModel> {
  try {
    const payload: UpdateIncomeTypeRequest = { name };
    const response = await api.put<IncomeTypeModel>(`/income-types/${id}`, payload);

    return response.data;
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao atualizar tipo de receita';

    throw new Error(message);
  }
}

async function remove(id: number): Promise<void> {
  try {
    await api.delete(`/income-types/${id}`);
  } catch (error: any) {
    const message = error.response?.data?.message || 'Erro ao excluir tipo de receita';

    throw new Error(message);
  }
}

export const incomeTypeService = {
  getAll,
  getById,
  create,
  update,
  remove
};
