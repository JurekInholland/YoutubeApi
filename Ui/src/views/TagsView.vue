<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';

const store = useYoutubeStore();
</script>

<template>
    <div v-for="tag in store.getVideoTags">
        {{ tag }}
    </div>
</template>