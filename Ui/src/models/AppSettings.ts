import { Model } from 'pinia-orm'

export default class AppSettings extends Model {
  static entity = 'settings'
  static piniaOptions = {
    persist: true
  }
  static fields() {
    return {
      id: this.number(0),
      color: this.string(''),
      darkMode: this.boolean(true),
      showSidebar: this.boolean(true)
    }
  }
}
