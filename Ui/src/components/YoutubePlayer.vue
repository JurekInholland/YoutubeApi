<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';
import type { PlayerState } from '@/types';
import { YoutubeIframe } from '@vue-youtube/component';
import { computed, nextTick, onBeforeUnmount, onMounted, ref, watch, type Ref } from 'vue';
import { useRoute } from 'vue-router';
import YoutubePlayerUpNext from './player/YoutubePlayerUpNext.vue';
const route = useRoute();
const store = useYoutubeStore();

const isInitialized = ref(false);

const props = defineProps<{
    videoId: string,
    playerState: PlayerState
}>()
const youtube = ref<HTMLDivElement | undefined>() as any;

const playerWidth = ref(computed(() => {
    return youtube.value?.$el.offsetWidth + "px";
}));

const onReady = (event: any) => {
    console.log('ready', event.target);
    const duration = event.target.getDuration()
    // const isMuted = event.target.isMuted()
    // const vol = event.target.getVolume()
    const isPlaying = event.target.getPlayerState() === 1;
    const tar = event.target;
    event.target.setVolume(props.playerState.volume * 100);
    event.target.startSeconds = props.playerState.currentTime;
    console.log("SEEKING TO ", props.playerState.currentTime)
    event.target.seekTo(props.playerState.currentTime, true);
    if (!props.playerState.isPlaying) {
        event.target.pauseVideo();
    }
    props.playerState.duration = duration;
    props.playerState.isPlaying = isPlaying;
    isInitialized.value = true;
    event.target.playVideo();
    // props.playerState.volume = isMuted ? 0 : vol;

};
const onStateChange = (event: any) => {
    const isPlaying = event.data === 1;
    props.playerState.isPlaying = isPlaying;
    console.log('isPlaying', isPlaying);
};
const onError = (event: any) => {
    console.log('error', event);
};

const onMessage = (event: MessageEvent) => {
    if (event.origin !== "https://www.youtube.com") {
        return;
    }

    if (!isInitialized.value) {
        return;
    }

    var data = JSON.parse(event.data);
    // console.log("data", data);

    if (data.info?.currentTime !== undefined) {
        props.playerState.currentTime = data.info.currentTime;
    }

    if (data.info?.volume !== undefined) {
        props.playerState.volume = data.info.volume / 100;
        // console.log("volume", data.info.volume);
    }
};

const onFullscreenChane = (event: any) => {
    if (document.fullscreenElement) {
        console.log("fullscreen");
    } else {
        console.log("not fullscreen");
    }
};

onMounted(() => {
    window.addEventListener("message", onMessage);
    document.addEventListener("fullscreenchange", onFullscreenChane)
});

onBeforeUnmount(() => {
    window.removeEventListener("message", onMessage);
    document.removeEventListener("fullscreenchange", onFullscreenChane)

});

</script>

<template>
    <div class="yotube-wrapper">
        <div>{{ playerState.duration - playerState.currentTime }}</div>
        <div class="overlay" v-if="playerState.duration - playerState.currentTime <= 1">
            <youtube-player-up-next :video="store.relatedVideos[0]" />
        </div>
        <youtube-iframe ref="youtube" class="iframe" :preserve="true" :video-id="props.videoId" @ready="onReady"
            @state-change="onStateChange" @error="onError" @message="onMessage" :player-vars="{
                // https://developers.google.com/youtube/player_parameters#Parameters
                iv_load_policy: 3,
                color: 'white',
                start: props.playerState.currentTime,
                modestbranding: 1,
                enablejsapi: 1,
                rel: 0,
                autoplay: 1,
            }">
        </youtube-iframe>
    </div>
</template>

<style >
.overlay {
    background-color: rgba(0, 0, 0, 1);
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
    bottom: 1.9rem;
    top: 3.6rem;
    right: 0;
    /* height: 100%; */
    width: 100%;
    z-index: 1;
    /* width: v-bind(playerWidth); */

}

.iframe {

    /* width: 100%; */
    /* max-width: 1920px;
    'max-width': '1920px', width: '100%', 'max-height': 80 + 'vh' */
}

.youtube-wrapper {
    /* position: relative; */
    display: flex;
    justify-content: center;
    align-items: center;
    /* max-width: calc(100vh - 12rem) !important; */
    /* margin: 0 auto; */

    /* background-color: red;
    overflow: hidden;
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0; */
    overflow: hidden;

}

iframe {
    width: 100%;
    height: 100%;
    max-width: unset;
    /* margin: 0 auto; */
    /* aspect-ratio: 16/9; */
}
</style>
