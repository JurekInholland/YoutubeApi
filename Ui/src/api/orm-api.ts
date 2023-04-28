import type YoutubeChannel from '@/models/YoutubeChannel'
import BaseApiService from './base'
import { apiUrls } from '@/constants'
import type YoutubeVideo from '@/models/YoutubeVideo'

export default class OrmService extends BaseApiService {
  public async getVideoById(videoId: string): Promise<YoutubeVideo | null> {
    const res = await this.request<YoutubeVideo>({
      method: 'GET',
      url: `${apiUrls.getVideo}`,
      params: { videoId: videoId }
    })
    if (res instanceof Error) {
      return null
    }
    return res
  }
}
