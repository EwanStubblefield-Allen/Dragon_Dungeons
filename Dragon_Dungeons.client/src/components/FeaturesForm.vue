<template>
  <div class="m-3">
    <p class="fs-1 fw-bold py-2">Features</p>

    <ul class="nav nav-tabs">
      <li class="nav-item">
        <p @click="selectable = 1" :class="{ 'active elevation-5': selectable == 1 }" class="nav-link selectable text-dark fw-bold">Physical</p>
      </li>

      <li class="nav-item">
        <p @click="selectable = 2" :class="{ 'active elevation-5': selectable == 2 }" class="nav-link selectable text-dark fw-bold">Other</p>
      </li>
    </ul>

    <div class="bg-dark p-3 rounded-bottom elevation-5">
      <form @submit.prevent="changeTab()" v-if="selectable == 1" class="row g-3">
        <div class="col-md-4 form-group">
          <label for="age">Age:</label>
          <input v-model="editable.age" type="number" class="form-control" id="age" min="1" placeholder="Age..." required>
        </div>
        <div class="col-md-4 form-group">
          <label for="height">Height:</label>
          <div class="input-group">
            <input v-model="editable.feet" type="number" class="form-control" id="height" min="1" max="10" placeholder="Feet..." required>
            <input v-model="editable.inches" type="number" class="form-control" id="height" min="0" max="11" placeholder="Inches..." required>
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

      <form v-else @submit.prevent="" class="row g-3">
        <div class="col-12 form-group">
          <label for="features">Features:</label>
          <textarea v-model="editable.features" class="form-control" id="features" rows="10" minlength="1" maxlength="1000" placeholder="Features..." required></textarea>
        </div>
        <div class="col-12 text-end">
          <router-link :to="{ name: 'Character', params: { characterId: 'background' } }">
            <button type="submit" class="btn btn-primary">Save</button>
          </router-link>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { AppState } from '../AppState.js'
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({})
    const selectable = ref(1)

    onBeforeUnmount(() => {
      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else if (editable.value.id) {
        updateCharacter()
      } else {
        createCharacter()
      }
    })

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
      let textElement = document.getElementById('age')
      textElement.focus()
    })

    function createCharacter() {
      try {
        charactersService.createTempCharacter(editable.value)
      } catch (error) {
        Pop.error(error.message, '[CREATING CHARACTER]')
      }
    }

    function updateCharacter() {
      try {
        charactersService.updateTempCharacter(editable.value)
      } catch (error) {
        Pop.error(error.message, '[UPDATING CHARACTER]')
      }
    }

    return {
      editable,
      selectable,

      changeTab() {
        selectable.value++
        setTimeout(() => {
          let textElement = document.getElementById('features')
          textElement.focus()
          textElement.setSelectionRange(textElement.value.length, textElement.value.length)
        }, 1)
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>