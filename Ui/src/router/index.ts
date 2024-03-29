import DefaultLayout from '../layouts/DefaultLayout.vue'
import HomeView from '../views/HomeView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(),

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
      component: () => import('../views/OrmVideoPage.vue'),
      meta: { layout: DefaultLayout }
    },
    {
      path: '/watch',
      name: 'watch',
      component: () => import('../views/OrmVideoPage.vue')
    },
    {
      path: '/shorts/:videoId',
      name: 'shorts',
      component: () => import('../views/OrmVideoPage.vue')
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
    },

    {
      path: '/channel/:channelId',
      name: 'channel-id',
      component: () => import('../views/ChannelView.vue')
    },

    // Channel routes
    {
      path: '/@:username',
      name: 'channel',
      component: () => import('../views/OrmChannel.vue')
    },
    {
      path: '/c/@:username',
      name: 'channel-c',
      component: () => import('../views/ChannelView.vue')
    },

    {
      path: '/playlist',
      name: 'playlist',
      props: (route) => ({
        list: route.query.list
      }),
      component: () => import('../views/PlaylistView.vue')
    },
    {
      path: '/new',
      name: 'new',
      component: () => import('../views/OrmVideoPage.vue')
    },

    {
      path: '/tags',
      name: 'tags',
      component: () => import('../views/TagsView.vue')
    },
    {
      path: '/orm',
      name: 'debugorm',
      component: () => import('../views/OrmTests.vue')
    },

    {
      path: '/ormvideo',
      name: 'debugormvideo',
      component: () => import('../views/OrmVideoPage.vue')
    },
    {
      path: '/ormvideo/:videoId',
      name: 'debugormvideo',
      component: () => import('../views/OrmVideoPage.vue')
    },
    {
      path: '/ormchannel/:channelId',
      name: 'debugormchannel',
      component: () => import('../views/OrmChannel.vue')
    },
    {
      path: '/debug',
      name: 'debugormplaylist',
      component: () => import('../views/DebugView.vue')
    }
  ],
  scrollBehavior() {
    window.scrollTo(0, 0)
  }
})

router.beforeEach((to, from, next) => {
  console.log("ROUTE TO:", to)
  if (to.name === 'watch' && to.query.v?.length !== 11) {
    console.log('INVALID VIDEO ID')
  }
  next()
})

export default router
