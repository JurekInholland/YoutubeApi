<script setup lang='ts'>
import { useYoutubeStore } from '@/stores/youtubeStore';
import QueueItem from '@/components/queue/QueueItem.vue';
import { onMounted, ref } from 'vue';
import { apiService } from '@/constants';

const youtubeStore = useYoutubeStore();

const queueInput = ref('');

onMounted(async () => {
    document.title = 'Download Queue';
    await youtubeStore.fetchQueue();
});

const deleteQueue = async () => {
    console.log('clearing queue');
    await youtubeStore.deleteQueue();
};
const clearQueue = async () => {
    console.log('clearing queue');
    await youtubeStore.clearQueue();
};

const processQueue = async () => {
    console.log('processing queue');
    await apiService.processQueue();
};

const enqueue = async () => {
    const videoId = queueInput.value;
    const success = await youtubeStore.enqueue(videoId);
    if (success) {
        queueInput.value = '';
    }
};
const onPaste = (e: ClipboardEvent) => {
    console.log('paste' + e);
}
</script>

<template >
    <div class="container">


        <div class='controls'>
            <button @click='clearQueue'>
                Clear Queue
            </button>
            <button @click="deleteQueue">
                Delete Queue
            </button>
            <input type='text' v-model='queueInput' name='' id=''>
            <button @click='enqueue'>
                Enqueue
            </button>
            <button @click='processQueue'>
                Process Queue
            </button>
        </div>
        <div class='item-container' @paste="onPaste">
            <QueueItem v-for='item in youtubeStore.queue' :key='item.id' :item='item' />
        </div>
    </div>
</template>

<style scoped lang='scss'>
.container {
    max-width: 1920px;
    margin: 0 auto;
}
button {
    background-color: rgba(255, 255, 255, .5);
    padding: .5rem;
    border-radius: .75rem;
    color: white;
}

.item-container {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin: 1rem;
}

.controls {
    display: flex;
    gap: 1rem;
    margin-left: 1rem;
}


input {
    background-color: white;
    color: black;
}
</style>
