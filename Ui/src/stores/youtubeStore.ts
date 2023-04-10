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
    relatedVideos: YoutubeVideo[]
  } => ({
    queue: [],
    videos: [],
    searchResults: {},
    searchQuery: '',
    currentVideo: undefined,
    searchSuggestions: [],
    relatedVideos: []
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
        console.log('getVideoById')
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
    async fetchRelatedVideos() {
      this.relatedVideos = []
      let results: YoutubeVideo[] = []
      let notFound: string[] = []

      for (const id of this.currentVideo?.relatedVideos || []) {
        let vid = this.getVideoById(id)
        if (vid) {
          if (this.relatedVideos.indexOf(vid) === -1) {
            this.relatedVideos.push(vid)
          }
          continue
        }
        notFound.push(id)
      }
      if (notFound.length > 0) {
        var res = await apiService.getRelatedVideos(notFound)
        for (const related of res) {
          if (this.relatedVideos.indexOf(related) !== -1) continue
          this.relatedVideos.push(related)
          if (this.videos.indexOf(related) === -1) this.videos.push(related)
        }
      } else {
        console.log('no related videos found')
      }
      this.relatedVideos = this.relatedVideos.sort((a, b) => {
        const indexA = this.currentVideo?.relatedVideos.indexOf(a.id)
        const indexB = this.currentVideo?.relatedVideos.indexOf(b.id)
        return indexA! - indexB!
      })
      // return results
    },

    async setSearchQuery(query: string) {
      this.searchQuery = query
      if (query.length < 13) {
        this.searchSuggestions = []
        return;
      }
      const res = await apiService.GetSearchCompletion(query)
      this.searchSuggestions = res
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
