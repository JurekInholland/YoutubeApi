<script setup lang="ts">
import { ref, onMounted, computed, type Ref } from 'vue';
import { Icon } from '@iconify/vue';
import type { IPlayerState } from '@/models';
import VolumeButton from '@/components/buttons/VolumeButton.vue';
import FullscreenButton from './buttons/FullscreenButton.vue';
import SettingsButton from './buttons/SettingsButton.vue';
import ExitFullscreenButton from './buttons/ExitFullscreenButton.vue';
import PlayPauseButton from './buttons/PlayPauseButton.vue';
import SvgButton from './buttons/SvgButton.vue';

const props = defineProps<{
    src: string,
    color: string
}>()

const playerState: Ref<IPlayerState> = ref<IPlayerState>({
    isPlaying: false,
    volume: "0.75",
    duration: 0,
    currentTime: 0,
    isFullscreen: false,
    storedVolume: 0,
    settings: false
});

// const paused = ref(true);

const previewPos = ref(0);

const startTime: Ref<number | undefined> = ref(0);
const startDate: Ref<number> = ref(0);
const animationId = ref(0);


const video = ref<HTMLVideoElement>();
const video_container = ref<HTMLDivElement>();
const timeline_container = ref<HTMLDivElement>();
const timeline = ref<HTMLDivElement>();

const thumb = ref<HTMLDivElement>();
// const settings = ref(false)

const duration: Ref<number> = ref(0);
// const duration = computed(() => {
//     return video.value?.duration ? formatTime(video.value.duration) : '0:00';
// });
const currentTime: Ref<number> = ref(0);

// const formattedDuration = computed(() => {
//     return duration ? formatTime(duration.value) : '0:00';
// });

// const formattedCurrentTime = computed(() => {
//     return currentTime ? formatTime(currentTime.value) : '0:00';
// });

const volume = computed(() => {
    return video.value?.volume ? video.value.volume : 0;
});

const handleBuffer = (e: Event) => {
    if (video.value == null) return;
    // if (video.value.buffered.length == 0) return;
    // console.log(video.value.buffered.length)
    let duration = Math.round((video.value.buffered.end(0) / video.value.duration) * 100) + 1;
    // console.log("handleBuffer", video.value.buffered.end(0), video.value.duration, duration, "buffered")
    timeline_container.value?.style.setProperty('--buffered', duration.toString() + "%");

}

