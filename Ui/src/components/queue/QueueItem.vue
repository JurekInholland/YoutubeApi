<script setup lang="ts">
import { DownloadStatus, type QueuedDownload } from '@/types';

const props = defineProps<{
    item: QueuedDownload
}>()


</script>

<template>
    <div class="queued-item">
        <p>{{ item.video.title }}</p>
        <img :src="item.video.thumbnail" alt="">
        <p>{{ item.queuedAt }}</p>
        <p>{{ DownloadStatus[item.status] }}</p>
    </div>
</template>

<style scoped lang="scss">
.queued-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 100%;
    background-color: #202020;
    border-radius: 10px;
    padding: 10px;
    margin: 10px;
    box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.5);
    transition: 150ms all ease-out;
    will-change: transform, box-shadow;

    &:hover {
        transform: scale(1.05);
        box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.8);
    }

    p {
        margin: 0;
        padding: 0;
        color: #fff;
    }

    img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        border-radius: 10px;
    }
}
</style>