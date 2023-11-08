<template>
  <div v-if="initiative">
    <div v-if="initiative.intCount == initiative.intTotal">
      <p class="fs-5 fw-bold">Turn Order:</p>
      <ol>
        <li v-for="e in initiative.entities" :key="e">{{ e.name }}</li>
      </ol>
      <div v-if="entities.length > 1" class="dropdown text-end">
        <button class="btn btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
          Select Monster
        </button>
        <ul class="dropdown-menu">
          <li v-for="(e, index) in entities" :key="e.name">
            <p @click="selectable = index" class="dropdown-item selectable">{{ e.name }}</p>
          </li>
        </ul>
      </div>

      <div v-if="entities.length" class="row">
        <div class="col-12 d-flex align-items-center">
          <p class="fs-3 pe-2">{{ entities[selectable].name }}</p>
          <router-link v-if="entities[selectable].url" :to="{ name: 'Info', params: { infoId: 'monsters', infoDetails: entities[selectable].index } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
          <router-link v-else :to="{ name: 'Character', params: { characterId: entities[selectable].id } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
        </div>

        <div class="col-4 text-center">
          <p>Hit Points</p>
          <p>{{ entities[selectable].hit_points ?? entities[selectable].hp }}</p>
        </div>
        <div class="col-4 text-center">
          <p>Armor Class</p>
          <p>{{ entities[selectable].armorClass ?? entities[selectable].armor_class[0].value }}</p>
        </div>
        <div class="col-4 text-center">
          <p>Hit Die</p>
          <p>{{ entities[selectable].hit_dice ?? `1d${entities[selectable].hitDie}` }}</p>
        </div>

        <div class="col-2 text-center">
          <p>Str</p>
          <div class="d-flex justify-content-center">
            <p class="pe-2">{{ entities[selectable].strength ?? entities[selectable].str }}</p>
            <small>{{ Math.floor(((entities[selectable].strength ?? entities[selectable].str) - 10) / 2) }}</small>
          </div>
        </div>
        <div class="col-2 text-center">
          <p>Dex</p>
          <div class="d-flex justify-content-center">
            <p class="pe-2">{{ entities[selectable].dexterity ?? entities[selectable].dex }}</p>
            <small>{{ Math.floor(((entities[selectable].dexterity ?? entities[selectable].dex) - 10) / 2) }}</small>
          </div>
        </div>
        <div class="col-2 text-center">
          <p>Con</p>
          <div class="d-flex justify-content-center">
            <p class="pe-2">{{ entities[selectable].constitution ?? entities[selectable].con }}</p>
            <small>{{ Math.floor(((entities[selectable].constitution ?? entities[selectable].con) - 10) / 2) }}</small>
          </div>
        </div>
        <div class="col-2 text-center">
          <p>Int</p>
          <div class="d-flex justify-content-center">
            <p class="pe-2">{{ entities[selectable].intelligence ?? entities[selectable].int }}</p>
            <small>{{ Math.floor(((entities[selectable].intelligence ?? entities[selectable].int) - 10) / 2) }}</small>
          </div>
        </div>
        <div class="col-2 text-center">
          <p>Wis</p>
          <div class="d-flex justify-content-center">
            <p class="pe-2">{{ entities[selectable].wisdom ?? entities[selectable].wis }}</p>
            <small>{{ Math.floor(((entities[selectable].wisdom ?? entities[selectable].wis) - 10) / 2) }}</small>
          </div>
        </div>
        <div class="col-2 text-center">
          <p>Cha</p>
          <div class="d-flex justify-content-center">
            <p class="pe-2">{{ entities[selectable].charisma ?? entities[selectable].cha }}</p>
            <small>{{ Math.floor(((entities[selectable].charisma ?? entities[selectable].cha) - 10) / 2) }}</small>
          </div>
        </div>
      </div>
      <div v-if="campaign.creatorId == account.id" class="text-end pt-2">
        <button @click="cancelBattle()" type="button" class="btn btn-secondary m-1">Cancel Battle</button>
        <button @click="awardXp()" type="button" class="btn btn-primary m-1">Award Xp</button>
      </div>
    </div>

    <div v-else-if="!initiative.entities?.find(e => e.creatorId == account.id && !e.initiative)">
      <p class="fs-2 fw-bold text-center">Waiting on others to roll for initiative!</p>
    </div>

    <form v-else @submit.prevent="updateInitiative()">
      <div v-for="(e, index) in initiative.entities" :key="e.id">
        <div v-if="e.creatorId == account.id" class="form-group py-2">
          <label :for="`initiative${index}`">{{ e.name }}'s Initiative</label>
          <input v-model="editable[index]" type="number" class="form-control" :id="`initiative${index}`" min="1" max="20" :aria-describedby="`initiativeHelpBlock${index}`" required>
          <div :id="`initiativeHelpBlock${index}`" class="form-text">
            Don't forget to include initiative bonuses!
          </div>
        </div>
      </div>
      <div class="text-end pt-2">
        <button v-if="campaign.creatorId == account.id" @click="cancelBattle()" type="button" class="btn btn-secondary m-1">Cancel Battle</button>
        <button class="btn btn-success" type="submit">Set Initiative</button>
      </div>
    </form>
  </div>
