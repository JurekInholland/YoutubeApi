<script setup lang="ts">
import { computed, onMounted, ref, watch, type Ref, type ComputedRef } from "vue"
import SidebarVideo from "@/components/SidebarVideo.vue"
import { useYoutubeStore } from "@/stores/youtubeStore"
import type YoutubeVideo from "@/models/YoutubeVideo"
import YoutubeVideoRepository from "@/repositories/YoutubeVideoRepository"
import { useRepo } from "pinia-orm"
import Spinner from "@/components/Spinner.vue"

const store = useYoutubeStore()

const videoRepo = useRepo(YoutubeVideoRepository)

const props = defineProps<{
    toggled: boolean,
    video: YoutubeVideo
}>()

const mounted = ref(false)
const spinnerVisible = ref(false)
const sidebarVideos: ComputedRef<YoutubeVideo[]> = computed(() => videoRepo.getRelatedVideos(props.video.relatedVideos))

onMounted(async () => {
    console.log("mounted")
    mounted.value = true
})

watch(() => props.video, async () => {
    console.log("FETCHING RELATED VIDEOS")
    await videoRepo.fetchRelatedVideos(props.video.relatedVideos)
}, { immediate: true })

watch(() => store.currentVideo, async () => {
    if (store.currentVideo === null || store.currentVideo === undefined) {
        return
    }
    console.log("FETCHING RELATED VIDEOS")
    await store.fetchRelatedVideos()
}, { immediate: true })

watch(sidebarVideos, () => {
    if (sidebarVideos.value.length > 10) {
        spinnerVisible.value = false
        return
    }
    spinnerVisible.value = true
    setTimeout(() => {
        spinnerVisible.value = false

    }, 5000)
})
</script>

<template>
    <div v-if="store.relatedVideos !== null">
        <Teleport v-if="mounted" :to="props.toggled ? '#primary' : '#tele'">
            <SidebarVideo v-for="vid of sidebarVideos" :video="vid" :key="vid.id" />
            <div v-if="spinnerVisible" class="spinner-container">
                <Spinner class="spinner" />
            </div>
        </Teleport>
    </div>
</template>

<style scoped lang="scss">
p {
  width: 100%;
}

.spinner-container {
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
