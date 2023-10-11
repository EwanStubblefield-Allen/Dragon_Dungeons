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

  <section v-if="selectable == 1" class="row mx-0 mb-2 p-2 bg-dark rounded-bottom elevation-5 overflow">
    <div class="col-12 col-sm-8 col-md-12 col-lg-8">
      <p>Weapons</p>
      <hr class="my-1">
      <section class="row">
        <p class="col-4">Type/Name:</p>
        <p class="col-4">Range:</p>
        <p class="col-4">Damage:</p>
      </section>
      <section v-for="(w, index) in equipment.weapons" :key="w.index" class="row align-items-center px-2">
        <hr class="my-1">
        <div class="col-4 d-flex">
          <i v-if="w.weapon_range == 'Melee'" class="mdi mdi-fencing pe-1" title="Melee"></i>
          <i v-else class="mdi mdi-bow-arrow pe-1" title="Range"></i>
          <p>{{ w.name }}</p>
        </div>
        <p class="col-4">
          {{ w.range.normal }}ft.
          <span v-if="w.range.long">/ {{ w.range.long }}ft.</span>
        </p>
        <p class="col-2 no-select" :title="w.damage.damage_type.name">{{ w.damage.damage_dice }}</p>
        <div class="col-2 d-flex justify-content-center">
          <i @click="unEquipItem(index)" class="mdi mdi-cancel text-danger selectable" title="Unequip"></i>
        </div>
      </section>
    </div>
    <div class="col-12 col-sm-4 col-md-12 col-lg-4">
      <p>Armor</p>
      <hr class="my-1">
      <div v-if="equipment.armor?.index">
        <u class="fw-bold">
          {{ equipment.armor.name }}
          <span v-if="equipment.armor.stealth_disadvantage" class="mdi mdi-ghost-off fs-5" title="Stealth Disadvantage"></span>
        </u>
        <p class="px-2">Category: {{ equipment.armor.armor_category }}</p>
        <p class="px-2">AC: {{ equipment.armor.armor_class.base }}</p>
        <button @click="unEquipItem()" class="btn btn-danger my-2" type="button">Unequip</button>
      </div>
      <p v-else>No Armor Equipped</p>
    </div>
  </section>

  <div v-else class="mb-2 p-2 bg-dark rounded-bottom elevation-5 overflow">
    <section class="row justify-content-around align-items-center mx-0 px-3">
      <p class="col-6 col-xl-3">Name:</p>
      <p class="col-3 col-xl-2 text-xl-end">Time:</p>
      <p class="col-3 col-xl-2 text-xl-end">Damage:</p>
      <p class="col-4 col-xl-2 text-center text-xl-start">Range:</p>
      <p class="col-4 col-xl-3 text-center text-xl-start">Other:</p>
    </section>
    <section v-for="c in selectable == 2 ? equipment.cantrips : equipment.spells" :key="c.index" class="row justify-content-around align-items-center mx-0 px-3">
      <hr class="my-1">
      <div class="col-6 col-xl-4 d-flex align-items-center">
        <i v-if="c.concentration" class="mdi mdi-meditation fs-6 pe-1" title="Concentration"></i>
        <i v-if="c.ritual" class="mdi mdi-candelabra-fire fs-6 pe-1" title="Ritual"></i>
        <i v-if="c.area_of_effect" class="mdi mdi-axis-arrow fs-6 pe-1" :title="`AOE: ${c.area_of_effect.size}ft. ${c.area_of_effect.type}`"></i>
        <p>{{ c.name }}</p>
      </div>
      <div class="col-3 col-xl-2">
        <p class="no-select" :title="c.casting_time">{{ c.casting_time.replace(/ .*/g, c.casting_time.charAt(2).toUpperCase()) }}</p>
      </div>
      <div class="col-3 col-xl-1">
        <p v-if="c.damage" class="no-select" :title="c.damage.damage_type.name">{{ handleDamage(c.damage) }}</p>
        <p v-else>--</p>
      </div>
      <div class="col-4 col-xl-2 text-center text-xl-start">
        <p>{{ c.range.replace(' feet', 'ft.') }}</p>
      </div>
      <div class="col-4 col-xl-3 d-flex justify-content-center justify-content-xl-start">
        <p v-for="component in c.components" :key="component" class="pe-1 no-select" :title="component == 'V' ? 'Verbal' : component == 'S' ? 'Somatic' : 'Material'">{{ component }},</p>
        <p v-if="c.duration" class="no-select" :title="c.duration">D: {{ c.duration.replace('Instantaneous', 'Inst').replace(/ .*/g, c.duration.charAt(2).toUpperCase()) }}</p>
      </div>
    </section>
  </div>
</template>

<script>
import { computed, onMounted, ref } from 'vue'
import { Character } from '../models/Character.js'
import { infosService } from '../services/InfosService.js'
import { charactersService } from '../services/CharactersService.js'
import { AppState } from '../AppState.js'
import { saveState } from '../utils/Store.js'
import Pop from '../utils/Pop.js'

export default {
  props: {
    characterProp: {
      type: Character,
      required: true
    }
  },

  setup(props) {
    const selectable = ref(1)

    onMounted(() => {
      getEquipment()
    })

    async function getEquipment() {
      try {
        const character = AppState.activeCharacter

        if (character.id == AppState.equipment.id) {
          return
        }
        AppState.equipment = { id: character.id, weapons: [], cantrips: [], spells: [] }

        if (character.armor?.index) {
          AppState.equipment.armor = await infosService.getInfoDetails(character.armor.url, false)
        }
        AppState.equipment.weapons = await Promise.all(character.weapons.map(async w => {
          return await infosService.getInfoDetails(w.url, false)
        }))
        AppState.equipment.cantrips = await Promise.all(character.cantrips.map(async c => {
          return await infosService.getInfoDetails(c.url, false)
        }))
        AppState.equipment.spells = await Promise.all(character.spells.map(async s => {
          return await infosService.getInfoDetails(s.url, false)
        }))
        saveState('equipment', AppState.equipment)
      } catch (error) {
        Pop.error(error.message, '[GETTING EQUIPMENT]')
      }
    }

    return {
      selectable,
      equipment: computed(() => AppState.equipment),

      handleDamage(damage) {
        let tempLevel = props.characterProp.level

        while (!damage.damage_at_character_level[tempLevel]) {
          tempLevel--
        }
        return damage.damage_at_character_level[tempLevel]
      },

      async unEquipItem(index = -1) {
        try {
          await charactersService.unEquipItem(index)
        } catch (error) {
          Pop.error(error.message, '[UNEQUIPPING ITEM]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  @media screen and (min-width: 1200px) {
    .overflow {
      overflow-y: auto;
      height: calc(var(--main-height) - 371px);
    }
  }
</style>