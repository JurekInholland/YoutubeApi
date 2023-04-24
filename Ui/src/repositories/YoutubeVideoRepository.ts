import { Repository } from 'pinia-orm'
import YoutubeVideo from '@/models/YoutubeVideo'
import { apiService } from '@/constants'

export default class YoutubeVideoRepository extends Repository {
  use = YoutubeVideo

  public getAll() {
    return this.all()
  }
  public getById(id: string) {
    return this.withAll().find(id)
  }
  public addVideo(video: YoutubeVideo) {
    this.save(video)
  }
  public deleteAll() {
    this.flush()
  }
  public getChannelVideos(channelId: string): YoutubeVideo[] {
    return this.with('youtubeChannel').all().filter((video) => video.youtubeChannelId === channelId) as YoutubeVideo[]
  }

  public async fetchById(id: string) {
    const res = await apiService.GetVideoInfo(id)
    if (res instanceof Error) {
      console.log('Error fetching video by id: ', res)
    }
    this.save(res)
  }

  public getRelatedVideos(ids: string[]): YoutubeVideo[] {
    return this.withAll().all().filter((video) => ids.includes(video.id)) as YoutubeVideo[]
  }

  public async fetchRelatedVideos(ids: string[]) {
    let notFoundIds: string[] = []

    for (const id of ids) {
      const existing = this.getById(id)
      if (existing === null) {
        notFoundIds.push(id)
      }
    }
    const videos = await apiService.getRelatedVideos(notFoundIds)
    this.save(videos)
  }
}
