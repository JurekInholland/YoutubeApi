import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createPersistedState } from 'pinia-plugin-persistedstate'

import { createManager } from '@vue-youtube/core'
// import vClickOutside from 'click-outside-vue3'
import vueClickOutsideElement from 'vue-click-outside-element'
import { VueSignalR } from '@quangdao/vue-signalr'
import { autoAnimatePlugin } from '@formkit/auto-animate/vue'

import App from './App.vue'
import router from './router'

import './assets/main.scss'
import { useYoutubeStore } from './stores/youtubeStore'
import { createORM } from 'pinia-orm'

const app = createApp(App)
app.use(vueClickOutsideElement)
const pinia = createPinia()
pinia.use(
  createPersistedState({
    auto: true
  })
)
pinia.use(createORM())
app.use(pinia)

const youtubeStore = useYoutubeStore()
app.provide('youtubeStore', youtubeStore)
app.use(VueSignalR, { url: '/api/signalr' })
app.use(autoAnimatePlugin)
app.use(router)
app.use(createManager())
// app.use(vClickOutside)
app.mount('#app')
