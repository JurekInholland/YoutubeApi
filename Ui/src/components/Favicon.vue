<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';
import { useFavicon } from '@vueuse/core';
import { watch } from 'vue';
import { useRoute } from 'vue-router';

const store = useYoutubeStore();
const route = useRoute();

const icon = useFavicon()

function createFavicon(backgroundColor: string, foregroundColor: string, rotated: boolean): string {

    const path = rotated ? "M14.07,14l-3.7-7.75a.31.31,0,0,1,.28-.44h7.62a.31.31,0,0,1,.28.45l-3.93,7.75A.31.31,0,0,1,14.07,14Z" : "M18.47,10.43l-7.75,3.69a.31.31,0,0,1-.44-.28V6.22a.31.31,0,0,1,.45-.28l7.75,3.93A.31.31,0,0,1,18.47,10.43Z"

    return `data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 28.34 20.05'%3E%3Cdefs%3E%3Cstyle%3E.cls-1%7Bfill:%23${backgroundColor};%7D.cls-2%7Bfill:%23${foregroundColor};%7D%3C/style%3E%3C/defs%3E%3Crect class='cls-1' width='28.34' height='20.05' rx='5.88'/%3E%3Cpath class='cls-2' d='${path}'/%3E%3C/svg%3E`

}
// const isVideoRoute: boolean = route.name === "video";

function setAppIcon() {
    console.log("#######")
    console.log(route.name)
    console.log(store.currentVideo?.localVideo)
    console.log(store.color)
    if (route.name === "video") {
        console.log("is vid")
        if (store.currentVideo!.localVideo !== undefined) {
            icon.value = createFavicon(store.color.replace('#', ''), "fff", true)
            return;
        }
        else {
            console.log("local video ICONS", store.currentVideo?.localVideo)

        }
    }
    icon.value = createFavicon("ff0000", "fff", false)
}


watch(() => route.path, async () => {
    setAppIcon();
})

watch(() => store.currentVideo, () => {
    setAppIcon();
}) 
</script>

<template></template>