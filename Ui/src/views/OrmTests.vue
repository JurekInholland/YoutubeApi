<script setup lang="ts">
import { useRepo } from 'pinia-orm'
import YoutubeVideo from '@/models/YoutubeVideo';
import { ormService } from '@/constants'
const videoRepo = useRepo(YoutubeVideo);


const fetchVideo = async () => {
    const video = await ormService.getVideoById("WFkSKEo3CVw");
    if (video === null) return;
    videoRepo.save(video);
    // console.log(video);
}

const test = videoRepo.with('youtubeChannel').all();
console.log(test);
</script>

<template>

<button @click="fetchVideo">Fetch Video</button>
<div v-if="videoRepo.with('youtubeChannel').all().length > 0" v-for="video of videoRepo.with('youtubeChannel').all()">
    {{ video.title }}
    {{ video.id }}
    <!-- {{ video.youtubeChannel.title }} -->
</div>
</template>

<style scoped>
button {
    color: white;
}
</style>