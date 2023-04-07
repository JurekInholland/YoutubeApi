<script setup lang="ts">
import { onMounted } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const username = route.params.username as string;

onMounted(() => {
    document.title = username;
});
</script>
<template>
    <h1>Channel</h1>
    <p>{{ username }}</p>
</template>