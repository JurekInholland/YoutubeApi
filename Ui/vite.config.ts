import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    // origin: 'http://localhost:5443',
    proxy: {
      '/api': {
        target: 'http://localhost:5003',
        changeOrigin: false,
        secure: false,
        ws: true
      }
    }
  }
})
