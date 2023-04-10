<script setup lang="ts">
import { ref, watch, markRaw, type Ref } from 'vue';
import { Icon } from '@iconify/vue';
import Logo from './Logo.vue';
import { apiService } from '@/constants';
import { useRouter } from 'vue-router';
import { useYoutubeStore } from '@/stores/youtubeStore';

const router = useRouter();
const store = useYoutubeStore();

const searchFocs = ref(false);
const searchQuery: Ref<string> = ref('');

const oldQuery: Ref<string> = ref('');

// const suggestions: Ref<string[]> = ref([]);

const activeIndex: Ref<number> = ref(-1);

const menuOpen: Ref<boolean> = ref(false);

const activeChange = () => {
    // console.log("E",e)
    console.log("active")
    searchFocs.value = !searchFocs.value;
}

watch(router.currentRoute, (val) => {
    console.log("ROUTE", val)
    searchFocs.value = false;
    menuOpen.value = false;
})
watch(searchFocs, (val) => {
    if (val) {
        activeIndex.value = -1;
    }

})
// watch(searchQuery, async (val) => {
//     console.log("searchQuery", val)
//     const res = await apiService.GetSearchCompletion(val);
//     if (res instanceof Error) {
//         console.log("ERROR", res)
//     }
//     else {
//         suggestions.value = res;
//     }
// });

const clearSearch = () => {
    store.searchQuery = '';
}

async function onChange() {
    console.log("onchange")
    oldQuery.value = store.searchQuery;
    await store.setSearchQuery(searchQuery.value);
    // console.log("CHANGE", e)
    // store.searchQuery = (e.target as HTMLInputElement).value;
}

const onKeyPress = async (e: KeyboardEvent) => {
    console.log("KEYPRESS", e.key)
    if (e.key === "ArrowDown") {
        activeIndex.value = (activeIndex.value + 1) % (store.searchSuggestions.length + 1);
        store.searchQuery = store.searchSuggestions[activeIndex.value] ?? oldQuery.value;


    }
    else if (e.key === "ArrowUp") {
        activeIndex.value = (activeIndex.value - 1 + store.searchSuggestions.length + 1) % (store.searchSuggestions.length + 1);        // activeIndex.value = (activeIndex.value - 1) % (store.searchSuggestions.length + 1);
        store.searchQuery = store.searchSuggestions[activeIndex.value] ?? oldQuery.value;

    }
    else if (e.key === "Enter") {
        if (activeIndex.value !== -1) {
            store.searchQuery = store.searchSuggestions[activeIndex.value];
            store.searchSuggestions = [];
        }
        else {
            store.searchQuery = oldQuery.value;
        }
        if (store.searchQuery === "") {
            return;
        }
        onSearch();
    }


}
const onResultMouseover = (idx: number) => {
    activeIndex.value = idx;
}

const onMouseLeave = () => {
    console.log("mouse leave")
    activeIndex.value = -1;
}
const onSearch = () => {
    console.log("SEARCHING FOR ", store.searchQuery)
    searchFocs.value = false;

    if (store.searchQuery === "") {
        return;
    }
    if (router.currentRoute.value.name !== "results") {
        router.push({ name: "results", query: { search_query: store.searchQuery } })
    }
    else {
        store.fetchSearchResults(store.searchQuery);
    }
    // store.fetchSearchResults(store.searchQuery)
}

const onSearchSuggestionClick = (e: Event) => {
    console.log("SUGGESTION CLICKED")
    const suggestion = (e.target as HTMLDivElement).innerText;
    store.searchQuery = suggestion;
    store.searchSuggestions = [];
    onSearch();
}

const onFocusOut = () => {
    setTimeout(() => {
        searchFocs.value = false;
    }, 150);
}
</script>

