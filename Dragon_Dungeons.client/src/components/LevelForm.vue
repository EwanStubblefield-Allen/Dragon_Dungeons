<template>
  <div v-if="character" class="fs-5">
    <p class="text-center fw-bold py-1">{{ character.name }} is now level {{ character.level }}!</p>
    <p v-if="character.bonus.prof != info.prof_bonus" class="py-1">Proficiency Bonus is now {{ info.prof_bonus }}!</p>

    <div v-if="!info.ability_score_bonuses">
      <p>Attribute Bonuses Available: {{ info.ability_score_bonuses }}</p>
      <div class="d-flex justify-content-around">
        <div v-for="a in attributes" :key="a" class="text-center">
          <p class="text-capitalize">{{ a }}</p>
          <p>{{ character[a] }}</p>
        </div>
      </div>
    </div>

    <p v-if="info.features?.length" class="pt-1">New Features:</p>
    <ul class="mb-1">
      <li v-for="f in info.features" :key="f.index" class="fs-6">
        {{ f.name }}
        <router-link :to="f.url.replace('api', 'info')" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </li>
    </ul>

    <div v-if="info.spellcasting">
      <div v-if="!info.spellcasting.cantrips_known" class="py-1">
        <p>Cantrips Available: {{ info.spellcasting.cantrips_known }}</p>
      </div>

      <div v-if="info.spellcasting.spells_known" class="py-1">
        <p>Spells Available: {{ info.spellcasting.spells_known }}</p>
      </div>

      <p>Spell Slots:</p>
      <ul>
        <li v-for="c in casting" :key="c">Level {{ c[0] }}: {{ c[1] }} spell{{ c[1] > 1 ? 's' : '' }}</li>
      </ul>
    </div>

    <div class="text-end">
      <button class="btn btn-success mt-2" type="button">Level Up!</button>
    </div>
  </div>
</template>

<!-- {
  "ability_score_bonuses": 0,
  "spellcasting": {
      "cantrips_known": 2,
      "spells_known": 2
  }
} -->

<script>
import { computed, onMounted, ref } from 'vue'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const info = ref({})
    const casting = ref([])

    onMounted(() => {
      document.getElementById('level').addEventListener('show.bs.modal', getCharacterLevel)
    })

    async function getCharacterLevel() {
      try {
        const character = AppState.activeCharacter
        info.value = await infosService.getInfoDetails(`api/classes/${character.class.toLowerCase()}/levels/${character.level}`, false)
        info.value.ability_score_bonuses -= character.bonus.ability

        if (info.value.spellcasting) {
          info.value.spellcasting.cantrips_known -= character.cantrips.length

          if (!info.value.spellcasting.spells_known) {
            switch (character.class) {
              case 'Wizard':
                info.value.spellcasting.spells_known = Math.floor((character.int - 10) / 2) + character.level
                break
              case 'Cleric':
              case 'Druid':
                info.value.spellcasting.spells_known = Math.floor((character.wis - 10) / 2) + character.level
                break
              case 'Paladin':
                info.value.spellcasting.spells_known = Math.floor((character.cha - 10) / 2) + character.level
                break
              default:
                Pop.error(`No case for ${character.class} in spells`)
                break
            }
          }
          info.value.spellcasting.spells_known -= character.spells.length
          casting.value = { ...info.value.spellcasting }
          delete casting.value.cantrips_known
          delete casting.value.spells_known
          casting.value = Object.entries(casting.value).filter((c, index) => {
            if (c[1] > 0) {
              c[0] = index + 1
              return true
            }
            return false
          })
        }
      } catch (error) {
        Pop.error(error.message, '[GETTING CHARACTER LEVEL]')
      }
    }

    return {
      info,
      casting,
      character: computed(() => AppState.activeCharacter),
      attributes: computed(() => AppState.attributes)
    }
  }
}
</script>

<style lang="scss" scoped></style>