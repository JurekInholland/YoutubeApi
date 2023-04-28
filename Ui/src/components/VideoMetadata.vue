<script setup lang="ts">
import { computed, ref, watch, type Ref, nextTick, onMounted } from 'vue';
import { useRouter } from 'vue-router';

import { Icon } from '@iconify/vue';

import type YoutubeVideo from '@/models/YoutubeVideo';
import { formatDateAgo, formatDescription, formatViews, formatDate, formatTitle } from '@/utils';
import ToggleButton from "@/components/ToggleButton.vue";
import ProgressBar from "@/components/ProgressBar.vue";
import { useYoutubeStore } from '@/stores/youtubeStore';
import { apiService } from '@/constants';

const router = useRouter();
const props = defineProps<{ video: YoutubeVideo, modelValue: boolean, cinema: boolean, useLocalPlayer: boolean }>();
const store = useYoutubeStore();

const isDescriptionExpanded: Ref<boolean> = ref(false);

watch(router.currentRoute, () => {
    isDescriptionExpanded.value = false;
})

const queueItem = computed(() => {
    return store.queue.find((item) => item.id === props.video.id);
})

const toggleDescription = () => {
    if (!isDescriptionExpanded.value) {
        isDescriptionExpanded.value = true;
    }
}
const toggleDescriptionButton = () => {
    isDescriptionExpanded.value = !isDescriptionExpanded.value;

    if (!isDescriptionExpanded.value) {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
}

const emits = defineEmits<{
    // (e: 'update:modelValue', value: boolean): void,
    (e: 'update:cinema', value: boolean): void,
    (e: 'update:customPlayer', value: boolean): void,
}>();

const toggleCinema = () => {
    console.log('toggleCinema', props.modelValue);
    // props.cinema = !props.cinema;
    emits('update:cinema', !props.modelValue);
    // emits('update:modelValue', !props.modelValue);
}

const tog = ref(false);

watch(tog, (val) => {
    console.log('watch tog' + val)
    emits('update:customPlayer', val)
})

const backupVideo = async () => {

    if (queueItem.value) {
        console.log("already in queue")
        return;
    }

    await apiService.EnqueueDownload(props.video.id);
    await store.fetchQueue();
    await apiService.processQueue();
    await store.updateDownloadProgress({
        id: props.video.id,
        progress: 0,
        eta: 0,
        status: "queued",
        speed: 0,
        fragment: "",
    });
}

const runtimeSeconds = computed(() => {
    const duration = props.video.duration as string;
    const [hours, minutes, seconds] = duration.split(":").map(part => parseInt(part));
    // console.log("runtimeSeconds", hours, minutes, seconds, hours * 3600 + minutes * 60 + seconds)
    return hours * 3600 + minutes * 60 + seconds;
})
onMounted(() => {
    console.log("mounted metadata" + props.modelValue)
    tog.value = props.useLocalPlayer;
})

watch(props, (val) => {
    console.log('watch props.modelValue' + val)
    tog.value = val.useLocalPlayer;
})
</script>

<template>
    <div class="metadata">
        <div class="flex">
            <h1>
                <div class="title" v-html="formatTitle(props.video.title)"></div>
            </h1>
            <ProgressBar v-if="queueItem?.progress" :value="queueItem.progress.progress"
                :text="queueItem?.progress.progress !== 0 ? `${queueItem.progress.eta}` : 'Processing queue...'" />
            <!-- <ProgressBar :value="50" text="lel"></ProgressBar> -->

        </div>


        <div id="top-row">
            <div id="owner">
                <router-link v-if="video.youtubeChannel"
                    :to="{ name: 'channel', params: { username: video.youtubeChannel?.title } }" id="avatar">
                    <img :src="`${video.youtubeChannel.thumbnailUrl}`" alt="avatar" />
                    <!-- <img :src="`api/Thumbnail/channel?channelId=${video.youtubeChannel?.id}`" alt="avatar" /> -->
                </router-link>
                <div class="upload-info">
                    <router-link v-if="video.youtubeChannel"
                        :to="{ name: 'channel', params: { username: video.youtubeChannel?.title } }">
                        <h3>{{ props.video.youtubeChannel?.title }}</h3>
                    </router-link>
                    <p>{{ props.video.youtubeChannel?.subscribers }} subscribers</p>
                    <!-- <p>{{ props.video.youtubeChannel?.id }}</p> -->
                    <!-- <p>{{ props.video.channel_follower_count }} subscribers</p> -->
                </div>

            </div>

            <div class="buttons">

                <div class="likes">
                    <Icon icon="iconoir:thumbs-up" />
                    <p>{{ formatViews(props.video.likeCount) }}</p>
                </div>
                <ToggleButton v-if="props.video.localVideo" v-model="tog">{{ tog ? 'Custom Player' : 'Youtube Player' }}
                </ToggleButton>
                <button v-if="!props.video.localVideo" :disabled="runtimeSeconds > 3600 || queueItem !== undefined"
                    @click="backupVideo" class="backup-button">
                    <Icon icon="material-symbols:cloud-download-rounded" />
                </button>

                <a v-else class="button" :href="'/api/LocalVideo/GetVideoStream?videoId=' + props.video.id">
                    <Icon icon="solar:download-minimalistic-linear" />

                    Download
                </a>

                <!-- <button v-else class="download-button">
                                                                        <Icon icon="material-symbols:cloud-download-rounded" />
                                                                    </button> -->
                <button class="cinema-button" @click="toggleCinema">
                    <Icon icon="mdi:cinema" />
                </button>

            </div>
        </div>

        <div :class="isDescriptionExpanded ? 'expanded' : 'collapsed'" id="description" @click.stop="toggleDescription">
            <div class="info">{{ formatViews(props.video.viewCount) }} views &nbsp;{{
                isDescriptionExpanded ? formatDate(props.video.uploadDate) : formatDateAgo(props.video.uploadDate)
            }}</div>
            <div class="desc">
                <span v-html="formatDescription(props.video.description)"></span>
            </div>
            <button @click.stop="toggleDescriptionButton">{{ isDescriptionExpanded ? 'Show less' : 'Show more' }}</button>
        </div>
    </div>
</template>

<style scoped lang="scss">
.flex {
    display: flex;
    flex-wrap: nowrap;
    gap: 1rem;
    align-items: center;
    overflow: hidden;

    div {
        margin-bottom: .5rem;
        min-width: 150px;
        flex-shrink: 1;
    }

    h1 {
        display: inline-flex;
        flex-wrap: wrap;
        // flex-shrink: 0;
        word-break: break-word;
    }
}

.buttons {
    display: flex;
    gap: .5rem;
    align-items: center;
    margin-top: 1rem;
    flex-basis: 100px;
    flex-grow: 1;

    button {
        white-space: nowrap;
    }
}

.collapsed {
    max-height: 136px;
    cursor: pointer;
    overflow: hidden;
}


.collapsed .desc {
    max-height: 60px;
}

.expanded {
    max-height: 100%;
    cursor: default;

    span {
        margin-bottom: 1rem;
        overflow: hidden;
    }
}

.likes {
    color: rgba(255, 255, 255, 0.8);
    display: flex;
    align-items: center;
    justify-content: flex-end;
    gap: .25rem;
    flex-grow: 1;
    margin-right: 1rem;

    svg {
        font-size: 1.75rem;
        margin-bottom: .2rem;
    }
}

* {
    font-family: Roboto, sans-serif;
}

.desc {
    display: flex;
    // align-items: center;
    overflow: hidden;


    p,
    span {
        max-height: 100%;
        overflow-wrap: break-word;
        white-space: pre-wrap;
    }

}

#avatar {
    img {
        width: 40px;
        height: 40px;
        border-radius: 50%;
    }
}

