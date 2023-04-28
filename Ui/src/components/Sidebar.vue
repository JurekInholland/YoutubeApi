<script setup lang="ts">
import { computed, onMounted, ref, watch, type Ref, type ComputedRef } from "vue"
import SidebarVideo from "@/components/SidebarVideo.vue"
import { useYoutubeStore } from "@/stores/youtubeStore"
import type YoutubeVideo from "@/models/YoutubeVideo"
import YoutubeVideoRepository from "@/repositories/YoutubeVideoRepository"
import { useRepo } from "pinia-orm"
import Spinner from "@/components/Spinner.vue"
import { useRoute } from "vue-router"
import { watchDebounced } from "@vueuse/core"

const store = useYoutubeStore()
const route = useRoute()
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

watchDebounced(() => props.video.id, async (val) => {
    console.log("FETCHING RELATED VIDEOS", val)
    await videoRepo.fetchRelatedVideos(props.video.relatedVideos)
}, { immediate: true, debounce: 1000, maxWait: 1000 })

// watch(() => store.currentVideo, async () => {
//     if (store.currentVideo === null || store.currentVideo === undefined) {
//         return
//     }
//     console.log("FETCHING RELATED VIDEOS")
//     await store.fetchRelatedVideos()
// }, { immediate: true })

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
    <div v-if="props.video.relatedVideos !== null">
        <Teleport v-if="mounted" :to="props.toggled ? '#primary-sidebar' : '#cinema-sidebar'">
            <div class="sidebar" v-auto-animate>
                <SidebarVideo v-for="vid of sidebarVideos" :video="vid" :key="vid.id" />
            </div>
            <div v-if="spinnerVisible" class="spinner-container">
                <Spinner class="spinner" />
            </div>
        </Teleport>
    </div>
</template>

<style scoped lang="scss">
.sidebar {
    display: flex;
    flex-direction: column;
    gap: .33rem;
}

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
