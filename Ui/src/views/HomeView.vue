<script setup lang="ts">
import VideoRow from '@/components/VideoRow.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const store = useYoutubeStore();
const route = useRoute()

const path = route.path;

onMounted(() => {
    document.title = "Youtube Clone"
})
</script>

<template>
    <main>
        <!-- <h1>home</h1>
        <p>{{ route }}</p> -->

        <VideoRow :videos="store.videos" />
        <VideoRow :videos="store.videos" :offset="1" />
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
    padding: 0 2rem;
    gap: 2rem;
    flex-grow: 1;
}
hr {
    border-color: var(--light-grey);
}
</style>
