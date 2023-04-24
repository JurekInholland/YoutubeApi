<script setup lang="ts">
import { useRepo } from 'pinia-orm'
import YoutubeVideo from '@/models/YoutubeVideo';
import YoutubeChannel from '@/models/YoutubeChannel';
import { ormService, apiService } from '@/constants'
import { computed, onMounted, ref, type ComputedRef, type Ref } from 'vue';
import { AxiosError } from 'axios';
import VideoLink from '@/components/VideoLink.vue';
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import YoutubeChannelRepository from '@/repositories/YoutubeChannelRepository';
import { computedAsync } from '@vueuse/core';
const videoRepo = useRepo(YoutubeVideo);
const repo = useRepo(YoutubeVideoRepository);
const channels = useRepo(YoutubeChannelRepository);

const chan = computed(() => channels.getById("UCuAXFkgsw1L7xaCfnd5JJOw"));

const channelVids = computed(() => useRepo(YoutubeVideoRepository).with('youtubeChannel').where('youtubeChannelId', "UCuAXFkgsw1L7xaCfnd5JJOw").orderBy(vid => vid.uploaded).get()) as ComputedRef<YoutubeVideo[]>;

const mountedChannel: Ref<YoutubeChannel | null> = ref(null)
const newFetch = async () => {
    await channels.fetchById("UCuAXFkgsw1L7xaCfnd5JJOw")
}

const fetchVideo = async () => {

    await repo.fetchById("dQw4w9WgXcQ");
}

const vids = computed(() => videoRepo.withAll().get());

const videos = ref<YoutubeVideo[]>([]);
onMounted(() => {
    // const test = videoRepo.with('youtubeChannel').first();
    videos.value = videoRepo.with('youtubeChannel').all();
    console.log(videoRepo.with('youtubeChannel').first());
    mountedChannel.value = useRepo(YoutubeChannel).with('videos').first();
})
// tryOnMounted(fetchVideo);

// console.log(videoRepo.piniaStore());
</script>

<template>
    <div>
        <div v-if="chan">
            channel:
            {{ chan.title }}
        </div>
        <button @click="newFetch">Fetch Video</button>
        <div class="container">
            <VideoLink v-if="channelVids" v-for="video in channelVids" :video="video" :key="video.id" />

        </div>

    </div>
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