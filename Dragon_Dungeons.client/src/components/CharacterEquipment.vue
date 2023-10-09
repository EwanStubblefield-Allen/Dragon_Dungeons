<template>
  <ul class="nav nav-tabs">
    <li class="nav-item">
      <p @click="selectable = 1" :class="{ 'active elevation-5': selectable == 1 }" class="nav-link selectable text-dark fw-bold">Equipment</p>
    </li>

    <li v-if="characterProp.cantrips" class="nav-item">
      <p @click="selectable = 2" :class="{ 'active elevation-5': selectable == 2 }" class="nav-link selectable text-dark fw-bold">Cantrips</p>
    </li>

    <li v-if="characterProp.spells" class="nav-item">
      <p @click="selectable = 3" :class="{ 'active elevation-5': selectable == 3 }" class="nav-link selectable text-dark fw-bold">Spells</p>
    </li>
  </ul>

  <section class="row mx-0 mb-2 p-2 bg-dark rounded-bottom elevation-5">
    <div class="col-12 col-sm-6 col-md-12 col-lg-6">
      <p>Weapons</p>
      <hr class="my-2">
      <div class="px-2">
        <p v-for="w in equipment.weapons" :key="w.index">{{ w.name }}</p>
      </div>
    </div>
  </section>
</template>

<script>
import { computed, onMounted, ref } from 'vue'
import { Character } from '../models/Character.js'
import { infosService } from '../services/InfosService.js'
import { AppState } from '../AppState.js'
import Pop from '../utils/Pop.js'

export default {
  props: {
    characterProp: {
      type: Character,
      required: true
    }
  },

  setup() {
    const selectable = ref(1)

    onMounted(() => {
      getEquipment()
    })

    async function getEquipment() {
      try {
        const character = AppState.activeCharacter
        AppState.equipment.armor = await infosService.getInfoDetails(character.armor.url, false)
        character.weapons.forEach(async w => {
          const weapon = await infosService.getInfoDetails(w.url, false)
          AppState.equipment.weapons.push(weapon)
        })
      } catch (error) {
        Pop.error(error.message, '[GETTING EQUIPMENT]')
      }
    }

    return {
      selectable,
      equipment: computed(() => AppState.equipment)
    }
  }
}
</script>

<style lang="scss" scoped></style>