import { Model } from 'pinia-orm'
import YoutubeChannel from './YoutubeChannel'

export default class YoutubeVideo extends Model {
  static entity = 'videos'
  static fields() {
    return {
      id: this.string(''),
      title: this.string(''),
      thumbnail: this.string(''),
      youtubeThumbnailUrl: this.string(''),

      description: this.string(''),
      width: this.number(0),
      height: this.number(0),
      dateAdded: this.number(0),
      lastUpdated: this.number(0),
      uploadDate: this.number(0),
      duration: this.string(''),
      viewCount: this.number(0),
      likeCount: this.number(0),
      categories: this.attr([]),
      relatedVideos: this.attr([]),
      playableInEmbed: this.boolean(false),


      youtubeChannel: this.belongsTo(YoutubeChannel, 'id')
    }
  }
}
