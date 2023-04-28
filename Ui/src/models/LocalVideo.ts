import { Model } from 'pinia-orm'
import YoutubeVideo from './YoutubeVideo'

export default class LocalVideo extends Model {
  static entity = 'localVideos'
  static piniaOptions = {
    persist: true
  }
  static fields() {
    return {
      id: this.attr(null),
      path: this.string(''),
      extension: this.string(''),
      width: this.number(0),
      height: this.number(0),
      fps: this.number(0),
      size: this.number(0),
      vbr: this.number(0),
      abr: this.number(0),
      // video: this.belongsTo(YoutubeVideo, 'id', 'id')
    }
  }
}
