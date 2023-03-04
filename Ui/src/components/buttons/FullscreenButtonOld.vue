<script setup lang="ts">
import { ref } from 'vue';
import MorphButton from './MorphButton.vue';
import gsap from 'gsap';

const svg = ref<SVGSVGElement | null>(null);
const onHover = (e: Event) => {
    // console.log("onHover", e)
    // if (e.target === svg.value) return;
    // if (e.target instanceof SVGPathElement) return;
    console.log("TARGET", e.target)

    gsap.to(svg.value, {
        startAt: { scale: 1 },
        duration: .25,
        scale: 1.2,
        repeat: 1,
        yoyo: true,
        ease: "power2.inOut"
    });
}

</script>

<template>
    <button @mouseover.stop="onHover">
        <svg ref="svg" height="100%" version="1.1" viewBox="0 0 36 36" width="100%">
            <g class="ytp-fullscreen-button-corner-0">
                <use class="ytp-svg-shadow" xlink:href="#ytp-id-7"></use>
                <path class="ytp-svg-fill" d="m 10,16 2,0 0,-4 4,0 0,-2 L 10,10 l 0,6 0,0 z" id="ytp-id-7"></path>
            </g>
            <g class="ytp-fullscreen-button-corner-1">
                <use class="ytp-svg-shadow" xlink:href="#ytp-id-8"></use>
                <path class="ytp-svg-fill" d="m 20,10 0,2 4,0 0,4 2,0 L 26,10 l -6,0 0,0 z" id="ytp-id-8"></path>
            </g>
            <g class="ytp-fullscreen-button-corner-2">
                <use class="ytp-svg-shadow" xlink:href="#ytp-id-9"></use>
                <path class="ytp-svg-fill" d="m 24,24 -4,0 0,2 L 26,26 l 0,-6 -2,0 0,4 0,0 z" id="ytp-id-9"></path>
            </g>
            <g class="ytp-fullscreen-button-corner-3">
                <use class="ytp-svg-shadow" xlink:href="#ytp-id-10"></use>
                <path class="ytp-svg-fill" d="M 12,20 10,20 10,26 l 6,0 0,-2 -4,0 0,-4 0,0 z" id="ytp-id-10"></path>
            </g>
        </svg>
    </button>
</template>

<style scoped>
button {
    background-color: goldenrod;
}
svg {
    pointer-events: none;
    fill: white;
}
</style>