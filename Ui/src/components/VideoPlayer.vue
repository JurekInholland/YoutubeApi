<script setup lang="ts">
import { ref, onMounted, computed, type Ref, nextTick, onBeforeUnmount, watch, onUnmounted } from 'vue';
import VolumeButton from '@/components/buttons/VolumeButton.vue';
import FullscreenButton from '@/components/buttons/FullScreenButton.vue';
import SettingsButton from '@/components/buttons/SettingsButton.vue';
import ExitFullscreenButton from '@/components/buttons/ExitFullscreenButton.vue';
import PlayPauseButton from '@/components/buttons/PlayPauseButton.vue';
import SvgButton from '@/components/buttons/SvgButton.vue';
import UpNext from '@/components/player/UpNext.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import type { PlayerState } from '@/types';
import { useRoute } from 'vue-router';
import PlayerTooltip from './player/PlayerTooltip.vue';
import SvgLink from './buttons/SvgLink.vue';

const route = useRoute();
const store = useYoutubeStore();

const props = defineProps<{
    src: string,
    color: string,
    cinema: boolean,
    playerState: PlayerState
}>()

const emits = defineEmits<{
    (event: 'update:cinema', value: boolean): void
    (event: 'update:pictureInPicture', value: boolean): void
}>()

// const playerState: Ref<IPlayerState> = ref<IPlayerState>({
//     isPlaying: false,
//     volume: "0.75",
//     duration: 0,
//     currentTime: 0,
//     isFullscreen: false,
//     storedVolume: 0,
//     settings: false,
//     cinema: false,
//     pictureInPicture: false,
// });
watch(route, (r) => {
    console.log("VIDEO R CHANGE", r)
})

watch(() => props.src, (src) => {
    console.log("WATCH SRC")
    video.value?.pause();
    cancelAnimationFrame(animationId.value);
    props.playerState.currentTime = 0;
    // props.playerState.isPlaying = false;

    setTimeout(() => {
        video.value?.play();
    }, 100)

    const vid = video.value;
    // debugger;
    console.log(vid)

    // debugger;

    // setInterval(() => {
    //     if (props.playerState.isFullscreen && idleTime.value < 100)
    //         idleTime.value += 1;
    // }, 100)


    // console.log("VIDEO src CHANGE", src)
    // setupPlayer();
})
const previewPos = ref(0);

const startTime: Ref<number | undefined> = ref(0);
const startDate: Ref<number> = ref(0);
const animationId = ref(0);


const video = ref<HTMLVideoElement>();
const video_container = ref<HTMLDivElement>();
const timeline_container = ref<HTMLDivElement>();
const timeline = ref<HTMLDivElement>();

const thumb = ref<HTMLDivElement>();

const idleTime: Ref<number> = ref(0);

const lastClick: Ref<number> = ref(0);
const clickTimeout: Ref<number | null> = ref(null)

const currentHoverTime: Ref<string> = ref("0:00");
const currentPositionOffset: Ref<number> = ref(0);
const currentPositionPercent: Ref<string> = ref("0%");

const mounted = ref(false);

const handleBuffer = async (evt: Event) => {
    if (video.value == null) return;

    await nextTick();
    console.log("handleBuffer", evt)
    let buffered = video.value.duration;
    try {
        const len = video.value.buffered.end.length;
        console.log("len", len)
        buffered = video.value.buffered.end(len - 1);
    } catch (e) {
        console.log("handleBuffer error", e)
    }
    let duration = Math.round((buffered / video.value.duration) * 100) + 1;
    // console.log("buffered: ", buffered, "duration: ", video.value.duration, "duration: ", duration, "%")
    timeline_container.value?.style.setProperty('--buffered', duration.toString() + "%");
}

