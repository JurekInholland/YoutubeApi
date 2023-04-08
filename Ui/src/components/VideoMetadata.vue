<script setup lang="ts">
import { computed, ref, watch, type Ref } from 'vue';
import { useRouter } from 'vue-router';
import { Vue3ToggleButton } from 'vue3-toggle-button'
import '../../node_modules/vue3-toggle-button/dist/style.css'

import { Icon } from '@iconify/vue';

import type { YoutubeVideo } from '@/types';
import { formatDateAgo, formatDescription, formatViews, formatDate } from '@/utils';
import SvgButton from './buttons/SvgButton.vue';
import ToggleButton from "@/components/ToggleButton.vue";

const router = useRouter();
const props = defineProps<{ video: YoutubeVideo, modelValue: boolean }>();

const isDescriptionExpanded: Ref<boolean> = ref(false);

watch(router.currentRoute, (val) => {
    isDescriptionExpanded.value = false;
})

// const formatDescription = computed(() => {
//     const baseUrl = window.location.origin;

//     return props.video.description
//         .replaceAll(/(?<!&)#([\w-]+)/g, '<span class="tag">#$1</span>')
//         .replaceAll("https://www.youtube.com/", baseUrl + "/")
//         .replaceAll("https://youtu.be/", baseUrl + "/")
//         .replaceAll(/@(\w+)/g, '<span class="tag"><a href="@$1" target="_blank"> @$1 </a></span>')

//         .replaceAll(/(\\n)/gm, ' <br>')
//         .replaceAll(/(\\r)/gm, '')
//         .replaceAll(/(https?:\/\/[^\s]+)/g, '<a href="$1" target="_blank" rel="noopener noreferrer">$1</a>')
//         .replaceAll('\\"', '"')
// });

const cleanedTitle = computed(() => {
    return props.video.title
        .replace(/(?<!&)#([\w-]+)/g, '<span class="tag"><a href="@$1" target="_blank">#$1</a></span>')
        .replace(/@(\w+)/g, '<span class="tag"><a href="@$1" target="_blank"> @$1 </a></span>')
        .replace(/(https?:\/\/[^\s]+)/g, '<a href="$1" target="_blank" rel="noopener noreferrer">$1</a>')
})

const toggleDescription = () => {
    if (!isDescriptionExpanded.value) {
        isDescriptionExpanded.value = true;
    }
}
const toggleDescriptionButton = () => {
    isDescriptionExpanded.value = !isDescriptionExpanded.value;
}

const emits = defineEmits<{
    (e: 'update:modelValue', value: boolean): void
}>();

const toggleCinema = () => {
    console.log('toggleCinema', props.modelValue);
    // props.cinema = !props.cinema;
    emits('update:modelValue', !props.modelValue);
}
const tog = ref(false);
</script>

<template>
    <div class="metadata">
        <h1 v-html="cleanedTitle" />

        <div id="top-row">
            <div id="owner">
                <router-link v-if="video.youtubeChannel"
                    :to="{ name: 'channel', params: { username: video.youtubeChannel?.title } }" id="avatar">
                    <img :src="`api/Thumbnail/channel?channelId=${video.youtubeChannel?.id}`" alt="avatar" />
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
                <div class="likes">
                    <Icon icon="iconoir:thumbs-up" />
                    <p>{{ formatViews(props.video.likeCount) }}</p>
                </div>
            </div>
            <div class="buttons">
                <!-- <button>
                                                                                                    <Icon icon="maki:cinema" />
                                                                                                    Kino mode
                                                                                                </button>
                                                                                                <button>
                                                                                                    <Icon icon="ph:floppy-disk" />
                                                                                                    Backup
                                                                                                </button> -->

                <ToggleButton v-model="tog">{{ tog ? 'Custom Player' : 'Youtube Player' }}</ToggleButton>

                <!-- <Vue3ToggleButton v-model="tog" :handleColor="'#cc00cc'"> </Vue3ToggleButton> -->
                <button class="download-button">
                    <Icon icon="material-symbols:cloud-download-rounded" />

                </button>

                <button class="cinema-button" @click="toggleCinema">
                    <Icon icon="maki:cinema" />
                </button>
                <!-- <SvgButton class="dlbtn" text="Download" view-box="0 0 24 24"
                                                                                                                                                                    path="M17 18V19H6V18H17ZM16.5 11.4L15.8 10.7L12 14.4V4H11V14.4L7.2 10.6L6.5 11.3L11.5 16.3L16.5 11.4Z" /> -->
                <!-- <button>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        <Icon icon="clarity:download-line" />
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        Download
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    </button> -->
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
.buttons {
    display: flex;
    gap: .5rem;
    align-items: center;

    button {
        white-space: nowrap;
    }
}

.collapsed {
    max-height: 136px;
    cursor: pointer;
    overflow: hidden;
}

#description.collapsed:hover {
    transition: background-color 0s;
    background-color: rgb(64, 64, 64);
}

#description.collapsed:active {
    transition: background-color 0s;
    background-color: rgb(84, 84, 84);
    border: 1px solid rgb(84, 84, 84);
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
    margin-top: 12px;
}

h1 {
    font-weight: 600;
    font-size: 20px;
    line-height: 28px;
    margin-bottom: 8px;
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
}

#owner {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 0.7rem;
    flex-basis: 100%;
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
    transition: border-color 1s ease;
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

#description p {
    line-height: 20px;
    margin-top: 1px;
    // white-space: pre-wrap;

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

button {
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
    /* gap: .5rem; */
    /* background-color: red; */

}

.download-button {
    width: 114px;

    svg {
        left: 6px;
    }
}

.cinema-button {
    width: 100px;

    svg {
        left: 6px;
    }
}

.download-button::after {
    aspect-ratio: 1;
    padding-left: 5px;
    // padding-left: 1rem;
    // padding-right: 19px;
    content: 'Download';
    max-height: 2rem;
    white-space: nowrap;
}

.cinema-button::after {
    aspect-ratio: 1;
    padding-left: 5px;
    // padding-left: 1rem;
    // padding-right: 19px;
    content: 'Cinema';
    max-height: 2rem;
    white-space: nowrap;
}

@media screen and (max-width: 620px) {

    .download-button {
        width: 36px;

        svg {
            left: 5px;
        }
    }

    .download-button::after {
        aspect-ratio: unset;
        content: '';
        padding: 0;
    }
}

button:active {
    transition: border-color 0s;
    background-color: rgb(84, 84, 84);
    border: 1px solid rgb(84, 84, 84);
}
</style>
<style>
.tag,
a {
    color: #3EA6FF;
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

button:hover {
    background-color: rgba(255, 255, 255, .2);
}

.cinema #description,
.cinema #top-row {
    max-width: calc(960px);
}
</style>
