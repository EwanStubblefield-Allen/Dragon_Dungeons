<template>
  <p class="fs-1 fw-bold m-3 py-2">Basics</p>

  <form @submit.prevent="changeCamPage()" class="row g-3 bg-dark text-dark m-3 p-3 rounded elevation-5">
    <div class="col-12 form-floating">
      <input v-model="editable.name" type="text" id="name" class="form-control" autocomplete="off" minlength="3" maxlength="100" placeholder="Name..." required>
      <label for="name">Campaign Name:</label>
    </div>

    <div class="col-12 form-group">
      <label class="text-light" for="description">Description:</label>
      <textarea v-model="editable.description" id="description" class="form-control" minlength="3" maxlength="1000" autocomplete="off" placeholder="Description..." rows="10"></textarea>
    </div>

    <!-- <div class="col-12 form-group">
      <label for="map" class="text-light">Map:</label>
      <input class="form-control" type="file" id="map">
    </div> -->

    <div class="col-12 text-end">
      <button type="submit" class="btn btn-primary">Save</button>
    </div>
  </form>
</template>

<script>
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})

    onMounted(() => {
      editable.value = { ...AppState.tempCampaign }
    })

    onBeforeUnmount(() => {
      handleSave()
    })

    async function handleSave() {
      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCampaign) {
        return
      } else {
        saveCampaign()
      }
    }

    function saveCampaign() {
      editable.value.creator = AppState.account
      campaignsService.saveCampaign(editable.value)
    }

    return {
      editable,

      changeCamPage() {
        campaignsService.changeCamPage(0)
        router.push({ name: 'BuildCampaign', params: { campaignStep: 'notes' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>