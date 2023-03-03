<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import TopBar from './components/TopBar.vue';
</script>

<template>
  <!-- <TopBar /> -->

  <!-- <div class="wrapper">

    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/about">About</RouterLink>
    </nav>
  </div> -->

  <RouterView />
</template>

<style scoped></style>
