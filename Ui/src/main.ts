import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createManager } from '@vue-youtube/core'
import vClickOutside from 'click-outside-vue3'

import App from './App.vue'
import router from './router'

import './assets/main.scss'
import { useYoutubeStore } from './stores/youtubeStore'

const app = createApp(App)

app.use(createPinia())

const youtubeStore = useYoutubeStore()
app.provide('youtubeStore', youtubeStore)

app.use(router)
app.use(createManager())
app.use(vClickOutside)
app.mount('#app')