<template>
    <div class="ytd-mmasthead">
        <div class="left">
            <button class="menu-button">
                <Icon class="menu" icon="mdi-light:menu" @click="menuOpen = !menuOpen" />
            </button>
            <router-link to="/">
                <Logo class="logo" />
            </router-link>
            <span id="country-code">
                .juri.lol
            </span>
        </div>

        <div class="center">
            <div class="search" :class="searchFocs ? 'active' : ''">
                <div class="ytd-searchbox">
                    <Icon v-if="searchFocs" style="font-size: 1.5rem;" class="search-icon" icon="system-uicons:search" />
                    <input @input="onChange" @focusin="searchFocs = true" @focusout="onFocusOut" v-model="searchQuery"
                        placeholder="Search" type="text" @keydown="onKeyPress">

                    <button class="close" v-if="store.searchQuery !== ''" @click="clearSearch">
                        <Icon style="font-size: 1.5rem;" icon="clarity:close-line" />

                    </button>
                </div>
                <div class="results"
                    v-if="searchFocs && store.searchSuggestions.length > 0 && store.searchQuery.length > 0">
                    <ul @mouseleave="onMouseLeave">
                        <li v-for="(suggestion, index) in store.searchSuggestions" key="index"
                            :class="{ selected: index === activeIndex }" @mouseover="onResultMouseover(index)"
                            @click="onSearchSuggestionClick">
                            <div>
                                <Icon style="font-size: 1rem;" icon="simple-line-icons:magnifier" />
                                <p>{{ suggestion }}</p>

                            </div>
                        </li>
                    </ul>
                </div>
                <button class="search-button" @click="onSearch">
                    <Icon style="font-size: 1.1rem;" icon="simple-line-icons:magnifier" />
                </button>

                <button class="mic-button">
                    <Icon style="font-size: 1.5rem;" icon="ic:baseline-mic"></Icon>
                </button>
            </div>

        </div>
        <div class="right">

            <button class="queue" @click="router.push('queue')">
                <Icon style="font-size: 1.5rem;" icon="carbon:query-queue" />
            </button>

            <button>
                <Icon style="font-size: 1.5rem;" icon="mdi:bell" />
            </button>

            <button>
                <Icon style="font-size: 1.5rem;" icon="mdi:account" />
            </button>

        </div>
    </div>


    <div class="side-nav" :class="menuOpen ? 'open' : ''">
        <div class="left">
            <button class="menu-button" @click="menuOpen = false">
                <Icon style="font-size: 1.5rem;" icon="mdi-light:menu" />
            </button>
            <router-link to="/">
                <Logo class="logo" />
            </router-link>
            <span id="country-code">
                .juri.lol
            </span>
        </div>
    </div>
</template>


<style scoped lang="scss">
.nav-bg {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: black;
    z-index: 1000;
    opacity: .5;
    transition: opacity .2s ease;
    pointer-events: none;
}

.side-nav {
    position: absolute;
    width: 240px;
    min-height: 100%;
    left: 0;
    top: 0;
    background-color: black;
    z-index: 1001;
    transform: translateX(-100%);
    transition: transform .2s ease;
    padding-left: 16px;

    // opacity: .5;

    button {
        svg {
            width: 28px;
            height: 28px;
        }
    }

    button:hover {
        background-color: transparent;
    }

    button:active {
        background-color: rgb(84, 84, 84);
    }
}

.open {
    transform: translateX(0);
}

.selected {
    background-color: rgb(238, 238, 238);
}

.close {
    position: absolute;
    padding: .5rem;
    background-color: transparent;
    transition: background-color .2s ease;
    border-radius: 50%;
    right: -3px;
    z-index: 10;
}

.close:hover {
    background-color: rgba(255, 255, 255, 0.125);
}

.results {
    // display: none;
    position: absolute;
    top: 2.75rem;
    left: 0;
    // height: 400px;
    width: calc(100% - 3rem);
    background-color: white;
    z-index: 2000;
    border-radius: 12px;
    box-shadow: 0px 5px 12px 0px rgba(0, 0, 0, 0.5);
    min-width: 340px;
    overflow: hidden;

    ul {
        color: black;
        margin: 1rem 0;

        svg {
            color: black;
            flex-shrink: 0;
        }

        li {
            white-space: nowrap;

            div {
                gap: 1.25rem;
                line-height: 1.5rem;
                display: flex;
                justify-content: flex-start;
                align-items: center;
                padding: .25rem 1.25rem;
                cursor: default;
                // background-color: red;
            }
        }


        // li:hover {
        //     background-color: rgb(238, 238, 238);
        // }
    }
}


svg {
    color: white;
}

