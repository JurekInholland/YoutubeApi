<script setup lang="ts">
import { ref, onMounted, computed, type Ref } from 'vue';
import { Icon } from '@iconify/vue';
import type { IPlayerState } from '@/models';

const props = defineProps<{
    src: string,
    color: string
}>()

const playerState: Ref<IPlayerState> = ref<IPlayerState>({
    isPlaying: false,
    volume: 1,
    duration: 0,
    currentTime: 0,
    isFullscreen: false
});

const paused = ref(true);

const video = ref<HTMLVideoElement>();
const video_container = ref<HTMLDivElement>();

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

onMounted(() => {
    window.addEventListener('keyup', handleKeyUp);
    if (video.value != null) {
        video.value.onplaying = () => {
            console.log("playing")
            playerState.value.isPlaying = true;
        }
        video.value.onpause = () => {
            playerState.value.isPlaying = false;
        }
        video.value.onloadeddata = () => {
            if (video.value?.duration)
                playerState.value.duration = video.value.duration;
        }
        video.value.ontimeupdate = () => {
            if (video.value?.currentTime)
                playerState.value.currentTime = video.value.currentTime;
        }
        video.value.onfullscreenchange = () => {
            playerState.value.isFullscreen = document.fullscreenElement != null;
        }
    }
});

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
    if (paused.value) {
        video.value?.play();
    } else {
        video.value?.pause();
    }
    paused.value = !paused.value;
}



function formatTime(duration: number) {
    return Math.floor(duration / 60) + ':' + ('0' + Math.floor(duration % 60)).slice(-2);
}
</script>

<template>
    <div ref="video_container" class="video-container" :class="paused ? 'paused' : ''"
        @dblclick.stop="onDoubleClick" @click="onPlayPause">

        <div class="video-controls-container">
            <div class="timeline-conainer"></div>
            <div class="controls">
                <button class="play-pause-btn" @click.stop="onPlayPause">
                    <Icon style="font-size: 1rem;" :icon="paused ? 'mdi:play' : 'mdi:pause'" />
                </button>
                <button>
                    <Icon style="font-size: 1rem;" icon="mdi:skip-next" @click.stop="" />
                </button>
                <div class="volume-container">
                    <div class="duration-container">
                        {{ formatTime(playerState.currentTime) }}/{{ formatTime(playerState.duration) }}
                    </div>
                    <button>
                        <Icon style="font-size: 1rem;" icon="mdi:volume-high" />
                    </button>
                </div>


                <button>
                    <Icon @click.stop="onDoubleClick" style="font-size: 1rem;" icon="mdi:fullscreen" />
                </button>
                <button>
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
                </button>

            </div>
        </div>
        <video :src="props.src" ref="video">
        </video>
    </div>
</template>

<style scoped>
*,
*::before,
*::after {
    box-sizing: border-box;
}

video::-webkit-media-controls {
    display: none !important;
}

.video-container {
    position: relative;
    width: 90%;
    max-width: 1000px;
    display: flex;
    justify-content: center;
    margin-inline: auto;
    background-color: black;
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
/* .video-container:focus-within .video-controls-container, */
.video-container:hover .video-controls-container {
    opacity: 1;
}

button {
    background: none;
    border: none;
    padding: 0;
    height: 30px;
    width: 30px;
    color: white;
}

.video-controls-container .controls {
    display: flex;
    gap: 1rem;
    padding: .25rem;
    align-items: center;
    font-size: 1.1rem;
}

.video-controls-container .controls button {
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
    display: flex;
    align-items: center;
    gap: .5rem;
    flex-grow: 1;
    flex-direction: column;
}
</style>