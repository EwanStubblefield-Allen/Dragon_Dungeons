<template>
  <p class="fs-1 fw-bold m-3 py-2">Monsters</p>

  <section class="row align-items-center g-3 bg-dark m-3 p-3 rounded elevation-5">
    <div class="col-12 col-md-6 d-flex justify-content-between justify-content-md-start">
      <button @click="selectable = editable.monsters" class="btn btn-primary mx-1" type="button">Show Selected</button>
      <button @click="selectable = monsters" class="btn btn-danger mx-1" type="button">Reset</button>
    </div>
    <form @submit.prevent="searchMonsters()" class="col-12 col-md-6">
      <div class="form-group d-md-flex align-items-center">
        <label for="search" class="fw-bold w-100">Search for activity:</label>
        <div class="input-group">
          <input v-model="search" id="search" class="form-control" type="text" minlength="3" maxlength="30" placeholder="Monster...">
          <button type="submit" class="input-group-text" id="search" title="Search"><i class="mdi mdi-magnify"></i></button>
        </div>
      </div>
    </form>

    <p @click="addMonster(s)" v-for="s in selectable" :key="s.index" :class="{ 'bg-light text-dark elevation-5': editable.monsters?.find(m => m.name == s.name) }" class="col-12 col-sm-4 col-md-3 col-lg-2 p-2 text-center selectable rounded">{{ s.name }}</p>

    <div class="col-12 text-end">
      <button @click="finishCreation()" type="button" class="btn btn-primary">Save</button>
    </div>
  </section>
</template>

<script>
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import { infosService } from '../services/InfosService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const selectable = ref([])
    const monsters = ref([])
    const search = ref('')
    let isFinished = false

    onMounted(() => {
      editable.value = { ...AppState.tempCampaign }
      getMonsters()
    })

    onBeforeUnmount(() => {
      handleSave()
    })

    async function handleSave() {
      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCampaign) {
        return
      } else if (isFinished) {
        createCampaign()
      } else {
        saveCampaign()
      }
    }

    async function createCampaign() {
      try {
        editable.value.npcs.map(n => n.picture = n.picture.url)
        await campaignsService.createCampaign(editable.value)
      } catch (error) {
        Pop.error(error.message, '[CREATING CAMPAIGN]')
      }
    }

    function saveCampaign() {
      editable.value.creator = AppState.account
      campaignsService.saveCampaign(editable.value)
    }

    async function getMonsters() {
      try {
        monsters.value = await infosService.getInfoById('monsters', false)
        selectable.value = [...monsters.value]
      } catch (error) {
        Pop.error(error.message, '[GETTING MONSTERS]')
      }
    }

    return {
      editable,
      selectable,
      monsters,
      search,

      searchMonsters() {
        selectable.value = monsters.value.filter(s => s.name.toLowerCase().includes(search.value.toLowerCase()))
      },

      addMonster(monster) {
        if (!editable.value.monsters) {
          editable.value.monsters = []
        }
        const foundIndex = editable.value.monsters.findIndex(s => s.index == monster.index)

        if (foundIndex != -1) {
          editable.value.monsters.splice(foundIndex, 1)
        } else {
          editable.value.monsters.push(monster)
        }
      },

      async finishCreation() {
        const isSure = await Pop.confirm('Are you sure you are finished creating this campaign?')

        if (!isSure) {
          return
        }
        isFinished = true
        router.push({ name: 'Home' })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>