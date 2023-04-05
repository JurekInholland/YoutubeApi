import ApiService from './api'

export const apiUrls = {
  getAllQueuedDownloads: 'Queue/all',
  clearQueue: 'Queue/clear',
  deleteFromQueue: 'Queue/',
  addToQueue: 'Queue/add',
  getVideoInfo: 'YoutubeVideo',
  getVideos: 'YoutubeVideo/all',
  getSearchCompletion: 'Youtube/searchCompletion',
  getSearchResults: 'YoutubeExplode/search'
}

export const apiService: ApiService = new ApiService()
