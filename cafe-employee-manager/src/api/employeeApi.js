import { BaseApi } from './baseApi';

class EmployeeApi extends BaseApi {
  constructor() {
    super('/Employee');
  }

  async getAll(cafeId) {
    return super.getAll(cafeId ? { cafeId } : undefined);
  }

  async create(employeeData) {
    if (!employeeData.gender && employeeData.gender !== 0) {
      throw new Error('Gender is required');
    }

    const requestData = {
      name: employeeData.name,
      emailAddress: employeeData.emailAddress,
      phoneNumber: employeeData.phoneNumber,
      gender: Number(employeeData.gender),
      cafeId: employeeData.cafeId || null,
    };

    return super.create(requestData);
  }

  async update(id, employeeData) {
    const requestData = {
      ...employeeData,
      gender: parseInt(employeeData.gender),
    };

    return super.update(id, requestData);
  }
}

export const employeeApi = new EmployeeApi();