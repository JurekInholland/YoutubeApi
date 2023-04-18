<script setup lang="ts">
</script>

<template>
    <div class="page-menu">
        <slot></slot>
    </div>
</template>


<style scoped lang="scss">
.page-menu {
    position: absolute;
    display: inline-flex;
    flex-grow: 0;
    background-color: #282828;
    color: white;
    border-radius: 12px;
    overflow: hidden;
    padding-bottom: .5rem;
    // padding: 0.5rem 0;
}
</style>