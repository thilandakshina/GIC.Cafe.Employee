import axios from 'axios';

const BASE_URL = 'https://localhost:5002/api';

export const axiosInstance = axios.create({
  baseURL: BASE_URL,
});

export const handleApiResponse = (response) => {
  if (response?.data) {
    if (response.data.success && response.data.data) {
      return response.data.data;
    }
    if (response.data.success) {
      return response.data;
    }
    if (response.data.message) {
      throw new Error(response.data.message);
    }
  }
  return response.data;
};