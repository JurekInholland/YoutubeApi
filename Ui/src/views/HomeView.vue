<script setup lang="ts">
import PlayPause from '@/components/PlayPause.vue';
import YoutubePlayer from '@/components/YoutubePlayer.vue';
import {computed, onMounted} from 'vue'
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
onMounted(() => {
    document.title = "Youtube Clone"
})
</script>

<template>
    <main>
        <h1>home</h1>
        <p>{{ route }}</p>
    </main>
</template>
<style scoped>
p {
    margin: 1rem 0;
}
</style>
