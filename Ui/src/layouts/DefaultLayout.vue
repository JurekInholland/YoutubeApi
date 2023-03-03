<script setup lang="ts">
import { defineComponent } from 'vue'
defineComponent({
    name: 'DefaultLayout'
})
</script>

<template>
    <h1>Def layout</h1>
    <TopBar />
    <main>
        <slot></slot>
    </main>
</template>
