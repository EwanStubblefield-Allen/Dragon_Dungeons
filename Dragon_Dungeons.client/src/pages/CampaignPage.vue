<template>
  <section v-if="campaign" class="row">
    <div class="col-12 col-md-6 h-md py-2">
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

    <div v-if="campaign.creatorId == account.id" class="col-12 col-md-6 h-md py-2">
      <div class="d-flex flex-column justify-content-between h-md-50">
        <div class="py-2">
          <div class="d-flex align-items-center">
            <p class="fs-3 fw-bold">Players:</p>
            <i @click="copyCode()" class="mdi mdi-content-copy px-1 fs-5 selectable" title="Copy Campaign Code"></i>
            <a class="mdi mdi-share-variant px-1 fs-5 text-dark selectable" :href="`mailto:?subject=Dungeons and Dragons Campaign&body=Join my Dnd Campaign! Code: ${campaign.id}`" title="Share Campaign Code Via Email"></a>
          </div>
          <div v-if="campaign.players.length" class="row overflow-auto players">
            <div v-for="p in campaign.players" :key="p.id" class="col-12 col-sm-6 col-md-12">
              <CharacterCard :characterProp="p" />
            </div>
          </div>
          <div v-else class="d-flex justify-content-center">
            <p class="fs-3">No current players in this campaign!</p>
          </div>
        </div>

        <ul class="nav nav-tabs">
          <li class="nav-item">
            <p @click="changeable = 1" :class="{ 'active elevation-5': changeable == 1 }" class="nav-link selectable text-dark fw-bold">Chat</p>
          </li>

          <li class="nav-item">
            <p @click="changeable = 2" :class="{ 'active elevation-5': changeable == 2 }" class="nav-link selectable text-dark fw-bold">Npcs</p>
          </li>

          <li class="nav-item">
            <p @click="changeable = 3" :class="{ 'active elevation-5': changeable == 3 }" class="nav-link selectable text-dark fw-bold">Monsters</p>
          </li>
        </ul>
      </div>

      <div class="bg-dark rounded-bottom elevation-5 overflow-auto h-md-50 pb-2">
        <div v-if="changeable == 1" class="d-flex align-items-end h-100">
          <form @submit.prevent="createComment()" class="px-2 w-100">
            <div class="input-group">
              <input v-model="editable" id="comment" class="form-control" type="text" minlength="2" maxlength="100" placeholder="Leave your comment...">
              <button type="submit" class="mdi mdi-plus input-group-text" title="Post Comment"></button>
            </div>
          </form>
        </div>

        <div v-else-if="changeable == 2">
          <div v-if="campaign.npcs.length" class="d-flex p-2">
            <div v-for="n in campaign.npcs" :key="n">
              <CharacterCard :characterProp="n" />
            </div>
          </div>

          <div v-else class="d-flex justify-content-center p-2">
            <p class="fs-4 fw-bold">No current npcs in this campaign!</p>
          </div>
        </div>

        <div v-else>
          <div v-if="campaign.monsters.length" class="row mx-0">
            <div v-for="m in campaign.monsters" :key="m" class="col-12 col-md-6 p-2">
              <router-link :to="m.url.replace('api', 'info')" class="px-2 fs-5 fw-bold text-light">
                <u>{{ m.name }}</u>
              </router-link>
              <hr class="my-2">
            </div>
          </div>

          <div v-else class="d-flex justify-content-center p-2">
            <p class="fs-4 fw-bold">No current monsters in this campaign!</p>
          </div>
        </div>
      </div>
    </div>
  </section>

  <div v-else>
    <Loader />
  </div>
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
    const editable = ref('')
    const selectable = ref(1)
    const changeable = ref(1)

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
      editable,
      selectable,
      changeable,
      account: computed(() => AppState.account),
      campaign: computed(() => AppState.activeCampaign),

      copyCode() {
        navigator.clipboard.writeText(this.campaign.id)
      }
    }
  },
  components: { CharacterCard }
}
</script>

<style lang="scss" scoped>
  @media screen and (min-width: 768px) {
    .h-md {
      height: var(--main-height);
    }

    .h-md-50 {
      height: 50%;
    }

    .players {
      height: 28vh;
    }
  }
</style>