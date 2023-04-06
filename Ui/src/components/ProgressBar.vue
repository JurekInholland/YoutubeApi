<script setup lang="ts">

const props = defineProps<{
    value: number,
    text: string
}>();

</script>

<template>
    <div class="wrapper">

        <div class="bar"
            :style="{ 'background': `linear-gradient(to right, red 0%, red ${value}%, transparent ${value}%, transparent 100%)` }">
            <p>Prgress</p>
        </div>
    </div>
</template>

<style scoped>
.wrapper {
    width: 100%;
    transition: all .5s ease !important;
}

.bar {
    color: white;
    width: 100%;
    height: 1.25rem;
    border-radius: 12px;
    border: 1px solid #fff;
    display: flex;
    justify-content: center;
    align-items: center;
}

/* progress {
    color:red;
} */
</style>
