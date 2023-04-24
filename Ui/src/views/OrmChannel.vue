<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import type YoutubeVideo from '@/models/YoutubeVideo';
import YoutubeChannelRepository from '@/repositories/YoutubeChannelRepository';
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import { useRepo } from 'pinia-orm';
import { computed, onMounted, type ComputedRef, ref, onBeforeUnmount } from 'vue';
import { useRoute } from 'vue-router';
import VideoRow from '@/components/VideoRow.vue';
const route = useRoute()

const channelId = computed(() => route.params.channelId) as ComputedRef<string>
const channel = computed(() => useRepo(YoutubeChannelRepository).getById(channelId.value))

const videos = computed(() => useRepo(YoutubeVideoRepository).with('youtubeChannel').where('youtubeChannelId', channelId.value).orderBy(vid => vid.uploaded).get()) as ComputedRef<YoutubeVideo[]>;
const loading = ref<boolean>(false)

const scrollOffset = computed(() => {
    return document.body.scrollHeight - window.innerHeight - window.scrollY;
})

const onScroll = () => {
    window.requestAnimationFrame(() => {
        if (scrollOffset.value < 100) {
            console.log("scrolled")
        }
    })
    console.log("scrl")
}

onMounted(async () => {
    console.log(channel.value)
    window.addEventListener('scroll', onScroll, { passive: true })
    await useRepo(YoutubeChannelRepository).fetchById(channelId.value)

})
onBeforeUnmount(() => {
    window.removeEventListener('scroll', onScroll)
})

</script>

<template>
    <div>
        <div v-if="channel">
            <img class="banner" :src="channel.bannerUrl" alt="">
            <p>{{ channel.title }}</p>
            <p>{{ channel.description }}</p>
            <img :src="channel.thumbnailUrl" alt="">

            <VideoRow :videos="videos" :offset="0" />
            <VideoRow :videos="videos" :offset="1" />
            <hr>
            <!-- <VideoLink v-for="video in videos" :video="video" :key="video.id" /> -->
        </div>
    </div>
</template>

<style scoped lang="scss">
.banner {
    width: 100%;
    height: 16vw;
    object-fit: cover;
    position: sticky;
    top: 56px;
}
</style>