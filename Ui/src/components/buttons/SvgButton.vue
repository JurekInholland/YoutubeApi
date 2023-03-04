<script setup lang="ts">
const props = defineProps({
    path: {
        type: String,
        default: ''
    },
    fill: {
        type: String,
        default: '#fff'
    },
    style: {
        type: String,
        default: ''
    }
})

</script>

<template>
    <button>
        <svg height="100%" version="1.1" viewBox="0 0 36 36" width="100%">
            <path :d="props.path" :fill="props.fill" id="ytp-id-21" :style="style"></path>
        </svg>
    </button>
</template>