import ApiService from './api'

export const apiUrls = {
  addToQueue: 'Queue/Add',
  deleteFromQueue: 'Queue/',
  getAllQueuedDownloads: 'Queue/GetAll',
  deleteQueue: 'Queue/Clear',
  processQueue: 'Queue/Process',
  resetQueue: 'Queue/Reset',


  getVideoInfo: 'YoutubeVideo/GetVideo',
  getVideos: 'YoutubeVideo/GetAll',

  getSearchCompletion: 'Youtube/searchCompletion',
  getLocalVideos: 'LocalVideo',

  getSearchResults: 'Scrape/SearchResults',
  getRelatedVideos: 'Scrape/multiple',
  getChannelByHandle: 'Scrape/handle',
  getChannelById: 'Scrape/channelId',
}

export const apiService: ApiService = new ApiService()
