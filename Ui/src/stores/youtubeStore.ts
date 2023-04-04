import { defineStore } from 'pinia'
import type { DownloadProgress, QueuedDownload } from '@/types'
import { apiService } from '@/constants'
import type { AxiosError } from 'axios'
export const useYoutubeStore = defineStore({
  id: 'youtubeStore',
  state: (): { queue: QueuedDownload[] } => ({
    queue: []
  }),

  getters: {
    orderedQueue: (state) => state.queue.sort((a, b) => a.queuedAt.getTime() - b.queuedAt.getTime())
  },
  actions: {
    async fetchQueue() {
      const res = await apiService.GetQueuedDownloads()
      if (res instanceof Error) {
        console.log(res)
      } else {
        this.queue = res
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
    },
    reset() {
      this.queue = []
    }
  }
})
