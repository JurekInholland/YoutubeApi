<script setup lang="ts">
import { ref, onMounted, type Ref } from 'vue';
import VideoPlayer from '@/components/VideoPlayer.vue';
import VideoMetadata from '@/components/VideoMetadata.vue';
import type { IRelatedVideo, IVideo } from '@/models';
import type { YoutubeVideo } from '@/types';
import SidebarVideo from '@/components/SidebarVideo.vue';

import http from 'http';

const playState = ref(false);
const playBtnPath = ref("M8,21 L12,21 L17,26 L17,10 L12,15 L8,15 L8,21 Z M19,14 L19,22 C20.48,21.32 21.5,19.77 21.5,18 C21.5,16.26 20.48,14.74 19,14 ZM19,11.29 C21.89,12.15 24,14.83 24,18 C24,21.17 21.89,23.85 19,24.71 L19,26.77 C23.01,25.86 26,22.28 26,18 C26,13.72 23.01,10.14 19,9.23 L19,11.29 Z M 7.07,6.87 -1.11,15.33 19.61,36.11 27.80,27.60 z");
const togglePlay = () => {
    playState.value = !playState.value;
    if (playState.value) {
        playBtnPath.value = "M8,21 L12,21 L17,26 L17,10 L12,15 L8,15 L8,21 Z M19,14 L19,22 C20.48,21.32 21.5,19.77 21.5,18 C21.5,16.26 20.48,14.74 19,14 ZM19,11.29 C21.89,12.15 24,14.83 24,18 C24,21.17 21.89,23.85 19,24.71 L19,26.77 C23.01,25.86 26,22.28 26,18 C26,13.72 23.01,10.14 19,9.23 L19,11.29 Z";
    } else {
        playBtnPath.value = "M8,21 L12,21 L17,26 L17,10 L12,15 L8,15 L8,21 Z M19,14 L19,22 C20.48,21.32 21.5,19.77 21.5,18 C21.5,16.26 20.48,14.74 19,14 Z ";
    }
    console.log("togglePlay")
}
const toggle = () => {
    console.log("toggle")
    playState.value = !playState.value;
}

const testvid: YoutubeVideo = {
    id: "dQw4w9WgXcQ",
    title: "Rick Astley - Never Gonna Give You Up (Official Music Video)",
    description: "The official video for “Never Gonna Give You Up” by Rick Astley\nTaken from the album ‘Whenever You Need Somebody’ – deluxe 2CD and digital deluxe out 6th May 2022 Pre-order here – https://RickAstley.lnk.to/WYNS2022ID\n\n“Never Gonna Give You Up” was a global smash on its release in July 1987, topping the charts in 25 countries including Rick’s native UK and the US Billboard Hot 100.  It also won the Brit Award for Best single in 1988. Stock Aitken and Waterman wrote and produced the track which was the lead-off single and lead track from Rick’s debut LP “Whenever You Need Somebody”.  The album was itself a UK number one and would go on to sell over 15 million copies worldwide.\n\nThe legendary video was directed by Simon West – who later went on to make Hollywood blockbusters such as Con Air, Lara Croft – Tomb Raider and The Expendables 2.  The video passed the 1bn YouTube views milestone on 28 July 2021.\n\nSubscribe to the official Rick Astley YouTube channel: https://RickAstley.lnk.to/YTSubID\n\nFollow Rick Astley:\nFacebook: https://RickAstley.lnk.to/FBFollowID \nTwitter: https://RickAstley.lnk.to/TwitterID \nInstagram: https://RickAstley.lnk.to/InstagramID \nWebsite: https://RickAstley.lnk.to/storeID \nTikTok: https://RickAstley.lnk.to/TikTokID\n\nListen to Rick Astley:\nSpotify: https://RickAstley.lnk.to/SpotifyID \nApple Music: https://RickAstley.lnk.to/AppleMusicID \nAmazon Music: https://RickAstley.lnk.to/AmazonMusicID \nDeezer: https://RickAstley.lnk.to/DeezerID \n\nLyrics:\nWe’re no strangers to love\nYou know the rules and so do I\nA full commitment’s what I’m thinking of\nYou wouldn’t get this from any other guy\n\nI just wanna tell you how I’m feeling\nGotta make you understand\n\nNever gonna give you up\nNever gonna let you down\nNever gonna run around and desert you\nNever gonna make you cry\nNever gonna say goodbye\nNever gonna tell a lie and hurt you\n\nWe’ve known each other for so long\nYour heart’s been aching but you’re too shy to say it\nInside we both know what’s been going on\nWe know the game and we’re gonna play it\n\nAnd if you ask me how I’m feeling\nDon’t tell me you’re too blind to see\n\nNever gonna give you up\nNever gonna let you down\nNever gonna run around and desert you\nNever gonna make you cry\nNever gonna say goodbye\nNever gonna tell a lie and hurt you\n\n#RickAstley #NeverGonnaGiveYouUp #WheneverYouNeedSomebody #OfficialMusicVideo",
    uploader: "Rick Astley",
    viewCount: 1000000000,
    likeCount: 10000000,

    // uploaderId: "UCuAXFkgsw1L7xaCfnd5JJOw",
    uploadDate: new Date(),
    thumbnail: "",
    duration: 212,
    dateAdded: new Date(),
    localVideo: null,
    dislikeCount: 100,
    //

    // channel_follower_count: 10000000,
    // comments: [],
}
const vids : Ref<Array<IRelatedVideo>> = ref([])

