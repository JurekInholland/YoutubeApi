<script setup lang="ts">
import { DownloadStatus, type QueuedDownload } from '@/types';
import { Icon } from '@iconify/vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { formatDateAgo } from '@/utils';
import ProgressBar from '../ProgressBar.vue';
const store = useYoutubeStore();

const props = defineProps<{
    item: QueuedDownload
}>()

const removeFromQueue = async () => {
    await store.dequeue(props.item);
}

</script>

<template>
    <div class="queued-item"
        :class="[item.status === DownloadStatus.Finished ? 'finished' : '', item.progress ? 'active' : '']">
        <img :src="item.video.youtubeThumbnailUrl" alt="">
        <div class="info">
            <router-link :to="`/watch?v=${item.video.id}`">
                <h3>{{ item.video.title }}</h3>

            </router-link>
            <p>{{ formatDateAgo(item.queuedAt) }}</p>
            <p>{{ DownloadStatus[item.status] }}</p>
            <!-- <p>{{ item.id }}</p> -->
            <ProgressBar v-if="item.progress" :value="item.progress!.progress" :text="`${item.progress!.eta}`" />
        </div>
        <!-- <div v-if="item.progress" class="progress">
                    <div>{{ item.progress.status }}</div>
                    <div>{{ item.progress.progress }}%</div>
                    <div>{{ item.progress.speed }}</div>
                </div> -->
        <button @click="removeFromQueue">
            <Icon icon="ic:round-delete" />
        </button>
    </div>
</template>

<style scoped lang="scss">
.finished {
    opacity: .5;
}




button {
    display: block;
    font-size: 2rem;
    color: red;
}

.queued-item.active {
    border: 2px solid red;

}

.queued-item {
    box-sizing: border-box;
    border: 2px solid rgba(255, 255, 255, .5);
    display: flex;
    // flex-direction: column;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: 100%;
    background-color: #202020;
    border-radius: 10px;
    gap: 1rem;
    transition: all .5s ease;
    padding: .75rem;
    overflow: hidden;


    .progress {
        flex-basis: 30%;
    }

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
        flex-shrink: 0;
    }
}
</style>