/* .menu-button {
} */
.search-button {
    border: 1px solid var(--ytd-searchbox-legacy-border-color);
    border-left: none;
    border-radius: 0 40px 40px 0;
    flex-grow: 0;
    width: 64px;
    background-color: rgb(32, 32, 32);
    display: flex;
    align-items: center;
    justify-content: center;

}

button {
    fill: white;
    color: white;
    /* height: 65px; */
    min-width: 28px;
    min-height: 28px;
    flex-shrink: 0;
    padding: 8px;
}

// button>svg {
//     height: 100%;
//     width: 100%;
// }

.mic-button {
    border-radius: 50%;
    background-color: rgb(18, 20, 20);
    margin-left: .5rem;
}

.menu-button {
    margin: 0;
    border-radius: 50%;
    background-color: transparent;
    padding: .25rem;
    flex-grow: 0;
    aspect-ratio: 1;
    margin-top: .5rem;
    width: 40px;
    height: 40px;
    display: flex;
    justify-content: center;
    align-items: center;
    box-sizing: border-box;
    border: 1px solid transparent;
}

.menu-button:hover {
    transition: all .2s ease;
    background-color: rgba(255, 255, 255, 0.125);
}

.menu-button:active {
    background-color: rgb(84, 84, 84);
    border: 1px solid rgb(84, 84, 84);
}

.menu {
    overflow: hidden;
    color: white;
    min-height: 28px;
    width: 28px;

}

input {
    appearance: none;
    border: none;
    font-family: Roboto, Noto, sans-serif;
    font-size: 16px;
    font-weight: 400;
    line-height: 24px;
    text-align: inherit;
    width: 100%;
    background-color: transparent;
    color: white;
    border-color: initial;
    box-shadow: none;
    border: none;
    outline: none;
}

.active {
    border-color: blue;
}

.search-icon {
    margin-right: .5rem;
    flex-shrink: 0;
}

/* .search-icon {
    display: none;
}
.ytd-searchbox.active>.search-icon {
display: block;
} */

.yt-icon {
    fill: white;
    width: 24px;
    height: 24px;
    flex-shrink: 0;
    flex-grow: 0;
}

.ytd-mmasthead {
    background-color: #0b0c0d;
    color: #fff;
    height: 56px;
    padding: 0 16px;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    // min-width: 500px;
    position: sticky;
    top: 0;
    z-index: 1000;
}

.left {
    display: flex;
    /* align-items: center; */
    justify-content: center;
    flex-shrink: 0;
    flex-grow: 0;
    // margin-left: 22px;
    margin-right: 1rem;
    font-size: 10px;
    gap: .65rem;
    width: 146px;
}


.center {
    align-items: center;
    flex: 0 1 720px;
    min-width: 0px;
    padding-left: 1.5rem;
    padding-right: 1rem;

}

.right {
    display: flex;
    // gap: 1rem;
    margin-left: 1rem;
    min-width: 185px;
    justify-content: flex-end;
    margin-right: .5rem;

    button,
    a {
        cursor: pointer;
        background-color: transparent;
        border-radius: 50%;
        aspect-ratio: 1;
        transition: all .2s ease;
        margin: 0;
        padding: .65rem;
    }

    button:hover {
        background-color: rgba(255, 255, 255, .125);
    }
}

.ytd-searchbox {
    position: relative;
    background-color: #0e0f0f;
    box-shadow: inset 0 1px 2px var(-vt-c-white-mute);
    color: var(-vt-c-white-mute);
    border-radius: 40px 0 0 40px;
    margin-left: 32px;
    padding: 0px 4px 0px 16px;
    border: 1px solid var(--ytd-searchbox-legacy-border-color);
    height: 100%;
    display: flex;
    flex-grow: 1;
    align-items: center;
    overflow: hidden;
}

.search {
    height: 40px;
    display: flex;
    /* margin-left: 32px; */
}

.search.active>.ytd-searchbox {
    margin-left: 0;
    border: 1px solid var(--input-focus-color);
}

.logo {
    max-height: 56px;
    flex-grow: 0;
    padding: 18px 0px 18px 6px;
}

#country-code {
    top: 12px;
    left: 129px;
    position: absolute;
    color: rgba(255, 255, 255, .75);
}

input:focus-within+.results {
    display: block;
}

// input:active+.results {
//     display: block;
// }
</style>

<style></style>