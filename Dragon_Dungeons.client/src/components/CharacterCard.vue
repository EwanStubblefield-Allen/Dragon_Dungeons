<template>
  <div class="card elevation-5">
    <img :src="characterProp.picture.url" class="card-img-top position-relative" :alt="characterProp.name">
    <i @click="removeCharacter()" class="mdi mdi-delete fs-5 text-danger selectable"></i>
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
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

export default {
  props: {
    characterProp: {
      type: Character,
      requited: true
    }
  },

  setup(props) {
    return {
      async removeCharacter() {
        try {
          const character = props.characterProp
          const res = await Pop.question('Delete Character?', `To delete the character: "${character.name}", type the name to confirm.`)

          if (res != character.name) {
            return Pop.toast('Character name does not match!')
          }
          const characterToDelete = await charactersService.removeCharacter(character)
          Pop.toast(`${characterToDelete.name} was deleted!`)
        } catch (error) {
          Pop.error(error.message, '[DELETING CHARACTER]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  i {
    position: absolute;
    top: 2px;
    right: 5px;
  }
</style>