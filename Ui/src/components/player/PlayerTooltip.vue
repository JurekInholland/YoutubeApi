<script setup lang="ts">
const props = defineProps<{
    text: string
}>();
</script>

<template>
    <div class="tt">
        {{ text }}
    </div>
</template>

<style scoped>
.tt {
    background-color: rgba(24, 24, 24, .9);
    padding: .45rem;
    top: -3rem;
    left: -25%;
    white-space: nowrap;
    border-radius: 4px;
    font-weight: 500;
}
</style>