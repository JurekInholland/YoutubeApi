<script setup lang="ts">
import SearchResult from '@/components/SearchResult.vue';
import { useYoutubeStore } from '@/stores/youtubeStore';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';


const store = useYoutubeStore();
const route = useRoute();

const searchQuery = route.query.search_query as string;

watch(() => route.query.search_query, async (newVal, oldVal) => {
    console.log("watch", newVal, oldVal)
    store.clearSearchResults();
    if (newVal !== oldVal && newVal !== "") {
        await store.fetchSearchResults(newVal as string)
    }
});
onMounted(async () => {
    console.log("SEARCHING FOR ", searchQuery)
    await store.fetchSearchResults(searchQuery)

})

</script>

<template >
    <section class="search-results" >
        <div class="result-list" >
            <h1>{{ route.query.search_query }}</h1>
            <!-- <p v-for="result in store.searchResults">{{ result.title }}</p> -->
            <SearchResult 
                v-for="result in store.searchResults" :video="result" />

        </div>
    </section>
</template>

<style scoped lang="scss">
.active {
    background-color: red;
}
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
