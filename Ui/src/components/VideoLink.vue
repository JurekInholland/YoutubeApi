<script setup lang="ts">
import type { YoutubeVideo } from '@/types';
import { formatViews, formatTitle, formatDuration } from '@/utils';


const props = defineProps<{
    video: YoutubeVideo
}>()

</script>


<template>
    <div class="video-container">
        <router-link v-if="video.id" class="outer" :to="{ name: 'video', params: { videoId: video.id ? video.id : '' } }">
            <div class="thumbnail">
                <div class="runtime">{{ formatDuration(video.duration) }}</div>
                <img :src="video.youtubeThumbnailUrl" alt="">
            </div>

            <div class="infos">
                <router-link :to="{ name: 'channel', params: { username: video.youtubeChannel?.title } }">
                    <img :src="`api/Thumbnail/channel?channelId=${video.youtubeChannel?.id}`" alt="avatar" />
                </router-link>
                <div class="text">
                    <h3 v-html="formatTitle(video.title)" class="title"></h3>
                    <router-link :to="{ name: 'channel', params: { username: video.youtubeChannel?.title } }">{{
                        video.youtubeChannel!.handle }}</router-link>
                    <!-- <p>pannenkoek2012</p> -->
                    <p>{{ formatViews(video.viewCount) }} views <span class="separator"> 2 days ago</span></p>
                </div>
            </div>
        </router-link>
    </div>
</template>

<style scoped lang="scss">
.separator::before {
    content: "•";
}

.thumbnail {
    width: 100%;
    aspect-ratio: 16/9;

    img {
        width: 100%;
        height: 100%;
    }
}

.thumbnail:hover .runtime {
    opacity: 0;
}

.text {

    text-overflow: ellipsis;
}

.infos {
    display: flex;
    gap: 1rem;
    // justify-content: center;
    align-items: center;
    max-width: 100%;

    h3 {
        font-weight: bold;
        max-height: 42px;
        overflow: hidden;
        margin-bottom: .5rem;
        color: white;
        text-overflow: ellipsis;
        line-height: 22px;
    }

    p {
        line-height: 22px;
        color: rgba(255, 255, 255, .75);
        // flex-grow: 1;
        flex-basis: calc(100% - 36px);
    }

    a {
        // position: relative;
        display: flex;
        justify-content: flex-start;
        align-items: flex-start;
        height: 100%;
        width: auto;

        // width: 36px;
        // height: 36px;
        // min-height: 36px;
        img {
            border-radius: 50%;
            object-fit: contain;
            flex-basis: 36px;
            max-width: 36px;
        }


    }
}

.outer {
    width: 100%;
    // position: relative;
}

.runtime {
    font-size: .8rem;
    line-height: .9rem;
    font-weight: bold;

    position: absolute;
    bottom: .5rem;
    right: .5rem;
    background-color: rgba(0, 0, 0, 0.8);
    color: white;
    z-index: 10;
    border-radius: 4px;
    // margin: 4px;
    padding: 4px 3px;
    transition: opacity .25s ease;
    width: auto;
}

.video-container {
    display: flex;
    height: auto;
    overflow: hidden;

    img {
        border-radius: 12px;

        max-width: 100%;
        max-height: 100%;
        object-fit: cover;
        aspect-ratio: 16/9;
    }

    a {
        // position: relative;
        display: flex;
        flex-direction: column;
        color: rgba(255, 255, 255, .75);
        // margin: 1rem;
        gap: .5rem;
    }

    a:hover {
        color: white;
    }
}
</style>
