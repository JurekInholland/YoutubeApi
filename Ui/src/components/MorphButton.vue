<script setup lang="ts">
import { ref, watch } from 'vue';
import gsap from 'gsap'

const props = defineProps<{
    path: string,
}>();


const pathStr = ref(props.path);

watch(() => pathStr.value, (newVal: any) => {
    gsap.to(props.path, {
        duration: 0.5,
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