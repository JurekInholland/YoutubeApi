<script setup lang="ts">
import { useSignalR } from '@quangdao/vue-signalr';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted, onBeforeUnmount } from 'vue';
import type { DownloadProgress, LocalVideo, TaskProgress } from '@/types';
import { TaskStatus, ApplicationTask } from '@/types';
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import { useRepo } from 'pinia-orm';
const signalR = useSignalR();
const store = useYoutubeStore();

const onDownloadProgress = (data: DownloadProgress) => {
    console.log("download progress", data)
    store.updateDownloadProgress(data);
}

const onTaskUpdate = (data: TaskProgress) => {
    console.log("Task update", ApplicationTask[data.task], TaskStatus[data.status])

}

const onLocalVideoUpdate = (data: LocalVideo) => {
    console.log("Local video update", data)
    // store.addLocalVideo(data);
    useRepo(YoutubeVideoRepository).addLocalVideo(data as any);
}

onMounted(() => {
    signalR.on("downloadProgress", onDownloadProgress)
    signalR.on("taskUpdate", onTaskUpdate)
    signalR.on("localVideo", onLocalVideoUpdate)
});
onBeforeUnmount(() => {
    signalR.off("downloadProgress", onDownloadProgress)
    signalR.off("taskUpdate", onTaskUpdate)
    signalR.off("localVideo", onLocalVideoUpdate)

})
</script>
<template></template>
