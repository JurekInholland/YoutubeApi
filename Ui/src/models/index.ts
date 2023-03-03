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
  volume: number
  currentTime: number
  duration: number
  isFullscreen: boolean
}