.metadata {
    // margin-top: 12px;
    width: 100%;
    transition: unset;
    // flex-grow: 1;

}

.cinema .metadata {
    width: auto;
    flex-grow: 1;
}

h1 {
    font-weight: 600;
    font-size: 20px;
    line-height: 34px;
    // margin-bottom: 8px;
    color: rgb(223, 220, 216)
}

h3 {
    /* font-size: 1.25rem; */
    font-weight: 500;
    color: rgb(245, 245, 245);
    // height: 22px;
    line-height: 22px;
    white-space: pre-wrap;
}

#top-row {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;

}

#owner {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 0.7rem;
    // margin-bottom: -.2rem;

}

#owner img {
    border-radius: 50%;
}

.upload-info {
    display: flex;
    flex-direction: column;

    p {
        color: rgba(255, 255, 255, 0.6);
        height: 18px;
        white-space: nowrap;
    }
}

.info {
    font-weight: 500;
    line-height: 28px;
    color: white;
}

#description {
    border-radius: 12px;
    line-height: 20px;
    padding: 12px;
    margin: 12px 0;
    background-color: rgba(255, 255, 255, 0.1);
    white-space: pre-wrap;
    overflow: hidden;
    text-overflow: ellipsis;
    font-size: 14px;
    position: relative;
    box-sizing: border-box;
    transition: border-color 1.5s ease, background-color 0s;
    border: 1px solid transparent;


    button {
        border: 1px solid transparent;
        // position: absolute;
        cursor: pointer;
        margin: 0;
        padding: .5rem;
        // bottom: 0;
        // right: 0;
        // color: red;
        // margin-bottom: 1rem;
        background-color: transparent;
        font-size: 14px;
        height: 24px;
        font-weight: 500;
        z-index: 200;
        // background-color: red;

    }

}

