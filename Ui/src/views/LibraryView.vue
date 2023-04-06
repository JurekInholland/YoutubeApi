<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted } from 'vue';

const store = useYoutubeStore();

onMounted(() => {
    store.fetchVideos();
})

</script>

<template>
    <div class="container">
        <div class="content">
            <VideoLink v-for="video in store.orderedVideos" :video="video" />
        </div>
    </div>
</template>

<style scoped lang="scss">
.container {
    display: flex;
    // overflow: hidden;
    justify-content: center;

    padding: 1rem;
    max-width: 2256px;
    // background-color: red;
    margin: 0 auto;

    .content {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
        flex-direction: row;

        gap: 1rem;
    }



}
</style>
