import { createPinia, defineStore } from "pinia";

export const useIsolatedRepo = useRepo(YoutubeVideo, createPinia())