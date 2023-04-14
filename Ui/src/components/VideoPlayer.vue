<script setup lang="ts">
import { ref, onMounted, computed, type Ref, nextTick, onBeforeUnmount } from 'vue';
import type { IPlayerState } from '@/models';
import VolumeButton from '@/components/buttons/VolumeButton.vue';
import FullscreenButton from '@/components/buttons/FullScreenButton.vue';
import SettingsButton from '@/components/buttons/SettingsButton.vue';
import ExitFullscreenButton from '@/components/buttons/ExitFullscreenButton.vue';
import PlayPauseButton from '@/components/buttons/PlayPauseButton.vue';
import SvgButton from '@/components/buttons/SvgButton.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';

const store = useYoutubeStore();

const props = defineProps<{
    src: string,
    color: string,
    cinema: boolean
}>()

const playerState: Ref<IPlayerState> = ref<IPlayerState>({
    isPlaying: false,
    volume: "0.75",
    duration: 0,
    currentTime: 0,
    isFullscreen: false,
    storedVolume: 0,
    settings: false,
    cinema: false
});


const previewPos = ref(0);

const startTime: Ref<number | undefined> = ref(0);
const startDate: Ref<number> = ref(0);
const animationId = ref(0);


const video = ref<HTMLVideoElement>();
const video_container = ref<HTMLDivElement>();
const timeline_container = ref<HTMLDivElement>();
const timeline = ref<HTMLDivElement>();

const thumb = ref<HTMLDivElement>();
const currentTime: Ref<number> = ref(0);

const volume = computed(() => {
    return video.value?.volume ? video.value.volume : 0;
});

const handleBuffer = async (evt: Event) => {
    if (video.value == null) return;
    await nextTick();
    let buffered = video.value.duration;
    try {
        buffered = video.value.buffered.end(0);
    } catch (e) {
        console.log("error", e)
    }
    let duration = Math.round((buffered / video.value.duration) * 100) + 1;

    timeline_container.value?.style.setProperty('--buffered', duration.toString() + "%");
}

onMounted(() => {
    window.addEventListener('keyup', handleKeyUp);
    if (video.value != null) {
        video.value.volume = parseFloat(playerState.value.volume);

        video.value.play()

        video.value.onplaying = async () => {
            console.log("onplaying")
            playerState.value.isPlaying = true;
            startTime.value = video.value?.currentTime
            startDate.value = Date.now()
            smoothUpdate();
        }

        video.value.onplay = async () => {
            console.log("onplay")
        }
        video.value.onpause = () => {
            console.log("onpause")
            playerState.value.isPlaying = false;
            cancelAnimationFrame(animationId.value);
        }

        video.value.onfullscreenchange = () => {
            playerState.value.isFullscreen = document.fullscreenElement != null;
        }
        video.value.ontimeupdate = (e: Event) => {
            handleBuffer(e)
            if (!video.value?.currentTime) return
            playerState.value.currentTime = video.value.currentTime;
            const perecent = Math.round(video.value.currentTime / video.value.duration * 100000) / 1000;
            timeline_container.value?.style.setProperty('--progress', perecent.toString() + "%");
        }

        video.value.onloadeddata = (e: Event) => {
            console.log("onloadeddata", e)
            console.log(video.value)
            if (video.value?.duration)
                playerState.value.duration = video.value.duration;
        }
        video.value.onprogress = (e: any) => {
            console.log("onprogress", e)
            handleBuffer(e)
        }
        video.value.onvolumechange = (e) => {
            let target = e.target as HTMLVideoElement;
            console.log("VOL CHANGE", e)
            playerState.value.volume = target.volume.toString();
        }
    }
});
onBeforeUnmount(() => {
    window.removeEventListener('keyup', handleKeyUp);
});
const toggleMute = (e: Event) => {
    (document.activeElement as HTMLElement).blur();

    if (video.value) {
        if (video.value.volume > 0) {
            playerState.value.storedVolume = video.value.volume;
            video.value.volume = 0;
        } else {
            video.value.volume = playerState.value.storedVolume;
        }
    }
}

const handleKeyUp = (e: KeyboardEvent) => {

    let testEl = document.activeElement as HTMLInputElement;

    if (testEl.type === "text") {
        return;
    }

    if (e.key === ' ' || e.key === 'k') {
        onPlayPause(e);
    }
    if (e.key == "f") {
        onDoubleClick(e);
    }
}