const setupPlayer = () => {
    if (video.value == null) return;
    // cancelAnimationFrame(animationId.value);

    video.value.volume = props.playerState.volume;
    video.value.currentTime = props.playerState.currentTime;
    if (props.playerState.isPlaying) {
        try {
            video.value.play();
        }
        catch (e) {
            console.log("errorasdasd", e)
        }
    }
    else {
        video.value.pause();
    }
}

onMounted(() => {

    setInterval(() => {
        if (props.playerState.isFullscreen && idleTime.value < 100)
            idleTime.value += 1;
    }, 100)

    window.addEventListener('mousemove', () => {
        idleTime.value = 0;
    })

    window.addEventListener('keydown', handleKeyDown);
    window.addEventListener('keyup', handleKeyUp);

    window.addEventListener('fullscreenchange', onFullscreenChange)

    if (video.value != null) {
        video.value.volume = props.playerState.volume;
        setupPlayer();

        video.value.onplaying = async () => {
            console.log("onplaying")
            props.playerState.isPlaying = video.value?.paused == false;
            startTime.value = video.value?.currentTime
            startDate.value = Date.now()
            smoothUpdate();
        }

        video.value.onpause = () => {
            if (!mounted.value) return;
            console.log("onpause")
            props.playerState.isPlaying = false;
            cancelAnimationFrame(animationId.value);
        }
        video.value.onenterpictureinpicture = () => {
            props.playerState.pictureInPicture = true;
        }
        video.value.onleavepictureinpicture = () => {
            props.playerState.pictureInPicture = false;
        }
        video.value.onfullscreenchange = () => {
            props.playerState.isFullscreen = document.fullscreenElement != null;
        }
        video.value.ontimeupdate = (e: Event) => {
            handleBuffer(e)
            if (!video.value?.currentTime) return
            props.playerState.currentTime = video.value.currentTime;
            const perecent = Math.round(video.value.currentTime / video.value.duration * 100000) / 1000;
            timeline_container.value?.style.setProperty('--progress', perecent.toString() + "%");
        }

        video.value.onloadeddata = (e: Event) => {
            if (video.value?.duration)
                props.playerState.duration = video.value.duration;
            video.value?.play()
            // console.log("onloadeddata", e)
            // console.log(video.value)
        }
        video.value.onprogress = (e: any) => {
            console.log("onprogress", e)
            handleBuffer(e)
        }
        video.value.onvolumechange = (e) => {
            let target = e.target as HTMLVideoElement;
            console.log("VOL CHANGE", target.volume)
            props.playerState.volume = target.volume;
        }
    }
    mounted.value = true;
});

onBeforeUnmount(() => {
    mounted.value = false;
    window.removeEventListener('keyup', handleKeyUp);
    window.removeEventListener('keydown', handleKeyDown);
    window.removeEventListener('fullscreenchange', onFullscreenChange)

    window.removeEventListener('mousemove', () => {
        idleTime.value = 0;
    })
});
const toggleMute = (e: Event) => {
    (document.activeElement as HTMLElement).blur();

    if (video.value) {
        if (video.value.volume > 0) {
            props.playerState.storedVolume = video.value.volume;
            video.value.volume = 0;
        } else {
            video.value.volume = props.playerState.storedVolume;
        }
    }
}

const handleKeyDown = (e: KeyboardEvent) => {

};

const handleKeyUp = (e: KeyboardEvent) => {
    e.preventDefault();

    let activeElement = document.activeElement as HTMLInputElement;

    if (activeElement.type === "text") {
        return;
    }

    if (e.key === ' ' || e.key === 'k') {
        onPlayPause(e);
    }
    if (e.key == "f") {
        onDoubleClick(e);
    }
}
const onFullscreenChange = (e: Event) => {
    props.playerState.isFullscreen = document.fullscreenElement != null;
}
const onDoubleClick = (e: Event) => {
    console.log("onDoubleClicke", e)
    if (!props.playerState.isFullscreen && video.value) {
        video_container.value?.requestFullscreen();
    }
    else
        document.exitFullscreen();
    props.playerState.isFullscreen = !props.playerState.isFullscreen;
}


