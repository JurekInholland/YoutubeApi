<script setup lang="ts">
import PlayPause from '@/components/PlayPause.vue';
import YoutubePlayer from '@/components/YoutubePlayer.vue';
import {computed} from 'vue'
import {useRoute} from 'vue-router'


const route = useRoute()

const path = route.path;

const startTime = computed(() => {
    return route.query.t ? parseInt(route.query.t as string) : 0
})

const parsedId = computed(() => {
    const split = path.substring(1).split("?")[0]
    if (split.length === 11) return split
    return route.query.v ? route.query.v as string : null
})

</script>

<template>
    <main>
        <h1>home</h1>
        <h1 v-if="!parsedId">Did not find video id in url</h1>
        <p>Route: {{ route }}</p>
        <p v-if="startTime > 0">StartTime:{{ startTime }}</p>
        <p>Path: {{ path }}</p>
        <p>parseD: {{ parsedId }}</p>

        <YoutubePlayer v-if="parsedId" :videoId="parsedId" :startTime="startTime" playerParameters="color=#333"/>
        <PlayPause/>
    </main>
</template>
<style scoped>
p {
    margin: 1rem 0;
}
</style>
