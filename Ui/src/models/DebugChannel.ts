import { Model } from 'pinia-orm'
import Video from './DebugVideo'
export default class Channel extends Model {
  static entity = 'dbgChannels'
  static piniaOptions = {
    persist: true
  }
  static fields() {
    return {
      id: this.string(''),
      title: this.string(''),
      videos: this.hasMany(Video, 'channelId')
    }
  }
}