const onClick = (e: Event) => {

    if (clickTimeout.value) {
        clearTimeout(clickTimeout.value)
        clickTimeout.value = null
        lastClick.value = 0
        console.log("dbclick")
        onDoubleClick(e)
    } else {
        clickTimeout.value = setTimeout(() => {
            clickTimeout.value = null

            if (lastClick.value === 1) {
                console.log("single click")
                onPlayPause(e)
            }
            lastClick.value -= 1
        }, 250)
        lastClick.value += 1
    }


    // // console.log("onClick", e)
    // if (Date.now() - lastClick.value < 300) {
    //     // onDoubleClick(e);
    //     console.log("double click")
    //     return;
    // }
    // else {
    //     console.log("single click")
    // }
    // lastClick.value = Date.now();
}

const onPlayPause = (e: Event) => {
    console.log("e", e)
    if (!props.playerState.isPlaying) {
        video.value?.play();
    } else {
        video.value?.pause();
    }
    props.playerState.isPlaying = !props.playerState.isPlaying;
}

const onVolumeChange = (e: Event) => {
    console.log("e", e)
    if (video.value) {
        video.value.volume = props.playerState.volume;
        console.log(video.value.volume)
    }
}
const auxclick = (e: Event) => {
    // console.log("ousside")
    props.playerState.settings = !props.playerState.settings
}
const onMouseMove = (e: MouseEvent) => {
    const rect = timeline_container.value?.getBoundingClientRect();
    if (rect == null)
        return;
    const percent = Math.min(Math.max(0, e.x - rect.x), rect.width) / rect.width;
    // console.log("onMouseMove", percent)
    previewPos.value = percent;
    timeline_container.value?.style.setProperty('--preview', percent.toString());
    let ctime = video.value!.duration * percent;
    currentHoverTime.value = formatTime(ctime);
    currentPositionOffset.value = percent;
    currentPositionPercent.value = (percent * 100).toString() + "%";

    // console.log("ctime", formatTime(ctime))
}
const onCinemaClick = (e: MouseEvent) => {
    emits("update:cinema", props.playerState.cinema)
}
const onPictureInPictureClick = (e: MouseEvent) => {

    props.playerState.pictureInPicture = !props.playerState.pictureInPicture;
    if (props.playerState.pictureInPicture)
        video.value?.requestPictureInPicture();
    else
        document.exitPictureInPicture();
    // emits("update:pictureInPicture", props.playerState.pictureInPicture)
}
const onTimelineClick = async (e: MouseEvent) => {

    let isPlaying = false;
    if (props.playerState.isPlaying) {
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
    const prog = (props.playerState.currentTime / props.playerState.duration) * 100;
    console.log("progress", Math.round(prog * 100) / 100)
    return Math.round(prog / 100) * 100;
})

const onReplay = () => {
    props.playerState.currentTime = 0;
    video.value?.play();
    props.playerState.isPlaying = true;
    // animationId.value = requestAnimationFrame(smoothUpdate);
}

const smoothUpdate = () => {
    const elapsed = (Date.now() - startDate.value) / 1000;
    const currentTime = startTime.value! + elapsed;

    if (currentTime < props.playerState.duration) {
        if (!thumb.value || !timeline.value) {
            console.log("no thumb ref");
            return;
        }
        const barWidth = timeline.value!.getBoundingClientRect().width * (currentTime / props.playerState.duration);
        timeline_container.value?.style.setProperty('--bar', barWidth.toString() + "px");
        animationId.value = requestAnimationFrame(smoothUpdate);
    }

}
</script>
<!-- @dblclick.stop="onDoubleClick" @click="onPlayPause"> -->

