<script setup lang="ts">
import VideoRow from '@/components/VideoRow.vue';
import type YoutubeVideo from '@/models/YoutubeVideo';
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { useRepo } from 'pinia-orm';
import { computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const store = useYoutubeStore();
const route = useRoute()

const path = route.path;

const videos = computed(() => {
    return useRepo(YoutubeVideoRepository).getAll()
})

const local = computed(() => {
    return videos.value.filter(v => v.localVideo !== null)
})

onMounted(() => {
    document.title = "Youtube Clone"
})
</script>

<template>
    <main>
        <!-- <h1>home</h1>
        <p>{{ route }}</p> -->
        <VideoRow :videos="local" :offset="0" />

        <hr>
        <VideoRow :videos="videos" :offset="0" />
        <VideoRow :videos="videos" :offset="1" />
        <hr>

    </main>
</template>
<style scoped>
p {
    margin: 1rem 0;
}

main {
    display: flex;
    flex-direction: column;
    max-width: 2256px;
    margin: 0 auto;
    margin-top: 1.5rem;
    padding: 0 2rem;
    gap: 2rem;
    flex-grow: 1;
}

hr {
    border-color: var(--light-grey);
}
</style>
