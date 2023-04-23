import { Repository, useRepo } from 'pinia-orm'
import YoutubeChannel from '@/models/YoutubeChannel'
import { apiService } from '@/constants'
import type YoutubeVideo from '@/models/YoutubeVideo'
import YoutubeVideoRepository from './YoutubeVideoRepository'
export default class YoutubeChannelRepository extends Repository {
  use = YoutubeChannel

  public getById(channelId: string): YoutubeChannel | null {
    const channel = this.find(channelId)
    const videos = useRepo(YoutubeVideoRepository).getChannelVideos(channelId)
    // debugger;
    // if (channel != null && videos != null) {
    //     channel.videos = videos
    // }
    // console.log('getById: ', channel)
    return channel
  }

  public getChannelVideos(channelId: string): YoutubeVideo[] | null {
    // console.log('getChannelVideos: ', channelId)
    // return this.find(channelId)?.with('videos').videos
    return useRepo(YoutubeVideoRepository).getChannelVideos(channelId)
  }

  public async fetchById(id: string): Promise<void> {
    const res = await apiService.getChannelById(id)
    if (res != undefined) {
      this.save(res)
        // useRepo(YoutubeVideoRepository).save(res.videos)
    }
  }
}
