<script setup lang="ts">
import { useSignalR } from '@quangdao/vue-signalr';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted, onBeforeUnmount } from 'vue';
import type { DownloadProgress } from '@/types';
const signalR = useSignalR();
const store = useYoutubeStore();

const onDownloadProgress = (data: DownloadProgress) => {
    console.log("download progress", data)
    store.updateDownloadProgress(data);
}


onMounted(() => {
    signalR.on("downloadProgress", onDownloadProgress)
});
onBeforeUnmount(() => {
    signalR.off("downloadProgress", onDownloadProgress)
})
</script>
<template></template>
