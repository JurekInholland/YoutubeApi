<template>
    <!-- <button @click="inputChange" style="color: white;">test btn</button> -->
    <div class="outer">
        <div class="container" :class="toggle ? 'cinema' : ''" id="container1" ref="container">
            <div class="layout">
                <div class="player-box">
                    <YoutubePlayer v-if="store.currentVideo" ref="playerEl" v-bind="$attrs" class="player" id="player"
                        :videoId="store.currentVideo.id" :start-time="30" :aspect-ratio="16 / 9" />

                </div>
                <div class="meta-cont">
                    <VideoMetadata v-if="store.currentVideo" class="metadata" @update:model-value="toggle = !toggle"
                        :modelValue="toggle" :video="store.currentVideo" />
                    <div id="primary"></div>
                </div>
            </div>
            <div id="tele">
            </div>

            <!-- <teleport v-if="mounted" :to="toggle ? '#primary' : '#tele'">
            <div class="secondary">
                this is the secondary
            </div>
        </teleport> -->
            <Sidebar key="sidebar" class="sidebar" v-if="mounted" :toggled="toggle" />
        </div>
    </div>
</template>
<script async setup lang="ts">
import VideoMetadata from '@/components/VideoMetadata.vue';
import YoutubePlayer from '@/components/YoutubePlayer.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { computed, nextTick, onBeforeUnmount, onMounted, ref, watch } from 'vue';
import { useResizeObserver } from '@vueuse/core'
import Sidebar from '@/components/Sidebar.vue';
import { useRoute } from 'vue-router';
import { formatTitle } from '@/utils';
const toggle = ref(false);

const route = useRoute();
const store = useYoutubeStore();
const calcH = ref("100%")
const startTime = computed(() => {
    return route.query.t ? parseInt(route.query.t as string) : 0
})
const parsedId = computed(() => {
    const split = route.path.substring(1).split("?")[0]
    if (split.length === 11) return split
    return route.query.v ? route.query.v as string : null
})
watch(() => route.path, async () => {
    console.log("watch parsedId", parsedId.value)
    await store.fetchCurrentVideo(parsedId.value!)


}, { immediate: true })

watch(() => store.currentVideo, async () => {
    await nextTick();
    document.title = formatTitle(store.currentVideo!.title)
    calculateHeight(playerEl.value?.$el.offsetWidth)
}, { immediate: true })


const playerEl = ref<HTMLDivElement | null>(null) as any;
const mounted = ref(false);

const isDisabled = () => {
    const res = !mounted.value || !toggle.value
    console.log("is disabled", res, mounted.value, toggle.value)
    return res
}
useResizeObserver(playerEl, (entries) => {
    console.log("RESIZE OBSERVER", entries[0].contentRect.width)
    calculateHeight(entries[0].contentRect.width)
})

// onBeforeMount(() => {
//     mounted.value = false;
// })
onMounted(() => {
    mounted.value = true;
    calculateHeight(playerEl.value?.$el.offsetWidth)
    // setTimeout(() => {
    //     mounted.value = true;
    // }, 1000)
})
onBeforeUnmount(() => {
    mounted.value = false;
})

const calculateHeight = (width: number) => {
    // let h = `calc(${width}px * (${store.currentVideo!.height} / ${store.currentVideo!.width}))`
    let h = `${Math.floor(width * (store.currentVideo!.height / store.currentVideo!.width))}px`
    if (toggle.value) {
        h = "100%"
    }
    console.log("CALCULATING HEIGHT", h, toggle.value)
    calcH.value = h
    // return h
}
// const elWidth = ref(playerEl.value?.$el.offsetWidth);

// playerEl.onRes
// const height = computed(() => {
//     let h = `calc(100vw * ${store.currentVideo!.height} / ${store.currentVideo!.width})`
//     if (!toggle.value && elWidth.value != undefined) {
//         console.log(elWidth.value)
//         h = "calc(" + elWidth.value + "px * 9 / 16)";
//     }
//     console.log("CALCULATING HEIGHT", h, toggle.value)

//     return h
// })


</script>


<style scoped lang="scss">
.outer {
    display: flex;
    justify-content: center;
}

.meta-cont {
    height: 100%;
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
    flex-basis: 200px;
    flex-grow: 1;
}

.player-box {
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    gap: var(--gutter-width);
    height: v-bind(calcH);
    max-width: var(--max-player-width);
    max-height: var(--max-p-height);
    min-width: 360px;

    // max-height: var(--max-player-height);
}

.player {
    width: 100%;
    // height: calc(100vw * 9 / 16);
    // max-height: v-bind(calcH);
    // max-height: calc(100% - 2 * var(--gutter-width));
    flex-basis: 100%;
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

    .metadata {
        flex-basis: 640px;
        flex-grow: 1;
        flex-grow: 1;
    }

    .meta-cont {
        max-width: 1754px;
        gap: var(--gutter-width);
        margin: 0 var(--gutter-width);


    }

    .layout {
        justify-content: center;
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
        gap: var(--gutter-width);
        // margin: 0 1rem;
        max-width: 1754px;
        flex-basis: 300px;
    }

    #regular,
    .secondary {
        display: flex;
        flex-grow: 1;
    }

    .secondary {
        flex-basis: 200px;
    }


}

.layout {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    flex-basis: 640px;
    flex-grow: 1;
    gap: var(--gutter-width);
    // margin-right: 1rem;
}

#primary {
    flex-grow: 1;
    flex: 1;
    height: 100%;
}

.secondary {
    background-color: green;
    flex-grow: .5;
    flex: 1;
    min-width: 300px;
}

@media screen and (min-width: 1036px) {

    #tele {
        flex-basis: 0;
        flex-grow: 0.5;
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
</style>