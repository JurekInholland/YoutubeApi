<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';


const props = defineProps<{
    value: number,
    text: string
}>();

const store = useYoutubeStore();

</script>

<template>
    <div class="wrapper">

        <div class="bar"
            :style="{ 'background': `linear-gradient(to right, ${store.color} 0%, ${store.color} ${value}%, transparent ${value}%, transparent 100%)` }">
            <p>{{text}}</p>
        </div>
    </div>
</template>

<style scoped>
.wrapper {
    flex-basis: 250px;
    flex-grow: 1;
    transition: all .5s ease !important;
}

.bar {
    color: white;
    width: 100%;
    height: 1.25rem;
    border-radius: 12px;
    border: 1px solid #fff;
    display: flex;
    justify-content: center;
    align-items: center;
}

/* progress {
    color:red;
} */
</style>
