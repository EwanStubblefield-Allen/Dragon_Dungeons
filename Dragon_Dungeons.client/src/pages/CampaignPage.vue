<template>
  <section v-if="campaign" class="row py-1">
    <div class="col-12 col-md-6">
      <p class="fs-2 fw-bold text-wrap text-truncate">{{ campaign.name }}</p>
      <hr class="my-2">
      <p class="fs-5 px-2">{{ campaign.description }}</p>
    </div>

    <div class="col-12 col-md-6">

    </div>
  </section>
</template>

<script>
import { computed, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()

    watchEffect(() => {
      if (route.params.campaignId) {
        getCampaignById()
      }
    })

    async function getCampaignById() {
      try {
        await campaignsService.getCampaignById(route.params.campaignId)
      } catch (error) {
        Pop.error(error.message, '[GETTING CAMPAIGN BY ID]')
        router.push('/')
      }
    }

    return {
      campaign: computed(() => AppState.activeCampaign)
    }
  }
}
</script>

<style lang="scss" scoped></style>