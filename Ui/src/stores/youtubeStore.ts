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
    searchResults: YoutubeVideo[]
  } => ({
    queue: [],
    videos: [],
    searchResults: []
  }),

  getters: {
    orderedQueue: (state) =>
      state.queue.sort((a, b) => new Date(a.queuedAt).getTime() - new Date(b.queuedAt).getTime()),
    orderedVideos: (state) =>
      state.videos.sort((a, b) => new Date(a.dateAdded).getTime() - new Date(b.dateAdded).getTime())
  },
  actions: {
    clearSearchResults() {
      this.searchResults = []
    },

    async fetchVideos() {
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
      const res = await apiService.GetSearchResults(query)
      if (res instanceof Error) {
        console.log(res)
      } else this.searchResults = res
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
