<script setup lang='ts'>
import type { ApplicationSettings } from '@/types';
import { ref, type Ref } from 'vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
const store = useYoutubeStore();

const settings: Ref<ApplicationSettings> = ref({
    showRelatedVideos: true,
    showComments: true,
    autoplay: true,
    color: 'red'

} as ApplicationSettings);

const onColorChange = () => {
    store.color = settings.value.color;
}

</script>

<template>
    <h1>Settings</h1>
    <section class='settings'>
        <label for=''>
            Show related videos
            <input type='checkbox' v-model='settings.showRelatedVideos' name='' id=''>
        </label>

        <label for=''>
            Show comments
            <input type='checkbox' v-model='settings.showComments' name='' id=''>
        </label>

        <label for=''>
            Color
            <input type='color' v-model='settings.color' @change="onColorChange" name='' id=''>
        </label>
    </section>
</template>

<style scoped>
.settings {
    max-width: 1280px;
    display: flex;
    flex-direction: column;
    gap: 1rem;
    background-color: brown;
    margin: 0 auto;
}

label {
    display: flex;
    align-items: center;
    gap: 1rem;
    font-weight: bold;
}

input[type="checkbox"] {
    width: 1.25rem;
    height: 1.25rem;
    cursor: pointer;
}
</style>
