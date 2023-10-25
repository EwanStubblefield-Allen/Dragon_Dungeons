<template>
  <div class="card elevation-5">
    <div class="card-body p-2">
      <h5 class="card-title text-center text-uppercase fs-3">{{ campaignProp.name }}</h5>
      <p class="fs-5 pb-2">{{ campaignProp.description }}</p>
      <div class="d-flex justify-content-end align-items-center">
        <router-link :to="{ name: 'Campaign', params: { campaignId: campaignProp.id } }" class="btn btn-primary elevation-5">Campaign Details</router-link>
      </div>
      <i v-if="campaignProp.creatorId == account.id" @click="removeCampaign(campaignProp)" class="mdi mdi-delete fs-5 text-danger selectable"></i>
    </div>
  </div>
</template>

<script>
import { computed } from 'vue'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import { Campaign } from '../models/Campaign.js'
import Pop from '../utils/Pop.js'

export default {
  props: {
    campaignProp: {
      type: Campaign,
      required: true
    }
  },

  setup() {
    return {
      account: computed(() => AppState.account),

      async removeCampaign(campaign) {
        try {
          const res = await Pop.question('Delete Campaign?', `To delete the campaign: "${campaign.name}", type the name to confirm.`)

          if (!res) {
            return
          }

          if (res != campaign.name) {
            return Pop.toast('Campaign name does not match!')
          }
          const campaignToRemove = await campaignsService.removeCampaign(campaign.id)
          Pop.toast(`${campaignToRemove.name} was deleted!`)
        } catch (error) {
          Pop.error(error.message, '[DELETING CAMPAIGN]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  i {
    position: absolute;
    top: 2px;
    right: 5px;
  }
</style>