const onDoubleClick = (e: Event) => {
    console.log("onDoubleClicke", e)
    if (!playerState.value.isFullscreen && video.value) {
        video_container.value?.requestFullscreen();
    }
    else
        document.exitFullscreen();
    playerState.value.isFullscreen = !playerState.value.isFullscreen;
}


const onPlayPause = (e: Event) => {
    console.log("e", e)
    if (!playerState.value.isPlaying) {
        video.value?.play();
    } else {
        video.value?.pause();
    }
    playerState.value.isPlaying = !playerState.value.isPlaying;
}

const onVolumeChange = (e: Event) => {
    console.log("e", e)
    if (video.value) {
        video.value.volume = parseFloat(playerState.value.volume);
        console.log(video.value.volume)
    }
}
const auxclick = (e: Event) => {
    // console.log("ousside")
    playerState.value.settings = !playerState.value.settings
}
const onMouseMove = (e: MouseEvent) => {
    const rect = timeline_container.value?.getBoundingClientRect();
    if (rect == null)
        return;
    const percent = Math.min(Math.max(0, e.x - rect.x), rect.width) / rect.width;
    // console.log("onMouseMove", percent)
    previewPos.value = percent;
    timeline_container.value?.style.setProperty('--preview', percent.toString());
}

const onTimelineClick = async (e: MouseEvent) => {

    let isPlaying = false;
    if (playerState.value.isPlaying) {
        isPlaying = true;
    }

    // cancelAnimationFrame(animationId.value);
    const rect = timeline_container.value?.getBoundingClientRect();
    if (rect == null || video.value == null)
        return;
    video.value.pause();
    let percent = Math.min(Math.max(0, e.x - rect.x), rect.width) / rect.width;
    // let percent = Math.round(video.value.currentTime / video.value.duration * 100000) / 1000;
    console.log("perc" + percent)
    video.value.currentTime = percent * video.value.duration;
    console.log("timeline click" + percent)
    previewPos.value = percent;
    // timeline_container.value?.style.setProperty('--preview', percent.toString());
    const barWidth = timeline.value!.getBoundingClientRect().width * (percent);

    timeline_container.value?.style.setProperty('--bar', barWidth.toString() + "px");

    if (isPlaying) {
        video.value.play();
    }
    //     await nextTick();
    //     smoothUpdate();
    //     await nextTick();
    //     cancelAnimationFrame(animationId.value);
    //     await nextTick();
    //     video.value.pause()

    //     // video.value.pause();
    // }
    // else {
    // }
    // animationId.value = requestAnimationFrame(smoothUpdate);

}

const formatTime = (duration: number) => {
    return Math.floor(duration / 60) + ':' + ('0' + Math.floor(duration % 60)).slice(-2);
}

const progress = computed(() => {
    const prog = (playerState.value.currentTime / playerState.value.duration) * 100;
    console.log("progress", Math.round(prog * 100) / 100)
    return Math.round(prog / 100) * 100;
})

const smoothUpdate = () => {
    const elapsed = (Date.now() - startDate.value) / 1000;
    const currentTime = startTime.value! + elapsed;

    if (currentTime < playerState.value.duration) {
        if (!thumb.value || !timeline.value) {
            console.log("no thumb ref");
            return;
        }
        const barWidth = timeline.value!.getBoundingClientRect().width * (currentTime / playerState.value.duration);
        timeline_container.value?.style.setProperty('--bar', barWidth.toString() + "px");
        animationId.value = requestAnimationFrame(smoothUpdate);
    }

}

</script>

