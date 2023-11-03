<template>
  <p class="fs-1 fw-bold m-3 py-2">Notes</p>

  <form @submit.prevent="changeCamPage()" class="row g-3 bg-dark text-light m-3 p-3 rounded elevation-5">
    <div class="col-12 d-flex justify-content-between">
      <p class="fs-3 fw-bold">Private Notes:</p>
      <button @click="addCategory('privateNotes')" type="button" class="btn btn-primary">Category +</button>
    </div>
    <div v-if="editable?.privateNotes" class="col-12">
      <section v-for="(p, index) in editable.privateNotes" :key="p" class="row">
        <div class="col-12 form-floating pt-3">
          <div class="input-group">
            <input v-model="editable.privateNotes[index].category" type="text" class="form-control" :id="`privateCategory${index}`" minlength="3" maxlength="100" placeholder="Category..." required>
            <button @click="addNote('privateNotes', index)" type="button" class="mdi mdi-plus input-group-text"></button>
            <button @click="removeCategory('privateNotes', index)" type="button" class="mdi mdi-delete input-group-text text-danger"></button>
          </div>
        </div>
        <div v-for="(n, i) in p.notes" :key="n" class="col-12 col-md-6 col-lg-4 pt-2">
          <div class="input-group pb-2">
            <input v-model="editable.privateNotes[index].notes[i].name" type="text" class="form-control" :id="`privateName${index}${i}`" minlength="3" maxlength="100" placeholder="Note Title..." required>
            <button @click="removeNote('privateNotes', index, i)" type="button" class="mdi mdi-delete input-group-text text-danger"></button>
          </div>
          <textarea v-model="editable.privateNotes[index].notes[i].description" :id="`privateDescription${index}${i}`" class="form-control" rows="5" placeholder="Description..." required></textarea>
        </div>
      </section>
    </div>

    <div class="col-12 d-flex justify-content-between">
      <p class="fs-3 fw-bold">Public Notes:</p>
      <button @click="addCategory('publicNotes')" type="button" class="btn btn-primary">Category +</button>
    </div>
    <div v-if="editable?.publicNotes" class="col-12">
      <section v-for="(p, index) in editable.publicNotes" :key="p" class="row">
        <div class="col-12 form-floating pt-3">
          <div class="input-group">
            <input v-model="editable.publicNotes[index].category" type="text" class="form-control" :id="`publicCategory${index}`" minlength="3" maxlength="100" placeholder="Category..." required>
            <button @click="addNote('publicNotes', index)" type="button" class="mdi mdi-plus input-group-text"></button>
            <button @click="removeCategory('publicNotes', index)" type="button" class="mdi mdi-delete input-group-text text-danger"></button>
          </div>
        </div>
        <div v-for="(n, i) in p.notes" :key="n" class="col-12 col-md-6 col-lg-4 pt-2">
          <div class="input-group pb-2">
            <input v-model="editable.publicNotes[index].notes[i].name" type="text" class="form-control" :id="`publicName${index}${i}`" minlength="3" maxlength="100" placeholder="Note Title..." required>
            <button @click="removeNote('publicNotes', index, i)" type="button" class="mdi mdi-delete input-group-text text-danger"></button>
          </div>
          <textarea v-model="editable.publicNotes[index].notes[i].description" :id="`publicDescription${index}${i}`" class="form-control" rows="5" placeholder="Description..." required></textarea>
        </div>
      </section>
    </div>

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
import Pop from '../utils/Pop.js'

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

      addCategory(type) {
        if (!editable.value[type]) {
          editable.value[type] = []
        }
        editable.value[type].push({})
      },

      async removeCategory(type, index) {
        const isSure = await Pop.confirm('Are you sure you want to delete this category?')

        if (!isSure) {
          return
        }
        editable.value[type].splice(index, 1)
      },

      addNote(type, index) {
        if (!editable.value[type][index].notes) {
          editable.value[type][index].notes = []
        }
        editable.value[type][index].notes.push({})
      },

      async removeNote(type, index, i) {
        const isSure = await Pop.confirm('Are you sure you want to delete this note?')

        if (!isSure) {
          return
        }
        editable.value[type][index].notes.splice(i, 1)
      },

      changeCamPage() {
        campaignsService.changeCamPage(1)
        router.push({ name: 'BuildCampaign', params: { campaignStep: 'encounters' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>