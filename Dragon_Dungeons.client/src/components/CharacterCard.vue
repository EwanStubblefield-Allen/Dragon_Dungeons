<template>
  <div class="card mb-2 elevation-5">
    <div class="row g-0">
      <div class="col-md-4">
        <img :src="characterProp.picture.url ?? characterProp.picture" class="vh-md-25 w-100 rounded" :alt="characterProp.name">
      </div>
      <div class="col-md-8">
        <div class="card-body d-flex flex-column justify-content-between p-2 h-100">
          <h3 class="text-truncate fw-bold">{{ characterProp.name }}</h3>
          <div class="d-flex justify-content-around pb-2">
            <p class="fs-5">{{ characterProp.race }} {{ characterProp.class }}</p>
            <p v-if="characterProp.level" class="fs-5">Level: {{ characterProp.level }}</p>
          </div>
          <div class="d-flex justify-content-end align-items-center">
            <router-link :to="{ name: 'Character', params: { characterId: characterProp.characterId ?? characterProp.id } }" class="btn btn-primary elevation-5">Character Details</router-link>
          </div>
        </div>
      </div>
    </div>
    <i v-if="characterProp.creatorId == account.id || campaign?.creatorId == account.id || !characterProp.creatorId" @click="removeCharacter()" class="mdi mdi-delete fs-5 text-danger selectable"></i>
  </div>
</template>

<script>
import { computed } from 'vue'
import { AppState } from '../AppState.js'
import { charactersService } from '../services/CharactersService.js'
import { npcsService } from '../services/NpcsService.js'
import { playersService } from '../services/PlayersService.js'
import Pop from '../utils/Pop.js'

export default {
  props: {
    characterProp: {
      type: Object,
      requited: true
    },
    locationProp: {
      type: String,
      required: true
    }
  },

  setup(props) {
    return {
      account: computed(() => AppState.account),
      campaign: computed(() => AppState.activeCampaign),

      async removeCharacter() {
        try {
          const character = props.characterProp
          const location = props.locationProp
          let characterToDelete

          if (location == 'npc') {
            const isSure = await Pop.confirm(`Are you sure you want to remove ${character.name} from this campaign?`)

            if (!isSure) {
              return
            }
            characterToDelete = await npcsService.removeNpc(character)
          } else {
            const res = await Pop.question('Delete Character?', `To delete the character: "${character.name}", type the name to confirm.`)

            if (!res) {
              return
            }

            if (res != character.name) {
              return Pop.toast('Character name does not match!')
            }

            if (location == 'account') {
              characterToDelete = await charactersService.removeCharacter(character)
              AppState.campaigns = AppState.campaigns.filter(c => c.players[0] != characterToDelete.id)
            } else {
              characterToDelete = await playersService.removePlayer(character)
            }
          }
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
  img {
    object-fit: cover;
    object-position: center;
  }

  i {
    position: absolute;
    top: 2px;
    right: 5px;
  }
</style>