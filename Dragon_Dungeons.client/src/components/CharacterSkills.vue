<template>
  <section class="row mx-0 my-2 p-2 bg-dark rounded elevation-5">
    <div v-for="a in attributes" :key="a" class="col-4 col-sm-2 text-center">
      <p class="fs-5 fw-bold">{{ a.toUpperCase() }}:</p>
      <div class="d-flex justify-content-around align-items-center">
        <p class="fs-5">{{ characterProp[a] }}</p>
        <p title="Modifier">{{ Math.floor((characterProp[a] - 10) / 2) }}</p>
      </div>
    </div>
  </section>

  <section class="row mx-0 my-3 p-2 bg-dark rounded elevation-5">
    <div v-for="s in Object.entries(skills)" :key="s" class="col-12 col-sm-6 col-md-12 col-xl-6 py-1">
      <div v-if="characterProp.skills.includes(s[0])" class="d-flex justify-content-between align-items-center">
        <i class="mdi mdi-circle"></i>
        <section class="row align-items-center flex-grow-1">
          <p class="col-7 text-end">{{ s[0] }}</p>
          <p class="col-3 text-center text-uppercase">{{ s[1] }}</p>
          <p title="Modifier" class="col-2 text-end">{{ Math.floor((characterProp[s[1]] - 10) / 2) + characterProp.bonus }}</p>
        </section>
      </div>
      <div v-else class="d-flex justify-content-between align-items-center">
        <i class="mdi mdi-circle-outline"></i>
        <section class="row align-items-center flex-grow-1">
          <p class="col-7 text-end">{{ s[0] }}</p>
          <p class="col-3 text-center text-uppercase">{{ s[1] }}</p>
          <p title="Modifier" class="col-2 text-end">{{ Math.floor((characterProp[s[1]] - 10) / 2) }}</p>
        </section>
      </div>
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