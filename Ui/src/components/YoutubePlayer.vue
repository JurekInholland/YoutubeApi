<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';
import { YoutubeIframe } from '@vue-youtube/component';
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
const route = useRoute();
const store = useYoutubeStore();
const props = defineProps<{
    videoId: string,
    startTime: number,
    aspectRatio: number
}>()


const youtube = ref();
</script>

<template>
    <div class="yotube-wrapper">
        <youtube-iframe ref="youtube" class="iframe" :style="{
            'aspect-ratio': props.aspectRatio,
        }" :preserveAspectRatio="true" :video-id="props.videoId" :player-vars="
    {
        // https://developers.google.com/youtube/player_parameters#Parameters
        iv_load_policy: 3,
        color: 'white',
        start: props.startTime,
        modestbranding: 1,
        enablejsapi: 1,
        rel: 0,
        autoplay: 1,

    }">
        </youtube-iframe>
    </div>
</template>

<style >
.iframe {
    /* max-width: 1920px;
    'max-width': '1920px', width: '100%', 'max-height': 80 + 'vh' */
}

.youtube-wrapper {

    /* max-width: calc(100vh - 12rem) !important; */
    /* margin: 0 auto; */

    /* background-color: red;
    overflow: hidden;
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0; */
}

iframe {
    width: 100%;
    height: 100%;
    max-width: unset;
    /* margin: 0 auto; */
    /* aspect-ratio: 16/9; */
}
</style>