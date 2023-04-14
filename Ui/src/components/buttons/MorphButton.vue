<script setup lang="ts">
import { ref, watch } from 'vue';
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
    <button aria-keyshortcuts="k" title="Play (k)"><svg height="100%" version="1.1" viewBox="0 0 36 36">
            <use xlink:href="#ytp-id-98"></use>
            <path :d="pathStr">
            </path>
        </svg>
    </button>
</template>
