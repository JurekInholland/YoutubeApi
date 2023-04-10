import { defineStore } from 'pinia'
import {
  DownloadStatus,
  type DownloadProgress,
  type QueuedDownload,
  type YoutubeVideo
} from '@/types'
import { apiService } from '@/constants'
import type { AxiosError } from 'axios'

export const useYoutubeStore = defineStore({
  id: 'youtubeStore',
  state: (): {
    queue: QueuedDownload[]
    videos: YoutubeVideo[]
    searchResults: { [key: string]: YoutubeVideo[] }
    searchQuery: string
    currentVideo: YoutubeVideo | undefined
    searchSuggestions: string[]
  } => ({
    queue: [],
    videos: [],
    searchResults: {},
    searchQuery: '',
    currentVideo: undefined,
    searchSuggestions: []
  }),

  getters: {
    orderedQueue: (state) =>
      [...state.queue].sort(
        (a, b) => new Date(a.queuedAt).getTime() - new Date(b.queuedAt).getTime()
      ),
    orderedVideos: (state) =>
      [...state.videos].sort(
        (a, b) => new Date(a.dateAdded).getTime() - new Date(b.dateAdded).getTime()
      ),

    getVideoById:
      (state) =>
      (id: string): YoutubeVideo | undefined => {
        const video = state.videos.find((video) => video.id === id)
        if (video !== undefined) return video
        const searchResult = Object.values(state.searchResults)
          .flat()
          .find((video) => video.id === id)
        return searchResult
      }
  },
  actions: {
    clearSearchResults() {
      // this.searchResults = {}
    },
    async fetchRelatedVideos(): Promise<YoutubeVideo[]> {
      let results: YoutubeVideo[] = []

      for (const id of this.currentVideo?.relatedVideos || []) {
        let vid = this.getVideoById(id)
        if (vid) {
          results.push(vid)
          continue
        }
        var res = await apiService.GetVideoInfo(id)
        if (res instanceof Error) {
          continue
        }
        this.videos.push(res)
        results.push(res)
      }
      return results
    },

    async setSearchQuery(query: string) {
      console.log('set search query')
      this.searchQuery = query
      const res = await apiService.GetSearchCompletion(query)
      if (res instanceof Error) {
        console.log('ERROR', res)
      } else {
        this.searchSuggestions = res
      }
    },

    async fetchCurrentVideo(videoId: string) {
      console.log('fetching current video: ' + videoId)
      const found: YoutubeVideo | undefined = Object.values(this.searchResults)
        .flat()
        .find((video) => video.id === videoId)

      if (found) {
        console.log('returning search result')
        this.currentVideo = found
        return
      }

      const foundVideo = this.videos.find((video) => video.id === videoId)
      if (foundVideo) {
        console.log('returning video from videos')
        this.currentVideo = foundVideo
        return
      }
      // debugger;

      const res = await apiService.GetVideoInfo(videoId)
      if (res instanceof Error) {
        console.log(res)
      } else {
        console.log('returning video from api')
        console.log(this.videos)
        this.currentVideo = res
        if (this.videos.find((v) => v.id === res.id)) return
        this.videos.push(res)
      }
    },

    async fetchVideos() {
      console.log('fetching videos')
      const res = await apiService.GetVideos()
      if (res instanceof Error) {
        console.log(res)
      } else this.videos = res
    },
    async fetchQueue() {
      const res = await apiService.GetQueuedDownloads()
      if (res instanceof Error) {
        console.log(res)
      } else this.queue = res
    },
    async fetchSearchResults(query: string) {
      const res = await apiService.getSearchResults(query)
      if (res instanceof Error) {
        console.log(res)
      } else {
        this.searchResults[query] = res

        for (const video of res) {
          if (this.videos.find((v) => v.id === video.id)) continue
          this.videos.push(...res)
        }
      }
    },

    async clearQueue() {
      await apiService.ClearQueue()
      this.reset()
    },

    async enqueue(videoId: string): Promise<boolean> {
      const queueItem: QueuedDownload | AxiosError = await apiService.EnqueueDownload(videoId)
      if (queueItem instanceof Error) {
        console.log(queueItem)
        return false
      }
      this.queue.push(queueItem)
      return true
    },

    async dequeue(queueItem: QueuedDownload) {
      await apiService.DeleteFromQueue(queueItem.id)
      this.queue = this.queue.filter((item) => item.id !== queueItem.id)
    },
    async updateDownloadProgress(progress: DownloadProgress) {
      const queueItem = this.queue.find((item) => item.id === progress.id)
      if (queueItem) {
        queueItem.progress = progress
      }
      if (progress.status === 'download') {
        queueItem!.status = DownloadStatus.Downloading
      } else if (progress.status === 'finished') {
        queueItem!.status = DownloadStatus.Finished
        queueItem!.progress = undefined
      }
    },
    reset() {
      this.queue = []
    }
  }
})
