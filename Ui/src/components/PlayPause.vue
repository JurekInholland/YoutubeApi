<script setup lang="ts">
import { ref } from 'vue'
const state = ref(false)
const toggle = () => {
    state.value = !state.value
}
</script>

<template>
    <div class="playpause">
        <input type="checkbox" value="None" id="playpause" name="check" />
        <label for="playpause" tabindex=1></label>
    </div>
</template>

<style scoped lang="scss">
.playpause {
  label {
    display: block;
    box-sizing: border-box;
    
    width: 0;
    height: 74px;
    
    cursor: pointer;

    border-color: transparent transparent transparent #202020;
    transition: 150ms all ease-out;
    will-change: border-width, border-style;
    
    // paused state
    border-style: double;
    border-width: 0px 0px 0px 61px;
  }
  input[type='checkbox'] {
    visibility: hidden;
    
    &:checked + label {
      // play state
      border-style: solid;
      border-width: 37px 0 37px 60px;
    }
  }
}
</style>
