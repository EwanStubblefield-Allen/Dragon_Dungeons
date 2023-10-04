<template>
  <div class="card elevation-5">
    <img :src="characterProp.picture.url" class="card-img-top" :alt="characterProp.name">
    <div class="card-body p-2">
      <h5 class="card-title text-center text-uppercase fs-3">{{ characterProp.name }}</h5>
      <div class="d-flex justify-content-around pb-2">
        <p class="fs-5">{{ characterProp.race }} {{ characterProp.class }}</p>
        <p class="fs-5">Level: {{ characterProp.level }}</p>
      </div>
      <div class="d-flex justify-content-end align-items-center">
        <router-link :to="{ name: 'Character', params: { characterId: characterProp.id } }" class="btn btn-primary elevation-5">Character Details</router-link>
      </div>
    </div>
  </div>
</template>

<script>
import { Character } from '../models/Character.js'

export default {
  props: {
    characterProp: {
      type: Character,
      requited: true
    }
  },

  setup() {
    return {}
  }
}
</script>

<style lang="scss" scoped></style>