<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import type YoutubeVideo from '@/models/YoutubeVideo';
import YoutubeChannelRepository from '@/repositories/YoutubeChannelRepository';
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import { useRepo } from 'pinia-orm';
import { computed, onMounted, type ComputedRef, ref, onBeforeUnmount, watch, nextTick } from 'vue';
import { useRoute } from 'vue-router';
import VideoRow from '@/components/VideoRow.vue';
import { formatDescription } from '@/utils';
import type YoutubeChannel from '@/models/YoutubeChannel';

const route = useRoute()

const channelId = computed(() => route.params.channelId) as ComputedRef<string>
const channelUsername = computed(() => route.params.username) as ComputedRef<string>
// const channel = computed(() => {
//     console.log(channelId.value, channelUsername.value)
//     if (channelId.value !== undefined)
//         return useRepo(YoutubeChannelRepository).getById(channelId.value)
//     if (channelUsername.value !== undefined)
//         return useRepo(YoutubeChannelRepository).getByHandle(channelUsername.value)
//     // return null
// })

const channel = ref<YoutubeChannel>()

const videos = computed(() => useRepo(YoutubeVideoRepository).withAll().where('youtubeChannelId', channel.value?.id).orderBy(vid => vid.uploaded).get()) as ComputedRef<YoutubeVideo[]>;

const onScroll = () => {
    parallax.value = window.scrollY
}


onMounted(async () => {
    console.log(channel.value)
    window.addEventListener('scroll', onScroll)
    await nextTick()
    if (channelId.value !== undefined) {
        console.log("fetchById")
        await useRepo(YoutubeChannelRepository).fetchById(channelId.value)
        channel.value = useRepo(YoutubeChannelRepository).getById(channelId.value)
    }
    if (channelUsername.value !== undefined) {
        console.log("fetchByHandle")
        await useRepo(YoutubeChannelRepository).fetchByHandle(channelUsername.value)
        channel.value = useRepo(YoutubeChannelRepository).getByHandle(`@${channelUsername.value}`)
    }
})
onBeforeUnmount(() => {
    window.removeEventListener('scroll', onScroll)
})
const parallax = ref<number>(-15)

const computedStyle = computed(() => {
    return {
        transform: `translateY(-${parallax.value / 2}px)`
    }
})
</script>

<template>
    <div>
        <div v-if="channel" class="channel">

            <img class="banner" :style="computedStyle" :src="channel?.bannerUrl" alt="">
            <div class="channel-content ">

                <div class="header">
                    <div class="avatar">
                        <img :src="channel.thumbnailUrl" alt="">
                    </div>
                    <div class="info">
                        <h1>{{ channel.title }}</h1>
                        <div class="stats">
                            {{ channel.handle }}&nbsp;
                            {{ channel.subscribers }} subscribers&nbsp;
                            {{ channel.videoCount }}&nbsp;
                        </div>
                        <div class="description" v-html="formatDescription(channel.description)"></div>

                    </div>
                </div>

                <div class="video-grid">
                    <VideoLink v-for="video in videos" :video="video" :key="video.id" />
                </div>

            </div>
            <!-- <VideoRow :videos="videos" :offset="0" />
                <VideoRow :videos="videos" :offset="1" /> -->
            <hr>
        </div>
    </div>
</template>

<style scoped lang="scss">
hr {
    margin-bottom: 15rem;
}

.channel {
    min-height: calc(100svh + 16vw);
}

.header {
    display: flex;
    gap: 1rem;
    margin-bottom: 2rem;
    align-items: center;

    h1 {
        font-size: 1.5rem;
        line-height: normal;
    }
}

.info {}

.stats {
    font-size: 14px;
    color: var(--text-color-soft);
    margin-bottom: 1rem;
    font-weight: 500;

}

.description {
    color: var(--text-color-soft);

}

.avatar {
    width: 175px;
    min-width: 125px;

    img {
        border-radius: 50%;
        object-fit: cover;
        object-position: center;
        width: 100%;
    }
}

.banner {
    width: 100%;
    height: 17vw;
    object-fit: contain;
    // object-position: center;
    position: sticky;
    top: 56px;
    // transform: translateY(v-bind(parallax));
    // transform: translateY(v-bind(parallax)+'px');
    // top: calc(56px + v-bind(parallax));
}

.channel-content {
    padding: 3rem;
    background-color: var(--background-color);
}

.video-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 1.5rem;
}
</style>