<script setup lang="ts">
import type { IRelatedVideo } from '@/models';
import { RouterLink } from 'vue-router'

const props = defineProps<{
    video: IRelatedVideo
}>()

</script>

<template>
    <router-link :to="'/' + props.video.id" class="video">
        <div class="thumbnail">
            <img :src="props.video.thumbnail" alt="">
            <div class="play-overlay">a</div>
        </div>
        <div class="details">
            <h3>{{ props.video.title }}</h3>
            <div>{{ props.video.channelTitle }}</div>
            <div>111K views {{ props.video.publishTime }}</div>
        </div>
    </router-link>
</template>

<style scoped>
.video {
    display: flex;
    gap: 6px;
    font-size: 12px;
    line-height: 18px;
    color: rgba(255, 255, 255, .65);
    text-overflow: elipsis;
    white-space: normal;
    text-decoration: none;
    margin-bottom: 2px;

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
    /* justify-content: center;
    align-items: center; */
}
.play-overlay {
    bottom: .25rem;
    right: .25rem;
    position: absolute;
    display: flex;
    flex-grow: 1;
    background: rgba(0,0,0,0.8);
    padding: 0 12px;
    /* background-color: red;
    width: 46px;
    height: 46px; */
    z-index: 1;
    border-radius: 3px;
}
</style>