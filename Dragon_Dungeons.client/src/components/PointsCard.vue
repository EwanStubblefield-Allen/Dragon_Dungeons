<template>
  <form @submit.prevent="" class="row g-3">
    <div class="col-12 fs-3 fw-bold text-end">Points: {{ points }}</div>
    <div v-for="a in attributes" :key="a" class="col-12 col-sm-6 col-md-3 col-lg-2 p-0">
      <p class="fs-5 text-center">{{ a.toUpperCase() }}</p>
      <div class="d-flex justify-content-around align-items-center p-2 manual-col">
        <p class="fs-3 text-center">{{ editable[a] }}</p>
        <div class="d-flex flex-column text-center flex-grow-1">
          <i v-if="points != 0 && editable[a] != 15" @click="changeAtt(a, 1)" class="mdi mdi-chevron-up selectable"></i>
          <i v-else class="mdi mdi-chevron-up text-secondary"></i>
          <i v-if="editable[a] != 8" @click="changeAtt(a, -1)" class="mdi mdi-chevron-down selectable"></i>
          <i v-else class="mdi mdi-chevron-down text-secondary"></i>
        </div>
        <p class="fs-3 text-center flex-grow-1">{{ Math.floor((editable[a] - 10) / 2) }}</p>
      </div>
      <div class="d-flex justify-content-around align-items-center px-2 fs-5">
        <p class="pe-2">Racial Bonus</p>
        <p>+0</p>
      </div>
    </div>
    <div class="col-12 text-end">
      <router-link :to="{ name: 'Character', params: { characterId: 'proficiencies' } }">
        <button type="submit" class="btn btn-primary">Save</button>
      </router-link>
    </div>
  </form>
</template>

<script>
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { AppState } from '../AppState.js'
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({})
    const attributes = ['strength', 'dexterity', 'constitution', 'intelligence', 'wisdom', 'charisma']
    const points = ref(27)

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }

      for (let a in attributes) {
        let att = attributes[a]

        if (!editable.value[att]) {
          editable.value[att] = 8
        }
        let modifier = editable.value[att] - 8

        if (modifier == 6) {
          modifier = 7
        }
        else if (modifier == 7) {
          modifier = 9
        }
        points.value -= modifier
      }
    })

    onBeforeUnmount(() => {
      if (AppState.tempCharacter.manual) {
        return
      }

      if (editable.value.id) {
        updateCharacter()
      }
      else {
        createCharacter()
      }
    })

    function createCharacter() {
      try {
        charactersService.createTempCharacter(editable.value)
      }
      catch (error) {
        Pop.error(error.message, '[CREATING CHARACTER]')
      }
    }

    function updateCharacter() {
      try {
        charactersService.updateTempCharacter(editable.value)
      }
      catch (error) {
        Pop.error(error.message, '[UPDATING CHARACTER]')
      }
    }

    return {
      editable,
      attributes,
      points,

      changeAtt(att, change) {
        editable.value[att] += change

        if (editable.value[att] > 13) {
          change *= 2
        }
        else if (editable.value[att] == 13 && change == -1) {
          change *= 2
        }
        points.value -= change
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .manual-col>* {
    width: 33%;
  }
</style>