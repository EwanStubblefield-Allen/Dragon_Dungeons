<template>
  <section class="row">
    <div class="col-12 col-md-3 col-lg-2 overflow-auto infoBar">
      <section class="row p-3">
        <div class="col-12 p-0">
          <p class="fs-3 text-capitalize p-2">Steps:</p>
          <hr class="mt-0">
        </div>
        <div v-for="(l, index) in list" :key="l" class="col-12 col-sm-6 col-md-12">
          <router-link :to="{ name: 'BuildCampaign', params: { campaignStep: l.toLowerCase().replaceAll(' ', '-') } }" v-if="camPage >= index">
            <p class="text-light selectable rounded px-2 py-1">{{ l }}</p>
          </router-link>
          <p v-else class="text-secondary rounded px-2 py-1">{{ l }}</p>
          <hr v-if="index != list.length - 1" class="my-2">
        </div>
      </section>
    </div>

    <div class="col-12 col-md-9 col-lg-10 offset-md-3 offset-lg-2">
      <div v-if="route.params.campaignStep == 'basics'">
        <CampaignBasics />
      </div>

      <div v-else-if="route.params.campaignStep == 'notes'">
        <CampaignNotes />
      </div>

      <div v-else-if="route.params.campaignStep == 'encounters'">
        <CampaignEncounters />
      </div>

      <div v-else-if="route.params.campaignStep == 'monsters'">
        <CampaignMonsters />
      </div>
    </div>
  </section>
</template>

<script>
import { computed, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { AppState } from "../AppState.js"
import CampaignBasics from '../components/CampaignBasics.vue'
import CampaignNotes from '../components/CampaignNotes.vue'
import CampaignEncounters from '../components/CampaignEncounters.vue'
import CampaignMonsters from '../components/CampaignMonsters.vue'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    const list = ['Basics', 'Notes', 'Encounters', 'Monsters']

    watchEffect(() => {
      let camPage = AppState.camPage

      for (let i = list.length - 1; i > camPage; i--) {
        if (list[i].toLowerCase().replace(' ', '-') == route.params.campaignStep && AppState.tempCharacter) {
          router.push({ name: 'BuildCampaign', params: { campaignStep: list[camPage].toLowerCase().replace(' ', '-') } })
        }
      }
    })

    return {
      route,
      list,
      camPage: computed(() => AppState.camPage)
    }
  },
  components: { CampaignBasics, CampaignNotes, CampaignEncounters, CampaignMonsters }
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
      height: var(--main-height);
      position: fixed;
    }
  }
</style>