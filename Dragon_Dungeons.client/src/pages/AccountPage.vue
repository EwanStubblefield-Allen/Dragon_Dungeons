<template>
  <div v-if="account.id">
    <section class="row justify-content-center">
      <div class="col-12">
        <ProfileDetails :profileProp="account" />
      </div>

      <div v-if="characters.length" class="col-10">
        <p class="fs-2 fw-bold">My Characters:</p>
        <section class="row py-3">
          <div v-for="c in characters" :key="c.id" class="col-12 col-sm-6">
            <CharacterCard :characterProp="c" />
          </div>
        </section>
      </div>

      <div class="col-10">
        <div class="d-md-flex justify-content-between">
          <p class="fs-2 fw-bold">My Campaigns:</p>
          <form v-if="characters.length" @submit.prevent="createPlayer()">
            <div class="d-md-flex">
              <select v-model="editable.character" class="form-select m-1" aria-label="Default select example" required>
                <option disabled>Character For Campaign</option>
                <option v-for="c in characters" :key="c.id" :value="c">{{ c.name }}</option>
              </select>
              <div class="input-group m-1">
                <input v-model="editable.campaignId" id="campaign" class="form-control" type="text" minlength="30" maxlength="40" placeholder="Campaign Code..." required>
                <button type="submit" class="mdi mdi-plus input-group-text" title="Join Campaign"></button>
              </div>
            </div>
          </form>
        </div>
        <section class="row py-3">
          <div v-for="c in campaigns" :key="c.id" class="col-12 col-sm-6 col-md-4 col-lg-3">
            <CampaignCard :campaignProp="c" />
          </div>
        </section>
      </div>
    </section>
  </div>

  <div v-else>
    <Loader />
  </div>
</template>

<script>
import { computed, ref } from 'vue'
import { AppState } from '../AppState'
import { playersService } from '../services/PlayersService.js'
import ProfileDetails from '../components/ProfileDetails.vue'
import CharacterCard from '../components/CharacterCard.vue'
import CampaignCard from '../components/CampaignCard.vue'
import Loader from '../components/Loader.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({})

    return {
      editable,
      account: computed(() => AppState.account),
      characters: computed(() => AppState.characters),
      campaigns: computed(() => AppState.campaigns),

      async createPlayer() {
        try {
          await playersService.createPlayer(editable.value)
          editable.value = {}
          Pop.success('You joined the campaign!')
        } catch (error) {
          Pop.error(error.message, '[CREATING CHARACTER]')
        }
      }
    }
  },
  components: { ProfileDetails, CharacterCard, CampaignCard, Loader }
}
</script>

<style scoped></style>