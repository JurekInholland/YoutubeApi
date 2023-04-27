import { Repository, type Item, useRepo } from 'pinia-orm'
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

  public async fetchAll() {
    const videos = await apiService.GetVideos()
    this.save(videos)
  }

  public getAll() : YoutubeVideo[] {
    return this.withAll().get() as YoutubeVideo[]
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
    console.log('fetching video by id:', id)
    const res = await apiService.GetVideoInfo(id)
    if (res instanceof Error) {
      console.log('Error fetching video by id: ', res)
      return null
    }
    console.log('RES ID: ', res)
    // var insert = useRepo(YoutubeVideo).make(res)
    // var insert = this.make(res)
    this.save(res)
  }

  public getRelatedVideos(ids: string[]): YoutubeVideo[] {
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
