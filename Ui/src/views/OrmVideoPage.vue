<script setup lang="ts">
import type YoutubeVideo from '@/models/YoutubeVideo';
import { useRepo } from 'pinia-orm'
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import VideoMetadata from "@/components/VideoMetadata.vue"
import YoutubePlayer from "@/components/YoutubePlayer.vue"
import { useYoutubeStore } from "@/stores/youtubeStore"
import { computed, nextTick, onBeforeUnmount, onMounted, ref, watch, type Ref } from "vue"
import Sidebar from "@/components/Sidebar.vue"
import { useRoute, useRouter } from "vue-router"
import VideoPlayer from "@/components/VideoPlayer.vue"
import NotFound from "@/components/NotFound.vue"
import type { PlayerState } from "@/types"


const route = useRoute();
const store = useYoutubeStore();
const playerEl = ref<HTMLDivElement | null>(null) as any
const mounted = ref(false)
const calcH = ref("100%")
const found = computed(() => {
    return currentVid && currentVid.value !== null && currentVid.value !== undefined
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
    return videoRepo.getById(parsedId.value!)
})

const relatedVideos = computed(() => {
    if (!currentVid.value) return []
    return videoRepo.getRelatedVideos(currentVid.value.relatedVideos)
})

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
    useLocalPlayer: false,
    width: 1280,
    height: 720
})
onMounted(() => {
    mounted.value = true
})

watch(route, async () => {
    playerStats.value.currentTime = 0;
    playerStats.value.duration = 100;
    if (parsedId.value === null) return
    if (!currentVid)
        await videoRepo.fetchById(parsedId.value);
}, { immediate: true });



watch(currentVid, async (val) => {
    if (val === null) {
        await videoRepo.fetchById(parsedId.value!)
        return;
    }
    console.log("WATCH CURRENT VID", val)
    await nextTick()
    calculateHeight(playerEl.value?.$el.offsetWidth)
    playerStats.value.width = val.width
    playerStats.value.height = val.height
    playerStats.value.useLocalPlayer = val.localVideo !== null
}, { immediate: true })



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
            <div class="container" :class="playerStats.cinema ? 'cinema' : ''" ref="container">
                <div class="layout">
                    <div class="player-box">
                        <YoutubePlayer v-if="currentVid && !playerStats.useLocalPlayer && currentVid?.playableInEmbed"
                            :player-state="playerStats" ref="playerEl" v-bind="$attrs" class="player" id="player"
                            :videoId="currentVid.id" :related-videos="relatedVideos" />

                        <VideoPlayer :player-state="playerStats" :cinema="playerStats.cinema"
                            :related-videos="relatedVideos" v-else-if="playerStats.useLocalPlayer" ref="playerEl"
                            :src="`/api/LocalVideo/GetVideoStream?videoId=${currentVid?.id}`" :color="store.color"
                            @update:cinema="toggleCinema" @update:picture-in-picture="onPictureInPicture" />

                        <div v-else-if="!currentVid!.playableInEmbed">NOT EMEBEDDABLE</div>
                        <div v-else>NOT FOUND</div>

                    </div>
                    <div class="meta-cont">
                        <VideoMetadata v-if="currentVid" class="metadata" @update:cinema="toggleCinema"
                            :cinema="playerStats.cinema" :useLocalPlayer="playerStats.useLocalPlayer"
                            :modelValue="playerStats.cinema" :video="currentVid" @update:custom-player="togglePlayer" />
                        {{ playerStats }}
                        <div id="primary-sidebar"></div>
                    </div>
                </div>
                <div id="cinema-sidebar" v-if="!playerStats.cinema">
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

#cinema-sidebar {
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

    #primary-sidebar {
        flex-grow: 1;
        display: flex;
        flex-direction: column;
        gap: calc(var(--gutter-width) / 3);
        max-width: 1754px;
        flex-basis: 300px;
        width: 100%;
        margin-bottom: 1.5rem;
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
    // transition: all .5s ease;
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

    #cinema-sidebar {
        display: flex;
        flex-direction: column;
        flex-basis: 300px;
        flex-grow: 1;
        max-width: 402px;
        min-height: calc(100svh - 96px);
    }

    .cinema #cinema-sidebar {
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