</template>

<script>
import { computed, onMounted, ref } from 'vue'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import { charactersService } from '../services/CharactersService.js'
import { infosService } from '../services/InfosService.js'
import { campaignHub } from '../handlers/CampaignHub.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref([])
    const selectable = ref(0)
    const entities = ref([])

    onMounted(() => {
      document.getElementById('initiative').addEventListener('show.bs.modal', () => {
        if (!entities.value.length) {
          getEntities()
        }
      })
    })

    async function getEntities() {
      try {
        let ent = AppState.activeCampaign.initiative.entities
        ent = ent.filter(e => e.creatorId == AppState.account.id)

        if (ent[0].id.includes('api')) {
          ent.forEach(async e => {
            const monster = await infosService.getInfoDetails(e.id, false)
            entities.value.push(monster)
          })
        } else {
          await charactersService.getCharacterById(ent[0].id)
          entities.value.push(AppState.activeCharacter)
        }
      } catch (error) {
        Pop.error(error.message, '[GETTING ENTITIES]')
      }
    }

    return {
      editable,
      selectable,
      entities,
      account: computed(() => AppState.account),
      campaign: computed(() => AppState.activeCampaign),
      initiative: computed(() => AppState.activeCampaign?.initiative),

      async updateInitiative() {
        try {
          const initiative = AppState.activeCampaign.initiative
          initiative.entities = initiative.entities.map((m, index) => {
            if (editable.value[index]) {
              m.initiative = editable.value[index]

              if (initiative.intTotal < initiative.intCount) {
                initiative.intTotal++
              }
            }
            return m
          })
          initiative.entities.sort((a, b) => b.initiative - a.initiative)
          await campaignsService.updateCampaign({ initiative }, AppState.activeCampaign.id)
          editable.value = []
        } catch (error) {
          Pop.error(error.message, '[UPDATING INITIATIVE]')
        }
      },

      async awardXp() {
        try {
          let xp = 0
          entities.value.forEach(e => {
            xp += e.xp
          })
          const isSure = await Pop.confirm(`You will be awarding players with ${xp}xp`)

          if (!isSure) {
            return
          }
          campaignHub.awardXp(this.campaign.id, xp)
          await this.cancelBattle(isSure)
        } catch (error) {
          Pop.error(error.message, '[AWARDING XP]')
        }
      },

      async cancelBattle(confirmed = false) {
        try {
          if (!confirmed) {
            const isSure = await Pop.confirm('Are you sure you want to cancel this battle?', 'Players will not be awarded xp!')

            if (!isSure) {
              return
            }
          }
          await campaignsService.updateCampaign({ initiative: {} }, AppState.activeCampaign.id)
          entities.value = []
          selectable.value = 0
        } catch (error) {
          Pop.error(error.message, '[CANCELING BATTLE]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>