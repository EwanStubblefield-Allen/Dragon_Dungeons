<template>
  <form @submit.prevent="addNote()">
    <div class="form-group py-2">
      <label for="title">Title</label>
      <input v-model="editable.name" type="text" class="form-control" id="title" minlength="3" maxlength="100" placeholder="Note Title..." required>
    </div>
    <div class="form-group py-2">
      <label for="description">Description</label>
      <textarea v-model="editable.description" id="description" class="form-control" rows="5" placeholder="Note Description..." required></textarea>
    </div>
    <div class="text-end">
      <button class="btn btn-success mt-2" type="submit">Add</button>
    </div>
  </form>
</template>

<script>
import { ref } from 'vue'
import { campaignsService } from '../services/CampaignsService.js'
import { AppState } from '../AppState.js'
import { Modal } from 'bootstrap'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({})

    function handleArray(key) {
      let campaignData = AppState.activeCampaign[key]

      if (!campaignData) {
        campaignData = []
      }

      if (key == 'events') {
        campaignData.push(editable.value)
      } else {
        campaignData[AppState.notes[1]].notes.push(editable.value)
      }
      return campaignData
    }

    return {
      editable,

      async addNote() {
        try {
          const notes = AppState.notes
          const campaignData = {}

          if (!notes) {
            campaignData.events = handleArray('events')
          } else if (notes[0] == 1) {
            campaignData.publicNotes = handleArray('publicNotes')
          } else {
            campaignData.privateNotes = handleArray('privateNotes')
          }
          await campaignsService.updateCampaign(campaignData, AppState.activeCampaign.id)
          Modal.getOrCreateInstance('#notesForm').hide()
          Pop.success('Note was added!')
          editable.value = {}
        } catch (error) {
          Pop.error(error.message, '[ADDING NOTE]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>