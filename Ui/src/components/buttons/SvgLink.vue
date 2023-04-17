<script setup lang="ts">
import type { RouteLocationRaw } from 'vue-router';

const props = defineProps<{
    href: RouteLocationRaw
    text: string
    path: string
}>()

const viewBox = '0 0 36 36'

</script>
<template>
    <div class="link">
        <router-link class="next" :to="href">
            <svg height="100%" version="1.1" :viewBox="viewBox" width="100%">
                <path :d="props.path" id="ytp-id-21"></path>
            </svg>
            {{ text }}
        </router-link>
        <div class="slot">
            <slot class=""></slot>

        </div>
    </div>
</template>

<style scoped lang="scss">
.link {
    position: relative;
    display: flex;
    overflow: visible;
    // opacity: 1;
    z-index: 100;
}

a {
    display: flex;
    align-items: center;
    gap: 6px;
}
svg {
    flex-shrink: 0;
    fill: rgba(255, 255, 255, .8);
}

</style>

<style>

.slot {
    position: absolute;
    opacity: 0;
    width: 0;
    height: 0;
    pointer-events: none;
}
.link:hover svg {
    fill: #fff;
}
.link:hover>.slot {
    opacity: 1;
    scale: 1;
    pointer-events: all;
}
</style>