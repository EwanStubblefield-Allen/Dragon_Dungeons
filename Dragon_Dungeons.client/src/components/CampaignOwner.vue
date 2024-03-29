<template>
  <div class="d-flex flex-column justify-content-between h-md-50">
    <div class="py-2">
      <div class="d-flex justify-content-between align-items-center">
        <div v-if="campaignProp.creatorId == account.id" class="d-flex align-items-center">
          <p class="fs-3 fw-bold">Players:</p>
          <i @click="copyCode(campaignProp.id)" class="mdi mdi-content-copy px-1 fs-5 selectable" title="Copy Campaign Code"></i>
          <a class="mdi mdi-share-variant px-1 fs-5 text-dark selectable" :href="`mailto:?subject=Dungeons and Dragons Campaign&body=Join my Dnd Campaign! Code: ${campaignProp.id}`" title="Share Campaign Code Via Email"></a>
        </div>
        <p v-else class="fs-3 fw-bold">My Player:</p>

        <button v-if="campaignProp.creatorId == account.id && campaignProp.players.length" type="button" class="btn btn-primary elevation-5" data-bs-toggle="modal" data-bs-target="#award">Award Players</button>
      </div>

      <div v-if="campaignProp.players.length" class="row overflow-auto players">
        <div v-for="p in campaignProp.players" :key="p.id" class="col-12 col-sm-6 col-md-12">
          <CharacterCard :characterProp="p" locationProp="player" />
        </div>
      </div>
      <div v-else class="d-flex justify-content-center">
        <p class="fs-3">No current players in this campaign!</p>
      </div>
    </div>

    <ul class="nav nav-tabs">
      <li class="nav-item">
        <p @click="selectable = 1" :class="{ 'active elevation-5': selectable == 1 }" class="nav-link selectable text-dark fw-bold">Chat</p>
      </li>

      <li v-if="campaignProp.creatorId == account.id" class="nav-item">
        <p @click="selectable = 2" :class="{ 'active elevation-5': selectable == 2 }" class="nav-link selectable text-dark fw-bold">Npcs</p>
      </li>

      <li v-if="campaignProp.creatorId == account.id" class="nav-item">
        <p @click="selectable = 3" :class="{ 'active elevation-5': selectable == 3 }" class="nav-link selectable text-dark fw-bold">Monsters</p>
      </li>
    </ul>
  </div>

  <div class="bg-dark rounded-bottom elevation-5 h-md-50 py-2">
    <div v-if="selectable == 1" class="d-flex flex-column justify-content-end h-100">
      <CampaignComment :campaignProp="campaignProp" />
    </div>

    <div v-else-if="selectable == 2">
      <form @submit.prevent="createNpc()" class="d-flex justify-content-end pb-2">
        <div class="px-2 w-sm-50 input-group">
          <select v-model="editable.npc" id="selectNpc" class="form-select" aria-label="Select Npc" required>
            <option disabled>Add Npc</option>
            <option v-for="c in characters" :key="c.id" :value="c">{{ c.name }}</option>
          </select>
          <button class="mdi mdi-plus input-group-text" type="submit" title="Add Npc"></button>
        </div>
      </form>

      <div v-if="campaignProp.npcs.length" class="row mx-0 overflow-auto">
        <div v-for="n in campaignProp.npcs" :key="n" class="col-12 col-sm-6 col-md-12">
          <CharacterCard :characterProp="n" locationProp="npc" />
        </div>
      </div>

      <div v-else class="d-flex justify-content-center p-2">
        <p class="fs-4 fw-bold">No current npcs in this campaign!</p>
      </div>
    </div>

    <div v-else>
      <div class="row justify-content-between align-items-center px-3">
        <div class="col-2">
          <i @click="prep ? initiateBattle() : prep = []" class="mdi mdi-sword-cross fs-5 selectable" title="Initiate Battle!"></i>
        </div>

        <form @submit.prevent="addMonster()" class="col-10 col-sm-6 col-md-10 col-lg-8 col-xl-6 d-flex justify-content-end">
          <div class="input-group">
            <select v-model="editable.monster" id="selectMonster" class="form-select" aria-label="Select Monster" required>
              <option disabled>Add Monster</option>
              <option v-for="m in monsters" :key="m.index" :value="m">
                {{ m.name }}
              </option>
            </select>
            <router-link :to="editable.monster.url.replace('api', 'info')" v-if="editable.monster" target="_blank" class="mdi mdi-information text-primary selectable input-group-text" title="Learn more"></router-link>
            <button class="mdi mdi-plus input-group-text" type="submit" title="Add Monster"></button>
          </div>
        </form>
      </div>

      <div v-if="campaignProp.monsters.length" class="row align-items-end mx-0">
        <div v-for="m in campaignProp.monsters" :key="m" class="col-12 col-sm-6 p-2">
          <div v-if="!prep" class="d-flex justify-content-between align-items-end">
            <router-link :to="m.url.replace('api', 'info')" target="_blank" class="fs-5 fw-bold text-light">
              <p class="px-2 text-decoration-underline">{{ m.name }}</p>
            </router-link>
            <div>
              <i @click="removeMonster(m)" class="mdi mdi-delete fs-5 text-danger selectable"></i>
            </div>
          </div>

          <div v-else>
            <p @click="addPrep(m)" class="px-2 fs-5 fw-bold rounded selectable" :class="{ 'bg-light text-dark elevation-5': prep?.find(p => p.name == m.name) }">{{ m.name }}</p>
          </div>
          <hr class="my-2">
        </div>
      </div>

      <div v-else class="d-flex justify-content-center p-2">
        <p class="fs-4 fw-bold">No current monsters in this campaign!</p>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, onMounted, ref } from 'vue'