// #description:not(:hover)::before {
//     transition: background-color 0;
// }
#description p {
    line-height: 20px;
    margin-top: 1px;
    // white-space: pre-wrap;

}

#description:hover {
    transition: background-color .2s;
    background-color: rgb(64, 64, 64);

}

#description.collapsed:hover {
    background-color: rgb(64, 64, 64);
    transition: background-color 0s, border-color 0s ease;

}

#description.expanded:hover {
    background-color: rgba(255, 255, 255, 0.1);
    transition: background-color .5s ease, border-color 2s ease;

}

#description.collapsed:active {
    background-color: rgb(84, 84, 84);
    border-color: rgb(92, 92, 92);
}

#owner p {
    font-size: 12px;
    line-height: 24px;

}


#owner .likes p {
    // color: white;
    font-weight: 500;
    font-size: 14px;
    text-align: center;
}

.button {
    padding-right: .75rem;
}

button,
.button {
    border: 1px solid transparent;
    box-sizing: border-box;
    display: flex;
    align-items: center;
    font-size: 14px;
    line-height: 36px;
    color: white;
    background-color: rgba(255, 255, 255, 0.1);

    border-radius: 18px;
    height: 36px;
    transition: border-color .75s ease;

    gap: 0;

    /* gap: .5rem; */
    /* background-color: red; */
    svg {
        width: 24px;
        height: 24px;
        margin-right: 1rem;
    }
}

button svg {
    margin-left: 2px;
    margin-right: .8rem;

}

.download-button {
    width: 114px;
}

.backup-button {
    width: 98px;


}

.cinema-button {
    width: 106px;

    svg {
        left: 6px;
    }
}

button,
.button {
    svg {
        font-size: 22px;
        left: 6px;
        flex-shrink: 0;
    }
}

button::after {
    aspect-ratio: 1;
    padding-left: 5px;
    max-height: 2rem;
    white-space: nowrap;
}

.download-button::after {
    content: 'Download';
}

.backup-button::after {
    content: 'Backup';
}

.cinema-button::after {
    aspect-ratio: 1;
    padding-left: 5px;
    // padding-left: 1rem;
    // padding-right: 19px;
    content: 'Theater';
    max-height: 2rem;
    white-space: nowrap;
}

.cinema .cinema-button::after {
    content: 'Default';
}

@media screen and (max-width: 680px) {

    .backup-button {
        width: 36px;

        svg {
            left: 5px;
        }
    }

    .backup-button::after {
        aspect-ratio: unset;
        content: '';
        padding: 0;
    }
}

.button:active,
button:active {
    transition: border-color 0s;
    background-color: rgb(84, 84, 84);
    border: 1px solid rgb(84, 84, 84);
}

button:disabled {
    pointer-events: none;
    opacity: 0.5;
}
</style>
<style>
.tag,
.desc a {
    color: var(--link-color);
}

.dlbtn {
    gap: 4px;
    font-weight: 500;
}

.dlbtn svg {
    height: 24px;
    width: 24px;
    /* margin-top: 10px; */
}

.dlbtn svg path {
    /* fill: yellow; */

}

.button:hover,
button:hover {
    background-color: rgba(255, 255, 255, .2);
}

/* .cinema #description,
.cinema #top-row {
    max-width: calc(960px);
} */
</style>
