<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted } from 'vue';

const store = useYoutubeStore();

onMounted(() => {
    // store.fetchVideos();
})

</script>

<template>
    <div class="container">
        <div class="content">
            <VideoLink v-for="video of store.orderedVideos" :video="video" />
        </div>
    </div>
</template>

<style scoped lang="scss">
.container {
    display: flex;
    flex-grow: 1;
    // overflow: hidden;
    // justify-content: center;

    padding: 1rem;
    // max-width: 2256px;
    width: 100vw;
    // background-color: red;
    // margin: 0 auto;

    .content {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
        flex-direction: row;
        width: 100%;
        gap: 1rem;
        margin: 1rem;
    }



}
</style>
