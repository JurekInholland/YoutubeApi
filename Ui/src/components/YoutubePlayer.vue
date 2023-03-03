<script setup lang="ts">
import { YoutubeIframe } from '@vue-youtube/component';
import { useRoute } from 'vue-router';
const route = useRoute();

const props = defineProps<{
    videoId: string,
    startTime: number
}>()
</script>


<template>
    <h1>route: {{ route }}</h1>
    <youtube-iframe :preserveAspectRatio="true" :video-id="props.videoId" :player-vars="
        {
            // https://developers.google.com/youtube/player_parameters#Parameters
            iv_load_policy: 3,
            color: 'white',
            start: props.startTime,
            modestbranding: 1,
            enablejsapi: 1,
            



        }"></youtube-iframe>
</template>
iframe {
    width: 100%;
    max-width: 100%;
}
<style scoped>
</style>