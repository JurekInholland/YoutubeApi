<script setup lang='ts'>
import YoutubePlayer from '@/components/YoutubePlayer.vue';
import VideoMetadata from '@/components/VideoMetadata.vue';
import SidebarVideo from '@/components/SidebarVideo.vue';
import Spinner from '@/components/Spinner.vue';
import type { IRelatedVideo } from '@/models';
import { computed, onMounted, ref, type Ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { apiService } from '@/constants';
import type { YoutubeVideo } from '@/types';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { formatDescription, formatTitle } from '@/utils';

const route = useRoute();
const store = useYoutubeStore();

const relatedVideos: Ref<Array<YoutubeVideo>> = ref([]);

const cinemaMode = ref(true);

const aspectRatio = computed(() => {
    if (store.currentVideo == undefined) return 16 / 9;
    // if (store.currentVideo.height > store.currentVideo.width) return 1;
    return Number(store.currentVideo.width) / Number(store.currentVideo.height);
});

watch(() => store.currentVideo, async (newId, oldId) => {
    console.log('trigger watch:');
    if (store.currentVideo == undefined) return;
    if (newId == oldId) {
        console.log('ids are the same');
        return;
    }
    console.log('processing');
    relatedVideos.value = [];

    let tempList: YoutubeVideo[] = [];

    const relatedIds = [...store.currentVideo.relatedVideos];

    for (const id of relatedIds) {
        const storedVid = store.getVideoById(id);
        if (storedVid != undefined) {
            relatedIds.splice(relatedIds.indexOf(id), 1);
            relatedVideos.value.push(storedVid);
            tempList.push(storedVid);

        }
    }

    console.log('current video changed', newId?.id, oldId?.id);
    const related = await apiService.getRelatedVideos(relatedIds);
    for (const vid of related) {
        tempList.push(vid);
        store.videos.push(vid);
    }
    relatedVideos.value = tempList.sort((a, b) => {
        const indexA = relatedIds.indexOf(a.id);
        const indexB = relatedIds.indexOf(b.id);
        return indexA - indexB;
    });
    // store.videos.push(...relatedVideos.value)
});

// todo: comapre
// watch(() => store.currentVideo, async () => {
//     if (store.currentVideo != undefined) {

//         if (relatedVideos.value.length > 0) {
//             return;
//         }
//         for (const vidId of store.currentVideo.relatedVideos) {
//             const storedVid = store.getVideoById(vidId)
//             if (storedVid) {

//                 relatedVideos.value.push(storedVid)
//                 return
//             }

//             const vid = await apiService.GetVideoInfo(vidId)
//             if (vid instanceof Error) {
//                 console.log("ERROR", vid)
//             }
//             else {
//                 store.videos.push(vid)
//                 relatedVideos.value.push(vid)
//             }
//         }
//     }
// }, { immediate: true })

const parsedId = computed(() => {
    const split = route.path.substring(1).split('?')[0];
    if (split.length === 11) return split;
    return route.query.v ? route.query.v as string : null;
});

const startTime = computed(() => {
    return route.query.t ? parseInt(route.query.t as string) : 0;
});

// const fetchVideo = async (id: string): Promise<YoutubeVideo | undefined> => {
//     const vid = await apiService.GetVideoInfo(id);
//     if (vid instanceof Error) {
//         console.log("ERROR", vid)
//         return undefined;
//     }
//     else {
//         // video.value = vid;
//         return vid;
//     }
// }

// const video: Ref<YoutubeVideo | undefined> = ref();


watch(() => route.path, async () => {
    console.log('watch parsedId', parsedId.value);
    store.fetchCurrentVideo(parsedId.value!);
}, { immediate: true });

watch(() => store.currentVideo, async () => {
    document.title = formatTitle(store.currentVideo!.title);
}, { immediate: true });

onMounted(async () => {
    document.title = 'Video';
    if (store.currentVideo === undefined) {
        console.log('VIDEO IS NULL');

        // store.currentVideo = await fetch(`/api/Info/GetVideoInfo?videoId=${parsedId.value}`,).then(res => res.json())
    } else {
        console.log('VIDEO IS NOT NULL', store.currentVideo);
        document.title = formatTitle(store.currentVideo!.title);
    }
    // vids.value = await fetch(`/api/Info/GetRelatedYoutubeVideos?videoId=${video.id}`,).then(res => res.json())
});

const toggleCinema = () => {
    cinemaMode.value = !cinemaMode.value;
};
watch(cinemaMode, (newVal) => {
    console.log('cine moide', newVal);
});
</script>

<template>
    <div id='container' :class="cinemaMode ? 'cinema' : 'default'">
        <div id='primary'>
            <!-- <YoutubePlayer class='player' v-if='parsedId' :video-id='parsedId' :start-time='startTime' -->
                           <!-- :aspect-ratio='aspectRatio' /> -->
            <!-- <VideoPlayer src="./vid.webm" color="green" /> -->
            <!-- <VideoMetadata v-if='store.currentVideo' :video='store.currentVideo' v-model='cinemaMode' /> -->
            <!-- <div v-auto-animate v-else class='loading'>
                <Spinner />
            </div>
            <button @click='toggleCinema'>CINEMA</button> -->

        </div>

        <div v-if='relatedVideos.length > 0' v-auto-animate id='secondary'>
            <SidebarVideo v-for='vid in relatedVideos' :video='vid' :key='vid.id' />
        </div>
        <div v-auto-animate v-else class='loading'>
            <Spinner />
        </div>
    </div>
</template>

<style scoped>
.default .player {
    /* margin: 0 auto; */
    /* max-height: calc(100vh - 16rem); */
    /* max-width: calc(100% - 3rem); */
}

.cinema .player {
    margin: 0;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: black;
    /* margin: 0 auto; */
}


#container.default {
    width: calc(100% - 3rem);
    max-width: calc(1280px + 402px + 1.5rem);
}

#container {
    display: flex;
    flex-direction: row;
    height: 100%;
    justify-content: center;
    /* justify-self: center; */
    /* min-width: calc(240px * (16 / 9)); */
    gap: 1rem;
    margin-inline: auto;
    flex-wrap: wrap;
}

#container button {
    color: white;
}

#primary {
    flex-grow: 1;
    flex-shrink: 1;

}

.cinema {
    flex-basis: 100%;
}

.default #primary {
    flex: 1;
    max-width: 1280px;
    flex-basis: 640px;
}


/* .cinema #secondary {
    flex-basis: 100%;
    margin: 1rem;
} */

.cinema .metadata {
    margin: 1rem;
}

.cinema {
    /* max-width: 100%; */
    flex-basis: 100%;
    /* margin-left: -1.5rem;
    margin-right: -1.5rem; */
    max-width: calc(100% - 1rem);
    max-height: unset;
}

#secondary {
    /* min-width: 300px; */
    min-height: calc(100vh - 3rem);
    flex-basis: 250px;
    flex-grow: 1;
    /* background: #333; */
    display: flex;
    flex-direction: column;
    gap: 6px;
}

.default #secondary {
    max-width: 402px;
}

@media screen and (max-width: 1280px) {
    .default #secondary {
        max-width: unset;
    }
}


</style>
