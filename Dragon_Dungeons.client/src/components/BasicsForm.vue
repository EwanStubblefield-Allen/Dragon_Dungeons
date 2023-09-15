<template>
  <p class="fs-1 fw-bold m-3 py-2">Basics</p>

  <form @submit.prevent="" class="row g-3 bg-dark text-dark m-3 p-3 rounded elevation-5">
    <div class="col-md-12 form-floating">
      <input v-model="editable.name" type="text" class="form-control" id="name" minlength="3" maxlength="100" placeholder="Name..." required>
      <label for="name">Character Name:</label>
    </div>
    <div class="col-md-4 form-group">
      <label class="text-light" for="class">
        Class:
        <router-link :to="{ name: 'Info', params: { infoId: 'classes', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select v-model="editable.class" id="class" class="form-select" required>
        <option v-for="c in dndClass" :key="c">{{ c }}</option>
      </select>
    </div>
    <div class="col-md-4 form-group">
      <label class="text-light" for="race">
        Race:
        <router-link :to="{ name: 'Info', params: { infoId: 'races', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select v-model="editable.race" id="race" class="form-select" required>
        <option v-for="r in race" :key="r">{{ r }}</option>
      </select>
    </div>
    <div class="col-md-4 form-group">
      <label class="text-light" for="alignment">
        Alignment:
        <router-link :to="{ name: 'Info', params: { infoId: 'alignments', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select v-model="editable.alignment" id="alignment" class="form-select" required>
        <option v-for="a in alignment" :key="a">{{ a }}</option>
      </select>
    </div>
    <div class="col-12 text-end">
      <router-link :to="{ name: 'Character', params: { characterId: 'features' } }">
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
    const dndClass = ['Barbarian', 'Bard', 'Cleric', 'Druid', 'Fighter', 'Monk', 'Paladin', 'Ranger', 'Rogue', 'Sorcerer', 'Warlock', 'Wizard']
    const race = ['Dragonborn', 'Dwarf', 'Elf', 'Gnome', 'Half-Elf', 'Half-Orc', 'Halfling', 'Human', 'Tiefling']
    const alignment = ['Chaotic Evil', 'Chaotic Good', 'Chaotic Neutral', 'Lawful Evil', 'Lawful Good', 'Lawful Neutral', 'Neutral', 'Neutral Evil', 'Neutral Good']

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
      dndClass,
      race,
      alignment
    }
  }
}
</script>

<style lang="scss" scoped></style>