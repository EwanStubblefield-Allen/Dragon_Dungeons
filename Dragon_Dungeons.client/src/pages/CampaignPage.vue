<template>
  <section v-if="campaign" class="row">
    <div class="col-12 col-md-6 height py-2">
      <div class="d-flex flex-column justify-content-between h-md-50">
        <div class="pb-2">
          <p class="fs-2 fw-bold text-wrap text-truncate">{{ campaign.name }}</p>
          <hr class="my-2">
          <p class="fs-5 px-2">{{ campaign.description }}</p>
        </div>

        <ul class="nav nav-tabs">
          <li class="nav-item">
            <p @click="selectable = 1" :class="{ 'active elevation-5': selectable == 1 }" class="nav-link selectable text-dark fw-bold">Public</p>
          </li>

          <li v-if="campaign.creatorId == account.id" class="nav-item">
            <p @click="selectable = 2" :class="{ 'active elevation-5': selectable == 2 }" class="nav-link selectable text-dark fw-bold">Private</p>
          </li>

          <li v-if="campaign.creatorId == account.id" class="nav-item">
            <p @click="selectable = 3" :class="{ 'active elevation-5': selectable == 3 }" class="nav-link selectable text-dark fw-bold">Events</p>
          </li>
        </ul>
      </div>

      <div v-if="selectable != 3" class="bg-dark rounded-bottom elevation-5 overflow-auto h-md-50 pb-2">
        <div v-for="p in selectable == 1 ? campaign.publicNote : campaign.privateNote" :key="p" class="row mx-0 p-2">
          <p class="fs-3 fw-bold col-12">{{ p[0] }}</p>
          <hr class="col-12 my-2">
          <div v-for="n in p[1]" :key="n" class="col-12 col-md-6 py-2">
            <p class="fs-5 fw-bold">{{ n.name }}</p>
            <hr class="my-2">
            <p class="px-2">{{ n.description }}</p>
          </div>
        </div>
      </div>

      <div v-else class="row mx-0 bg-dark rounded-bottom elevation-5 overflow-auto h-md-50 pb-2">
        <div v-for="e in campaign.events" :key="e" class="col-12 col-md-6 p-2">
          <p class="px-2 fs-3 fw-bold">{{ e.name }}</p>
          <hr class="my-2">
          <p class="px-2">{{ e.description }}</p>
        </div>
      </div>
    </div>

    <div class="col-12 col-md-6 height py-2">
      <div class="d-flex flex-column justify-content-between h-md-50">
        <div v-if="campaign.characters" class="d-flex overflow-auto pb-2">
          <div v-for="c in campaign.characters" :key="c.id">
            <CharacterCard :characterProp="c" />
          </div>
        </div>
        <div v-else class="d-flex justify-content-center">
          <p class="fs-3 fw-bold">No current players in this campaign!</p>
        </div>

        <ul class="nav nav-tabs">
          <li class="nav-item">
            <p @click="selectable = 1" :class="{ 'active elevation-5': selectable == 1 }" class="nav-link selectable text-dark fw-bold">Public</p>
          </li>

          <li v-if="campaign.creatorId == account.id" class="nav-item">
            <p @click="selectable = 2" :class="{ 'active elevation-5': selectable == 2 }" class="nav-link selectable text-dark fw-bold">Private</p>
          </li>

          <li v-if="campaign.creatorId == account.id" class="nav-item">
            <p @click="selectable = 3" :class="{ 'active elevation-5': selectable == 3 }" class="nav-link selectable text-dark fw-bold">Events</p>
          </li>
        </ul>
      </div>

      <div v-if="selectable != 3" class="bg-dark rounded-bottom elevation-5 overflow-auto h-md-50 pb-2">
        <div v-for="p in selectable == 1 ? campaign.publicNote : campaign.privateNote" :key="p" class="row mx-0 p-2">
          <p class="fs-3 fw-bold col-12">{{ p[0] }}</p>
          <hr class="col-12 my-2">
          <div v-for="n in p[1]" :key="n" class="col-12 col-md-6 py-2">
            <p class="fs-5 fw-bold">{{ n.name }}</p>
            <hr class="my-2">
            <p class="px-2">{{ n.description }}</p>
          </div>
        </div>
      </div>

      <div v-else class="row mx-0 bg-dark rounded-bottom elevation-5 overflow-auto h-md-50 pb-2">
        <div v-for="e in campaign.events" :key="e" class="col-12 col-md-6 p-2">
          <p class="px-2 fs-3 fw-bold">{{ e.name }}</p>
          <hr class="my-2">
          <p class="px-2">{{ e.description }}</p>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import { computed, ref, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import CharacterCard from '../components/CharacterCard.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    const selectable = ref(1)
    watchEffect(() => {
      if (route.params.campaignId) {
        getCampaignById()
      }
    })

    async function getCampaignById() {
      try {
        await campaignsService.getCampaignById(route.params.campaignId)
      }
      catch (error) {
        Pop.error(error.message, '[GETTING CAMPAIGN BY ID]')
        router.push('/')
      }
    }
    return {
      selectable,
      account: computed(() => AppState.account),
      campaign: computed(() => AppState.activeCampaign)
    }
  },
  components: { CharacterCard }
}
</script>

<style lang="scss" scoped>
  @media screen and (min-width: 768px) {
    .height {
      height: var(--main-height);
    }

    .h-md-50 {
      height: 50%;
    }
  }
</style>