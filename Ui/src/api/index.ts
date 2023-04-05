import axios, { AxiosError, type AxiosInstance, type AxiosRequestConfig } from 'axios'
import type { QueuedDownload, YoutubeVideo } from '@/types'
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
  public async GetSearchResults(query: string) {
    const res = await this.request<YoutubeVideo[]>({
      method: 'GET',
      url: apiUrls.getSearchResults,
      params: { query: query }
    })
    return res
  }
  
  public async GetSearchCompletion(query: string): Promise<string[] | AxiosError> {
    const res = await this.request<string[]>({
      method: 'GET',
      url: apiUrls.getSearchCompletion,
      params: { query: query }
    })
    console.log(res)
    return res
  }

  public async GetVideos(): Promise<YoutubeVideo[] | AxiosError> {
    const res = await this.request<YoutubeVideo[]>({
      method: 'GET',
      url: apiUrls.getVideos
    })
    console.log(res)
    return res
  }

  public async GetVideoInfo(videoId: string): Promise<YoutubeVideo | AxiosError> {
    const res = await this.request<YoutubeVideo>({
      method: 'GET',
      url: `${apiUrls.getVideoInfo}`,
      params: { videoId: videoId }
    })
    console.log(res)
    return res
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
  public async DeleteFromQueue(videoId: string): Promise<void> {
    await this.request({
      method: 'DELETE',
      url: `${apiUrls.deleteFromQueue}/${videoId}`
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
