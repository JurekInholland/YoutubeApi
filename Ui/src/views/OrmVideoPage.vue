<script setup lang="ts">
import type YoutubeVideo from '@/models/YoutubeVideo';
import { computed, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useRepo } from 'pinia-orm'
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import Sidebar from '@/components/Sidebar.vue';
import Channel from '@/models/DebugChannel';
import Video from '@/models/DebugVideo';
const route = useRoute();

const videoRepo = useRepo(YoutubeVideoRepository);
const parsedId = computed(() => {
    const substring = route.path.substring(1);
    if (substring.length === 11) return substring
    const pathId = substring.split("/")[1]
    if (pathId !== undefined && pathId.length === 11) return pathId
    return route.query.v ? route.query.v as string : null
})

watch(route, async () => {
}, { immediate: true });


const currentVid = computed(() => {
    return videoRepo.getById(parsedId.value!) as YoutubeVideo
})

watch(currentVid, async (val) => {
    if (val === null) {
        videoRepo.fetchById(parsedId.value!)
    }
    console.log("WATCH CURRENT VID", val)
}, { immediate: true })

function storeTestData() {
    useRepo(Channel).save({
        id: "test",
        title: "test channel",
        videos: [
            { id: "vid1id", title: 'vid1', channelId: 'test' },
            { id: "vid2id", title: 'vid2', channelId: 'test' },
            { id: "vid3id", title: 'vid3', channelId: 'test' }
        ]
    })
}
const dbgVids = computed(() => {
    // return useRepo(Channel).find('test')?.videos ?? []
    // return useRepo(Channel).withAll().first()?.videos ?? []

    // working?
    return useRepo(Video).withAll().where('channelId', 'test').get()
    // return useRepo(Channel).withAll().first()?.videos ?? []

})
</script>
<template>
    <div>
        <button @click="storeTestData">
            Test video/channel
        </button>

        <div v-for="vid in dbgVids">
            {{ vid.title }}
        </div>

        <p>Video: {{ currentVid?.title }}</p>

        <div class="links">
            <router-link v-for="rel in currentVid?.relatedVideos" :to="{ name: 'debugormvideo', params: { videoId: rel } }">
                {{ rel }}
            </router-link>
        </div>
        <div id="primary">

        </div>
        <div id="#tele">

        </div>
        <Sidebar :video="currentVid" :toggled="true" />
    </div>
</template>

<style scoped lang="scss">
.links {
    display: flex;
    flex-direction: column;
}
</style>