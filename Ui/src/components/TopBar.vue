<script setup lang="ts">
import { ref, watch, markRaw, type Ref } from 'vue';
import { Icon } from '@iconify/vue';
import Logo from './Logo.vue';
import { apiService } from '@/constants';
import { useRouter } from 'vue-router';

const router = useRouter();

const searchFocs = ref(false);
const searchQuery: Ref<string> = ref('');

const oldQuery: Ref<string> = ref('');

const suggestions: Ref<string[]> = ref([]);

const activeIndex: Ref<number> = ref(-1);

const activeChange = () => {
    // console.log("E",e)
    console.log("active")
    searchFocs.value = !searchFocs.value;
}

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
    searchQuery.value = '';
}

const onChange = async () => {

    const res = await apiService.GetSearchCompletion(searchQuery.value);
    if (res instanceof Error) {
        console.log("ERROR", res)
    }
    else {
        suggestions.value = res;
    }
    oldQuery.value = searchQuery.value;
    // console.log("CHANGE", e)
    // searchQuery.value = (e.target as HTMLInputElement).value;
}

const onKeyPress = (e: KeyboardEvent) => {
    console.log("KEYPRESS", e.key)

    if (e.key === "ArrowDown") {
        activeIndex.value = (activeIndex.value + 1) % (suggestions.value.length + 1);
        searchQuery.value = suggestions.value[activeIndex.value] ?? oldQuery.value;


    }
    else if (e.key === "ArrowUp") {
        activeIndex.value = (activeIndex.value - 1 + suggestions.value.length + 1) % (suggestions.value.length + 1);        // activeIndex.value = (activeIndex.value - 1) % (suggestions.value.length + 1);
        searchQuery.value = suggestions.value[activeIndex.value] ?? oldQuery.value;

    }
    else if (e.key === "Enter") {
        if (activeIndex.value !== -1) {
            searchQuery.value = suggestions.value[activeIndex.value];
            suggestions.value = [];
        }
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
    console.log("SEARCHING FOR ", searchQuery.value)
    if (searchQuery.value === "") {
        return;
    }
    router.push({ name: "results", query: { search_query: searchQuery.value } })
    // store.fetchSearchResults(searchQuery.value)
}

const onSearchSuggestionClick = (e: Event) => {
    console.log("SUGGESTION CLICKED")
    const suggestion = (e.target as HTMLDivElement).innerText;
    searchQuery.value = suggestion;
    suggestions.value = [];
    onSearch();
}

const onFocusOut = () => {
    setTimeout(() => {
        searchFocs.value = false;
    }, 100);
}
</script>

<template>
    <div class="ytd-mmasthead">
        <div class="left">
            <!-- <div class="yt-icon">
                                                                                                                                                                                                                                                                                                                                <svg viewBox="0 0 24 24" preserveAspectRatio="xMidYMid meet" focusable="false" class="style-scope yt-icon"
                                                                                                                                                                                                                                                                                                                                    style="pointer-events: none; display: block; width: 100%; height: 100%;">
                                                                                                                                                                                                                                                                                                                                        <g class="style-scope yt-icon">
                                                                                                                                                                                                                                                                                                                                        <path d="M21,6H3V5h18V6z M21,11H3v1h18V11z M21,17H3v1h18V17z" class="style-scope yt-icon"></path>
                                                                                                                                                                                                                                                                                                                                    </g>
                                                                                                                                                                                                                                                                                                                                </svg>
                                                                                                                                                                                                                                                                                                                            </div> -->
            <button class="menu-button">
                <Icon class="menu" icon="mdi-light:menu" />
            </button>
            <Logo class="logo" />
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

                    <button class="close" v-if="searchQuery !== ''" @click="clearSearch">
                        <Icon style="font-size: 1.5rem;" icon="clarity:close-line" />

                    </button>
                    <div class="results" v-if="searchFocs && suggestions.length > 0 && searchQuery.length > 0">
                        <ul @mouseleave="onMouseLeave">

                            <li v-for="(suggestion, index) in suggestions" key="index"
                                :class="{ selected: index === activeIndex }" @mouseover="onResultMouseover(index)"
                                @click="onSearchSuggestionClick">
                                <Icon style="font-size: 1rem;" icon="simple-line-icons:magnifier" />
                                <p>{{ suggestion }}</p>
                            </li>

                            <!-- <li>
                                                                                                                                                                                                                                            <Icon style="font-size: 1rem;" icon="simple-line-icons:magnifier" />
                                                                                                                                                                                                                                            <p>this is a test</p>
                                                                                                                                                                                                                                        </li>
                                                                                                                                                                                                                                        <li>
                                                                                                                                                                                                                                            <Icon style="font-size: 1rem;" icon="simple-line-icons:magnifier" />
                                                                                                                                                                                                                                            <p>this is a test</p>
                                                                                                                                                                                                                                        </li> -->
                        </ul>
                    </div>
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
            <Icon style="font-size: 1.5rem;" icon="mdi:bell" />
            <Icon style="font-size: 1.5rem;" icon="mdi:account" />
        </div>
    </div>
</template>


<style scoped lang="scss">
.selected {
    background-color: rgb(238, 238, 238);
}

.close {
    position: absolute;
    padding: .5rem;
    background-color: transparent;
    transition: background-color .2s ease;
    border-radius: 50%;
    right: -5px;
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
    width: 100%;
    background-color: white;
    z-index: 200;
    border-radius: 12px;
    box-shadow: 0px 5px 12px 0px rgba(0, 0, 0, 0.5);


    ul {
        color: black;
        margin: 1rem 0;

        svg {
            color: black;
        }

        li {
            gap: 1.25rem;
            line-height: 1.5rem;
            display: flex;
            justify-content: flex-start;
            align-items: center;
            padding: .25rem 1.25rem;
            cursor: default;

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
    min-width: 500px;
}

.left {
    display: flex;
    /* align-items: center; */
    justify-content: center;
    flex-shrink: 0;
    flex-grow: 0;
    margin-left: 22px;
    margin-right: 1rem;
    font-size: 10px;
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
    gap: 1rem;
    margin-left: 1rem;
    min-width: 185px;
    justify-content: flex-end;
    margin-right: .5rem;
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
}

.search {
    height: 40px;
    display: flex;
    /* margin-left: 32px; */
}

.search.active>.ytd-searchbox {
    margin-left: 0;
}

.search.active>.ytd-searchbox {
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