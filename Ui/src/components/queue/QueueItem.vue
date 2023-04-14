<script setup lang="ts">
import { DownloadStatus, type QueuedDownload } from '@/types';
import { Icon } from '@iconify/vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { formatDateAgo, formatTitle } from '@/utils';
import ProgressBar from '../ProgressBar.vue';
const store = useYoutubeStore();

const props = defineProps<{
    item: QueuedDownload
}>()

const removeFromQueue = async () => {
    await store.dequeue(props.item);
}
const color = store.color;
</script>

<template>
    <div class="queued-item"
        :class="[item.status === DownloadStatus.Finished ? 'finished' : '', item.progress ? 'active' : '']">
        <router-link class="thumbnail-link" :to="`/watch?v=${item.video.id}`">

            <img :src="item.video.youtubeThumbnailUrl" alt="">
        </router-link>
        <div class="info">
            <h3>
                <router-link v-html="formatTitle(item.video.title)" :to="`/watch?v=${item.video.id}`">
                </router-link>
            </h3>
            <p>Queued {{ formatDateAgo(item.queuedAt) }}</p>



        <div class="author">
            <router-link v-if="item.video.youtubeChannel"
                :to="{ name: 'channel', params: { username: item.video.youtubeChannel?.title } }" id="avatar">

                    <img :src="`api/Thumbnail/channel?channelId=${item.video.youtubeChannel?.id}`" alt="">
                    <p>{{ item.video.youtubeChannel?.title }}</p>
                </router-link>
            </div>


            <p>{{ DownloadStatus[item.status] }}

                <Icon icon="quill:checkmark" v-if="item.status === DownloadStatus.Finished" />
            </p>
            <!-- <p>{{ item.id }}</p> -->
            <ProgressBar v-if="item.progress" :value="item.progress!.progress" :text="`${item.progress!.eta}`" />
        </div>
        <!-- <div v-if="item.progress" class="progress">
                                                                                            <div>{{ item.progress.status }}</div>
                                                                                            <div>{{ item.progress.progress }}%</div>
                                                                                            <div>{{ item.progress.speed }}</div>
                                                                                        </div> -->
        <button @click="removeFromQueue">
            <Icon icon="ic:round-delete" />
        </button>
    </div>
</template>

<style scoped lang="scss">
.finished {
    opacity: .5;
}



button {
    display: block;
    font-size: 2rem;
    color: v-bind(color);
}

.queued-item.active {
    border: 2px solid v-bind(color);

}

.queued-item {
    box-sizing: border-box;
    border: 2px solid rgba(255, 255, 255, .5);
    display: flex;
    // flex-direction: column;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: 100%;
    background-color: #202020;
    border-radius: 10px;
    gap: 1rem;
    transition: all .5s ease;
    padding: .75rem;
    overflow: hidden;


    .progress {
        flex-basis: 30%;
    }

    .info {
        display: flex;
        flex-direction: column;
        gap: .5rem;
        flex-grow: 1;
        flex-wrap: wrap;
    }



    h3 {
        font-weight: bold;
        color: var(--text-color);

    }

    // &:hover {
    //     transform: scale(1.05);
    //     box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.8);
    // }

    p {
        margin: 0;
        padding: 0;
        color: rgba(255, 255, 255, 0.5)
    }



    .thumbnail-link {
        min-width: 140px;
    }

    .thumbnail-link img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        border-radius: 10px;
        aspect-ratio: 16/9;
        max-width: 180px;
        flex-shrink: 0;
    }

    .info a {
        display: inline-flex;
        line-height: 22px;
        flex-wrap: wrap;
    }

    .author {
        display: inline-flex;

        a {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: .5rem;
        }
    }

}
</style>

<style>
.tag {
    margin-right: .5rem;
    color: var(--link-color);
    z-index: 100;
}


h3>a {
    margin-right: .5rem;
}
</style>