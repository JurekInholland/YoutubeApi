import { Model } from 'pinia-orm'
import YoutubeVideo from './YoutubeVideo'
import { ArrayCast } from 'pinia-orm/dist/casts'

export default class YoutubeChannel extends Model {
  static entity = 'channels'
  static piniaOptions = {
    persist: true
  }

  static fields() {
    return {
      id: this.attr(null),
      title: this.string(''),
      handle: this.string(''),
      thumbnailUrl: this.string(''),
      bannerUrl: this.string(''),
      subscriberCount: this.number(0),
      videoCount: this.number(0),
      channelUrl: this.string(''),
      avatar: this.string(''),
      banner: this.string(''),
      description: this.string(''),
      videos: this.hasMany(YoutubeVideo, 'youtubeChannelId', 'id')
    }
  }
    static casts() {
      return {
        videos: ArrayCast
      }
    }
}
