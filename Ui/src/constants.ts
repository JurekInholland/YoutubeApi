import ApiService from './api'
import OrmService from './api/orm-api'

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
  getRelatedVideos: 'Scrape/Multiple',
  getChannelByHandle: 'Scrape/YoutubeChannelByHandle',
  getChannelById: 'Scrape/YoutubeChannelById'
}

export const apiService: ApiService = new ApiService()

export const ormService: OrmService = new OrmService()

export const defaultColor: string = '#38cd6d'
