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

  <section v-if="selectable == 1" class="row mx-0 mb-2 p-2 bg-dark rounded-bottom elevation-5">
    <div class="col-12 col-sm-8 col-md-12 col-lg-8">
      <p>Weapons</p>
      <hr class="my-1">
      <section class="row">
        <p class="col-4">Type/Name</p>
        <p class="col-4">Range</p>
        <p class="col-4">Damage</p>
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

  <section v-else-if="selectable == 2" class="row mx-0 mb-2 p-2 bg-dark rounded-bottom elevation-5">
    <section class="row">
      <p class="col-4">Type/Name</p>
      <p class="col-4">Range</p>
      <p class="col-4">Damage</p>
    </section>
  </section>

  <section v-else-if="selectable == 3" class="row mx-0 mb-2 p-2 bg-dark rounded-bottom elevation-5">
    <section class="row">
      <p class="col-4">Type/Name</p>
      <p class="col-4">Range</p>
      <p class="col-4">Damage</p>
    </section>
  </section>
</template>

<script>
import { computed, onMounted, ref } from 'vue'
import { Character } from '../models/Character.js'
import { infosService } from '../services/InfosService.js'
import { charactersService } from '../services/CharactersService.js'
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

        if (character.armor?.index) {
          AppState.equipment.armor = await infosService.getInfoDetails(character.armor.url, false)
        }
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
      equipment: computed(() => AppState.equipment),

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

<style lang="scss" scoped></style>