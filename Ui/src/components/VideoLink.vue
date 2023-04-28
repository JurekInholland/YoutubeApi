<script setup lang="ts">
import type YoutubeVideo from '@/models/YoutubeVideo'
import { useYoutubeStore } from '@/stores/youtubeStore';
import { formatViews, formatTitle, formatDuration, formatDateAgo } from '@/utils';
import { Icon } from '@iconify/vue';

const store = useYoutubeStore();

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
                <router-link v-if="video.youtubeChannel"
                    :to="{ name: 'channel', params: { username: video.youtubeChannel.title } }">
                    <img :src="`/api/Thumbnail/channel?channelId=${video.youtubeChannel?.id}`" alt="avatar" />
                </router-link>
                <div class="text">
                    <div class="flex">

                        <h3 class="h3" v-html="formatTitle(video.title)"></h3>
                        <div class="svg" v-if="video.localVideo" :style="{ color: store.color }">
                            <Icon icon="material-symbols:arrow-circle-down-rounded" />

                        </div>
                    </div>
                    <router-link v-if="video.youtubeChannel"
                        :to="{ name: 'channel-id', params: { channelId: video.youtubeChannel.id } }">{{
                            video.youtubeChannel!.title }}</router-link>
                    <!-- <p>pannenkoek2012</p> -->
                    <p>{{ formatViews(video.viewCount) }} views <span class="separator">{{ formatDateAgo(video.uploadDate)
                    }}</span></p>
                </div>
            </div>
        </router-link>
    </div>
</template>

<style scoped lang="scss">
.separator::before {
    content: "•";
}

.flex {
    display: inline-flex;
    align-items: flex-start;
    gap: .5rem;

    .h3 {
        color: var(--text-color);
        font-size: 1rem;
        font-weight: 500;
        line-height: 22px;
        // width: 100%;
        // flex: 0 1;
        flex-grow: 0;
        word-break: break-all;
        margin-bottom: .33rem;
    }

    .svg {
        flex-grow: 1;
        align-items: flex-start;
        margin-top: .15rem;
        font-size: 1rem;

    }


    // justify-content: center;
}

svg {

    padding: -4px;
    box-sizing: border-box;

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
    display: inline-flex;
    flex-direction: column;
    text-overflow: ellipsis;
    font-size: .9rem;
    line-height: normal;
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

    .flex,
    a:hover .flex {
        color: white;
    }

}
</style>
