<script setup lang="ts">
import { ref } from 'vue';
import MorphButton from '@/components/buttons/MorphButton.vue';
import VideoPlayer from '@/components/VideoPlayer.vue';
import PlayPauseButton from '@/components/buttons/PlayPauseButton.vue';
import VolumeButton from '@/components/buttons/VolumeButton.vue';

const playState = ref(false);
const playBtnPath = ref("M8,21 L12,21 L17,26 L17,10 L12,15 L8,15 L8,21 Z M19,14 L19,22 C20.48,21.32 21.5,19.77 21.5,18 C21.5,16.26 20.48,14.74 19,14 ZM19,11.29 C21.89,12.15 24,14.83 24,18 C24,21.17 21.89,23.85 19,24.71 L19,26.77 C23.01,25.86 26,22.28 26,18 C26,13.72 23.01,10.14 19,9.23 L19,11.29 Z M 7.07,6.87 -1.11,15.33 19.61,36.11 27.80,27.60 z");
const togglePlay = () => {
    playState.value = !playState.value;
    if (playState.value) {
        playBtnPath.value = "M8,21 L12,21 L17,26 L17,10 L12,15 L8,15 L8,21 Z M19,14 L19,22 C20.48,21.32 21.5,19.77 21.5,18 C21.5,16.26 20.48,14.74 19,14 ZM19,11.29 C21.89,12.15 24,14.83 24,18 C24,21.17 21.89,23.85 19,24.71 L19,26.77 C23.01,25.86 26,22.28 26,18 C26,13.72 23.01,10.14 19,9.23 L19,11.29 Z";
    } else {
        playBtnPath.value = "M8,21 L12,21 L17,26 L17,10 L12,15 L8,15 L8,21 Z M19,14 L19,22 C20.48,21.32 21.5,19.77 21.5,18 C21.5,16.26 20.48,14.74 19,14 Z ";
    }
    console.log("togglePlay")
}
const toggle = () => {
    console.log("toggle")
    playState.value = !playState.value;
}
</script>

<template>
    <h1>vidtest</h1>
    <VideoPlayer color="red" src="./vid.webm" />
    <!-- <PlayPauseButton :isPaused="playState" @click="toggle" /> -->
    <!-- <MorphButton :path="playBtnPath" @click="togglePlay" /> -->
</template>