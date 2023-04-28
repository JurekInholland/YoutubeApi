import { Model } from 'pinia-orm'
import YoutubeChannel from './YoutubeChannel'
import { DateCast } from 'pinia-orm/dist/casts'
import LocalVideo from './LocalVideo'

export default class YoutubeVideo extends Model {
  static entity = 'videos'
  static piniaOptions = {
    persist: true
  }

  get uploaded() {
    return new Date(this.uploadDate)
  }

  static fields() {
    return {
      id: this.attr(null),
      title: this.string(''),
      thumbnail: this.string(''),
      youtubeThumbnailUrl: this.string(''),

      description: this.string(''),
      width: this.number(0),
      height: this.number(0),
      dateAdded: this.attr(''),
      lastUpdated: this.attr(''),
      uploadDate: this.attr(''),
      duration: this.string(''),
      viewCount: this.number(0),
      likeCount: this.number(0),
      categories: this.attr([]),
      relatedVideos: this.attr([]),
      playableInEmbed: this.boolean(false),
      youtubeChannelId: this.attr(null),
      youtubeChannel: this.belongsTo(YoutubeChannel, 'youtubeChannelId'),
      localVideo: this.hasOne(LocalVideo, 'id')
    }
  }
  static casts() {
    return {
      dateAdded: DateCast,
      lastUpdated: DateCast,
      uploadDate: DateCast
    }
  }
}
