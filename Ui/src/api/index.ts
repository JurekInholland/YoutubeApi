import axios, { AxiosError, type AxiosInstance, type AxiosRequestConfig } from 'axios'
import type { QueuedDownload } from '@/types'
import { apiUrls } from '@/constants'

export default class ApiService {
  private axiosInstance: AxiosInstance

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: '/api',
      //   timeout: 5000,
      headers: {
        'Content-Type': 'application/json'
      }
    })
  }

  public async GetQueuedDownloads(): Promise<QueuedDownload[] | AxiosError> {
    const res = await this.request<QueuedDownload[]>({
      method: 'GET',
      url: apiUrls.getAllQueuedDownloads
    })
    console.log(res)
    return res
  }
  public async ClearQueue(): Promise<void> {
    await this.request({
      method: 'DELETE',
      url: apiUrls.clearQueue
    })
  }
  public async EnqueueDownload(videoId: string): Promise<QueuedDownload | AxiosError> {
    const res = await this.request<QueuedDownload>({
      method: 'POST',
      url: apiUrls.addToQueue,
      data: {
        videoId: videoId
      }
    })
    return res
  }

  private async request<T>(config: AxiosRequestConfig): Promise<T | AxiosError> {
    try {
      const { data } = await this.axiosInstance.request<T>(config)
      return data
    } catch (error) {
      return error as AxiosError
    }
  }
}