<template>
    <div ref="video_container" class="video-container"
        :class="[playerState.isPlaying ? '' : 'paused', playerState.settings ? 'settings' : '', idleTime > 50 ? 'cursor-hidden' : '']"
        @click="onClick">

        <div class="video-controls-container" :class="idleTime > 50 && playerState.isFullscreen ? 'hidden' : ''">
            <div ref="timeline_container" class="timeline-container" @mousemove="onMouseMove" @click.stop="onTimelineClick"
                @mousedown="">
                <div class="cTime">{{ currentHoverTime }}</div>
                <!-- <div class="ctime-cont"> -->
                <!-- </div> -->
                <div ref="timeline" class="timeline">
                    <div class="buffered"></div>

                </div>
                <div class="tp">

                    <div ref="thumb" class="thumb-indicator">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>
            <div class="controls" @click.stop="">
                <PlayPauseButton v-if="playerState.duration - playerState.currentTime > 1" class="playPause"
                    @click.stop="onPlayPause" :is-paused="!playerState.isPlaying">
                    <PlayerTooltip :text="playerState.isPlaying ? 'Pause (k)' : 'Play (k)'" />

                </PlayPauseButton>

                <SvgButton v-else class="replay-button" @click.stop="onReplay"
                    path="M 18,11 V 7 l -5,5 5,5 v -4 c 3.3,0 6,2.7 6,6 0,3.3 -2.7,6 -6,6 -3.3,0 -6,-2.7 -6,-6 h -2 c 0,4.4 3.6,8 8,8 4.4,0 8,-3.6 8,-8 0,-4.4 -3.6,-8 -8,-8 z">
                    <PlayerTooltip text="Replay" />
                </SvgButton>

                <SvgLink v-if="store.relatedVideos[0]" class="button"
                    path="M 12,24 20.5,18 12,12 V 24 z M 22,12 v 12 h 2 V 12 h -2 z" text=""
                    :href="{ name: 'watch', query: { v: store.relatedVideos[0].id } }">
                    <UpNext :video="store.relatedVideos[0]" />
                </SvgLink>

                <!-- <SvgButton class="next-button" path="M 12,24 20.5,18 12,12 V 24 z M 22,12 v 12 h 2 V 12 h -2 z">
                                                            <UpNext :video="store.relatedVideos[0]" />
                                                        </SvgButton> -->
                <div class="volume-container">
                    <VolumeButton id="volume-btn" @click.stop="toggleMute" :volume="playerState.volume">

                        <PlayerTooltip :text="playerState.volume > 0 ? 'Mute (m)' : 'Unmute (m)'" />

                    </VolumeButton>
                    <input class="volume-input" @click.stop="" @input="onVolumeChange"
                        :style="{ 'background-size': Math.round(Math.floor(playerState.volume * 100)) + '% 100%' }" :min="0"
                        :max="1" step="any" v-model="playerState.volume" type="range">
                </div>


                <div class="duration-container">
                    {{ formatTime(playerState.currentTime) }} / {{ formatTime(playerState.duration) }}

                </div>
                {{ store.currentVideo!.height }}p
                <SettingsButton :toggled="playerState.settings" :hd="store.currentVideo!.width >= 960"
                    @click.stop="playerState.settings = !playerState.settings">
                    <PlayerTooltip v-if="!playerState.settings" text="Settings" />
                </SettingsButton>
                <SvgButton :style="'fill-rule: evenodd'" @click.stop="onPictureInPictureClick"
                    path="M25,17 L17,17 L17,23 L25,23 L25,17 L25,17 Z M29,25 L29,10.98 C29,9.88 28.1,9 27,9 L9,9 C7.9,9 7,9.88 7,10.98 L7,25 C7,26.1 7.9,27 9,27 L27,27 C28.1,27 29,26.1 29,25 L29,25 Z M27,25.02 L9,25.02 L9,10.97 L27,10.97 L27,25.02 L27,25.02 Z">
                    <PlayerTooltip text="Miniplayer (i)" />
                </SvgButton>
                <SvgButton :style="'fill-rule: evenodd'" @click.stop="onCinemaClick"
                    :path="playerState.cinema ? 'm 26,13 0,10 -16,0 0,-10 z m -14,2 12,0 0,6 -12,0 0,-6 z' : 'm 28,11 0,14 -20,0 0,-14 z m -18,2 16,0 0,10 -16,0 0,-10 z'">
                    <PlayerTooltip :text="playerState.cinema ? 'Default mode' : 'Theater mode (t)'" />

                </SvgButton>

                <FullscreenButton class="fullscreen-button" v-if="!playerState.isFullscreen" @click.stop="onDoubleClick">
                    <PlayerTooltip text="Full screen (f)" />
                </FullscreenButton>
                <ExitFullscreenButton class="fullscreen-button" v-else @click.stop="onDoubleClick">
                    <PlayerTooltip text="Exit full screen (f)" />

                </ExitFullscreenButton>

                <div v-if="playerState.settings" class="settings-container" v-click-outside-element="auxclick">
                    <ul>
                        <li>settings entry one</li>
                        <li>set2</li>
                    </ul>
                </div>
            </div>
        </div>
        <video :muted="false" class="player" id="vid" aria-description="video" :src="props.src" ref="video">
        </video>
    </div>
