<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';
import QueueItem from '@/components/queue/QueueItem.vue';
import { inject, onMounted, ref } from 'vue';

const youtubeStore = useYoutubeStore();

const queueInput = ref("");

onMounted(async () => {
    await youtubeStore.fetchQueue();
});

const clearQueue = async () => {
    console.log("clearing queue");
    await youtubeStore.clearQueue();
};

const enqueue = async () => {
    const videoId = queueInput.value;
    youtubeStore.enqueue(videoId);
};
</script>

<template>
    <h1>Queue</h1>
    <div>
        <QueueItem v-for="item in youtubeStore.queue" :key="item.id" :item="item" />
    </div>
    <div class="controls">
        <button @click="clearQueue">
            Clear Queue
        </button>
        <input type="text" v-model="queueInput" name="" id="">
        <button @click="enqueue">
            Enqueue
        </button>
    </div>
</template>

<style scoped lang="scss">
.controls {
    display: flex;
    gap: 1rem;
}

button {
    color: white;
}

input {
    background-color: white;
    color: black;
}
</style>