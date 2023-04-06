<script setup lang="ts">
import type { IVideo } from '@/models';
import type { YoutubeVideo } from '@/types';
import { Icon } from '@iconify/vue';
import SvgButton from './buttons/SvgButton.vue';

const props = defineProps<{ video: YoutubeVideo }>();
</script>

<template>
    <div class="metadata">
        <h1>{{ props.video.title }}</h1>

        <div id="top-row">
            <div id="owner">
                <router-link :to="{ name: 'channel', params: { username: video.youtubeChannel?.title } }" id="avatar">
                    <img :src="`api/Thumbnail/channel?channelId=${video.youtubeChannel?.id}`" alt="avatar" />
                </router-link>
                <div class="upload-info">
                <h3>{{ props.video.youtubeChannel?.title }}</h3>
                <p>{{ props.video.youtubeChannel?.id }}</p>
                <!-- <p>{{ props.video.channel_follower_count }} subscribers</p> -->
                </div>
            </div>
            <div>
                <SvgButton class="dlbtn" text="Download" view-box="0 0 24 24"
                    path="M17 18V19H6V18H17ZM16.5 11.4L15.8 10.7L12 14.4V4H11V14.4L7.2 10.6L6.5 11.3L11.5 16.3L16.5 11.4Z" />
                <!-- <button>
                                                                                                    <Icon icon="clarity:download-line" />
                                                                                                    Download
                                                                                                </button> -->
            </div>
        </div>
        <div id="description">
            <div class="info">{{ props.video.viewCount }} views 1 year ago</div>
            <p>{{ props.video.description.slice(0, 300) }}</p>
        </div>
    </div>
</template>

<style scoped lang="scss">
* {
    font-family: Roboto;
}

#avatar {
    img {
        width: 48px;
        height: 48px;
        border-radius: 50%;
    }
}

.metadata {
    margin-top: 12px;
}

h1 {
    font-weight: 600;
    font-size: 20px;
    line-height: 2.8rem;
    margin-bottom: 2px;
    color: rgb(223, 220, 216)
}

h3 {
    /* font-size: 1.25rem; */
    font-weight: 500;
    color: rgb(245, 245, 245);
}

#top-row {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
}

#owner {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 0.5rem;
}

#owner img {
    border-radius: 50%;
}

.upload-info {
    display: flex;
    flex-direction: column;
    gap: 0.33rem;
}

.info {
    font-weight: 500;
    line-height: 28px;
}

#description {
    border-radius: 12px;
    line-height: 20px;
    padding: 12px;
    margin: 12px 0;
    background-color: rgba(255, 255, 255, 0.1);
    white-space: pre-wrap;
}

#owner p {
    font-size: 12px;
    color: rgb(170, 170, 170)
}

button {
    display: flex;
    align-items: center;
    font-size: 14px;
    line-height: 36px;
    color: white;
    background-color: rgba(255, 255, 255, 0.1);
    padding-left: 1rem;
    padding-right: 19px;
    border-radius: 18px;
    height: 36px;
    /* gap: .5rem; */
    /* background-color: red; */
}
</style>
<style>
.dlbtn {
    gap: 4px;
    font-weight: 500;
}

.dlbtn svg {
    height: 24px;
    width: 24px;
    /* margin-top: 10px; */
}

.dlbtn svg path {
    /* fill: yellow; */

}

.dlbtn:hover {
    background-color: rgba(255, 255, 255, 0.2);
}
</style>