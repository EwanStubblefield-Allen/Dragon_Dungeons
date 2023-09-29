<template>
  <div class="m-3">
    <p class="fs-1 fw-bold py-2">Encounters</p>

    <ul class="nav nav-tabs">
      <li class="nav-item">
        <p @click="selectable = 1" :class="{ 'active elevation-5': selectable == 1 }" class="nav-link selectable text-dark fw-bold">Events</p>
      </li>

      <li class="nav-item">
        <p @click="selectable = 2" :class="{ 'active elevation-5': selectable == 2 }" class="nav-link selectable text-dark fw-bold">NPCs</p>
      </li>
    </ul>

    <div class="bg-dark p-3 rounded-bottom elevation-5">
      <form @submit.prevent="selectable = 2" v-if="selectable == 1" class="row g-3">
        <div class="col-12 d-flex justify-content-between">
          <p class="fs-3 fw-bold">Events:</p>
          <button @click="addEvent()" type="button" class="btn btn-primary">Event +</button>
        </div>
        <div v-if="editable?.events" class="col-12">
          <section class="row">
            <div v-for="(e, index) in editable.events" :key="e" class="col-12 col-md-6 col-lg-4 py-3">
              <div class="input-group pb-2">
                <input v-model="editable.events[index].name" type="text" class="form-control" :id="`eventName${index}`" minlength="3" maxlength="100" placeholder="Event Title..." required>
                <button @click="removeEvent(index)" type="button" class="mdi mdi-delete input-group-text text-danger"></button>
              </div>
              <textarea v-model="editable.events[index].description" :id="`eventDescription${index}`" class="form-control" rows="5" placeholder="Description..." required></textarea>
            </div>
          </section>
        </div>

        <div class="col-12 text-end">
          <button type="submit" class="btn btn-primary">Save</button>
        </div>
      </form>

      <form v-else @submit.prevent="changeCamPage()" class="row g-3">
        <div v-if="characters.length" class="col-12">
          <div class="form-group">
            <label for="npc">Select Character to Use as Npc</label>
            <div class="input-group">
              <select v-model="character" id="npc" class="form-select" aria-label="NPC dropdown">
                <option selected disabled>Select...</option>
                <option v-for="c in characters" :key="c.id" :value="c" :disabled="editable.npcs?.includes(c)">{{ c.name }}</option>
              </select>
              <button @click="addNpc()" type="button" :class="character == 'Select...'" class="mdi mdi-plus input-group-text"></button>
            </div>
          </div>

          <div class="row mx-2">
            <div v-for="n in editable?.npcs" :key="n" class="col-12 col-sm-6 col-lg-4 text-dark">
              <div class="d-flex justify-content-between align-items-center bg-light rounded elevation-5 p-2 mt-3">
                <img :src="n.picture" :alt="n.name">
                <p class="text-uppercase">{{ n.name }}</p>
                <div class="d-flex align-items-center">
                  <i class="mdi mdi-information text-primary selectable fs-5 px-1"></i>
                  <i @click="removeNpc(n.id)" type="button" class="mdi mdi-delete text-danger px-1"></i>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div v-else>
          <p class="fs-3 fw-bold text-center">Build a character to add as an Npc!</p>
        </div>

        <div class="col-12 text-end">
          <button type="submit" class="btn btn-primary">Save</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const selectable = ref(1)
    const character = ref('Select...')

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
      selectable,
      character,
      characters: computed(() => AppState.characters),

      addEvent() {
        if (!editable.value.events) {
          editable.value.events = []
        }
        editable.value.events.push({})
      },

      async removeEvent(index) {
        const isSure = await Pop.confirm('Are you sure you want to remove this event?')

        if (!isSure) {
          return
        }
        editable.value.events.splice(index, 1)
      },

      addNpc() {
        if (!editable.value.npcs) {
          editable.value.npcs = []
        }
        editable.value.npcs.push(character.value)
        character.value = 'Select...'
      },

      async removeNpc(characterId) {
        const isSure = await Pop.confirm('Are you sure you want to remove this npc?')

        if (!isSure) {
          return
        }
        editable.value.npcs = editable.value.npcs.filter(n => n.id != characterId)
      },

      changeCamPage() {
        campaignsService.changeCamPage(2)
        router.push({ name: 'Campaign', params: { campaignId: 'creatures' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>