<template>
  <section class="row mx-0 my-3 p-3 bg-dark rounded elevation-5">
    <div v-for="a in attributes" :key="a" class="col-4 col-sm-2 text-center">
      <p class="fs-5 fw-bold">{{ a.toUpperCase() }}:</p>
      <div class="d-flex justify-content-around align-items-center">
        <p class="fs-5">{{ characterProp[a] }}</p>
        <p title="Modifier">{{ Math.floor((characterProp[a] - 10) / 2) }}</p>
      </div>
    </div>
  </section>

  <section class="row mx-0 my-3 p-3 bg-dark rounded elevation-5">
    <div v-for="s in Object.entries(skills)" :key="s" class="col-12 col-sm-6 col-md-12 col-lg-6 py-1">
      <section v-if="characterProp.skills.includes(s[0])" class="row justify-content-between align-items-center">
        <i class="col-2 text-center mdi mdi-circle"></i>
        <p class="col-6 text-end">{{ s[0] }}</p>
        <div class="col-4 d-flex">
          <p class="w-50 text-center text-uppercase">{{ s[1] }}</p>
          <p title="Modifier" class="w-50 text-center">{{ Math.floor((characterProp[s[1]] - 10) / 2) + characterProp.bonus }}</p>
        </div>
      </section>
      <section v-else class="row justify-content-between align-items-center">
        <i class="col-2 text-center mdi mdi-circle-outline"></i>
        <p class="col-6 text-end">{{ s[0] }}</p>
        <div class="col-4 d-flex">
          <p class="w-50 text-center text-uppercase">{{ s[1] }}</p>
          <p title="Modifier" class="w-50 text-center">{{ Math.floor((characterProp[s[1]] - 10) / 2) }}</p>
        </div>
      </section>
    </div>
  </section>
</template>

<script>
import { AppState } from '../AppState.js'
import { Character } from '../models/Character.js'

export default {
  props: {
    characterProp: {
      type: Character,
      required: true
    }
  },

  setup() {
    const attributes = AppState.attributes
    const skills = {
      'Acrobatics': 'dex',
      'Animal Handling': 'wis',
      'Arcana': 'int',
      'Athletics': 'str',
      'Deception': 'cha',
      'History': 'int',
      'Insight': 'wis',
      'Intimidation': 'cha',
      'Investigation': 'int',
      'Medicine': 'wis',
      'Nature': 'int',
      'Perception': 'wis',
      'Performance': 'cha',
      'Persuasion': 'cha',
      'Religion': 'int',
      'Sleight of Hand': 'dex',
      'Stealth': 'dex',
      'Survival': 'wis'
    }

    return {
      attributes,
      skills
    }
  }
}
</script>

<style lang="scss" scoped></style>