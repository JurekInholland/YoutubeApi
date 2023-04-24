<script setup lang="ts">
import type YoutubeVideo from '@/models/YoutubeVideo';
import { useRepo } from 'pinia-orm'
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';


import VideoMetadata from "@/components/VideoMetadata.vue"
import YoutubePlayer from "@/components/YoutubePlayer.vue"
import { useYoutubeStore } from "@/stores/youtubeStore"
import { computed, nextTick, onBeforeUnmount, onMounted, ref, watch, type Ref } from "vue"
import { useResizeObserver } from "@vueuse/core"
import Sidebar from "@/components/Sidebar.vue"
import { useRoute } from "vue-router"
import { formatTitle } from "@/utils"
import VideoPlayer from "@/components/VideoPlayer.vue"
import NotFound from "@/components/NotFound.vue"
import type { PlayerState } from "@/types"
import { defaultState } from '@/constants';

const route = useRoute();
const store = useYoutubeStore();
const playerEl = ref<HTMLDivElement | null>(null) as any
const mounted = ref(false)
const calcH = ref("100%")
const found = computed(() => {
    return currentVid.value !== null && currentVid.value !== undefined
})

const videoRepo = useRepo(YoutubeVideoRepository);
const parsedId = computed(() => {
    const substring = route.path.substring(1);
    if (substring.length === 11) return substring
    const pathId = substring.split("/")[1]
    if (pathId !== undefined && pathId.length === 11) return pathId
    return route.query.v ? route.query.v as string : null
})

const aspectRatio = computed(() => {
    if (!currentVid.value) return 16 / 9
    return currentVid.value.width / currentVid.value.height
})
const currentVid = computed(() => {
    return videoRepo.getById(parsedId.value!) as YoutubeVideo
})

onMounted(() => {
    mounted.value = true
})

watch(route, async () => {
    // found.value = false
    // playerStats.value.currentTime = 0;
    if (parsedId.value === null) return
    await videoRepo.fetchById(parsedId.value);
    playerStats.value.useLocalPlayer = currentVid.value?.localVideo !== undefined
    if (currentVid.value && currentVid.value !== null && currentVid.value !== undefined) found.value = true
    // found.value = true
}, { immediate: true });



watch(currentVid, async (val) => {
    if (val === null) {
        await videoRepo.fetchById(parsedId.value!)
        // found.value = true
    }
    console.log("WATCH CURRENT VID", val)
    await nextTick()
    calculateHeight(playerEl.value?.$el.offsetWidth)
}, { immediate: true })

// function storeTestData() {
//     useRepo(Channel).save({
//         id: "test",
//         title: "test channel",
//         videos: [
//             { id: "vid1id", title: 'vid1', channelId: 'test' },
//             { id: "vid2id", title: 'vid2', channelId: 'test' },
//             { id: "vid3id", title: 'vid3', channelId: 'test' }
//         ]
//     })
// }
// const dbgVids = computed(() => {
//     // working?
//     return useRepo(Video).withAll().where('channelId', 'test').get()
//     // return useRepo(Channel).withAll().first()?.videos ?? []
// })

const playerStats = ref<PlayerState>({
    isPlaying: false,
    volume: 0.5,
    currentTime: 0,
    duration: 100,
    isFullscreen: false,
    storedVolume: 0.5,
    settings: false,
    cinema: false,
    pictureInPicture: false,
    useLocalPlayer: false
})
const togglePlayer = (val: boolean) => {
    console.log("toggle player" + val)
    playerStats.value.useLocalPlayer = val
}
const onPictureInPicture = (val: boolean) => {
    console.log("picture in picture" + val)

}
const toggleCinema = () => {
    playerStats.value.cinema = !playerStats.value.cinema
}
const calculateHeight = (width: number) => {
    if (!currentVid.value) return
    // let h = `calc(${width}px * (${currentVid.value!.height} / ${currentVid.value!.width}))`
    let h = `${Math.floor(width * (currentVid.value.height / currentVid.value.width))}px`
    if (playerStats.value.cinema) {
        h = "100%"
    }
    calcH.value = h
}
</script>
<template>
    <div>
        <div class="outer" v-if="found && mounted">
            <div class="container" :class="playerStats.cinema ? 'cinema' : ''" id="container1" ref="container">
                <div class="layout">
                    <div class="player-box">
                        <YoutubePlayer v-if="currentVid && !playerStats.useLocalPlayer && currentVid?.playableInEmbed"
                            :player-state="playerStats" ref="playerEl" v-bind="$attrs" class="player" id="player"
                            :videoId="currentVid.id" />

                        <VideoPlayer :player-state="playerStats" :cinema="playerStats.cinema"
                            v-else-if="playerStats.useLocalPlayer" ref="playerEl"
                            :src="`/api/LocalVideo/GetVideoStream?videoId=${currentVid?.id}`" :color="store.color"
                            @update:cinema="toggleCinema" @update:picture-in-picture="onPictureInPicture">

                        </VideoPlayer>
                        <div v-else-if="!currentVid!.playableInEmbed">NOT EMEBEDDABLE</div>
                        <div v-else>NOT FOUND</div>

                    </div>
                    <div class="meta-cont">
                        <VideoMetadata v-if="currentVid" class="metadata" @update:cinema="toggleCinema"
                            :cinema="playerStats.cinema" :useLocalPlayer="playerStats.useLocalPlayer"
                            :modelValue="playerStats.cinema" :video="currentVid" @update:custom-player="togglePlayer" />
                        <div id="primary" v-auto-animate></div>
                    </div>
                </div>
                <div id="tele" v-auto-animate v-if="!playerStats.cinema">
                </div>
            </div>
            <Sidebar :video="currentVid" key="sidebar" class="sidebar" v-if="mounted" :toggled="playerStats.cinema" />
        </div>
        <div v-else-if="!mounted">

        </div>
        <NotFound v-else />
    </div>
