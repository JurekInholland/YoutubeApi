<script setup lang="ts">
import gsap from 'gsap';

const regular = [
    "m 10,16 2,0 0,-4 4,0 0,-2 -6,0 0,6 0,0 z",
    "m 20,12 4,0 0,4 2,0 0,-6 -6,0 0,2 0,0 z",
    "m 24,24 -4,0 0,2 6,0 0,-6 -2,0 0,4 0,0 z",
    "m 12,24 0,-4 -2,0 0,6 6,0 0,-2 -4,0 0,0 z",
]

const hovered = [
    "m 9,15 2,0 0,-4 4,0 0,-2 -6,0 0,6 0,0 z",
    "m 21,11 4,0 0,4 2,0 0,-6 -6,0 0,2 0,0 z",
    "m 25,25 -4,0 0,2 6,0 0,-6 -2,0 0,4 0,0 z",
    "m 11,25 0,-4 -2,0 0,6 6,0 0,-2 -4,0 0,0 z"
]

const onHover = (e: Event) => {
    gsap.fromTo("#ytp-id-435",
        { attr: { d: regular[0] }, },
        {
            duration: 0.25,
            yoyo: true,
            repeat: 1,
            ease: "quart.inOut",
            attr: { d: hovered[0] }
        });
    gsap.fromTo("#ytp-id-436",
        { attr: { d: regular[1] }, },
        {
            duration: 0.25,
            yoyo: true,
            repeat: 1,
            ease: "quart.inOut",
            attr: { d: hovered[1] }
        });
    gsap.fromTo("#ytp-id-437",
        { attr: { d: regular[2] }, },
        {
            duration: 0.25,
            yoyo: true,
            repeat: 1,
            ease: "quart.inOut",
            attr: { d: hovered[2] }
        });
    gsap.fromTo("#ytp-id-438",
        { attr: { d: regular[3] }, },
        {
            duration: 0.25,
            yoyo: true,
            repeat: 1,
            ease: "quart.inOut",
            attr: { d: hovered[3] }
        });
}

</script>

<template>
    <button @mouseover.stop="onHover">
        <svg ref="svg" height="100%" version="1.1" viewBox="0 0 36 36" width="100%">
            <g class="ytp-fullscreen-button-corner-2">
                <path class="ytp-svg-fill" :d="regular[0]" id="ytp-id-435"></path>
            </g>
            <g class="ytp-fullscreen-button-corner-3">
                <path class="ytp-svg-fill" :d="regular[1]" id="ytp-id-436"></path>
            </g>
            <g class="ytp-fullscreen-button-corner-0">
                <path class="ytp-svg-fill" :d="regular[2]" id="ytp-id-437"></path>
            </g>
            <g class="ytp-fullscreen-button-corner-1">
                <path class="ytp-svg-fill" :d="regular[3]" id="ytp-id-438"></path>
            </g>
        </svg>
    </button>
</template>

<style scoped>
button {
    fill: #fff;
}
svg {
    pointer-events: none;
}
</style>