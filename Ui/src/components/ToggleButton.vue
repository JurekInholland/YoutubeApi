<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';


const store = useYoutubeStore();

const props = defineProps<{
    modelValue: boolean,
}>();

const emits = defineEmits<{
    (e: 'update:modelValue', value: boolean): void
}>();

const updateValue = (event: Event) => {
    // const value = (event.target as HTMLInputElement)?.;
    emits('update:modelValue', !props.modelValue);
}

const color = store.color;

</script>

<template>
    <div class="togglebutton">

        <div class="toggle">
            <input type="checkbox" :v-model="modelValue" @input="updateValue">
            <span class="track">
                <span class="handle" :class="modelValue ? 'on' : ''"></span>
            </span>
        </div>
        <p class="text">
            <slot></slot>
        </p>
    </div>
</template>

<style scoped lang="scss">
.togglebutton {
    display: flex;
    gap: .75rem;
}

.text {
    font-weight: 500;
    white-space: nowrap;
    flex-shrink: 1;

}

@media screen and (max-width: 620px) {
    .text {
        display: none;
    }
}


.track {
    border-radius: 6px;
    pointer-events: none;
    position: absolute;
    left: 0;
    width: 100%;
    height: 75%;
    background-color: rgba(255, 255, 255, .25);
}

.handle {
    position: absolute;
    aspect-ratio: 1;
    background-color: white;
    border-radius: 50%;
    height: 200%;
    left: 0;
    top: -50%;

    transition: transform .25s ease, background-color .25s ease-out;
    transform: translateX(0px);
}

.on {
    transform: translateX(19px);
    // background-color: #3ea6ff;
    background-color: v-bind(color);
}

.toggle {
    // background-color: transparent;
    width: 40px;
    height: 14px;
    display: flex;
    align-items: center;
    justify-content: center;

    input {
        width: 100%;
        height: 100%;
        cursor: pointer;
        opacity: 0;

    }
}
</style>