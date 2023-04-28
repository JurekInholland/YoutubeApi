import axios, { AxiosError, type AxiosInstance, type AxiosRequestConfig } from 'axios'

export default class BaseApiService {
  protected axiosInstance: AxiosInstance

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: '/api',
      headers: {
        'Content-Type': 'application/json'
      }
    })
  }

  protected async request<T>(config: AxiosRequestConfig): Promise<T | AxiosError> {
    try {
      const { data } = await this.axiosInstance.request<T>(config)
      return data
    } catch (error) {
      return error as AxiosError
    }
  }
}
