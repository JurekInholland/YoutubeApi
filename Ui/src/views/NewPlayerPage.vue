<template>
  <div>
    <!-- <button @click="inputChange" style="color: white;">test btn</button> -->
    <div class="outer" v-if="found">
      <div class="container" :class="toggle ? 'cinema' : ''" id="container1" ref="container">
        <div class="layout">
          <div class="player-box">
            <YoutubePlayer v-if="store.currentVideo && !useLocalPlayer && store.currentVideo?.playableInEmbed"
              ref="playerEl" v-bind="$attrs" class="player" id="player" :videoId="store.currentVideo.id"
              :start-time="startTime" :aspect-ratio="store.currentVideo.width / store.currentVideo.height" />

            <VideoPlayer :cinema="toggle" v-else-if="useLocalPlayer" ref="playerEl"
              :src="`/api/LocalVideo/GetVideoStream?videoId=${store.currentVideo?.id}`" :color="store.color"
              @update:cinema="toggle = !toggle" @update:picture-in-picture="onPictureInPicture"
              >

            </VideoPlayer>
            <div v-else-if="!store.currentVideo!.playableInEmbed">NOT EMEBEDDABLE</div>
            <div v-else>NOT FOUND</div>

          </div>
          <div class="meta-cont">
            <VideoMetadata v-if="store.currentVideo" class="metadata" @update:model-value="toggle = !toggle"
              :modelValue="toggle" :video="store.currentVideo" @update:custom-player="togglePlayer" />
            <div id="primary" v-auto-animate></div>
          </div>
        </div>
        <div id="tele" v-auto-animate>
        </div>

        <!-- <teleport v-if="mounted" :to="toggle ? '#primary' : '#tele'">
                      <div class="secondary">
                          this is the secondary
                      </div>
                  </teleport> -->
      </div>
      <Sidebar key="sidebar" class="sidebar" v-if="mounted" :toggled="toggle" />
    </div>
    <NotFound v-else />
  </div>
</template>
<script async setup lang="ts">
import VideoMetadata from "@/components/VideoMetadata.vue"
import YoutubePlayer from "@/components/YoutubePlayer.vue"
import { useYoutubeStore } from "@/stores/youtubeStore"
import { computed, nextTick, onBeforeUnmount, onMounted, ref, watch } from "vue"
import { useResizeObserver } from "@vueuse/core"
import Sidebar from "@/components/Sidebar.vue"
import { useRoute } from "vue-router"
import { formatTitle } from "@/utils"
import VideoPlayer from "@/components/VideoPlayer.vue"
import NotFound from "@/components/NotFound.vue"
import { useFavicon } from '@vueuse/core'

const toggle = ref(false)

const useLocalPlayer = ref(false)
const found = ref(false)
const route = useRoute()
const store = useYoutubeStore()
const calcH = ref("100%")


const icon = useFavicon()

const startTime = computed(() => {
  return route.query.t ? parseInt(route.query.t as string) : 0
})

const togglePlayer = (val: boolean) => {
  console.log("toggle player" + val)
  useLocalPlayer.value = val
}
const onPictureInPicture = (val: boolean) => {
  console.log("picture in picture" + val)

}
const parsedId = computed(() => {
  const split = route.path.substring(1).split("?")[0]
  if (split.length === 11) return split
  return route.query.v ? route.query.v as string : null
})

watch(() => route.path, async () => {
  console.log("watch parsedId", parsedId.value)
  useLocalPlayer.value = store.currentVideo?.localVideo !== undefined



  await store.fetchCurrentVideo(parsedId.value!)
  if (store.currentVideo !== null) found.value = true

}, { immediate: true })

watch(() => store.currentVideo, async () => {
  await nextTick()
  if (!store.currentVideo) return
  useLocalPlayer.value = store.currentVideo?.localVideo !== undefined


  document.title = formatTitle(store.currentVideo.title)
  calculateHeight(playerEl.value?.$el.offsetWidth)
}, { immediate: true })


