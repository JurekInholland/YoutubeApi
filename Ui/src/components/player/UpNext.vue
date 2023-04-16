<script setup lang="ts">
import type { YoutubeVideo } from '@/types';

const props = defineProps<{
    video: YoutubeVideo
}>();

</script>

<template>
    <div class="up-next">
        <div class="thumbnail-cont">
            <img :src="video.youtubeThumbnailUrl" alt="">
        </div>
        <div class="infos">
            next (shift+n)
            <span>
                {{ video.title }}
            </span>
        </div>
    </div>
</template>

<style scoped lang="scss">
.thumbnail-cont {
    flex-basis: 50%;
    background-color: red;

}

.infos {
    flex-basis: 50%;
    font-family: "Roboto", sans-serif;
    font-weight: 500;
    flex: 1;
    display: flex;
    flex-direction: column;
    text-transform: uppercase;
    margin: .33rem;
    margin-top: 0;
    line-height: 28px;
    overflow: visible;
    text-overflow: clip;
    color: rgba(255, 255, 255, .65);

    span {
        color: #fff;
        display: -webkit-box;
        overflow: hidden;
        line-clamp: 2;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        text-transform: none;
        text-align: left;
        line-height: normal;
        // background-color: green;
        text-overflow: clip;
        font-size: .8rem;

    }
}

.up-next {
    position: absolute;
    left: -3rem;
    top: -112px;
    width: 316px;
    height: 94px;
    border-radius: 12px;
    display: flex;
    background-color: rgba(28, 28, 28, .9);
    overflow: hidden;
    gap: .5rem;

    img {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
}
</style>