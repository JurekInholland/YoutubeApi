import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createManager } from '@vue-youtube/core';
import vClickOutside from "click-outside-vue3"

import App from './App.vue'
import router from './router'

import './assets/main.scss'

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(createManager())
app.use(vClickOutside)
app.mount('#app')
