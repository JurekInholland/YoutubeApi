<script setup lang="ts">
import gsap from 'gsap';

const regular = [
    "m 14,14 -4,0 0,2 6,0 0,-6 -2,0 0,4 0,0 z",
    "m 22,14 0,-4 -2,0 0,6 6,0 0,-2 -4,0 0,0 z",
    "m 20,26 2,0 0,-4 4,0 0,-2 -6,0 0,6 0,0 z",
    "m 10,22 4,0 0,4 2,0 0,-6 -6,0 0,2 0,0 z"
]

const hovered = [
    "m 15,15 -4,0 0,2 6,0 0,-6 -2,0 0,4 0,0 z",
    "m 21,15 0,-4 -2,0 0,6 6,0 0,-2 -4,0 0,0 z",
    "m 19,25 2,0 0,-4 4,0 0,-2 -6,0 0,6 0,0 z",
    "m 11,21 4,0 0,4 2,0 0,-6 -6,0 0,2 0,0 z"
]

// const svg = ref<SVGElement>()

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
        <div class="tooltip">
            <slot></slot>
        </div>
    </button>
</template>

<style scoped>
svg {
    pointer-events: none;
    fill: white;
}
.tooltip {
    pointer-events: none;
    opacity: 0;
    scale: .75;
    transition: all .25s ease;
    position: absolute;
    left: -1rem;

}
button:hover .tooltip {
    opacity: 1;
    scale: 1;
}
</style>
