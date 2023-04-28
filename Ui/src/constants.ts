import ApiService from './api'
import OrmService from './api/orm-api'
import type { PlayerState } from '@/types'
export const apiUrls = {
  addToQueue: 'Queue/Add',
  deleteFromQueue: 'Queue/',
  getAllQueuedDownloads: 'Queue/GetAll',
  deleteQueue: 'Queue/Clear',
  processQueue: 'Queue/Process',
  resetQueue: 'Queue/Reset',

  getVideo: 'YoutubeVideo/GetVideo',
  getAllVideos: 'YoutubeVideo/GetAll',

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

export const defaultState: PlayerState = {
  isPlaying: false,
  volume: 0.5,
  currentTime: 0,
  duration: 100,
  isFullscreen: false,
  storedVolume: 0.5,
  settings: false,
  cinema: false,
  pictureInPicture: false,
  useLocalPlayer: false
}
