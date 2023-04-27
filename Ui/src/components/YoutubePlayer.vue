<script setup lang="ts">
import { useYoutubeStore } from '@/stores/youtubeStore';
import type { PlayerState } from '@/types';
import { YoutubeIframe } from '@vue-youtube/component';
import { computed, onBeforeUnmount, onMounted, ref } from 'vue';
import YoutubePlayerUpNext from './player/YoutubePlayerUpNext.vue';
import { useRepo } from 'pinia-orm';
import YoutubeVideoRepository from '@/repositories/YoutubeVideoRepository';
import type YoutubeVideo from '@/models/YoutubeVideo';
const store = useYoutubeStore();

const isInitialized = ref(false);

const props = defineProps<{
    videoId: string,
    playerState: PlayerState
}>()
const youtube = ref<HTMLDivElement | undefined>() as any;

const upNextVideo = computed(() => {
    const vid = useRepo(YoutubeVideoRepository).getById(props.videoId)
    return useRepo(YoutubeVideoRepository).getById(vid?.relatedVideos[0]) as YoutubeVideo;
});

const onReady = (event: any) => {
    console.log('ready', event.target);
    const duration = event.target.getDuration()
    const isPlaying = event.target.getPlayerState() === 1;
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

};
const onStateChange = (event: any) => {

    const events: { [key: string]: string } = {
        "-1": "unstarted",
        "0": "ended",
        "1": "playing",
        "2": "paused",
        "3": "buffering",
        "5": "video cued"
    }
    console.log('onStateChange', events[event.data.toString()]);
    // -1 (unstarted)
    // 0 (ended)
    // 1 (playing)
    // 2 (paused)
    // 3 (buffering)
    // 5 (video cued).

    const isPlaying = event.data === 1;

    if (isPlaying) {
        props.playerState.duration = event.target.getDuration();
    }

    if (event.data === 0) {
        props.playerState.currentTime = props.playerState.duration;
    }
    props.playerState.isPlaying = isPlaying;
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
    if (data.info?.currentTime !== undefined) {
        props.playerState.currentTime = data.info.currentTime;
    }

    if (data.info?.volume !== undefined) {
        if (data.info.muted) {
            props.playerState.volume = 0
        }
        else {
            props.playerState.volume = data.info.volume / 100;
        }
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
        <div class="overlay" v-if="playerState.duration - playerState.currentTime < 1 && !playerState.isPlaying">
            <youtube-player-up-next :video="upNextVideo" />
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
                    controls: 2
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
    width: 100%;
    z-index: 1;

}


.youtube-wrapper {
    display: flex;
    justify-content: center;
    align-items: center;
    overflow: hidden;

}

iframe {
    width: 100%;
    height: 100%;
    max-width: unset;
}
</style>