</template>


<style scoped lang="scss">
.outer {
    display: flex;
    justify-content: center;
    width: 100%;
    max-width: 100%;
    margin-top: var(--gutter-width);
}

.meta-cont {
    flex-grow: 1;
    width: 100%;
    max-width: var(--max-content-width);
    display: flex;
    flex-wrap: wrap;
}

#tele {
    flex-basis: 300px;
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    gap: calc(var(--gutter-width) / 3);
    flex-wrap: nowrap;
    margin-bottom: var(--gutter-width);
}

.player-box {
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    gap: var(--gutter-width);
    min-width: 360px;
    flex-grow: 0;
}

.player {
    width: 100%;
    height: 100%;
    max-height: var(--max-p-height);
    flex-basis: 100%;
    aspect-ratio: v-bind(aspectRatio);
    min-height: 320px;
}

.container {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    max-width: 1754px;
    margin: 0 var(--gutter-width);
    gap: var(--gutter-width);

    width: calc((100svh - 3 * var(--gutter-width)) * v-bind(aspectRatio) + 402px);

}

.cinema {
    justify-content: center;
    width: 100%;
    align-items: center;
    max-width: 100%;
    gap: 0;
    padding: 0;
    margin: 0;

    display: block;

    .player-box {
        background-color: black;
        aspect-ratio: initial;
    }

    .metadata {
        flex-basis: 640px;
        flex-grow: 1;
    }

    .meta-cont {
        width: min(calc((100svh - 3 * var(--gutter-width)) * v-bind(aspectRatio) + 402px), 1754px);
        max-width: calc(100% - 3rem);
        margin-left: 1.5rem;
        margin-right: 1.5rem;
        gap: var(--gutter-width);
        max-height: unset;
    }

    .layout {
        justify-content: center;
        align-items: center;
        overflow: hidden;
    }

    #primary {
        flex-grow: 1;
        display: flex;
        flex-direction: column;
        gap: calc(var(--gutter-width) / 3);
        max-width: 1754px;
        flex-basis: 300px;
        width: 100%;
    }

    #regular,
    .secondary {
        display: flex;
        flex-grow: 1;
    }

    .secondary {
        flex-basis: 300px;
    }


}

.layout {
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    flex-basis: 540px;
    flex-grow: 1;
    gap: var(--gutter-width);
    transition: all .5s ease;
}

#primary {
    flex: 1;
    flex-grow: 1;
}

.secondary {
    flex: 1;
    background-color: green;
    flex-grow: .5;
    min-width: 300px;
}

@media screen and (min-width: 1036px) {

    #tele {
        display: flex;
        flex-direction: column;
        flex-basis: 300px;
        flex-grow: 1;
        max-width: 402px;
        min-height: calc(100svh - 96px);
    }

    .cinema #tele {
        display: none;
    }

    .cinema #primary {
        max-width: 402px;
    }
}
</style>

<style>
:root {

    --nav-height: 56px;
    --max-p-height: calc(100svh - 204px);
    --max-player-height: calc(100svh - (var(--nav-height) + 2 * var(--gutter-width)));
    --max-content-width: calc(1280px + var(--sidebar-width) + 3 * var(--gutter-width));
    --min-player-height: 480px;
    --sidebar-width: 402px;
    --gutter-width: 1.5rem;
}

.video-container {
    max-height: var(--max-p-height);
}
</style>