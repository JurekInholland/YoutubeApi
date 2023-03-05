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
            <path :d="props.path" :fill="props.fill" id="ytp-id-21" :style="style"></path>
        </svg>
        <span v-if="props.text!=''">{{ props.text }}</span>
    </button>
</template>