import { apiService, defaultColor } from '@/constants'
import {
  type DownloadProgress,
  DownloadStatus,
  type LocalVideo,
  type QueuedDownload,
  type YoutubeVideo
} from '@/types'
import type { PlayerState } from '@vue-youtube/component'
import type { AxiosError } from 'axios'
import { defineStore } from 'pinia'
import { type Ref, ref } from 'vue'

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
    color: Ref<string>
    // playerState: PlayerState
  } => ({
    queue: [],
    videos: [],
    searchResults: {},
    searchQuery: '',
    currentVideo: undefined,
    searchSuggestions: [],
    relatedVideos: [],
    color: ref(defaultColor)
    // playerState: {}
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
    localVideos: (state) => state.videos.filter((video) => video.localVideo !== undefined),

    channelVideos: (state) => (channelId: string) =>
      state.videos.filter((video: YoutubeVideo) => video.youtubeChannel?.id === channelId),

    getVideoTags: (state) => {
      const tags: { [key: string]: number } = {}
      for (const video of state.videos) {
        if (video.categories === undefined) continue
        for (const tag of video.categories) {
          tags[tag] = (tags[tag] || 0) + 1
        }
      }
      let lst = []
      for (const [key, value] of Object.entries(tags)) {
        if (value < 2) continue
        lst.push([key, value])
      }
      return lst.sort((a, b) => Number(b[1]) - Number(a[1]))
    },
    getQueueItem: (state) => (videoId: string) => {
      return state.queue.find((item) => item.video.id === videoId)
    },

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

    setColor(color: string) {
      this.color = color
    },
    async fetchChannelById(id: string) {
      const res = await apiService.getChannelById(id)
      if (res?.videos === undefined) return
      for (const video of res?.videos) {
        if (video === undefined || video === null || video.id === null) continue
        video.youtubeChannel = res
        if (this.videos.filter((v) => v.id === video.id).length === 0) this.videos.push(video)
        else {
          const index = this.videos.findIndex((v) => v.id === video.id)
          this.videos[index] = video
        }
      }
    },

    async fetchChannelByHandle(handle: string) {
      const res = await apiService.getChannelByHandle(handle)
      for (const video of res) {
        if (video === undefined || video === null || video.id === null) continue
        // debugger
        if (this.videos.filter((v) => v.id === video.id).length === 0) this.videos.push(video)
      }
    },

    async fetchLocalVideos() {
      const res = await apiService.getLocalVideos()
      for (const video of res) {
        if (this.videos.filter((v) => v.id === video.id).length === 0) this.videos.push(video)
      }
    },
    async fetchRelatedVideos() {
      if (this.currentVideo === undefined) return
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
      this.relatedVideos.sort((a, b) => {
        const indexA = this.currentVideo?.relatedVideos.indexOf(a.id)
        const indexB = this.currentVideo?.relatedVideos.indexOf(b.id)
        return indexA! - indexB!
      })
      // return results
    },

    async addLocalVideo(localVideo: LocalVideo) {
      // const video = this.videos.find((v) => v.id === localVideo.id)
      const queueEntry = this.queue.find((v) => v.id === localVideo.id)
      if (queueEntry?.video) {
        queueEntry.video.localVideo = localVideo
      }
      const vid = this.videos.find((v) => v.id === localVideo.id)
      if (vid) {
        vid.localVideo = localVideo
      }
    },

    async setSearchQuery(query: string) {
      this.searchQuery = query
      if (query.length < 1) {
        this.searchSuggestions = []
        return
      }
      this.searchSuggestions = await apiService.GetSearchCompletion(query)
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
        this.currentVideo = undefined
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
      this.queue = (await apiService.getQueuedDownloads()).sort(
        (a, b) => new Date(b.queuedAt).getTime() - new Date(a.queuedAt).getTime()
      )
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

    async deleteQueue() {
      await apiService.deleteQueue()
      this.resetQueue()
    },
    async clearQueue() {
      await apiService.resetQueue()
      await this.fetchQueue()
      // this.queue = []
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
      if (queueItem === undefined) {
        console.log("couldn't find queue item")
        return
      }
      queueItem.progress = progress

      if (progress.status === 'download') {
        queueItem.status = DownloadStatus.Downloading
      } else if (progress.status === 'finished') {
        queueItem.status = DownloadStatus.Finished
        queueItem.progress = undefined
      }
    },
    resetQueue() {
      this.queue = []
    }
  }
})
