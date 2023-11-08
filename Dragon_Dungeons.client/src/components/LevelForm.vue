<template>
  <div v-if="character">
    <p class="text-center fs-5">{{ character.name }} is now level {{ character.level }}!</p>
    <div class="text-end">
      <button class="btn btn-success mt-2" type="button">Level Up!</button>
    </div>
  </div>
</template>

<!-- {
  "ability_score_bonuses": 0,
  "prof_bonus": 2,
  "features": [
      {
          "index": "channel-divinity-1-rest",
          "name": "Channel Divinity (1/rest)",
          "url": "/api/features/channel-divinity-1-rest"
      },
      {
          "index": "channel-divinity-turn-undead",
          "name": "Channel Divinity: Turn Undead",
          "url": "/api/features/channel-divinity-turn-undead"
      }
  ],
  "spellcasting": {
      "cantrips_known": 3,
      "spell_slots_level_1": 3,
      "spell_slots_level_2": 0,
      "spell_slots_level_3": 0,
      "spell_slots_level_4": 0,
      "spell_slots_level_5": 0,
      "spell_slots_level_6": 0,
      "spell_slots_level_7": 0,
      "spell_slots_level_8": 0,
      "spell_slots_level_9": 0
  },
  "class_specific": {
      "channel_divinity_charges": 1,
      "destroy_undead_cr": 0
  }
} -->

<script>
import { computed, onMounted } from 'vue'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import { logger } from '../utils/Logger.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    onMounted(() => {
      document.getElementById('level').addEventListener('show.bs.modal', getCharacterLevel)
    })

    async function getCharacterLevel() {
      try {
        const character = AppState.activeCharacter
        const info = await infosService.getInfoDetails(`api/classes/${character.class.toLowerCase()}/levels/${character.level}`, false)
        logger.log(info)
      } catch (error) {
        Pop.error(error.message, '[GETTING CHARACTER LEVEL]')
      }
    }

    onMounted(() => {

    })

    return {
      character: computed(() => AppState.activeCharacter)
    }
  }
}
</script>

<style lang="scss" scoped></style>