<template>
  <form @submit.prevent="addCategory()">
    <div class="form-group py-2">
      <label for="categoryTitle">Category Title</label>
      <input v-model="editable.category" type="text" class="form-control" id="categoryTitle" minlength="3" maxlength="100" placeholder="Category Title..." required>
    </div>
    <div class="form-group py-2">
      <label for="noteTitle">Note Title</label>
      <input v-model="editable.notes[0].name" type="text" class="form-control" id="noteTitle" minlength="3" maxlength="100" placeholder="Note Title..." required>
    </div>
    <div class="form-group py-2">
      <label for="description">Description</label>
      <textarea v-model="editable.notes[0].description" id="description" class="form-control" rows="5" placeholder="Note Description..." required></textarea>
    </div>
    <div class="text-end">
      <button class="btn btn-success mt-2" type="submit">Add</button>
    </div>
  </form>
</template>

<script>
import { ref } from 'vue'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import { Modal } from 'bootstrap'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({ notes: [{}] })

    return {
      editable,

      async addCategory() {
        try {
          const campaign = AppState.activeCampaign
          const campaignData = {}

          if (AppState.notes == 1) {
            campaignData.publicNotes = [...campaign.publicNotes]
            campaignData.publicNotes.push(editable.value)
          } else {
            campaignData.privateNotes = [...campaign.privateNotes]
            campaignData.privateNotes.push(editable.value)
          }
          await campaignsService.updateCampaign(campaignData, campaign.id)
          Modal.getOrCreateInstance('#categoryForm').hide()
          Pop.success('Category was added!')
          editable.value = { notes: [{}] }
        } catch (error) {
          Pop.error(error.message, '[ADDING CATEGORY]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>