import ApiService from './api'

export const apiUrls = {
  getAllQueuedDownloads: 'Queue/all',
  clearQueue: 'Queue/clear',
  deleteFromQueue: 'Queue/',
  addToQueue: 'Queue/add',
  getVideoInfo: 'YoutubeExplode/video'
}

export const apiService: ApiService = new ApiService()
