import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import DefaultLayout from '../layouts/DefaultLayout.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { layout: DefaultLayout }
    },
    {
      path: '/:videoId',
      name: 'video',
      component: () => import('../views/VideoPage.vue'),
      meta: { layout: DefaultLayout }
    },
    {
      path: '/videotest',
      name: 'videotest',
      component: () => import('../views/LocalPlayerTest.vue')
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    },
    {
      path: '/queue',
      name: 'queue',
      component: () => import('../views/QueueView.vue')
    },
    {
      path: '/library',
      name: 'library',
      component: () => import('../views/LibraryView.vue')
    },
    {
      path: '/results',
      name: 'results',
      props: (route) => ({
        searchQuery: route.query.search_query
      }),
      component: () => import('../views/SearchResultView.vue')
    },
    {
      path: '/settings',
      name: 'settings',
      component: () => import('../views/SettingsView.vue')
      
    }
  ]
})

export default router
