<script setup lang="ts">
import type  YoutubeVideo  from '@/models/YoutubeVideo';
import { formatTitle } from '@/utils';
import { onBeforeMount, onBeforeUnmount, onMounted, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();
const props = defineProps<{
    video: YoutubeVideo
}>();

let interval: number;

const countDown = ref(5);

watch(route, (r) => {
    clearInterval(interval);
})

onMounted(() => {
    countDown.value = 5;
    interval = setInterval(() => {
        countDown.value--;
        if (countDown.value < 0) {
            clearInterval(interval);
            router.push({ name: 'video', params: { videoId: props.video.id } })
            countDown.value = 5;
        }
    }, 1000);
})

onBeforeUnmount(() => {
    clearInterval(interval);
})
</script>

<template>
    <div class="player-up-next" v-if="countDown >= 0">
        <div class="up-next-header">Up next in <span>{{ countDown }}</span></div>
        <div class="thumbnail-container">
            <img :src="video.youtubeThumbnailUrl" alt="">
        </div>
        <h2 v-html="formatTitle(video.title)"></h2>
        <span>{{ video.youtubeChannel?.title }}</span>
        <div class="controls">
            <button>Cancel</button>
            <button>Play Now</button>
        </div>
    </div>
</template>

<style scoped lang="scss">
.player-up-next {
    font-style: Roboto, Arial, sans-serif;
    max-width: 360px;
    display: flex;
    flex-shrink: 1;
    flex-direction: column;
    color: var(--text-color);
    gap: 1rem;

    // background-color: grey;
    .up-next-header {
        color: rgb(128, 128, 128);

        span {
            color: white;
        }
    }

    h2 {
        font-size: 1.1rem;
        line-height: 24px;
        font-weight: 500;
        margin: 0;
    }

    .controls {
        display: flex;
        gap: 1rem;

        button {
            justify-content: center;
            font-size: .8rem;
            font-weight: 600;
            text-transform: uppercase;
            flex-basis: 50%;
            background-color: rgb(64, 64, 64);
            border-radius: 18px;
            padding: .75rem 0;
            color: white;
        }
    }
}

.thumbnail-container {
    width: 100%;
    flex-shrink: 0;
    aspect-ratio: 16/9;
    border-radius: 12px;
    overflow: hidden;
    border: 1px solid rgb(128, 128, 128);

    img {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
}
</style>