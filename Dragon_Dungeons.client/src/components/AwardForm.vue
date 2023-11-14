<template>
  <form @submit.prevent="awardPlayers()" class="row">
    <div class="col-4 form-group py-2">
      <label for="awardXp">Award Xp</label>
      <input v-model="editable.xp" type="number" class="form-control" id="awardXp" min="0" max="50000">
    </div>
    <div class="col-4 form-group py-2">
      <label for="awardCp">Award Cp</label>
      <input v-model="editable.currency[0]" type="number" class="form-control" id="awardCp" min="0" max="5000">
    </div>
    <div class="col-4 form-group py-2">
      <label for="awardSp">Award Sp</label>
      <input v-model="editable.currency[1]" type="number" class="form-control" id="awardSp" min="0" max="5000">
    </div>
    <div class="col-4 form-group py-2">
      <label for="awardEp">Award Ep</label>
      <input v-model="editable.currency[2]" type="number" class="form-control" id="awardEp" min="0" max="5000">
    </div>
    <div class="col-4 form-group py-2">
      <label for="awardGp">Award Gp</label>
      <input v-model="editable.currency[3]" type="number" class="form-control" id="awardGp" min="0" max="5000">
    </div>
    <div class="col-4 form-group py-2">
      <label for="awardPp">Award Pp</label>
      <input v-model="editable.currency[4]" type="number" class="form-control" id="awardPp" min="0" max="5000">
    </div>

    <div class="text-end pt-2">
      <button class="btn btn-success" type="submit">Award</button>
    </div>
  </form>
</template>

<script>
import { ref } from 'vue'
import { AppState } from '../AppState.js'
import { campaignHub } from '../handlers/CampaignHub.js'
import { Modal } from 'bootstrap'

export default {
  setup() {
    const editable = ref({ currency: [] })

    return {
      editable,

      awardPlayers() {
        campaignHub.awardPlayers(AppState.activeCampaign.id, editable.value)
        Modal.getOrCreateInstance('#award').hide()
        editable.value = { currency: [] }
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>