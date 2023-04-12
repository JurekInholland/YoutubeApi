<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const store = useYoutubeStore();

const username = route.params.username as string;

onMounted(async () => {
    document.title = `@${username}`;
    await store.fetchChannelByHandle(username);
});
</script>
<template>
    <h1>Channel</h1>
    <p>{{ username }}</p>
    <div class="container">
        <section class="videos">
            <VideoLink v-for="video of store.channelVideos(`@${username}`)" :video="video" />

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