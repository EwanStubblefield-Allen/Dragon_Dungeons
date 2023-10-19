<template>
  <div class="card elevation-5">
    <img :src="campaignProp.picture.url" class="card-img-top position-relative" :alt="campaignProp.name">
    <i @click="removeCampaign()" class="mdi mdi-delete fs-5 text-danger selectable"></i>
    <div class="card-body p-2">
      <h5 class="card-title text-center text-uppercase fs-3">{{ campaignProp.name }}</h5>
      <p class="fs-5 pb-2">{{ campaignProp.description }}</p>
      <div class="d-flex justify-content-end align-items-center">
        <router-link :to="{ name: 'Campaign', params: { campaignId: campaignProp.id } }" class="btn btn-primary elevation-5">Campaign Details</router-link>
      </div>
    </div>
  </div>
</template>

<script>
import { Campaign } from '../models/Campaign.js'

export default {
  props: {
    campaignProp: {
      type: Campaign,
      required: true
    }
  },

  setup() {
    return {}
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