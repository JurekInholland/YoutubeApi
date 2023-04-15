export interface IPlayerOptions {
  start: number
  videoId: string
  volume: number
  autoplay: boolean
  controls: boolean
}

const playerOptions: IPlayerOptions = {
  start: 0,
  videoId: '',
  volume: 0,
  autoplay: false,
  controls: false
}
// ------------------------------------

export interface IPlayerState {
  isPlaying: boolean
  volume: string
  currentTime: number
  duration: number
  isFullscreen: boolean
  storedVolume: number
  settings: boolean
  cinema: boolean
  pictureInPicture: boolean
}

export interface IVideo {
  id: string
  title: string
  description: string
  uploader: string
  view_count: number
  like_count: number
  upload_date: number
  channel_follower_count: number
  comments: IYoutubeComment[]
}

export interface IRelatedVideo {
  id: string
  title: string
  description: string
  thumbnail: string
  channelId: string
  channelTitle: string
  publishTime: Date
}

export interface IYoutubeComment {
  id: string

  text: string
  author: string
  author_id: string
  author_thumbnail: string
  authorIsUploader: boolean
  like_count: number
  time_text: string
  parent_id: string
  isFavorited: boolean
}
