<script setup lang="ts">
import type { YoutubeVideo } from '@/types';
import { formatDateAgo, formatViews } from '@/utils';
import { onBeforeUnmount, onMounted, ref } from 'vue';


const props = defineProps<{
    video: YoutubeVideo
}>();



onMounted(() => {

    window.addEventListener("pointerup", onPointerUp)
})

onBeforeUnmount(() => {
    window.removeEventListener("pointerup", onPointerUp)
})
const isDown = ref(false);

const onPointerDown = (e: PointerEvent) => {
    isDown.value = true;
}
const onPointerUp = (e: PointerEvent) => {
    console.log("UP")
    isDown.value = false;
}

</script>

<template>
    <div>
        <router-link class="result" :to="'/watch?v=' + video.id">

            <div class="thumbnail">
                <img :src="video.thumbnail" alt="">
            </div>
            <div class="infos" :class="isDown ? 'active' : ''" @pointerdown.prevent="onPointerDown">
                <h3>{{ video.title }}</h3>
                <p>{{ formatViews(video.viewCount) }} views . {{ formatDateAgo(new Date(video.uploadDate)) }}</p>

                <div class="author">
                    <img src="" alt="">
                    <p>{{ video.uploader }}</p>
                </div>
                <p class="description">{{ video.description }}</p>
            </div>
        </router-link>
    </div>
</template>


<style scoped lang="scss">
.active {
    background-color: red;
}
.description {
    height: 1rem;
    overflow: hidden;
    text-overflow: ellipsis;
    flex-grow: 0;
    word-wrap: break-word;
}

.result {
    display: flex;
    // background-color: green;
    gap: 1rem;
    color: unset;
    text-decoration: none;

    .infos {
        display: flex;
        flex-direction: column;
        gap: 1rem;
        // flex-grow: 0;
        flex: 1 2 auto;
        transition: all .5s ease;
        width: 100%;

        h3 {
            font-size: 18px;
            font-weight: bold;
            line-height: 22px;
        }
    }
}

.result:focus-within {
    background-color: red;
}

.thumbnail {
    aspect-ratio: 16/9;
    max-width: 33%;
    // flex-basis: 300px;
    flex: 2 0 auto;
    // flex-grow: 1;
    // background-color: red;

    img {
        border-radius: 12px;
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
}
</style>