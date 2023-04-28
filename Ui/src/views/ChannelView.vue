<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import type { YoutubeVideo } from '@/types';
import { formatDescription } from '@/utils';
import { onMounted, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const router = useRouter();
const route = useRoute();
const store = useYoutubeStore();


const username = route.params.username as string;
const id = route.params.channelId as string;
// watch(route, (r) => {
//     console.log("!!!!!!!!!!!!!! CHANGE", r)
//     debugger;
// })

const channelVideos = ref<YoutubeVideo[]>([]);


onMounted(async () => {
    document.title = `@${username}`;
    console.log("CH ID", id)
    // await store.fetchChannelById(id);
    // channelVideos.value = store.channelVideos(id);
    // await store.fetchChannelByHandle(username);
});
</script>
<template>
    <p>{{ route }}</p>
    <!-- <div class="container" v-if="channelVideos.length > 0">
        <div class="banner">
            <img src="" alt="">
        </div>
        <h1> {{ channelVideos[0].youtubeChannel!.title }}</h1>
        <p>{{ channelVideos[0].youtubeChannel!.handle }}</p>
        <p>{{ channelVideos[0].youtubeChannel!.subscribers }}</p>
        <p>{{ channelVideos[0].youtubeChannel!.videoCount }}</p>
        <p v-html="formatDescription(channelVideos[0].youtubeChannel!.description)"></p>
        <div class="container">
            <section class="videos">
                <VideoLink v-for="video of channelVideos" :video="video" />

            </section>
        </div>
    </div> -->
</template>

<style scoped lang="scss">
.red
{
    background-color: red;
    padding: 1rem;
}
.container {
    display: flex;
    flex-direction: column;
    flex-grow: 1;
    padding: 1rem;
    width: 100%;

    .videos {
        display: flex;
        gap: 1rem;
        flex-wrap: wrap;
        overflow: hidden;

    }
}
</style>