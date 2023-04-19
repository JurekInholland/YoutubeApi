<script setup lang='ts'>
import type { ApplicationSettings } from '@/types';
import { ref, type Ref } from 'vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { defaultColor } from '@/constants';
import ToggleButton from '@/components/ToggleButton.vue';
const store = useYoutubeStore();

const settings: Ref<ApplicationSettings> = ref({
    showRelatedVideos: true,
    showComments: false,
    autoplay: true,
    color: store.color 

} as ApplicationSettings);

const onColorChange = () => {
    store.color = settings.value.color;
}

</script>

<template>
    <section class='settings'>
        <h1>Settings</h1>
        <label for=''>
            Show related videos
            <ToggleButton v-model='settings.showRelatedVideos' />
        </label>

        <label for=''>
            Show comments
            <ToggleButton :disabled="true" v-model='settings.showComments' />

        </label>

        <label for=''>
            Color
            <input type='color' v-model='settings.color' @change="onColorChange" name='' id=''>
        </label>
    </section>
</template>

<style scoped lang="scss">
.settings {
    font-family: Roboto, sans-serif;
    max-width: 1280px;
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin: 0 auto;
    padding: 1rem;

    h1 {
        font-weight: 600;
        font-size: 2rem;
    }
}

label {
    display: flex;
    align-items: center;
    gap: 1rem;
    font-weight: 500;
    height: 27px;
}

input[type="checkbox"] {
    width: 1.25rem;
    height: 1.25rem;
    cursor: pointer;
}
</style>
