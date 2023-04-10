
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


const relatedVideos: Ref<Array<YoutubeVideo>> = ref([])

onMounted(async () => {
    console.log("mounted")
    await store.fetchRelatedVideos();
})

watch(() => store.currentVideo, async () => {
    await store.fetchRelatedVideos();
}, { immediate: true })
</script>

<template>
    <div v-if="store.relatedVideos.length > 0">
        <Teleport :to="props.toggled ? '#primary' : '#tele'">
            <SidebarVideo v-for="vid of store.relatedVideos" :video="vid" :key="vid.id" />
        </Teleport>
    </div>
</template>

<style scoped lang="scss">
p {
    width: 100%;
}
</style>
