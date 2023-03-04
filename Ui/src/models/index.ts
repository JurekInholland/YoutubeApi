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
  isFullscreen: boolean,
  storedVolume: number,
  settings: boolean
}