<template>
  <div class="card mb-2">
    <div class="row g-0">
      <div class="col-md-4">
        <img :src="characterProp.picture.url ?? characterProp.picture" class="vh-md-25 w-100 rounded" :alt="characterProp.name">
      </div>
      <div class="col-md-8">
        <div class="card-body d-flex flex-column justify-content-between p-2 h-100">
          <h3 class="card-title fw-bold">{{ characterProp.name }}</h3>
          <div class="d-flex justify-content-around pb-2">
            <p class="fs-5">{{ characterProp.race }} {{ characterProp.class }}</p>
            <p class="fs-5">Level: {{ characterProp.level }}</p>
          </div>
          <div class="d-flex justify-content-end align-items-center">
            <router-link :to="{ name: 'Character', params: { characterId: characterProp.id } }" class="btn btn-primary elevation-5">Character Details</router-link>
          </div>
        </div>
      </div>
    </div>
    <i @click="removeCharacter()" class="mdi mdi-delete fs-5 text-danger selectable"></i>
  </div>
</template>

<script>
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

export default {
  props: {
    characterProp: {
      type: Object,
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