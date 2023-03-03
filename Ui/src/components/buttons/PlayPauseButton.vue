
<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import MorphButton from '../MorphButton.vue';

const props = defineProps<{
    isPaused: boolean
}>();

const paused = ref(false);

watch(() => props.isPaused, (newVal: boolean) => {
    paused.value = newVal;
    console.log("watch isPaused", newVal, path.value)
    path.value = paused.value ? 'M 12,26 18.5,22 18.5,14 12,10 z M 18.5,22 25,18 25,18 18.5,14 z' : 'M 12,26 16,26 16,10 12,10 z M 21,26 25,26 25,10 21,10 z';
});

const path = ref("M 12,26 18.5,22 18.5,14 12,10 z M 18.5,22 25,18 25,18 18.5,14 z");

// const path = computed(() => {
//     return paused ? 'M 12,26 18.5,22 18.5,14 12,10 z M 18.5,22 25,18 25,18 18.5,14 z' : 'M 12,26 16,26 16,10 12,10 z M 21,26 25,26 25,10 21,10 z';
// });

</script>
<template>
    <MorphButton :path="path" />
</template>