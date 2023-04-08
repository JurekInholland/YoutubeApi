<template>
    <teleport v-if="mounted" :to="toggle ? '#cinema' : '#regular'">
        <YoutubePlayer ref="playerEl" v-bind="$attrs" :style="{ 'height': calcH }" class="player" id="player"
            videoId="bULCuf9uOM4" :start-time="30" :aspect-ratio="16 / 9" />
    </teleport>
    <input type="checkbox" v-model="toggle" name="" id="">
    <div class="container" id="container1" ref="container">
        <!-- HERE -->
        <div id="cinema"></div>
        <div class="layout">
            <div id="primary" ref="primary">
                <div id="regular"></div>

                <VideoMetadata :model-value="false" :video="store.currentVideo!" />
            </div>
            <div class="secondary">
                this is the secondary
            </div>
        </div>
    </div>
    <button @click="swap">swap</button>
</template>
<script setup lang="ts">
import VideoMetadata from '@/components/VideoMetadata.vue';
import YoutubePlayer from '@/components/YoutubePlayer.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { computed, onMounted, ref } from 'vue';
// import { useResizeObserver } from '@vueuse/core'


const swap = () => {
    // const pl = document.getElementById("player");
    // const c = document.getElementById("container");
    // const p = document.getElementById("primary");
    // console.log("SWAPPING", playerEl.value)
    toggle.value = !toggle.value;



    // if (toggle.value) {

    //     // p?.removeChild(pl!);
    //     c?.prepend(pl!);
    // }
    // else {
    //     // c?.removeChild(pl!);
    //     p?.prepend(pl!);
    // }
    // const prim = primary.value as HTMLDivElement
    // const playerE = playerEl.value as HTMLDivElement
    // prim.removeChild(playerE)
    // container.value.appendChild(playerEl.value as Node)
    // if (primary.value === undefined) {
    //     console.log("primary is undefined")
    //     return;
    // }
    // if (toggle.value) {
    //     primary.value.removeChild(playerEl.value as Node)
    //     container.value.appendChild(playerEl.value as Node)
    // }
    // else {
    //     container.value.removeChild(playerEl.value as Node)
    //     primary.value.appendChild(playerEl.value as Node)
    // }

}
const primary = ref<HTMLDivElement | null>(null) as any;
const container = ref<HTMLDivElement | null>(null) as any;
const playerEl = ref<HTMLDivElement | null>(null) as any;
const mounted = ref(false);

onMounted(() => {
    mounted.value = true;

})
const store = useYoutubeStore();
// useResizeObserver(playerEl, (entries) => {
//     console.log("RESIZE OBSERVER", entries[0].contentRect.width)
//     calcH.value = calculateHeight(entries[0].contentRect.width)
// })
const calculateHeight = (hei: number) => {
    let h = `calc(100vw * ${store.currentVideo!.height} / ${store.currentVideo!.width})`
    if (!toggle.value) {
        h = `calc(${hei}px * 9 / 16)`;
    }
    return h
}
const elWidth = ref(playerEl.value?.$el.offsetWidth);
const calcH = ref("480px")

playerEl.onRes
// const height = computed(() => {
//     let h = `calc(100vw * ${store.currentVideo!.height} / ${store.currentVideo!.width})`
//     if (!toggle.value && elWidth.value != undefined) {
//         console.log(elWidth.value)
//         h = "calc(" + elWidth.value + "px * 9 / 16)";
//     }
//     console.log("CALCULATING HEIGHT", h, toggle.value)

//     return h
// })


const toggle = ref(false);
</script>


<style scoped lang="scss">
.player {
    width: 100%;
    // height: calc(100vw * 9 / 16);
    max-height: 87vh;
    flex-basis: 100%;
    // width: 100%;
    // height: auto;
}

.container {
    // background-color: darkblue;
    // height: 100vh;
    width: 100vw;
    max-width: 100%;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: center;
    gap: var(--gutter-width);
    // grid-template-columns: 2fr 1fr;
}

.layout {
    padding: 1rem;

    width: 100%;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    max-width: calc(1280px + var(--sidebar-width) + 3 * var(--gutter-width));
    gap: var(--gutter-width);
    // background-color: violet;
}

#primary {
    // background-color: red;
    flex-grow: 1;
    flex: 1;
    // aspect-ratio: 16/9;
    // min-width: max(360px, 66%);
}

.secondary {
    background-color: green;
    flex-grow: 1;
    flex: 1;
    max-width: 402px;
    min-width: 300px;
}

.sidebar {
    background-color: grey;
}
</style>

<style>
:root {
    --sidebar-width: 402px;
    --gutter-width: 1rem;
}
</style>