<template>
    <div ref="video_container" class="video-container"
        :class="[playerState.isPlaying ? '' : 'paused', playerState.settings ? 'settings' : '']"
        @dblclick.stop="onDoubleClick" @click="onPlayPause">
        <div class="video-controls-container">
            <div ref="timeline_container" class="timeline-container" @mousemove="onMouseMove" @click.stop="onTimelineClick"
                @mousedown="">
                <div ref="timeline" class="timeline">
                    <div class="buffered"></div>
                </div>
                <div ref="thumb" class="thumb-indicator"></div>
            </div>
            <div class="controls">
                <PlayPauseButton class="playPause" @click.stop="onPlayPause" :is-paused="!playerState.isPlaying" />
                <SvgButton path="M 12,24 20.5,18 12,12 V 24 z M 22,12 v 12 h 2 V 12 h -2 z" />
                <div class="volume-container">
                    <VolumeButton id="volume-btn" @click.stop="toggleMute" :volume="parseFloat(playerState.volume)" />
                    <input class="volume-input" @click.stop="" @input="onVolumeChange"
                        :style="{ 'background-size': Math.round(parseFloat(playerState.volume) * 100) + '% 100%' }" :min="0"
                        :max="1" step="any" v-model="playerState.volume" type="range">
                </div>


                <div class="duration-container">
                    {{ formatTime(playerState.currentTime) }} / {{ formatTime(playerState.duration) }}

                </div>
                {{ store.currentVideo!.height }}p
                <SettingsButton :toggled="playerState.settings" :hd="store.currentVideo!.width >= 960"
                    @click.stop="playerState.settings = !playerState.settings" />
                <SvgButton :style="'fill-rule: evenodd'"
                    path="M25,17 L17,17 L17,23 L25,23 L25,17 L25,17 Z M29,25 L29,10.98 C29,9.88 28.1,9 27,9 L9,9 C7.9,9 7,9.88 7,10.98 L7,25 C7,26.1 7.9,27 9,27 L27,27 C28.1,27 29,26.1 29,25 L29,25 Z M27,25.02 L9,25.02 L9,10.97 L27,10.97 L27,25.02 L27,25.02 Z" />
                <SvgButton :style="'fill-rule: evenodd'"
                    path="m 28,11 0,14 -20,0 0,-14 z m -18,2 16,0 0,10 -16,0 0,-10 z" />

                <FullscreenButton v-if="!playerState.isFullscreen" @click.stop="onDoubleClick" />
                <ExitFullscreenButton v-else @click.stop="onDoubleClick" />

                <div v-if="playerState.settings" class="settings-container" v-click-outside-element="auxclick">
                    <ul>
                        <li>settings entry one</li>
                        <li>set2</li>
                    </ul>
                </div>
            </div>
        </div>
        <video class="player" id="vid" aria-description="video" :src="props.src"  ref="video">
        </video>
    </div>
</template>

<style scoped>
*,
*::before,
*::after {
    box-sizing: border-box;
}

#vid {}

.video-container {
    width: 100%;
    flex-basis: 100%;
    display: flex;
    min-height: 320px;
}

.video-container.theater,
.video-container.full-screen {
    max-width: initial;
    width: 100%;
}


.video-container.full-screen {
    height: 100%;
    max-height: 100vh;

}

.video-container.full-screen .video-container.controls {
    z-index: 100;
}

.video-container.theater {
    max-height: 90vh;

}

video {
    width: 100%;
    max-width: 100%;
}

.video-controls-container {
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    color: white;
    z-index: 100;
    opacity: 0;
    transition: opacity 150ms ease;
}

.video-controls-container::before {
    content: "";
    position: absolute;
    bottom: 0;
    background: linear-gradient(to top, rgba(0, 0, 0, 0.75), transparent);
    width: 100%;
    aspect-ratio: 6/1;
    z-index: -1;
    pointer-events: none;
}

.video-container.paused .video-controls-container,
.video-container.settings .video-controls-container,
/* .video-container:focus-within .video-controls-container, */
.video-container:hover .video-controls-container {
    opacity: 1;
}

.playPause {
    scale: .85;
}

button {
    background: none;
    border: none;
    padding: 0;
    height: 44px;
    width: 44px;
    color: white;
    flex-shrink: 0;
}

.video-controls-container .controls {
    display: flex;
    gap: 4px;
    padding: 0 .75rem;
    align-items: center;
    font-size: 1.1rem;
    padding-bottom: 2px;
    height: 36px;
}

.controls button {
    opacity: 1;
    fill: rgba(255,255,255,0.85);
    transition: fill 50ms ease;
}

.video-controls-container .controls button:hover {
    /* opacity: 1; */
    fill: rgba(255,255,255,1);

}

.video-controls-container .controls svg {
    fill: white;
    width: 100%;
    height: 100%;
}

.duration-container {
    font-size: 13px;
    color: rgb(211, 207, 201);
    display: flex;
    justify-content: center;
    /* align-items: center; */
    gap: .5rem;
    flex-grow: 1;
    flex-direction: column;
    white-space: nowrap;
}



