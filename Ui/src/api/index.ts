import { apiUrls } from '@/constants'
import type { QueuedDownload, YoutubeVideo } from '@/types'
import axios, { AxiosError, type AxiosInstance, type AxiosRequestConfig } from 'axios'
import type YoutubeChannel from '@/models/YoutubeChannel'
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
  // ------------------ ORM CALLS ------------------

  // ------------------ API CALLS ------------------

  public async getChannelById(id: string): Promise<YoutubeChannel | null> {
    const res = await this.request<YoutubeChannel>({
      method: 'GET',
      url: apiUrls.getChannelById,
      params: { channelId: id }
    })
    if (res instanceof Error) {
      return null
    }
    return res
  }

  public async getChannelByHandle(handle: string): Promise<YoutubeChannel | null> {
    console.log("getChannelByHandle: ", handle)
    const res = await this.request<YoutubeChannel>({
      method: 'POST',
      url: apiUrls.getChannelByHandle,
      data: handle
    })
    if (res instanceof Error) {
      return null
    }
    return res
  }

  public async getLocalVideos(): Promise<YoutubeVideo[]> {
    const res = await this.request<YoutubeVideo[]>({
      method: 'GET',
      url: apiUrls.getLocalVideos
    })
    if (res instanceof Error) {
      return []
    }
    return res
  }

  public async processQueue() {
    return await this.request({
      method: 'GET',
      url: apiUrls.processQueue
    })
  }

  public async getSearchResults(query: string) {
    const res = await this.request<YoutubeVideo[]>({
      method: 'GET',
      url: apiUrls.getSearchResults,
      params: { query: query }
    })
    return res
  }

  public async GetSearchCompletion(query: string): Promise<string[]> {
    const res = await this.request<string[]>({
      method: 'GET',
      url: apiUrls.getSearchCompletion,
      params: { query: query }
    })
    if (res instanceof Error) {
      return []
    }
    console.log(res)
    return res
  }

  public async GetVideos(): Promise<YoutubeVideo[]> {
    const res = await this.request<YoutubeVideo[]>({
      method: 'GET',
      url: apiUrls.getAllVideos
    })
    if (res instanceof Error) {
      return []
    }
    return res
  }

  public async getRelatedVideos(videoIds: string[]): Promise<YoutubeVideo[]> {
    const res = await this.request<YoutubeVideo[]>({
      method: 'POST',
      url: apiUrls.getRelatedVideos,
      data: videoIds
    })
    if (res instanceof Error) {
      return []
    }
    return res
  }

  public async GetVideoInfo(videoId: string): Promise<YoutubeVideo | AxiosError> {
    const res = await this.request<YoutubeVideo>({
      method: 'GET',
      url: `${apiUrls.getVideo}`,
      params: { videoId: videoId }
    })
    console.log(res)
    return res
  }

  public async getQueuedDownloads(): Promise<QueuedDownload[]> {
    const res = await this.request<QueuedDownload[]>({
      method: 'GET',
      url: apiUrls.getAllQueuedDownloads
    })
    if (res instanceof Error) {
      return []
    }
    return res
  }

  public async deleteQueue(): Promise<void> {
    await this.request({
      method: 'DELETE',
      url: apiUrls.deleteQueue
    })
  }

  public async resetQueue(): Promise<void> {
    await this.request({
      url: apiUrls.resetQueue
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
