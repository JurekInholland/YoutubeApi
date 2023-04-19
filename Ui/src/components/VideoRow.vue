<script setup lang="ts">
import type { YoutubeVideo } from '@/types';
import { computed } from '@vue/reactivity';
import { onBeforeMount, onMounted, ref, watch } from 'vue';
import VideoLink from './VideoLink.vue';


const props = withDefaults(defineProps<{
    videos: YoutubeVideo[]
    offset: number
}>(), {
    offset: 0
})

const number = ref(1)

const displayVideos = computed(() => {
    
    return props.videos.slice(props.offset * number.value, (props.offset + 1) * number.value)
})

const onResize = () => {
    const w = window.innerWidth
    if (w <= 356) {
        number.value = 1
        return
    }
    let numberOfVideos = Math.floor(w / 356)
    if (numberOfVideos > 6) numberOfVideos = 6
    number.value = numberOfVideos
}

onMounted(() => {
    onResize()
    console.log("mounted")
    window.addEventListener("resize", onResize)
})

onBeforeMount(() => {
    console.log("before mount")
    window.removeEventListener("resize", onResize)

})

</script>

<template>
    <section class="video-row">
        <VideoLink v-for="video in displayVideos" :key="video.id" :video="video" />

    </section>
</template>

<style scoped lang="scss">
.video-row {
    position: relative;
    width: 100%;

    // height: calc((100vw + 1.5rem)* 9 / 16);
    overflow: hidden;
    display: flex;
    flex-grow: 1;
    gap: 1rem;
    // padding: 1rem 2rem;

    div {
        position: relative;
        flex: 1;
        flex-shrink: 1;
        flex-grow: 0;
        flex-basis: 100%;

        counter-increment: item;
    }

}

ol,
li {
    list-style: none;
    margin: 0;
    padding: 0;
}

li {
    display: flex;
    flex-grow: 1;
    gap: 1rem;

}

.s1 {
    background-color: red;
}

.s2 {
    background-color: green;
}

.s3 {
    background-color: yellow;
}

.s4 {
    background-color: purple;
}
</style>