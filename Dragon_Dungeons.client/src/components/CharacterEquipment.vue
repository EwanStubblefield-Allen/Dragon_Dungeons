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
    <div class="col-6">
      <p>Weapons</p>
      <hr class="my-2">
    </div>
  </section>
</template>

<script>
import { ref } from 'vue'
import { Character } from '../models/Character.js'

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
      selectable
    }
  }
}
</script>

<style lang="scss" scoped></style>