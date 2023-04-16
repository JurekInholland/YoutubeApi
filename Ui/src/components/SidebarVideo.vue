<script setup lang="ts">
import type { YoutubeVideo } from '@/types';
import { formatDateAgo, formatViews, formatDuration, formatDescription, formatTitle } from '@/utils';

const props = defineProps<{
    video: YoutubeVideo
}>()

</script>

<template>
    <router-link :to="{ name: 'watch', query: { v: video.id }, }" class="video-cont">
        <div class="thumbnail">
            <img :src="props.video.youtubeThumbnailUrl" alt="">
            <div class="play-overlay">{{ formatDuration(props.video.duration) }}</div>
        </div>
        <div class="details">
            <h3 v-html="formatTitle(props.video.title)"></h3>
            <p>{{ props.video.youtubeChannel?.title }}</p>
            <p>{{ formatViews(props.video.viewCount) }} views {{ formatDateAgo(props.video.uploadDate) }}</p>
        </div>
    </router-link>
</template>

<style scoped>
.details p {
    max-height: 18px;
    text-overflow: ellipsis;
    flex-shrink: 1;
    overflow: hidden;
    min-width: 133px;
}

.video-cont {
    display: flex;
    gap: 6px;
    font-size: 12px;
    line-height: 18px;
    color: rgba(255, 255, 255, .65);
    text-overflow: elipsis;
    white-space: normal;
    margin-bottom: 2px;
    min-height: 95px;
    flex-grow: 0;

}

.video-cont:hover .play-overlay {
    opacity: 0;
}

h3 {
    font-size: 14px;
    font-weight: 600;
    color: white;
    margin-bottom: .2rem;
    max-height: 36px;
    white-space: pre-wrap;
    overflow: hidden;
}

img {
    border-radius: 8px;
    /* width: 168px;
    height: 94px; */
    aspect-ratio: 16/9;
    max-width: 168px;
    object-fit: cover;
}

.thumbnail {
    position: relative;
    display: flex;
    /* min-width: 200px; */
    /* min-width: 168px; */
    height: auto;
    flex-grow: 0;
    /* justify-content: center;
    align-items: center; */
    /* overflow: hidden; */
}

.play-overlay {
    bottom: .25rem;
    right: .25rem;
    position: absolute;
    display: flex;
    flex-grow: 1;
    background: rgba(0, 0, 0, 0.8);
    padding: 0 4px;
    color: white;
    /* background-color: red;
    width: 46px;
    height: 46px; */
    z-index: 1;
    border-radius: 3px;
    opacity: 1;
    transition: opacity .2s ease;
}
</style>
