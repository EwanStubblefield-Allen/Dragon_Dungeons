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
      <select v-model="editable.class" @change="checkSpells(editable.class)" id="class" class="form-select" required>
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
      <button type="submit" class="btn btn-primary">Save</button>
    </div>
  </form>

  <div v-if="options.length" class="m-3">
    <ul class="nav nav-tabs">
      <li v-if="cantrips.length" class="nav-item">
        <p @click="selectable = [0, 'cantrips']" :class="{ 'active elevation-5': !selectable[0] }" class="nav-link selectable text-dark fw-bold">Cantrips</p>
      </li>
      <li v-if="spells.length" class="nav-item">
        <p @click="selectable = [1, 'spells']" :class="{ 'active elevation-5': selectable[0] }" class="nav-link selectable text-dark fw-bold">Spells</p>
      </li>
    </ul>

    <div class="bg-dark p-3 rounded-bottom elevation-5">
      <div v-for="(opt, index) in options" :key="opt">
        <div v-if="selectable[0] == index" class="g-3">
          <p class="fs-3 fw-bold">Choose {{ opt.choose }} {{ selectable[1] }}
            <router-link :to="{ name: 'Info', params: { infoId: selectable[1], infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
          </p>
          <section class="row p-2">
            <p @click="addPro(o.item.name, opt.choose)" v-for="o in opt.from.options" :key="o.item.index" :class="{ 'bg-light text-dark elevation-5': editable[selectable[1]]?.includes(o.item.name) }" class="col-6 col-sm-4 col-md-3 col-lg-2 p-2 text-center selectable rounded">{{ o.item.name }}</p>
          </section>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'
import { logger } from '../utils/Logger.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const cantrips = ref([])
    const spells = ref([])
    const dndClass = ['Barbarian', 'Bard', 'Cleric', 'Druid', 'Fighter', 'Monk', 'Paladin', 'Ranger', 'Rogue', 'Sorcerer', 'Warlock', 'Wizard']
    const race = ['Dragonborn', 'Dwarf', 'Elf', 'Gnome', 'Half-Elf', 'Half-Orc', 'Halfling', 'Human', 'Tiefling']
    const alignment = ['Chaotic Evil', 'Chaotic Good', 'Chaotic Neutral', 'Lawful Evil', 'Lawful Good', 'Lawful Neutral', 'Neutral', 'Neutral Evil', 'Neutral Good']

    onBeforeUnmount(() => {
      handleSave()
    })

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
    })

    async function handleSave() {
      if (editable.value.race) {
        await getRace()
      }

      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else if (editable.value.id) {
        updateCharacter()
      } else {
        createCharacter()
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
      alignment,
      spells,
      cantrips,

      async checkSpells(selectedClass) {
        const level = await infosService.getInfoDetails(`api/classes/${selectedClass.toLowerCase().replaceAll(' ', '-')}/levels/1`, false)

        // if (level.spellcasting) {

        // }
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