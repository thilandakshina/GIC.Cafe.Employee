import { BaseApi } from './baseApi';
import { convertFileToBase64 } from './utils';

class CafeApi extends BaseApi {
  constructor() {
    super('/Cafe');
  }

  async getAll(location = '') {
    return super.getAll({ location });
  }

  async create(data) {
    const requestData = {
      name: data.name,
      description: data.description,
      location: data.location,
    };

    if (data.logo instanceof File) {
      const base64Logo = await convertFileToBase64(data.logo);
      requestData.logo = base64Logo.split(',')[1];
    }

    return super.create(requestData);
  }

  async update(id, data) {
    const requestData = {
      name: data.name,
      description: data.description,
      location: data.location,
    };

    if (data.logo instanceof File) {
      const base64Logo = await convertFileToBase64(data.logo);
      requestData.logo = base64Logo.split(',')[1];
    } else if (typeof data.logo === 'string' && data.logo) {
      requestData.logo = data.logo;
    }

    return super.update(id, requestData);
  }
}

export const cafeApi = new CafeApi();