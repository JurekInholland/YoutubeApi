<script setup lang="ts">
import { DownloadStatus, type QueuedDownload } from '@/types';
import { Icon } from '@iconify/vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { formatDateAgo } from '@/utils';
const store = useYoutubeStore();

const props = defineProps<{
    item: QueuedDownload
}>()

const removeFromQueue = async () => {
    await store.dequeue(props.item);
}

</script>

<template>
    <div class="queued-item">
        <img :src="item.video.thumbnail" alt="">
        <div class="info">
            <h3>{{ item.video.title }}</h3>
            <p>{{ formatDateAgo(new Date(item.queuedAt)) }}</p>
            <p>{{ DownloadStatus[item.status] }}</p>
            <p>{{ item.id }}</p>

        </div>
        <button @click="removeFromQueue">
            <Icon icon="ic:round-delete" />
        </button>
    </div>
</template>

<style scoped lang="scss">
button {
    display: block;
    font-size: 2rem;
    color: red;
}


.queued-item {
    display: flex;
    // flex-direction: column;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: 100%;
    background-color: #202020;
    border-radius: 10px;
    gap: 1rem;

    .info {
        display: flex;
        flex-direction: column;
        gap: .5rem;
        flex-grow: 1;
        flex-wrap: wrap;
    }

    h3 {
        font-weight: bold;
    }

    // &:hover {
    //     transform: scale(1.05);
    //     box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.8);
    // }

    p {
        margin: 0;
        padding: 0;
        color: rgba(255, 255, 255, 0.5)
    }

    img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        border-radius: 10px;
        aspect-ratio: 16/9;
        max-width: 180px;
    }
}
</style>