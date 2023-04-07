<script setup lang="ts">
import SearchResult from '@/components/SearchResult.vue';
import Spinner from '@/components/Spinner.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';


const store = useYoutubeStore();
const route = useRoute();

const searchQuery = route.query.search_query as string;

watch(() => route.query.search_query, async (newVal, oldVal) => {
    console.log("watch", newVal, oldVal)
    store.searchQuery = newVal as string;
    store.clearSearchResults();
    if (newVal !== oldVal && newVal !== "") {
        await store.fetchSearchResults(newVal as string)
    }
});
onMounted(async () => {
    document.title = searchQuery + " - YouTube"
    store.searchQuery = searchQuery;

    if (store.searchResults[searchQuery]) {
        return;
    }
    await store.fetchSearchResults(searchQuery)

})

</script>

<template >
    <section class="search-results">
        <div class="result-list"
            v-if="store.searchResults[store.searchQuery] && store.searchResults[store.searchQuery].length > 0">
            <SearchResult v-for="result in store.searchResults[store.searchQuery]" :video="result" />
        </div>
        <div v-auto-animate v-else class="loading">
            <Spinner />
        </div>
    </section>
</template>

<style scoped lang="scss">
// .active {
//     background-color: red;
// }

.search-results {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    gap: 1rem;
    padding: 1rem;
    // max-width: 2256px;
    max-width: 1096px;
    margin: 0 auto;

    .result-list {
        display: flex;
        flex-direction: column;
        gap: 1rem;
        width: 100%;
    }
}
</style>
