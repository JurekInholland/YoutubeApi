import { Repository } from 'pinia-orm'
import YoutubeVideo from '@/models/YoutubeVideo'
import { apiService } from '@/constants'

export default class YoutubeVideoRepository extends Repository {
  use = YoutubeVideo

  public getAll() {
    return this.all()
  }
  public getById(id: string) {
    return this.find(id)
  }
  public addVideo(video: YoutubeVideo) {
    this.save(video)
  }
  public deleteAll() {
    this.flush()
  }
  public getChannelVideos(channelId: string) {
    return this.all().filter((video) => video.youtubeChannelId === channelId)
  }

  public async fetchById(id: string) {
    const res = await apiService.GetVideoInfo(id)
    if (res instanceof Error) {
      console.log('Error fetching video by id: ', res)
    }
    this.save(res)
  }
}
