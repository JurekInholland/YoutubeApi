<script setup lang="ts">
import { defineProps, ref, watch } from 'vue';
import gsap from 'gsap'

const props = defineProps({
    path: {
        type: String,
        default: ''
    },
    duration: {
        type: Number,
        default: 0.33
    }
})


const pathStr = ref(props.path);

watch(() => props.path, (newVal: any) => {
    gsap.to(pathStr, {
        duration: props.duration,
        value: newVal,
        ease: "power2.inOut"
    });
    console.log("default changed");
});
</script>

<template>
    <button class="ytp-play-button ytp-button" aria-keyshortcuts="k" data-title-no-tooltip="Play"
        aria-label="Play keyboard shortcut k" title="Play (k)"><svg height="100%" version="1.1" viewBox="0 0 36 36"
            width="100%">
            <use class="ytp-svg-shadow" xlink:href="#ytp-id-98"></use>
            <path class="ytp-svg-fill" :d="pathStr" id="ytp-id-98">
            </path>
        </svg>
    </button>
</template>

<style scoped>
body {
    background-color: #333;
}
</style>