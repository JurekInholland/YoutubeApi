<script setup lang="ts">
import { useSignalR } from '@quangdao/vue-signalr';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted, onBeforeUnmount } from 'vue';
import type { DownloadProgress, TaskProgress } from '@/types';
const signalR = useSignalR();
const store = useYoutubeStore();

const onDownloadProgress = (data: DownloadProgress) => {
    console.log("download progress", data)
    store.updateDownloadProgress(data);
}

const onTaskUpdate = (data: TaskProgress) => {
    console.log("Task update", data)
}

onMounted(() => {
    signalR.on("downloadProgress", onDownloadProgress)
    signalR.on("taskUpdate", onTaskUpdate)
});
onBeforeUnmount(() => {
    signalR.off("downloadProgress", onDownloadProgress)
    signalR.off("taskUpdate", onTaskUpdate)
})
</script>
<template></template>
