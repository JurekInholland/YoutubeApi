<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';
import UpNext from '../player/UpNext.vue';
const store = useYoutubeStore();
const props = defineProps({
    path: {
        type: String,
        default: ''
    },
    // fill: {
    //     type: String,
    //     default: 'rgba(255,255,255,0.7)'
    // },
    style: {
        type: String,
        default: ''
    },
    text: {
        type: String,
        default: ''
    },
    viewBox: {
        type: String,
        default: '0 0 36 36'
    }
})

</script>

<template>
    <button>
        <svg height="100%" version="1.1" :viewBox="props.viewBox" width="100%">
            <path :d="props.path" id="ytp-id-21" :style="style"></path>
        </svg>
        <span v-if="props.text != ''">{{ props.text }}</span>
        <div class="tooltip">
            <slot></slot>
        </div>
    </button>
</template>

<style scoped>
.tooltip {
    
    opacity: 0;
    scale: .75;
    transition: all .25s ease;
    position: absolute;

}
button:hover .tooltip {
    opacity: 1;
    scale: 1;
}
</style>