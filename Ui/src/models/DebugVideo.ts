import { Model } from 'pinia-orm'
import Channel from './DebugChannel';

export default class Video extends Model {
    static entity = 'dbgVideos';
    static piniaOptions = {
        persist: true
      }
    static fields() {
        return {
            id: this.string(''),
            title: this.string(''),
            channelId: this.attr(null),
            channel: this.belongsTo(Channel, 'channelId')
        }
    }
}