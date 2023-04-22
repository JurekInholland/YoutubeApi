import { Model } from 'pinia-orm'
import YoutubeVideo from './YoutubeVideo'

export default class LocalVideo extends Model {
  static entity = 'localVideos'
  static fields() {
    return {
      id: this.string(''),
      path: this.string(''),
      extension: this.string(''),
      width: this.number(0),
      height: this.number(0),
      fps: this.number(0),
      size: this.number(0),
      vbr: this.number(0),
      abr: this.number(0),
      youtubeVideo: this.belongsTo(YoutubeVideo, 'id')
    }
  }
}
