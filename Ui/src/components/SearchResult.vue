<script setup lang="ts">
import type { YoutubeVideo } from '@/types';
import { formatDateAgo, formatDescription, formatViews } from '@/utils';
import { computed, onBeforeUnmount, onMounted, ref } from 'vue';


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
    <div class="main" :class="isDown ? 'active' : ''" @pointerdown.prevent="onPointerDown">
        <!-- <router-link class="result" :to="'/watch?v=' + video.id"> -->
        <div class="result">
            <router-link :to="'/watch?v=' + video.id" class="thumbnail">
                <img :src="video.youtubeThumbnailUrl" alt="">
            </router-link>
            <div class="infos">
                <router-link :to="'/watch?v=' + video.id">

                    <h3>{{ video.title }}</h3>
                    <p class="view-count">{{ formatViews(video.viewCount) }} views . {{ formatDateAgo(new
                        Date(video.uploadDate)) }}</p>

                    <div class="author" :to="{ name: 'channel', params: { username: video.youtubeChannel?.title } }">

                        <img :src="`api/Thumbnail/channel?channelId=${video.youtubeChannel?.id}`" alt="">
                        <p>{{ video.youtubeChannel?.title }}</p>
                    </div>
                </router-link>
                <div v-html="formatDescription(video.description)" class="description"></div>
            </div>
            <!-- </router-link> -->
        </div>
    </div>
</template>


<style scoped lang="scss">
.view-count {
    color: rgba(255, 255, 255, .7);
    margin-bottom: 1rem;
}

.main {
    border: 1px solid rgba(255, 255, 255, 0);
    transition: border-color 1s ease;
    padding: .25rem;
}

a:hover {
    color: white;
}

.author {
    display: flex;
    align-items: center;
    gap: .5rem;
    color: rgba(255, 255, 255, .7);
    transition: color .2s ease;
    margin-bottom: 1rem;

    img {
        width: 1.5rem;
        height: 1.5rem;
        border-radius: 50%;
        object-fit: cover;
    }
}



.description {
    overflow: hidden;
    text-overflow: ellipsis;
    flex-grow: 0;
    // word-wrap: break-word;
    line-height: 1rem;
    max-height: 2rem;
    color: rgba(255, 255, 255, .7);


}

.result {
    display: flex;
    // background-color: green;
    gap: 1rem;
    color: unset;
    z-index: 1;

}

.infos {
    display: flex;
    flex-direction: column;
    // outline: 1px solid rgba(128, 128, 128, 0);
    // flex-grow: 0;
    flex: 1 2 auto;
    margin-top: .25rem;
    width: 100%;
    overflow: hidden;
    font-size: 12px;

    h3 {
        font-size: 18px;
        font-weight: 400;
        color: white;
        // font-weight: bold;
        line-height: 26px;
        max-height: 52px;
        margin-bottom: .2rem;
        // overflow: hidden;
        // text-overflow: ellipsis;
        // white-space: nowrap;
        // flex-grow: 0;
        // word-wrap: break-word;
    }
}

.active {
    background-color: rgb(54, 54, 54);
    border: 1px solid rgb(54, 54, 54);
    transition: none;
    // outline: 2px solid rgb(128, 128, 128);
}

// .result:focus-within {
//     background-color: red;
// }

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
<style>
.description {
    z-index: 100;
}

.description a {
    color: var(--link-color);
    z-index: 100;
}
</style>