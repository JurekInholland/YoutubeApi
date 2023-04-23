<script setup lang="ts">
import { useRepo } from 'pinia-orm'
import YoutubeVideo from '@/models/YoutubeVideo';
import YoutubeChannel from '@/models/YoutubeChannel';
import { ormService, apiService } from '@/constants'
import { computed, onMounted, ref } from 'vue';
import { AxiosError } from 'axios';
import VideoLink from '@/components/VideoLink.vue';
const videoRepo = useRepo(YoutubeVideo);


const fetchVideo = async () => {
    // const video = await ormService.getVideoById("WFkSKEo3CVw");
    const video = await apiService.GetVideoInfo("ZHlenYEeNz0")
    // const isVid = video instanceof YoutubeVideo;
    if (!(video instanceof AxiosError)) {
        const channel = await apiService.getChannelById(video.youtubeChannel!.id);
        console.log("CHANNEL: ", channel)
        debugger;
        if (channel !== undefined) {
            // const inserted = videoRepo.insert(channel.videos!)
            const inserted = useRepo(YoutubeChannel).save(channel)
            console.log(inserted);
            // debugger;
        }
    }
    // videoRepo.save(video);
    // console.log(video);
}

const vids = computed(() => videoRepo.withAll().get());

const videos = ref<YoutubeVideo[]>([]);
onMounted(() => {
    // const test = videoRepo.with('youtubeChannel').first();
    videos.value = videoRepo.with('youtubeChannel').get();
    console.log(videoRepo.with('youtubeChannel').first());
})
// tryOnMounted(fetchVideo);

// console.log(videoRepo.piniaStore());
</script>

<template>
    <div>
        <button @click="fetchVideo">Fetch Video</button>
        <div class="container">
            <VideoLink v-for="video in vids" :video="video" :key="video.id" />

        </div>

    </div>
    <!-- <div v-if="vids.length > 0" v-for="video in vids">
        {{ video.title }}
        {{ video.id }}
        {{ video.youtubeChannel.title }}
    </div> -->
</template>

<style scoped>
.container {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
    gap: 1.5rem;
    padding: 1.5rem;
}

button {
    color: white;
}
</style>