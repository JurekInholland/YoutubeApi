import ApiService from './api'

export const apiUrls = {
  getAllQueuedDownloads: 'Queue/all',
  deleteQueue: 'Queue/clear',
  resetQueue: 'Queue/reset',
  deleteFromQueue: 'Queue/',
  addToQueue: 'Queue/add',
  getVideoInfo: 'YoutubeVideo',
  getVideos: 'YoutubeVideo/all',
  getSearchCompletion: 'Youtube/searchCompletion',
  getSearchResults: 'Scrape/search',
  processQueue: 'Queue/process',
  getRelatedVideos: 'Scrape/multiple',
  getLocalVideos: 'LocalVideo',

  getChannelByHandle: 'Scrape/handle',
  getChannelById: 'Scrape/channelId',
}

export const apiService: ApiService = new ApiService()
