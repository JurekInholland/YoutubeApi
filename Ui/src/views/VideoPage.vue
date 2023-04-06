<script setup lang="ts">
import YoutubePlayer from '@/components/YoutubePlayer.vue';
import VideoMetadata from '@/components/VideoMetadata.vue';
import SidebarVideo from '@/components/SidebarVideo.vue';

import type { IRelatedVideo, IVideo } from '@/models';
import { computed, onMounted, ref, type Ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import VideoPlayer from '@/components/VideoPlayer.vue';
import { apiService } from '@/constants';
import { AxiosError } from 'axios';
import type { YoutubeVideo } from '@/types';
import { useYoutubeStore } from '@/stores/youtubeStore';
const route = useRoute()
const store = useYoutubeStore()

const parsedId = computed(() => {
    var split = route.path.substring(1).split("?")[0]
    if (split.length === 11) return split
    return route.query.v ? route.query.v as string : null
})

const startTime = computed(() => {
    return route.query.t ? parseInt(route.query.t as string) : 0
})

const fetchVideo = async (id: string): Promise<YoutubeVideo | undefined> => {
    const vid = await apiService.GetVideoInfo(id);
    if (vid instanceof Error) {
        console.log("ERROR", vid)
        return undefined;
    }
    else {
        // video.value = vid;
        return vid;
    }
}

// const video: Ref<YoutubeVideo | undefined> = ref();

const vids: Ref<Array<IRelatedVideo>> = ref([])

watch(() => route.path, async () => {
    console.log("watch parsedId", parsedId.value)
    store.fetchCurrentVideo(parsedId.value!)
    // vids.value = await fetch(`/api/Info/GetRelatedYoutubeVideos?videoId=${store.currentVideo.id}`,).then(res => res.json())
}, { immediate: true })

watch(() => store.currentVideo, async () => {
    // if (store.currentVideo != undefined) {
    //     vids.value = await fetch(`/api/Info/GetRelatedYoutubeVideos?videoId=${store.currentVideo.id}`,).then(res => res.json())
    // }
}, { immediate: true })

onMounted(async () => {
    if (store.currentVideo === undefined) {
        console.log("VIDEO IS NULL")

        // store.currentVideo = await fetch(`/api/Info/GetVideoInfo?videoId=${parsedId.value}`,).then(res => res.json())
    }
    else {
        console.log("VIDEO IS NOT NULL", store.currentVideo)
    }
    // vids.value = await fetch(`/api/Info/GetRelatedYoutubeVideos?videoId=${video.id}`,).then(res => res.json())
})

</script>

<template>
    <div id="container">
        <div id="primary">
            <YoutubePlayer v-if="parsedId" :video-id="parsedId" :start-time="startTime" />
            <!-- <VideoPlayer src="./vid.webm" color="green" /> -->
            <VideoMetadata v-if="store.currentVideo" :video="store.currentVideo" />
            <div v-else>LOADING</div>
        </div>

        <div id="secondary">
            <p>sidebar</p>
            <!-- <SidebarVideo v-for="vid in vids" :video="vid" /> -->
        </div>
    </div>
</template>

<style scoped>
#container {
    display: flex;
    flex-direction: row;
    height: 100%;
    width: calc(100% - 3rem);
    max-width: calc(1280px + 402px + 1.5rem);
    justify-content: center;
    justify-self: center;
    min-width: calc(240px * (16 / 9));
    gap: 1.5rem;
    margin-inline: auto;
    margin-top: 1.5rem;
    flex-flow: wrap;
}

#primary {
    flex: 1;
    max-width: 1280px;
    flex-basis: 640px;
    flex-grow: 12.5;
}

#secondary {
    min-width: 300px;
    flex-basis: 300px;
    flex-grow: 1;
    background: #333;
    display: flex;
    flex-direction: column;
    gap: 4px;
}
</style>