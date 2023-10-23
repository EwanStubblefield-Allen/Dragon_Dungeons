<template>
  <section class="row">
    <div class="col-12 col-md-3 col-lg-2 overflow-auto infoBar">
      <section class="row p-2">
        <div class="col-12 p-0">
          <p class="fs-3 text-capitalize p-2">{{ route.params.infoId.replace('-', ' ') }}:</p>
          <hr class="mt-0">
        </div>

        <div v-for="(i, index) in infoArr" :key="i" class="col-12 col-sm-6 col-md-12">
          <router-link :to="{ name: 'Info', params: { infoId: route.params.infoId, infoDetails: i.index } }">
            <p class="text-light selectable rounded p-2">{{ i.name }}</p>
          </router-link>
          <hr v-if="index + 1 < infoArr.length" class="my-2">
        </div>
      </section>
    </div>

    <div v-if="infoHtml" v-html="infoHtml" class="col-12 col-md-9 col-lg-10 offset-md-3 offset-lg-2 my-2">
    </div>

    <div v-else-if="route.params.infoDetails != 'search'">
      <Loader />
    </div>
  </section>
</template>

<script>
import { useRoute } from 'vue-router'
import { AppState } from '../AppState.js'
import { computed, onUnmounted, watchEffect } from 'vue'
import { infosService } from '../services/InfosService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()

    onUnmounted(() => {
      AppState.infoArr = []
      AppState.infoHtml = ''
    })

    watchEffect(() => {
      getInfoById(route.params.infoId)
    })

    watchEffect(() => {
      getInfoDetails(route.params.infoDetails)
    })

    async function getInfoById(infoId) {
      try {
        if (!infoId) {
          return
        }
        await infosService.getInfoById(infoId)
      } catch (error) {
        Pop.error(error.message, '[GETTING INFO BY ID]')
      }
    }

    async function getInfoDetails(i) {
      try {
        AppState.infoHtml = ''

        if (!i || i == 'search') {
          return
        }
        await infosService.getInfoDetails(route.fullPath.replace('info', 'api'))
      } catch (error) {
        Pop.error(error.message, '[SHOWING INFO]')
      }
    }

    return {
      route,
      infoArr: computed(() => AppState.infoArr),
      infoDetails: computed(() => AppState.infoDetails),
      infoHtml: computed(() => AppState.infoHtml)
    }
  }
}
</script>

<style lang="scss" scoped>
  .infoBar {
    background-color: var(--oxford);
    color: white;
    height: 25vh;
  }

  @media screen and (min-width: 768px) {
    .infoBar {
      min-height: var(--main-height);
      position: fixed;
    }
  }
</style>