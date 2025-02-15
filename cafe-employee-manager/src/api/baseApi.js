import { axiosInstance, handleApiResponse } from './config';

export class BaseApi {
  constructor(endpoint) {
    this.endpoint = endpoint;
  }

  async getAll(params = {}) {
    try {
      const response = await axiosInstance.get(this.endpoint, { params });
      return handleApiResponse(response);
    } catch (error) {
      console.error(`Get all ${this.endpoint} error:`, error);
      throw new Error(`Failed to fetch ${this.endpoint}`);
    }
  }

  async getById(id) {
    try {
      const response = await axiosInstance.get(`${this.endpoint}/${id}`);
      return handleApiResponse(response);
    } catch (error) {
      console.error(`Get ${this.endpoint} error:`, error);
      throw new Error(`Failed to fetch ${this.endpoint}`);
    }
  }

  async create(data) {
    try {
      const response = await axiosInstance.post(this.endpoint, data);
      return handleApiResponse(response);
    } catch (error) {
      console.error(`Create ${this.endpoint} error:`, error);
      const errorMessage = error.response?.data?.message || `Failed to create ${this.endpoint}`;
      throw new Error(errorMessage);
    }
  }

  async update(id, data) {
    try {
      const response = await axiosInstance.put(`${this.endpoint}/${id}`, { ...data, id });
      return handleApiResponse(response);
    } catch (error) {
      console.error(`Update ${this.endpoint} error:`, error);
      const errorMessage = error.response?.data?.message || `Failed to update ${this.endpoint}`;
      throw new Error(errorMessage);
    }
  }

  async delete(id) {
    try {
      const response = await axiosInstance.delete(`${this.endpoint}/${id}`);
      return handleApiResponse(response);
    } catch (error) {
      console.error(`Delete ${this.endpoint} error:`, error);
      const errorMessage = error.response?.data?.message || `Failed to delete ${this.endpoint}`;
      throw new Error(errorMessage);
    }
  }
}