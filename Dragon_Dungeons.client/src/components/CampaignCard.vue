<template>
  <div class="card elevation-5">
    <div class="card-body p-2">
      <h5 class="card-title text-center text-uppercase fs-3">{{ campaignProp.name }}</h5>
      <p class="fs-5 pb-2">{{ campaignProp.description }}</p>
      <div class="d-flex justify-content-end align-items-center">
        <router-link :to="{ name: 'Campaign', params: { campaignId: campaignProp.id } }" class="btn btn-primary elevation-5">Campaign Details</router-link>
      </div>
      <i v-if="campaignProp.id == account.id" @click="removeCampaign()" class="mdi mdi-delete fs-5 text-danger selectable"></i>
    </div>
  </div>
</template>

<script>
import { computed } from 'vue'
import { AppState } from '../AppState.js'
import { Campaign } from '../models/Campaign.js'

export default {
  props: {
    campaignProp: {
      type: Campaign,
      required: true
    }
  },

  setup() {
    return {
      account: computed(() => AppState.account)
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