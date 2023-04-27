<script lang="ts" setup>
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import { useRepo } from 'pinia-orm';
import { onMounted } from 'vue';



onMounted(async () => {
    await useRepo(YoutubeVideoRepository).fetchById('oynpXDDCguE')
})
</script>
<template>
    <h1>Debug</h1>
</template>
