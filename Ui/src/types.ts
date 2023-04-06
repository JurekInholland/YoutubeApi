export enum DownloadStatus {
  Queued = 0,
  Downloading = 1,
  Finished = 2,
  Error = 3
}
export enum ApplicationTask {
  ProcessDownloadQueue,
  ProcessDownloadQueueItem,
  CleanUpDownloadQueue
}
export enum TaskStatus {
  NeverRan,
  Started,
  Finished,
  Error
}

export interface YoutubeChannel {
  id: string
  title: string
  handle: string
  channelUrl: string
  thumbnailUrl: string
  subscribers: string
  videos?: YoutubeVideo[]
}

export interface LocalVideo {
  id: string
  width: number
  height: number
  size: number
  path: string
}

export interface YoutubeVideo {
  id: string
  title: string
  thumbnail: string
  youtubeThumbnailUrl: string
  description: string
  dateAdded: Date
  lastUpdated: Date
  uploadDate: Date
  duration: string
  viewCount: number
  likeCount: number
  categories: string[]
  relatedVideos: string[]

  localVideo: LocalVideo | null
  youtubeChannel: YoutubeChannel | null
}

export interface QueuedDownload {
  id: string
  status: DownloadStatus
  queuedAt: Date
  video: YoutubeVideo
  progress?: DownloadProgress
}

export interface VideoSearchResult {
  id: string
  title: string
  channel_title: string
  channel_id: string
  duration: number
  thumbnail: string
}

export interface DownloadProgress {
  id: string
  status: string
  progress: number
  speed: number
  eta: number
  fragment: string
}

export interface TaskProgress {
  time: Date
  task: ApplicationTask
  status: TaskStatus
}

export interface ApplicationSettings {
  showRelatedVideos: boolean
  showComments: boolean
  autoplay: boolean
}
