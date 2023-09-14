<template>
  <nav class="row navbar navbar-expand-lg navbar-dark bg-russian px-3 elevation-5">
    <router-link class="col-6 navbar-brand mx-2 d-flex d-lg-none justify-content-center align-items-center" :to="{ name: 'Home' }">
      <img alt="logo" src="../assets/img/Logo.png" height="45" />
    </router-link>

    <div class="col-6 text-end d-lg-none">
      <button @click="dismissInfo()" class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarText" aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
    </div>

    <div class="col-12 collapse navbar-collapse" id="navbarText">
      <ul class="row navbar-nav flex-row justify-content-between mx-0 w-100">
        <div class="col-12 col-md-6 col-lg-5 d-flex align-items-center">
          <section class="row flex-grow-1 py-2">
            <li class="col-4 d-flex justify-content-center align-items-center">
              <router-link :to="{ name: 'Home' }" class="btn text-success lighten-30 selectable text-uppercase">
                Home
              </router-link>
            </li>

            <li class="col-4 d-flex justify-content-center align-items-center">
              <button type="button" role="button" :class="{ route: route.fullPath.includes('info') }" class="btn text-success lighten-30" data-bs-toggle="collapse" data-bs-target="#infoCollapse" aria-expanded="false" aria-controls="infoCollapse">INFO</button>
            </li>

            <li class="col-4 d-flex justify-content-center align-items-center">
              <router-link :to="{ name: 'Help' }" class="btn text-success lighten-30 selectable text-uppercase">
                Help
              </router-link>
            </li>
          </section>
        </div>

        <li class="col-2 d-none d-lg-flex justify-content-center align-items-center">
          <router-link class="navbar-brand d-flex justify-content-center align-items-center mx-0" :to="{ name: 'Home' }">
            <img alt="logo" src="../assets/img/Logo.png" height="45" />
          </router-link>
        </li>

        <li class="col-6 col-md-4 col-lg-2 d-flex justify-content-center align-items-center">
          <router-link :to="{ name: 'Character', params: { characterId: 'basics' } }" :class="{ route: route.fullPath.includes('character') }" class="btn text-success lighten-30 selectable text-uppercase">
            Build Character
          </router-link>
        </li>

        <li class="col-5 col-md-2 col-lg-2 d-flex justify-content-center align-items-center">
          <router-link :to="{ name: 'Campaign' }" class="btn text-success lighten-30 selectable text-uppercase">
            Campaign
          </router-link>
        </li>

        <div class="col-12 col-lg-1 d-flex justify-content-md-end align-items-center">
          <!-- LOGIN COMPONENT HERE -->
          <Login />
        </div>
      </ul>
    </div>
  </nav>

  <section id="infoCollapse" class="row collapse bg-dark">
    <div v-for="(i, index) in info" :key="index" class="col-12 col-sm-4 col-md-3 py-2">
      <hr class="m-0 py-2">
      <router-link :to="`/info/${i[1].replace('/api/', '').replace(/'/g, '')}/search`" @click="clearInfoHtml(); dismissInfo()" class="text-light fw-bold selectable text-uppercase p-2">
        {{ i[0].replace('-', ' ') }}
      </router-link>
    </div>
  </section>
</template>

<script>
import { computed, onMounted } from 'vue'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import { useRoute } from 'vue-router'
import { Collapse } from 'bootstrap'
import Login from './Login.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()

    onMounted(() => {
      getInfo()
    })

    async function getInfo() {
      try {
        await infosService.getInfo()
      } catch (error) {
        Pop.error(error.message, '[GETTING INFO]')
      }
    }

    return {
      route,
      info: computed(() => Object.entries(AppState.info)),

      clearInfoHtml() {
        AppState.infoHtml = ''
        AppState.infoArr = []
      },

      dismissInfo() {
        if (document.getElementById('infoCollapse').classList.contains('show')) {
          Collapse.getOrCreateInstance('#infoCollapse').hide()
        }
      }
    }
  },
  components: { Login }
}
</script>

<style scoped>
  a:hover {
    text-decoration: none;
  }

  .navbar-brand {
    background-color: #e9ecef;
    height: 60px;
    width: 60px;
    border-bottom: none !important;
    border-radius: 50% !important;
  }

  .nav-link {
    text-transform: uppercase;
  }

  .navbar-nav .router-link-exact-active,
  .collapse .router-link-exact-active,
  .route {
    border-bottom: 2px solid var(--bs-success);
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }

  @media screen and (min-width: 992px) {
    nav {
      height: 76px;
    }
  }
</style>