import { createRouter, createWebHashHistory } from 'vue-router'
import { authGuard } from '@bcwdev/auth0provider-client'

function loadPage(page) {
  return () => import(`./pages/${page}.vue`)
}

const routes = [
  {
    path: '/',
    name: 'Home',
    component: loadPage('HomePage')
  },
  {
    path: '/about',
    name: 'About',
    component: loadPage('AboutPage')
  },
  {
    path: '/account',
    name: 'Account',
    component: loadPage('AccountPage'),
    beforeEnter: authGuard
  },
  {
    path: '/help',
    name: 'Help',
    component: loadPage('HelpPage')
  },
  {
    path: '/character/:characterId',
    name: 'Character',
    component: loadPage('CharacterPage'),
    beforeEnter: authGuard
  },
  {
    path: '/campaign/:campaignId',
    name: 'Campaign',
    component: loadPage('CampaignPage'),
    beforeEnter: authGuard
  },
  {
    path: '/info/:infoId/:infoDetails/:infoSpec?/:infoMore?',
    name: 'Info',
    component: loadPage('InfoPage')
  }
]

export const router = createRouter({
  linkActiveClass: 'router-link-active',
  linkExactActiveClass: 'router-link-exact-active',
  history: createWebHashHistory(),
  routes
})