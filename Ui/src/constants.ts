import ApiService from './api'

export const apiUrls = {
  getAllQueuedDownloads: 'Queue/all',
  clearQueue: 'Queue/clear',
  deleteFromQueue: 'Queue/',
  addToQueue: 'Queue/add',
  getVideoInfo: 'YoutubeVideo',
  getVideos: 'YoutubeVideo/all',
  getSearchCompletion: 'Youtube/searchCompletion',
  getSearchResults: 'Scrape/search',
  processQueue: 'Queue/process',
  getRelatedVideos: 'Scrape/multiple'
}

export const apiService: ApiService = new ApiService()
