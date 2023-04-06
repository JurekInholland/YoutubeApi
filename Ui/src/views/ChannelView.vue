<script setup lang="ts">
import { useRoute } from 'vue-router';

const route = useRoute();
const username = route.params.username as string;
</script>
<template>
    <h1>Channel</h1>
    <p>{{ username }}</p>
</template>