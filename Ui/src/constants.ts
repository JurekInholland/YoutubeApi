import ApiService from './api'

export const apiUrls = {
  getAllQueuedDownloads: 'Queue/all',
  clearQueue: 'Queue/clear',
  addToQueue: 'Queue/add',
  getVideoInfo: 'YoutubeExplode/video'
}

export const apiService: ApiService = new ApiService()