watch(() => toggle.value, async () => {
  console.log("Toggle")
}, { immediate: true })

const playerEl = ref<HTMLDivElement | null>(null) as any
const mounted = ref(false)

const isDisabled = () => {
  const res = !mounted.value || !toggle.value
  console.log("is disabled", res, mounted.value, toggle.value)
  return res
}
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
  if (toggle.value) {
    h = "100%"
  }
  // console.log("CALCULATING HEIGHT", h, toggle.value)
  calcH.value = h
}

const aspectRatio = computed(() => {
  if (!store.currentVideo) return 16 / 9
  return store.currentVideo.width / store.currentVideo.height
})
</script>


<style scoped lang="scss">
.outer {
  display: flex;
  justify-content: center;
  // min-height: calc(100vh - 55px);
}

.meta-cont {
  // height: 100%;
  flex-grow: 1;
  width: 100%;
  max-width: var(--max-content-width);
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
  // height: 100%;
}

.player-box {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  gap: var(--gutter-width);
  max-height: min(v-bind(calcH), var(--max-p-height));
  // max-width: var(--max-player-width);
  // max-height: var(--max-p-height);
  min-width: 360px;
  flex-grow: 1;

  // max-height: var(--max-player-height);
}

.player {
  width: 100%;
  height: 100%;
  // height: calc(100vw * 9 / 16);
  // max-height: v-bind(calcH);
  // max-height: calc(100% - 2 * var(--gutter-width));
  max-height: var(--max-p-height);
  flex-basis: 100%;
  aspect-ratio: v-bind(aspectRatio);
  min-height: 320px;

  // width: 100%;
  // height: auto;
}

.container {
  // background-color: darkblue;
  // height: 100vh;
  // width: 100vw;
  // max-width: 100%;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  // justify-content: center;
  flex-grow: 1;
  // gap: var(--gutter-width);
  max-width: 1754px;
  margin: 0 var(--gutter-width);
  gap: var(--gutter-width);

}

.cinema {
  justify-content: center;
  width: 100%;
  align-items: center;
  max-width: 100vw;
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
    // max-width: calc(100vw - 2 * var(--gutter-width));
    max-width: min(calc(100vw - 3 * var(--gutter-width)), 1754px);
    gap: var(--gutter-width);
    // margin: 0 var(--gutter-width);
    // padding-right: 3rem;

  }

  .layout {
    justify-content: center;
    align-items: center;
  }

  .player {
    max-height: var(--max-p-height);
  }

  // .layout {
  //     max-width: calc(1280px + var(--sidebar-width) + 3 * var(--gutter-width));
  // }
  #primary {
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    // justify-content: center;
    // flex-direction: row;
    // flex-wrap: wrap;
    // margin: 0 1rem;
    gap: calc(var(--gutter-width) / 3);
    // margin: 0 1rem;
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
  //flex-direction: row;
  flex-wrap: wrap;
  flex-basis: 640px;
  flex-grow: 1;
  gap: var(--gutter-width);
  transition: all .5s ease;
  // max-height: calc(100% - 50vh);
  // margin-right: 1rem;
}

#primary {
  flex: 1;
  flex-grow: 1;
  height: 100%;
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
  }

  .cinema #primary {
    max-width: 402px;
  }
}
</style>

<style>
:root {

  --nav-height: 56px;
  --max-p-height: calc(100vh - 172px);
  /* --max-player-width: calc(100vw - (var(var(--nav-height)) + 2 * var(--gutter-width))); */


  --max-player-width: calc(100vw);

  --max-player-height: calc(100vh - (var(--nav-height) + 2 * var(--gutter-width)));

  --max-content-width: calc(1280px + var(--sidebar-width) + 3 * var(--gutter-width));

  --min-player-height: 480px;
  --sidebar-width: 402px;
  --gutter-width: 1.5rem;
}

.video-container {
  max-height: var(--max-p-height);
}
</style>
