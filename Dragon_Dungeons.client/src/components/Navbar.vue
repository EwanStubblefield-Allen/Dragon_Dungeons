<template>
  <nav class="navbar navbar-expand-lg navbar-dark bg-dark px-3">
    <router-link class="navbar-brand d-flex" :to="{ name: 'Home' }">
      <div class="d-flex flex-column align-items-center">
        <img alt="logo" src="../assets/img/cw-logo.png" height="45" />
      </div>
    </router-link>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarText" aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarText">
      <ul class="navbar-nav me-auto">
        <li>
          <router-link :to="{ name: 'About' }" class="btn text-success lighten-30 selectable text-uppercase">
            About
          </router-link>
        </li>
        <li class="dropdown">
          <button type="button" role="button" :class="{ route: route.fullPath.includes('info') }" class="btn text-success lighten-30 dropdown-toggle mx-2" data-bs-toggle="dropdown" aria-expanded="false">INFO</button>

          <div class="dropdown-menu p-0 overflow-auto" aria-labelledby="infoDropdown">
            <div v-for="(i, index) in info" :key="index">
              <hr class="m-0 px-2">
              <router-link :to="`/info/${i[1].replace('/api/', '').replace(/'/g, '')}/search`" @click="clearInfoHtml()" class="dropdown-item fw-bold selectable text-uppercase">
                {{ i[0].replace('-', ' ') }}
              </router-link>
            </div>
          </div>
        </li>
      </ul>
      <!-- LOGIN COMPONENT HERE -->
      <Login />
    </div>
  </nav>
</template>

<script>
import { computed, onMounted } from 'vue'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import { useRoute } from 'vue-router'
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

  .nav-link {
    text-transform: uppercase;
  }

  .navbar-nav .router-link-exact-active,
  .route {
    border-bottom: 2px solid var(--bs-success);
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }

  .overflow-auto {
    height: 25vh;
  }

  @media screen and (min-width: 768px) {
    nav {
      height: 64px;
    }
  }
</style>