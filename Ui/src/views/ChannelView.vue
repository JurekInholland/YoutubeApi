<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted, watch } from 'vue';
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

onMounted(async () => {
    document.title = `@${username}`;
    console.log("CH ID", id)
    await store.fetchChannelById(id);
    // await store.fetchChannelByHandle(username);
});
</script>
<template>
    <h1>Channel</h1>
    <p>{{ username }}</p>
    <div class="container">
        <section class="videos">
            <!-- <VideoLink v-for="video of store.videos" :video="video" /> -->
            <VideoLink v-for="video of store.channelVideos(id)" :video="video" />

        </section>
    </div>
</template>

<style scoped lang="scss">
.container {
    display: flex;
    flex-direction: column;
    flex-grow: 1;
    padding: 1rem;
    width: 100vw;

    .videos {
        display: flex;
        gap: 1rem;
        flex-wrap: wrap;
        overflow: hidden;

    }
}
</style>