</template>

<style scoped>
*,
*::before,
*::after {
    box-sizing: border-box;
}

.tp {
    width: 0;
    position: absolute;
}

.ctime-cont {
    display: flex;
    width: 100%;
    height: 0;
    position: relative;
    height: 1.5rem;
}

.cTime {
    font-size: 0.8rem;
    opacity: 0;
    transition: opacity .25s ease;
    position: absolute;
    text-align: center;
    left: v-bind(currentPositionPercent);
    bottom: 2rem;
    margin-left: -1rem;
    /* right: calc(100% - var(--preview) * 100%); */

}

.timeline-container:hover .cTime {
    opacity: 1;
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

.hidden {
    opacity: 0 !important;
}

.cursor-hidden {
    cursor: none !important;
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
    font-size: .85rem;
}

.controls {
    position: relative;
}

.controls .button {
    width: 44px;
    height: 44px;
}

.controls .button,
.controls button {
    opacity: 1;
    fill: var(--button-color);
    transition: fill 50ms ease;
}

.video-controls-container .controls .button:hover {
    background-color: transparent;
    fill: rgba(255, 255, 255, 1);
}

.video-controls-container .controls button:hover {
    /* opacity: 1; */
    fill: rgba(255, 255, 255, 1);

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

.volume-container {
    width: 42px;
    transition: width 150ms ease-in-out;
}

.volume-container:hover,
#volume-btn:focus-within .volume-container,
.volume-container:focus-within {
    width: 112px;

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
    transition: background-size 150ms ease-in-out;
}

.timeline-container:hover>.timeline {
    height: 7px;
    /* transform: scaleY(1.4); */
}

.timeline-container:hover>.timeline::before {
    height: 100%;
}

/* .timeline-container:hover .thumb-indicator {

} */

.timeline-container:hover .circle {
    transform: scale(.65);
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
    position: absolute;
    /* top: 3.5px; */
    /* top: -6px; */
    top: calc(-1.5rem / 3);
    left: -4px;
    /* left: -2px;
    top: 8px;
    height: 13px; */
    transition: opacity 150ms ease;

    aspect-ratio: 1;
    z-index: 2;
    pointer-events: none;
    width: 1.5rem;
    height: 1.5rem;
    transform: translateX(var(--bar));
    /* width: var(--size);
    height: var(--size); */
    /* transition: width .5s ease, height .5s ease; */
}

.circle {
    opacity: 0;
    border-radius: 50%;
    background-color: v-bind(color);
    width: 100%;
    height: 100%;
    transition: transform .25s ease, opacity .25s ease;
    transform: scale(.2);
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
<style>
:root {
    --button-color: rgba(255, 255, 255, 0.85);
}
</style>
