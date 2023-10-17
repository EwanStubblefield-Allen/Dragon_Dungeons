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
          <abbr v-if="w.weapon_range == 'Melee'" class="mdi mdi-fencing pe-1" title="Melee"></abbr>
          <abbr v-else class="mdi mdi-bow-arrow pe-1" title="Range"></abbr>
          <p>{{ w.name }}</p>
        </div>
        <p class="col-4">
          {{ w.range.normal }}ft.
          <span v-if="w.range.long">/ {{ w.range.long }}ft.</span>
        </p>
        <abbr class="col-2 no-select" :title="w.damage.damage_type.name">{{ w.damage.damage_dice }}</abbr>
        <div class="col-2 d-flex justify-content-center">
          <abbr @click="unEquipItem(index)" class="mdi mdi-cancel text-danger selectable" title="Unequip"></abbr>
        </div>
      </section>
    </div>
    <div class="col-12 col-sm-4 col-md-12 col-lg-4">
      <p>Armor</p>
      <hr class="my-1">
      <div v-if="equipment.armor?.index">
        <u class="fw-bold">
          {{ equipment.armor.name }}
          <abbr v-if="equipment.armor.stealth_disadvantage" class="mdi mdi-ghost-off fs-5" title="Stealth Disadvantage"></abbr>
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
      <p class="col-6 col-xl-3 text-center text-xl-start">Other:</p>
    </section>
    <div v-if="selectable == 2">
      <section v-for="cantrip in equipment.cantrips" :key="cantrip.index" class="row justify-content-around align-items-center mx-0 px-3">
        <CharacterMagic :magicProp="cantrip" />
      </section>
    </div>
    <div v-else-if="selectable == 3">
      <div v-for="level in Object.entries(characterProp.casting)" :key="level" class="py-2">
        <CharacterSpells :level="level" />
      </div>
      <div class="text-end">
        <button @click="updateSpells(characterProp.spells)" class="btn btn-primary" type="button">Save</button>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, ref } from 'vue'
import { Character } from '../models/Character.js'
import { charactersService } from '../services/CharactersService.js'
import { AppState } from '../AppState.js'
import { saveState } from '../utils/Store.js'
import CharacterMagic from './CharacterMagic.vue'
import CharacterSpells from './CharacterSpells.vue'
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

    return {
      selectable,
      equipment: computed(() => AppState.equipment),

      async unEquipItem(index = -1) {
        try {
          await charactersService.unEquipItem(index)
          Pop.toast('Unequipped Item!')
        }
        catch (error) {
          Pop.error(error.message, '[UNEQUIPPING ITEM]')
        }
      },

      async updateSpells(spells) {
        try {
          await charactersService.updateCharacter({ spells: spells })
          saveState('equipment', AppState.equipment)
        } catch (error) {
          Pop.error(error.message, '[UPDATING SPELLS]')
        }
      }
    }
  },
  components: { CharacterMagic, CharacterSpells }
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