<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import SignalR from './components/SignalR.vue';
import TopBar from './components/TopBar.vue';
</script>

<template>
  <SignalR />
  <TopBar />

  <div class="wrapper">

    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/about">About</RouterLink>
    </nav>
  </div>

  <RouterView />
</template>

<style lang="scss">
@import '../node_modules/@fontsource/roboto/400.css';
@import '../node_modules/@fontsource/roboto/500.css';
</style>