import { AppState } from '../AppState.js'
import { Campaign } from '../models/Campaign.js'
import { campaignsService } from '../services/CampaignsService.js'
import { npcsService } from '../services/NpcsService.js'
import { infosService } from '../services/InfosService.js'
import CharacterCard from './CharacterCard.vue'
import CampaignComment from './CampaignComment.vue'
import Pop from '../utils/Pop.js'

export default {
  props: {
    campaignProp: {
      type: Campaign,
      required: true
    }
  },
  setup(prop) {
    const editable = ref({})
    const selectable = ref(1)
    const monsters = ref([])
    const prep = ref(null)

    onMounted(() => {
      getMonsters()
    })

    async function getMonsters() {
      try {
        monsters.value = await infosService.getInfoById('monsters', false)
      } catch (error) {
        Pop.error(error.message, '[GETTING MONSTERS]')
      }
    }

    return {
      editable,
      selectable,
      monsters,
      prep,
      account: computed(() => AppState.account),
      characters: computed(() => AppState.characters),

      copyCode(campaignId) {
        navigator.clipboard.writeText(campaignId)
      },

      async createNpc() {
        try {
          await npcsService.createNpc({ ...editable.value.npc })
          Pop.success('Npc was added to this campaign!')
        } catch (error) {
          Pop.error(error.message, '[CREATING NPC]')
        } finally {
          editable.value.npc = {}
        }
      },

      async addMonster() {
        try {
          const campaign = AppState.activeCampaign

          if (campaign.monsters.find(m => m.index == editable.value.monster.index)) {
            return Pop.toast('Monster is already in this campaign!')
          }
          campaign.monsters.push(editable.value.monster)
          await campaignsService.updateCampaign({ monsters: campaign.monsters }, campaign.id)
          Pop.success('Monster was added to this campaign!')
        } catch (error) {
          Pop.error(error.message, '[ADDING MONSTERS]')
        } finally {
          editable.value.monster = null
        }
      },

      addPrep(monster) {
        const foundIndex = prep.value.findIndex(s => s.index == monster.index)

        if (foundIndex != -1) {
          prep.value.splice(foundIndex, 1)
        } else {
          prep.value.push(monster)
        }
      },

      async initiateBattle() {
        try {
          if (!prep.value.length) {
            return prep.value = null
          }
          const isSure = await Pop.confirm('Initiate Battle?', 'You can end it at any time!')
          const campaign = prop.campaignProp

          if (!isSure) {
            return prep.value = null
          }
          const entities = campaign.players
            .concat(prep.value)
            .map(p => {
              return {
                id: p.characterId ?? p.url,
                name: p.name,
                creatorId: p.creatorId ?? campaign.creatorId,
                initiative: 0
              }
            })
          const initiative = {
            entities,
            intTotal: 0,
            intCount: campaign.players.length + prep.value.length
          }
          campaignsService.updateCampaign({ initiative }, campaign.id)
        } catch (error) {
          Pop.error(error.message, '[INITIATING BATTLE]')
        }
      },

      async removeMonster(monster) {
        try {
          const isSure = await Pop.confirm(`Are you sure you want to remove ${monster.name} from this campaign?`)

          if (!isSure) {
            return
          }
          const campaign = AppState.activeCampaign
          campaign.monsters = campaign.monsters.filter(m => m.index != monster.index)
          await campaignsService.updateCampaign({ monsters: campaign.monsters }, campaign.id)
          Pop.toast('Monster was removed from this campaign')
        } catch (error) {
          Pop.error(error.message, '[REMOVING MONSTER]')
        }
      }
    }
  },
  components: { CharacterCard, CampaignComment }
}
</script>

<style lang="scss" scoped>
  @media screen and (min-width: 768px) {
    .players {
      height: 28vh;
    }
  }

  @media screen and (min-width: 543px) {
    .w-sm-50 {
      width: 50%;
    }
  }
</style>