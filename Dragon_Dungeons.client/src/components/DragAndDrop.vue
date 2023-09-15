<template>
  <section class="row g-3">
    <div class="col-12 fs-3 fw-bold text-end">
      <button @click="randomizeAtt()" class="btn btn-primary">Randomize</button>
    </div>

    <div class="col-12">
      <draggable v-model="editable" class="row dragArea g-3">
        <div class="col-12 col-sm-6 col-md-3 col-lg-2 draggable" v-for="(a, i) in attributes" :key="a">
          <p class="fs-5">{{ a.toUpperCase() }}</p>
          <div class="d-flex justify-content-around align-items-center p-2">
            <input v-model="editable[i]" type="number" class="form-control fs-4" style="width: 66.66% !important;" min="1" max="20" required>
            <p class="fs-3 text-center">{{ Math.floor((editable[i] - 10) / 2) }}</p>
          </div>
          <div class="d-flex justify-content-around align-items-center px-2 fs-5">
            <p class="pe-2">Racial Bonus</p>
            <p>+0</p>
          </div>
        </div>
      </draggable>
    </div>

    <div class="col-12 text-end">
      <router-link :to="{ name: 'Character', params: { characterId: 'proficiencies' } }">
        <button type="submit" class="btn btn-primary">Save</button>
      </router-link>
    </div>
  </section>
</template>

<script>
import { defineComponent, onBeforeUnmount, onMounted, ref } from 'vue'
import { VueDraggableNext } from 'vue-draggable-next'
import { AppState } from '../AppState.js'
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

export default defineComponent({
  data() {
    const editable = ref([])
    const attributes = ['strength', 'dexterity', 'constitution', 'intelligence', 'wisdom', 'charisma']

    onMounted(() => {
      let temp = AppState.tempCharacter
      editable.value.id = temp?.id

      for (let a in attributes) {
        let att = attributes[a]

        if (temp ? !temp[att] : true) {
          editable.value[a] = 8
        } else {
          editable.value[a] = temp[att]
        }
      }
    })

    onBeforeUnmount(() => {
      let temp = AppState.tempCharacter

      if (!temp.manual) {
        return
      }

      for (let a in attributes) {
        temp[attributes[a]] = editable.value[a]
      }
      temp.manual = true

      if (temp.id) {
        updateCharacter(temp)
      }
      else {
        createCharacter(temp)
      }
    })

    function createCharacter(temp) {
      try {
        charactersService.createTempCharacter(temp)
      }
      catch (error) {
        Pop.error(error.message, '[CREATING CHARACTER]')
      }
    }

    function updateCharacter(temp) {
      try {
        charactersService.updateTempCharacter(temp)
      }
      catch (error) {
        Pop.error(error.message, '[UPDATING CHARACTER]')
      }
    }

    return {
      editable,
      attributes,
      enabled: true,
      dragging: false,

      randomizeAtt() {
        for (let e in editable.value) {
          editable.value[e] = Math.ceil(Math.random() * 20)
        }
      }
    }
  },
  components: {
    draggable: VueDraggableNext
  }
})
</script>

<style lang="scss" scoped>
  .draggable {
    cursor: grab;
    border: 2px dashed white;
  }
</style>