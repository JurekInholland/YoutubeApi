import { Repository, type Item } from 'pinia-orm'
import YoutubeVideo from '@/models/YoutubeVideo'
import { apiService } from '@/constants'
import type LocalVideo from '@/models/LocalVideo'

export default class YoutubeVideoRepository extends Repository {
  use = YoutubeVideo

  public addLocalVideo(video: LocalVideo) {
    const existing = this.getById(video.id)
    if (existing) {
      existing.localVideo = video
      this.save(existing)
    }
  }

  public getAll() {
    return this.withAll().get()
  }
  public getById(id: string): YoutubeVideo {
    return this.withAll().find(id) as YoutubeVideo
  }
  public addVideo(video: YoutubeVideo) {
    this.save(video)
  }
  public deleteAll() {
    this.flush()
  }
  public getChannelVideos(channelId: string): YoutubeVideo[] {
    return this.withAll()
      .where((video) => video.channelId === channelId)
      .get() as YoutubeVideo[]
  }

  public async fetchById(id: string) {
    const res = await apiService.GetVideoInfo(id)
    if (res instanceof Error) {
      console.log('Error fetching video by id: ', res)
    }
    this.save(res)
  }

  public getRelatedVideos(ids: string[]) : YoutubeVideo[] {
    return this.withAll()
      .where((video) => ids.includes(video.id))
      .get() as YoutubeVideo[]
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
