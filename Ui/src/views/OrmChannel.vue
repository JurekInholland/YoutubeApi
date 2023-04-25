<script setup lang="ts">
import VideoLink from '@/components/VideoLink.vue';
import type YoutubeVideo from '@/models/YoutubeVideo';
import YoutubeChannelRepository from '@/repositories/YoutubeChannelRepository';
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import { useRepo } from 'pinia-orm';
import { computed, onMounted, type ComputedRef, ref, onBeforeUnmount, watch } from 'vue';
import { useRoute } from 'vue-router';
import VideoRow from '@/components/VideoRow.vue';
import { formatDescription } from '@/utils';

const route = useRoute()

const channelId = computed(() => route.params.channelId) as ComputedRef<string>
const channel = computed(() => useRepo(YoutubeChannelRepository).getById(channelId.value))

const videos = computed(() => useRepo(YoutubeVideoRepository).with('youtubeChannel').where('youtubeChannelId', channelId.value).orderBy(vid => vid.uploaded).get()) as ComputedRef<YoutubeVideo[]>;

const onScroll = () => {
    parallax.value = window.scrollY
}


onMounted(async () => {
    console.log(channel.value)
    window.addEventListener('scroll', onScroll)
    await useRepo(YoutubeChannelRepository).fetchById(channelId.value)
})
onBeforeUnmount(() => {
    window.removeEventListener('scroll', onScroll)
})
const parallax = ref<number>(-15)

const computedStyle = computed(() => {
    return {
        transform: `translateY(-${parallax.value / 3}px)`
    }
})
</script>

<template>
    <div>
        <div v-if="channel">

            <img class="banner" :style="computedStyle" :src="channel?.bannerUrl" alt="">
            <div class="channel-content ">

                <div class="header">

                    <img class="avatar" :src="channel.thumbnailUrl" alt="">
                    <div class="info">
                        <h1>{{ channel.title }}</h1>
                        <div>
                            {{ channel.handle }}&nbsp;
                            {{ channel.subscribers }} subscribers&nbsp;
                            {{ channel.videoCount }}&nbsp;
                        </div>
                        <div v-html="formatDescription(channel.description)"></div>

                    </div>
                </div>

                <VideoRow :videos="videos" :offset="0" />
                <VideoRow :videos="videos" :offset="1" />
                <hr>
            </div>
            <!-- <VideoLink v-for="video in videos" :video="video" :key="video.id" /> -->
        </div>
    </div>
</template>

<style scoped lang="scss">
hr
{
    margin-bottom: 15rem;
}
.header {
    display: flex;
    gap: 1rem;
    h1 {
        font-size: 1.5rem;
        line-height: normal;
    }
}

.avatar {

    border-radius: 50%;
    object-fit: cover;
    object-position: center;
}

.banner {
    width: 100%;
    height: 16vw;
    object-fit: contain;
    // object-position: center;
    position: sticky;
    top: 56px;
    // transform: translateY(v-bind(parallax));
    // transform: translateY(v-bind(parallax)+'px');
    // top: calc(56px + v-bind(parallax));
}

.channel-content {
    padding: 1rem;
    background-color: var(--background-color);
}
</style>