input[type="range"] {
    -webkit-appearance: none;
    appearance: none;
    /* margin-right: 15px; */
    height: 4px;
    background: rgba(255, 255, 255, 0.2);
    border-radius: 0;
    background-image: linear-gradient(#fff, #fff);
    background-size: 75% 100%;
    background-repeat: no-repeat;
    cursor: pointer;

}

input[type="range"]::-webkit-slider-thumb {
    -webkit-appearance: none;
    cursor: pointer;
    height: 12px;
    width: 12px;
    border-radius: 50%;
    background: #fff;
    margin-left: 1px;
    /* cursor: ew-resize; */
    /* box-shadow: 0 0 2px 0 #555; */
    transition: background .3s ease-in-out;
}

input[type=range]::-webkit-slider-runnable-track {
    -webkit-appearance: none;
    box-shadow: none;
    border: none;
    background: transparent;
    margin: 0 -2px;
}

.volume-container,
.volume-input {
    display: flex;
    align-items: center;
}

.volume-input input {
    /* width: 100%; */
    /* height: 4px; */
    border-radius: 0;
    width: 100%;
}


.volume-input {
    width: 0;
    transform-origin: left;
    transform: scaleX(0);
    margin-right: .25rem;
    transition: width 150ms ease-in-out, transform 150ms ease-in-out, margin-right 150ms ease-in-out;
}

.volume-container:hover .volume-input,
#volume-btn:focus-within .volume-input,

.volume-container:focus-within .volume-input {
    width: 60px;
    transform: scaleX(1);
    margin-right: .5rem;
}


.timeline-container {
    /* top: 5px; */
    display: flex;
    height: 20px;
    margin-inline: .5rem;
    cursor: pointer;
    align-items: center;
    /* background: blue; */
}



.timeline {
    overflow: hidden;

    top: 4px;
    height: 3px;
    /* transform: scaleY(.6); */
    margin: 0 .25rem;
    width: 100%;
    position: relative;
    background-color: rgba(255, 255, 255, 0.2);
    /* background-image: linear-gradient(rgba(255, 255, 255, 0.2), rgba(255, 255, 255, 0.2));
    background-size: 50% 100%;
    background-repeat: no-repeat; */


    transition: transform .15s cubic-bezier(0, 0, 0.2, 1), top .15s cubic-bezier(0, 0, 0.2, 1), height .1s cubic-bezier(0, 0, .2, 1);
}

.buffered {
    height: 100%;
    background-image: linear-gradient(rgba(255, 255, 255, 0.3), rgba(255, 255, 255, 0.3));
    background-size: var(--buffered) 100%;
    background-repeat: no-repeat;
}

.timeline-container:hover>.timeline {
    height: 7px;
    /* transform: scaleY(1.4); */
}

.timeline-container:hover>.timeline::before {
    height: 100%;
}

.timeline-container:hover .thumb-indicator {
    --scale: 1;
    opacity: 1;

}

.timeline::before {
    content: "";
    position: absolute;
    top: 0;
    left: 0;
    height: 0%;
    right: calc(100% - var(--preview) * 100%);
    background-color: rgba(255, 255, 255, 0.5);
    transition: width 150ms ease-in-out, height 100ms ease-in-out;
}

.timeline::after {
    content: "";
    transform: translateX(var(--bar));
    height: 100%;
    width: 100%;
    position: absolute;
    top: 0;
    left: -100%;
    /* background-color: lime; */
    background-color: v-bind(color);
    /* content: "";
    position: absolute;
    top: 0;
    left: 0;
    height: 100%;
    right: calc(100% - var(--progress));
    
    transition: width 150ms ease-in-out, right 100ms ease; */
}



.thumb-indicator {
    --scale: 0;
    opacity: 0;
    scale: var(--scale);
    position: absolute;
    left: -2px;
    top: 8px;
    height: 13px;
    transition: opacity 150ms ease;
    transform: translateX(var(--bar));

    background-color: v-bind(color);
    border-radius: 50%;
    aspect-ratio: 1;
    z-index: 2;
    pointer-events: none;
}

.settings-container {
    position: absolute;
    right: 24px;
    bottom: 90px;
    display: flex;
    gap: 1rem;
    align-items: center;
    background-color: rgba(48, 48, 48, 0.85);
    border-radius: 15px;
    margin: 0;
    padding: 1rem;
    line-height: 2rem;
}

.player {
    max-height: 100%;
}
</style>