onMounted(async () => {
    console.log("mounted")
    vids.value = await fetch(`/api/Info/GetRelatedYoutubeVideos?videoId=${testvid.id}`,).then(res => res.json())
    console.log(vids)
})

</script>

<template>
    <div id="container">
        <div id="primary">
            <VideoPlayer color="red" src="./vid.webm" />

            <VideoMetadata :video="testvid" />

        </div>

        <div id="secondary">
            <p>sidebar</p>
            <SidebarVideo v-for="vid in vids" :video="vid" />


        </div>

    </div>
    <!-- <h1>vidtest</h1> -->
    <!-- <PlayPauseButton :isPaused="playState" @click="toggle" /> -->
    <!-- <MorphButton :path="playBtnPath" @click="togglePlay" /> -->
</template>

<style scoped>
:root {
    --ytd-watch-flexy-max-player-width: calc((100vh - (var(--ytd-watch-flexy-masthead-height) + var(--ytd-margin-6x) + var(--ytd-watch-flexy-space-below-player))) * (var(--ytd-watch-flexy-width-ratio) / var(--ytd-watch-flexy-height-ratio)));
}

#container {
    display: flex;
    flex-direction: row;
    height: 100%;
    width: calc(100% - 3rem);
    /* max-width: calc(100% - 3rem); */
    max-width: calc(1280px + 402px + 1.5rem);
    justify-content: center;
    justify-self: center;
    /* flex-grow: 1; */
    min-width: calc(240px * (16 / 9));
    /* flex: 1;
    flex-basis: 0; */
    gap: 1.5rem;
    /* padding: 0 1.5rem; */
    margin-inline: auto;
    /* margin: 0 auto; */
    margin-top: 1.5rem;
    /* flex-wrap: wrap; */
    flex-flow: wrap;
}

#primary {
    flex: 1;
    /* max-width: var(--ytd-watch-flexy-max-player-width); */
    /* max-width: calc((100vh - (56px + 24px + 136px))*(16/9)); */
    max-width: 1280px;
    flex-basis: 640px;
    /* min-width: calc(360px * (16 / 9)); */
    /* flex-basis: 0;*/
    flex-grow: 12.5;
}

#secondary {
    /* flex: 1; */
    /* max-width: 402px; */
    /* width: 402px; */
    min-width: 300px;
    flex-basis: 300px;
    flex-grow: 1;
    background: #333;
    display: flex;
    flex-direction: column;
    gap: 4px;
}


</style>