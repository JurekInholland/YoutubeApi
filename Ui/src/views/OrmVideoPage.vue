<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const parsedId = computed(() => {
  const substring = route.path.substring(1);
  if (substring.length === 11) return substring
  const pathId = substring.split("/")[1]
  if (pathId !== undefined && pathId.length === 11) return pathId
  return route.query.v ? route.query.v as string : null
})


</script>
<template>
    <div>

        <p>Video: {{ }}</p>

    </div>
</template>