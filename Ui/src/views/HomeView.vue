<script setup lang="ts">
import VideoRow from '@/components/VideoRow.vue';
import { computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'


const route = useRoute()

const path = route.path;

onMounted(() => {
    document.title = "Youtube Clone"
})
</script>

<template>
    <main>
        <h1>home</h1>
        <p>{{ route }}</p>

        <VideoRow />

    </main>
</template>
<style scoped>
p {
    margin: 1rem 0;
}
</style>
