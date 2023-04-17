
<script setup lang="ts">
import { onBeforeMount, ref, watch } from 'vue';
import MorphButton from './MorphButton.vue';

const props = defineProps<{
    isPaused: boolean
}>();

const paused = ref(props.isPaused);

watch(() => props.isPaused, (newVal: boolean) => {
    paused.value = newVal;
    console.log("watch isPaused", newVal, path.value)
    path.value = paused.value ? 'M 12,26 18.5,22 18.5,14 12,10 z M 18.5,22 25,18 25,18 18.5,14 z' : 'M 12,26 16,26 16,10 12,10 z M 21,26 25,26 25,10 21,10 z';
});

const path = ref("");

onBeforeMount(() => {
    console.log("PAUSED:", paused.value, path.value)
    path.value = paused.value ? 'M 12,26 18.5,22 18.5,14 12,10 z M 18.5,22 25,18 25,18 18.5,14 z' : 'M 12,26 16,26 16,10 12,10 z M 21,26 25,26 25,10 21,10 z';
    console.log("onMounted", path.value)
});

</script>
<template>
    <MorphButton :path="path">
        <slot></slot>

    </MorphButton>
</template>
<style scoped>
.slot {
    position: absolute;

    pointer-events: none;
}
</style>