onMounted(() => {
    window.addEventListener('keyup', handleKeyUp);
    if (video.value != null) {
        video.value.volume = parseFloat(playerState.value.volume);

        // video.value.onplaying = () => {
        //     console.log("playing")
        // }
        video.value.onplay = () => {
            playerState.value.isPlaying = true;
            // console.log("play", this)
            startTime.value = video.value?.currentTime
            startDate.value = Date.now()
            smoothUpdate();
            // this.lastUpdate = Date.now();
        }
        video.value.onpause = () => {
            console.log("onpause")
            playerState.value.isPlaying = false;
            cancelAnimationFrame(animationId.value);
        }
        // video.value.onloadeddata = () => {
        //     if (video.value?.duration)
        //         playerState.value.duration = video.value.duration;
        // }


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
            // handleBuffer(e)
            // if (video.value?.currentTime) {
            //     console.log("onloadeddata", e)

            //     playerState.value.currentTime = video.value.currentTime;
            //     const perecent = Math.round(video.value.currentTime / video.value.duration);
            //     timeline_container.value?.style.setProperty('--progress', perecent.toString());
            // }
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

const toggleMute = (e: Event) => {
    (document.activeElement as HTMLElement).blur();
    // let target = e.target as HTMLButtonElement;
    // target.focus();

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
        // video.value.controls = false;
        // video.value.removeAttribute("controls");
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
    console.log("ousside")
    playerState.value.settings = !playerState.value.settings
}
const onMouseMove = (e: MouseEvent) => {
    const rect = timeline_container.value?.getBoundingClientRect();
    if (rect == null)
        return;
    const percent = Math.min(Math.max(0, e.x - rect.x), rect.width) / rect.width;
    console.log("onMouseMove", percent)
    previewPos.value = percent;
    timeline_container.value?.style.setProperty('--preview', percent.toString());
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
    const elapsed = (Date.now() - startDate.value!) / 1000;
    const currentTime = startTime.value! + elapsed;
    // console.log("elapsed", elapsed, "currentTime", currentTime)
    if (currentTime < playerState.value.duration) {
        // playerState.value.currentTime = currentTime;
        // startTime.value = Date.now();
        // console.log("WIDTH", timeline.value!.getBoundingClientRect().width)
        // console.log("elapsed:", elapsed, "total", playerState.value.duration, "current", currentTime)
        // console.log("percent", (playerState.value.currentTime / playerState.value.duration))
        const barWidth = timeline.value!.getBoundingClientRect().width * (currentTime / playerState.value.duration);
        // const barPercent = Math.round(currentTime / playerState.value.duration * 100 * 100) / 100;
        // console.log("smoothUpdate", barWidth)
        if (!thumb.value) {
            console.log("no thumb ref");
            return;
        }
        // thumb.value.style.transform = `translateX(${barWidth}px)`;
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
            <div ref="timeline_container" class="timeline-container" @mousemove="onMouseMove" @mousedown="">
                <div ref="timeline" class="timeline">
                    <div class="buffered"></div>
                </div>
                <div ref="thumb" class="thumb-indicator"></div>
            </div>
            <div class="controls">
                <!-- <button class="play-pause-btn" @click.stop="onPlayPause">
                                                                                                                                                                                                                                                            <Icon style="font-size: 1rem;" :icon="paused ? 'mdi:play' : 'mdi:pause'" />
                                                                                                                                                                                                                                                        </button> -->
                <PlayPauseButton @click.stop="onPlayPause" :is-paused="!playerState.isPlaying" />
                <SvgButton path="M 12,24 20.5,18 12,12 V 24 z M 22,12 v 12 h 2 V 12 h -2 z" />
                <!-- <button>
                                                                                                                                                                                                                    <Icon style="font-size: 1rem;" icon="mdi:skip-next" @click.stop="" />
                                                                                                                                                                                                                </button> -->
                <div class="volume-container">
                    <!-- <button>
                                                                                                                                                                                                                                                                                                    <Icon style="font-size: 1rem;" icon="mdi:volume-high" />
                                                                                                                                                                                                                                                                                                </button> -->
                    <!-- <div class="volume-input"> -->
                    <VolumeButton id="volume-btn" @click.stop="toggleMute" :volume="parseFloat(playerState.volume)" />
                    <input class="volume-input" @click.stop="" @input="onVolumeChange"
                        :style="{ 'background-size': Math.round(parseFloat(playerState.volume) * 100) + '% 100%' }" :min="0"
                        :max="1" step="any" v-model="playerState.volume" type="range">
                    <!-- </div> -->


                </div>


                <div class="duration-container">
                    {{ formatTime(playerState.currentTime) }} / {{ formatTime(playerState.duration) }}

                </div>
                <SettingsButton :toggled="playerState.settings"
                    @click.stop="playerState.settings = !playerState.settings" />
                <SvgButton :style="'fill-rule: evenodd'"
                    path="M25,17 L17,17 L17,23 L25,23 L25,17 L25,17 Z M29,25 L29,10.98 C29,9.88 28.1,9 27,9 L9,9 C7.9,9 7,9.88 7,10.98 L7,25 C7,26.1 7.9,27 9,27 L27,27 C28.1,27 29,26.1 29,25 L29,25 Z M27,25.02 L9,25.02 L9,10.97 L27,10.97 L27,25.02 L27,25.02 Z" />
                <SvgButton :style="'fill-rule: evenodd'"
                    path="m 28,11 0,14 -20,0 0,-14 z m -18,2 16,0 0,10 -16,0 0,-10 z" />

                <FullscreenButton v-if="!playerState.isFullscreen" @click.stop="onDoubleClick" />
                <ExitFullscreenButton v-else @click.stop="onDoubleClick" />

                <div v-if="playerState.settings" class="settings-container" v-click-outside="auxclick">
                    <ul>
                        <li>settings entry one</li>
                        <li>set2</li>
                    </ul>
                </div>

                <!-- <button>
                                                                                                                                                                                                                                            <Icon @click.stop="onDoubleClick" style="font-size: 1rem;" icon="mdi:fullscreen" />
                                                                                                                                                                                                                                        </button> -->
                <!-- <button>
                                                                                                                                                                                                                            <Icon style="font-size: 1rem;" icon="mdi:settings" />
                                                                                                                                                                                                                        </button>
                                                                                                                                                                                                                        <button>
                                                                                                                                                                                                                            <Icon style="font-size: 1rem;" icon="mdi:download" />
                                                                                                                                                                                                                        </button>
                                                                                                                                                                                                                        <button>
                                                                                                                                                                                                                            <Icon style="font-size: 1rem;" icon="mdi:share" />
                                                                                                                                                                                                                        </button>
                                                                                                                                                                                                                        <button>
                                                                                                                                                                                                                            <Icon style="font-size: 1rem;" icon="mdi:playlist-plus" />
                                                                                                                                                                                                                        </button> -->

            </div>
        </div>
        <video id="vid" src="http://media.w3.org/2010/05/sintel/trailer.mp4" ref="video">
        </video>
    </div>


    <!-- <ExitFullscreenButton :is-hovered="settings" @click="settings = !settings" /> -->
<!-- 
    <p>
        {{ playerState.currentTime }}<br />
        {{ playerState.duration }}<br />
        {{ playerState.isPlaying }}<br />
        {{ playerState.isFullscreen }}<br />
        {{ playerState.volume }}<br />
    </p> -->
</template>

<style scoped>
*,
*::before,
*::after {
    box-sizing: border-box;
}

/* button::before {
    content: "";
    display: block;
    width: 12px;
    position: absolute;
    top: 5px;
    bottom: 0;
    left: -12px;
} */

.video-container {
    position: relative;
    /* width: 100%; */
    
    display: flex;
    justify-content: center;
    margin-inline: auto;
    /* margin: 0 24px; */
    /* margin-left: 24px;
    margin-right: 24px; */
    /* padding-right: 24px; */
    background-color: black;
    overflow: hidden;
}

.video-container.theater,
.video-container.full-screen {
    max-width: initial;
    width: 100%;
}


.video-container.full-screen {
    height: 100%;
}

.video-container.full-screen .video-container.controls {
    z-index: 100;
}

.video-container.theater {
    max-height: 90vh;

}

.video-container.full-screen {
    max-height: 100vh;
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
    padding: 0 .5rem;
    align-items: center;
    font-size: 1.1rem;
    padding-bottom: 2px;
}

.controls button {
    opacity: .85;
    transition: opacity 150ms ease;
}

.video-controls-container .controls button:hover {
    opacity: 1;
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
    background-color: red;
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
    /* transform: translateX(-50%) scale(var(--scale)); */
    height: 13px;
    /* top: -50%; */
    /* left: -.3rem; */
    /* left: var(--progress);
    transition: left 100ms ease; */
    /* transition: none; */
    transition: opacity 150ms ease;
    transform: translateX(var(--bar));

    background-color: red;
    border-radius: 50%;
    /* transition: transform 150ms ease; */
    aspect-ratio: 1 / 1 !important;
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
</style>