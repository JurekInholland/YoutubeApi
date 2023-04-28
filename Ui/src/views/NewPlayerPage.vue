<template>
  <div>
    <div class="outer" v-if="found">
      <div class="container" :class="playerStats.cinema ? 'cinema' : ''" id="container1" ref="container">
        <div class="layout">
          <div class="player-box">
            <YoutubePlayer v-if="store.currentVideo && !playerStats.useLocalPlayer && store.currentVideo?.playableInEmbed"
              :player-state="playerStats" ref="playerEl" v-bind="$attrs" class="player" id="player"
              :videoId="store.currentVideo.id" />

            <VideoPlayer :player-state="playerStats" :cinema="playerStats.cinema" v-else-if="playerStats.useLocalPlayer"
              ref="playerEl" :src="`/api/LocalVideo/GetVideoStream?videoId=${store.currentVideo?.id}`"
              :color="store.color" @update:cinema="toggleCinema" @update:picture-in-picture="onPictureInPicture">

            </VideoPlayer>
            <div v-else-if="!store.currentVideo!.playableInEmbed">NOT EMEBEDDABLE</div>
            <div v-else>NOT FOUND</div>

          </div>
          <div class="meta-cont">
            <VideoMetadata v-if="store.currentVideo" class="metadata" @update:cinema="toggleCinema"
              :cinema="playerStats.cinema" :useLocalPlayer="playerStats.useLocalPlayer" :modelValue="playerStats.cinema"
              :video="store.currentVideo" @update:custom-player="togglePlayer" />
            <div id="primary" v-auto-animate></div>
          </div>
        </div>
        <div id="tele" v-auto-animate v-if="!playerStats.cinema">
        </div>
      </div>
      <Sidebar key="sidebar" class="sidebar" v-if="mounted" :toggled="playerStats.cinema" />
    </div>
    <NotFound v-else />
  </div>
</template>
<script async setup lang="ts">
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

// const toggle = ref(false)

const useLocalPlayer = ref(false)
const found = ref(false)
const route = useRoute()
const store = useYoutubeStore()
const calcH = ref("100%")

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


const startTime = computed(() => {
  return route.query.t ? parseInt(route.query.t as string) : 0
})

const togglePlayer = (val: boolean) => {
  console.log("toggle player" + val)
  playerStats.value.useLocalPlayer = val
}
const onPictureInPicture = (val: boolean) => {
  console.log("picture in picture" + val)

}
const parsedId = computed(() => {
  const substring = route.path.substring(1);
  if (substring.length === 11) return substring
  const pathId = substring.split("/")[1]
  if (pathId !== undefined && pathId.length === 11) return pathId
  return route.query.v ? route.query.v as string : null
})

watch(route, async () => {
  found.value = false
  playerStats.value.currentTime = 0;
  if (parsedId.value === null) return

  await store.fetchCurrentVideo(parsedId.value!)
  playerStats.value.useLocalPlayer = store.currentVideo?.localVideo !== undefined
  if (store.currentVideo && store.currentVideo !== null && store.currentVideo !== undefined) found.value = true

}, { immediate: true })

watch(() => store.currentVideo, async () => {
  if (store.currentVideo === null || store.currentVideo === undefined) return
  useLocalPlayer.value = store.currentVideo?.localVideo !== undefined


  document.title = formatTitle(store.currentVideo.title)
  await nextTick()
  calculateHeight(playerEl.value?.$el.offsetWidth)
}, { immediate: true })


watch(() => playerStats.value.cinema, async () => {
  console.log("playerStats.cinema")
}, { immediate: true })

const playerEl = ref<HTMLDivElement | null>(null) as any
const mounted = ref(false)

useResizeObserver(playerEl, (entries) => {
  // console.log("RESIZE OBSERVER", entries[0].contentRect.width)
  calculateHeight(entries[0].contentRect.width)
})

// onBeforeMount(() => {
//     mounted.value = false;
// })
onMounted(() => {
  mounted.value = true
  // if (store.currentVideo !== null) found.value = true;
  calculateHeight(playerEl.value?.$el.offsetWidth)
  // setTimeout(() => {
  //     mounted.value = true;
  // }, 1000)
})
onBeforeUnmount(() => {
  mounted.value = false
})

const calculateHeight = (width: number) => {
  if (!store.currentVideo) return
  // let h = `calc(${width}px * (${store.currentVideo!.height} / ${store.currentVideo!.width}))`
  let h = `${Math.floor(width * (store.currentVideo.height / store.currentVideo.width))}px`
  if (playerStats.value.cinema) {
    h = "100%"
  }
  // console.log("CALCULATING HEIGHT", h, toggle.value)
  calcH.value = h
}

const aspectRatio = computed(() => {
  if (!store.currentVideo) return 16 / 9
  return store.currentVideo.width / store.currentVideo.height
})

const toggleCinema = () => {
  playerStats.value.cinema = !playerStats.value.cinema
}

</script>


<style scoped lang="scss">
.outer {
  display: flex;
  justify-content: center;
  width: 100%;
  max-width: 100%;
  margin-top: var(--gutter-width);
  // min-height: calc(100vh - 55px);
}

.meta-cont {
  // height: 100%;
  flex-grow: 1;
  width: 100%;
  max-width: var(--max-content-width);
  // max-height: 100vh;
  display: flex;
  flex-wrap: wrap;
}

// .sidebar {
//     display: flex;
//     flex-direction: row;
// }

#tele {
  flex-basis: 300px;
  flex-grow: 1;
  display: flex;
  flex-direction: column;
  gap: calc(var(--gutter-width) / 3);
  flex-wrap: nowrap;
  margin-bottom: var(--gutter-width);
  // height: 100%;
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
