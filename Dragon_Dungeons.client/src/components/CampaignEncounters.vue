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
      <form @submit.prevent="changeTab()" v-if="selectable == 1" class="row g-3">
        <div class="col-md-4 form-group">
          <label for="age">Age:</label>
          <input v-model="editable.age" type="number" class="form-control" id="age" min="1" max="1000" placeholder="Age..." required>
        </div>

        <div class="col-md-4 form-group">
          <label for="inch">Height:</label>
          <div class="input-group">
            <input v-model="editable.feet" type="number" class="form-control" id="inch" min="1" max="100" placeholder="Feet..." required>
            <input v-model="editable.inches" type="number" class="form-control" id="feet" min="0" max="11" placeholder="Inches..." required>
          </div>
        </div>

        <div class="col-md-4 form-group">
          <label for="weight">Weight:</label>
          <div class="input-group">
            <input v-model="editable.weight" type="number" class="form-control" id="weight" min="1" max="500" placeholder="Weight..." required>
            <div class="input-group-text">.lb</div>
          </div>
        </div>

        <div class="col-md-4 form-group">
          <label for="eyes">Eyes:</label>
          <input v-model="editable.eyes" type="text" class="form-control" id="eyes" minlength="3" maxlength="30" placeholder="Eyes..." required>
        </div>

        <div class="col-md-4 form-group">
          <label for="skin">Skin:</label>
          <input v-model="editable.skin" type="text" class="form-control" id="skin" minlength="3" maxlength="30" placeholder="Skin..." required>
        </div>

        <div class="col-md-4 form-group">
          <label for="hair">Hair:</label>
          <input v-model="editable.hair" type="text" class="form-control" id="hair" minlength="3" maxlength="30" placeholder="Hair..." required>
        </div>

        <div class="col-12 text-end">
          <button type="submit" class="btn btn-primary">Save</button>
        </div>
      </form>

      <form v-else @submit.prevent="changeCamPage()" class="row g-3">
        <div v-if="characters.length">
          <div class="form-group">
            <label for="npc">Select Character to Use as Npc</label>
            <div class="input-group">
              <select v-model="character" id="npc" class="form-select" aria-label="NPC dropdown">
                <option v-for="c in characters" :key="c.id" :value="c">{{ c.name }}</option>
              </select>
              <button @click="addNpc()" type="button" class="mdi mdi-plus input-group-text"></button>
            </div>
          </div>

          <div v-for="n in editable?.npcs" :key="n">
            <p>{{ n.name }}</p>
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

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const selectable = ref(1)
    const character = ref({})

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

      addNpc() {
        if (!editable.value.npcs) {
          editable.value.npcs = []
        }
        editable.value.npcs.push(character.value)
      },

      changeCamPage() {
        campaignsService.changeCamPage(2)
        router.push({ name: 'Campaign', params: { campaignId: 'notes' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>