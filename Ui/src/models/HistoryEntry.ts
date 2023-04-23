import { Model } from 'pinia-orm'
import YoutubeVideo from '@/models/YoutubeVideo'
import { DateCast } from 'pinia-orm/dist/casts'

export default class HistoryEntry extends Model {
  static entity = 'history'
  static piniaOptions = {
    persist: true
  }
  static fields() {
    return {
      id: this.string(''),
      youtubeVideo: this.belongsTo(YoutubeVideo, 'id'),
      progress: this.number(0),
      lastPlayed: this.attr(null)
    }
  }
  static casts() {
    return {
      lastPlayed: DateCast
    }
  }
}
