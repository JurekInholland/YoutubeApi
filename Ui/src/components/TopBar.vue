<script setup lang="ts">
import { ref } from 'vue';
import { Icon } from '@iconify/vue';
import Logo from './Logo.vue';
const searchFocs = ref(false);
const searchQuery = ref('');
const activeChange = () => {
    // console.log("E",e)
    console.log("active")
    searchFocs.value = !searchFocs.value;
}
</script>

<template>
    <div class="ytd-mmasthead">
        <div class="left">
            <!-- <div class="yt-icon">
                                            <svg viewBox="0 0 24 24" preserveAspectRatio="xMidYMid meet" focusable="false" class="style-scope yt-icon"
                                                style="pointer-events: none; display: block; width: 100%; height: 100%;">
                                                <g class="style-scope yt-icon">
                                                    <path d="M21,6H3V5h18V6z M21,11H3v1h18V11z M21,17H3v1h18V17z" class="style-scope yt-icon"></path>
                                                </g>
                                            </svg>
                                        </div> -->
            <button class="menu-button">
                <Icon class="menu" icon="mdi-light:menu" />
            </button>
            <Logo class="logo" />
            <span id="country-code">
                .juri.lol
            </span>
        </div>

        <div class="center">
            <div class="search" :class="searchFocs ? 'active' : ''">
                <div class="ytd-searchbox">
                    <Icon v-if="searchFocs" style="font-size: 1.5rem;" class="search-icon" icon="system-uicons:search" />
                    <input @focusin="searchFocs = true" @focusout="searchFocs = false" v-model="searchQuery"
                        placeholder="Search" type="text">

                    <button v-if="searchQuery !== ''">
                        <Icon style="font-size: 1.5rem;" icon="mdi:close" />

                    </button>
                </div>
                <button class="search-button">
                    <Icon style="font-size: 1.5rem;" icon="system-uicons:search" />
                </button>

                <button class="mic-button">
                    <Icon style="font-size: 1.5rem;" icon="ic:baseline-mic"></Icon>
                </button>
            </div>

        </div>
        <div class="right">
            <Icon style="font-size: 1.5rem;" icon="mdi:bell" />
            <Icon style="font-size: 1.5rem;" icon="mdi:account" />
        </div>
    </div>
</template>


<style scoped>
svg {
    color: white;
}

/* .menu-button {
} */
.search-button {
    border: 1px solid var(--ytd-searchbox-legacy-border-color);
    border-left: none;
    border-radius: 0 40px 40px 0;
    flex-grow: 0;
    width: 64px;
}

button {
    fill: white;
    color: white;
    /* height: 65px; */
    min-width: 28px;
    min-height: 28px;
    flex-shrink: 0;
    padding: 8px;
}

button>svg {
    height: 100%;
    width: 100%;
}
.mic-button {
    border-radius: 50%;
    background-color: rgb(18,20,20);
    margin-left: .5rem;
}
.menu {
    overflow: hidden;
    color: white;
    min-height: 28px;
    width: 28px;
}

input {
    appearance: none;
    border: none;
    font-family: Roboto, Noto, sans-serif;
    font-size: 16px;
    font-weight: 400;
    line-height: 24px;
    text-align: inherit;
    width: 100%;
    background-color: transparent;
    color: white;
    border-color: initial;
    box-shadow: none;
    border: none;
    outline: none;
}

.active {
    border-color: blue;
}

.search-icon {
    margin-right: .5rem;
}

/* .search-icon {
    display: none;
}
.ytd-searchbox.active>.search-icon {
display: block;
} */

.yt-icon {
    fill: white;
    width: 24px;
    height: 24px;
    flex-shrink: 0;
    flex-grow: 0;
}

.ytd-mmasthead {
    background-color: #0b0c0d;
    color: #fff;
    height: 56px;
    padding: 0 16px;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    min-width: 500px;
}

.left {
    display: flex;
    /* align-items: center; */
    justify-content: center;
    flex-shrink: 0;
    flex-grow: 0;
    margin-left: 22px;
    margin-right: 1rem;
    font-size: 10px;
}

.center {
    align-items: center;
    flex: 0 1 720px;
    min-width: 0px;
    padding-left: 2.5rem;
    padding-right: 2.5rem;

}

.right {
    display: flex;
    gap: 1rem;
    margin-left: 1rem;
    min-width: 185px;
    justify-content: flex-end;
    margin-right: .5rem;
}

.ytd-searchbox {
    background-color: #0e0f0f;
    box-shadow: inset 0 1px 2px var(-vt-c-white-mute);
    color: var(-vt-c-white-mute);
    border-radius: 40px 0 0 40px;
    margin-left: 32px;
    padding: 0px 4px 0px 16px;
    border: 1px solid var(--ytd-searchbox-legacy-border-color);
    height: 100%;
    display: flex;
    flex-grow: 1;
    align-items: center;
}

.search {
    height: 40px;
    display: flex;
    /* margin-left: 32px; */
}

.search.active>.ytd-searchbox {
    margin-left: 0;
}

.search.active>.ytd-searchbox {
    border: 1px solid var(--input-focus-color);
}

.logo {
    max-height: 56px;
    flex-grow: 0;
    padding: 18px 0px 18px 6px;
}

#country-code {
    top: 12px;
    left: 129px;
    position: absolute;
    color: rgba(255, 255, 255, .75);
}
</style>