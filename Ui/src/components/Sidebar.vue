
<script setup lang="ts">
import { apiService } from '@/constants';
import type { YoutubeVideo } from '@/types';
import { onMounted, ref, watch, type Ref } from 'vue';
import SidebarVideo from '@/components/SidebarVideo.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
const store = useYoutubeStore();

const props = defineProps<{
    toggled: boolean,
}>()

const mounted = ref(false)

const relatedVideos: Ref<Array<YoutubeVideo>> = ref([])

onMounted(async () => {
    console.log("mounted")
    mounted.value = true;
    // await store.fetchRelatedVideos();
})

watch(() => store.currentVideo, async () => {
    if (store.currentVideo === null || store.currentVideo === undefined) {
        return;
    }
    console.log("FETCHING RELATED VIDEOS")
    await store.fetchRelatedVideos();
}, { immediate: true })
</script>

<template>
    <div v-if="store.relatedVideos !== null">
        <Teleport v-if="mounted" :to="props.toggled ? '#primary' : '#tele'">
            <SidebarVideo v-for="vid of store.relatedVideos" :video="vid" :key="vid.id" />
        </Teleport>
    </div>
</template>

<style scoped lang="scss">
p {
    width: 100%;
}
</style>
