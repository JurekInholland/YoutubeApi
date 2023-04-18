<script setup lang="ts">
import { Icon } from '@iconify/vue';

const props = defineProps<{
    to: string
    icon: string
    text: string
}>();
</script>

<template>
    <router-link :to="to">
        <Icon :icon="icon" />
        {{ text }}
    </router-link>
</template>

<style scoped lang="scss">
.svg {
    width: 100%;
    height: 100%;
}

a {
    font-size: 14px;
    line-height: 2rem;

    font-weight: 400;
    display: flex;
    /* justify-content: center; */
    align-items: center;
    gap: 1.5rem;
    padding: .25rem 1.25rem;

    svg {
        scale: 1.5;
    }
}

a:hover {
    background-color: rgba(255, 255, 255, 0.1);
}
</style>