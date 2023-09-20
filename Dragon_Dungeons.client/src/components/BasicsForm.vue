<template>
  <p class="fs-1 fw-bold m-3 py-2">Basics</p>

  <form @submit.prevent="changeCharPage()" class="row g-3 bg-dark text-dark m-3 p-3 rounded elevation-5">
    <div class="col-md-12 form-floating">
      <input v-model="editable.name" type="text" class="form-control" id="name" minlength="3" maxlength="100" placeholder="Name..." required>
      <label for="name">Character Name:</label>
    </div>

    <div class="col-md-4 form-group">
      <label class="text-light" for="class">
        Class:
        <router-link :to="{ name: 'Info', params: { infoId: 'classes', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select @change="areYouSure('class')" v-model="editable.class" id="class" class="form-select" required>
        <option v-for="c in dndClass" :key="c">{{ c }}</option>
      </select>
    </div>

    <div class="col-md-4 form-group">
      <label class="text-light" for="race">
        Race:
        <router-link :to="{ name: 'Info', params: { infoId: 'races', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select @change="areYouSure('race')" v-model="editable.race" id="race" class="form-select" required>
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
      <button type="submit" class="btn btn-primary">Save</button>
    </div>
  </form>
</template>

<script>
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const dndClass = ['Barbarian', 'Bard', 'Cleric', 'Druid', 'Fighter', 'Monk', 'Paladin', 'Ranger', 'Rogue', 'Sorcerer', 'Warlock', 'Wizard']
    const race = ['Dragonborn', 'Dwarf', 'Elf', 'Gnome', 'Half-Elf', 'Half-Orc', 'Halfling', 'Human', 'Tiefling']
    const alignment = ['Chaotic Evil', 'Chaotic Good', 'Chaotic Neutral', 'Lawful Evil', 'Lawful Good', 'Lawful Neutral', 'Neutral', 'Neutral Evil', 'Neutral Good']

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
    })

    onBeforeUnmount(() => {
      handleSave()
    })

    async function handleSave() {
      if (editable.value.race) {
        await getRace()
      }

      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else {
        saveCharacter()
      }
    }

    async function getRace() {
      try {
        const race = await infosService.getInfoDetails(`api/races/${editable.value.race.toLowerCase().replaceAll(' ', '-')}`, false)
        editable.value.bonus = {}
        race.ability_bonuses.forEach(b => editable.value.bonus[b.ability_score.index] = b.bonus)
      } catch (error) {
        Pop.error(error.message, '[GETTING RACE]')
      }
    }

    function saveCharacter() {
      editable.value.creator = AppState.account
      charactersService.saveCharacter(editable.value)
    }

    return {
      editable,
      dndClass,
      race,
      alignment,

      async areYouSure(type) {
        if (!AppState.tempCharacter[type]) {
          return
        }
        const isSure = await Pop.confirm('If you change this all progress will be reset!! Continue?')

        if (!isSure) {
          editable.value[type] = AppState.tempCharacter[type]
          return
        }
        const temp = editable.value[type]
        AppState.charPage = 0
        editable.value = {}
        AppState.tempCharacter = {}
        editable.value[type] = temp
      },

      changeCharPage() {
        charactersService.changeCharPage(0)
        router.push({ name: 'Character', params: { characterId: 'features' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>