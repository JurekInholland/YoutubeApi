import { Model } from 'pinia-orm'
import YoutubeVideo from './YoutubeVideo'
import type { V } from 'pinia-orm/dist/Data-77556be8'
import { ArrayCast } from 'pinia-orm/dist/casts'

export default class YoutubeChannel extends Model {
  static entity = 'channels'
  static piniaOptions = {
    persist: true
  }

  static fields() {
    return {
      id: this.string(''),
      title: this.string(''),
      handle: this.string(''),
      thumbnailUrl: this.string(''),
      bannerUrl: this.string(''),
      subscribers: this.string(''),
      videoCount: this.string(''),
      channelUrl: this.string(''),
      avatar: this.string(''),
      banner: this.string(''),
      description: this.string(''),
      videos: this.hasMany(YoutubeVideo, 'youtubeChannelId')
    }
  }
    static casts(): V {
      return {
        videos: ArrayCast
      }
    }
}
