<script setup lang="ts">
import type YoutubeVideo from '@/models/YoutubeVideo';
import { useResizeObserver } from '@vueuse/core';
import { computed, ref } from 'vue';


const props = defineProps<{
    videos: YoutubeVideo[]
}>();

const colums = ref(0);
const rows = ref(0);

const grid = ref<HTMLElement | null>(null);
useResizeObserver(grid, (entries) => {
    const { width, height } = entries[0].contentRect;
    // console.log("RESIZE", (width / 240) | 0, (height / 135) | 0,)
    // colums.value = (width / 245) | 0;

    colums.value = height * (9 / 16) | 0;
    rows.value = (height / 135) | 0;
    console.log("COLUMS", colums.value)

})

const computedStyle = computed(() => {
    return {
        'grid-template-columns': `repeat(${colums.value}, auto)`,
        // 'grid-template-rows': '1fr',
        // 'grid-template-rows': `repeat(${rows.value - 1}, 1fr)`,
    }
})

const limitedVideos = computed(() => {
    return props.videos.slice(0, colums.value * rows.value);
})

</script>

<template>
    <div class="up-next-grid" ref="grid" :style="computedStyle">
        <div class="up-next-item" v-for="video in videos">
            <router-link :to="{ name: 'watch', query: { v: video.id }, }">

                <img :src="video.youtubeThumbnailUrl" alt="">

            </router-link>
        </div>
    </div>
</template>


<style scoped lang="scss">
.up-next-grid {
    background-color: red;
    box-sizing: border-box;
    width: 100%;
    height: 100%;
    max-height: 100%;
    max-width: 100%;
    overflow: hidden;
    // display: flex;
    // flex-wrap: wrap;
    display: grid;
    grid-auto-rows: 0;
    // gap: .25rem;
    // grid-template-columns: 1fr 1fr;
    // grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
    // grid-template-columns: repeat(fit-content(20%), minmax(120px, 33%));

    // grid-template-rows: repeat(fit-content(100%), minmax(120px, 1fr));
    // grid-auto-rows: 2; // grid-template-columns: repeat(auto-fit, minmax(1fr, auto));
    padding: .15rem;

    .up-next-item {
        background-color: red;
        display: flex;
        flex: 1;
        max-width: 100%;
        max-height: 100%;
        // display: flex;
        // flex-shrink: 1;
        // flex-grow: 1;
        grid-column: span 2;
        aspect-ratio: 16/9;
        // margin: .15rem;
        // width: 30%;
        // height: 25%;

        // display: flex;
        // align-items: center;
        // justify-content: center;
        a {
            width: 100%;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
    }

    // .up-next-item:before {
    //     content: "";
    //     display: block;
    //     height: 0;
    //     width: 0;
    //     padding-bottom: calc(9/16 * 100%);
    // }
}
</style>
