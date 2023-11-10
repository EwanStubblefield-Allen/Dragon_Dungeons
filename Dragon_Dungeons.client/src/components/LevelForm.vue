<template>
  <div v-if="character" class="fs-5">
    <p class="text-center fw-bold py-1">{{ character.name }} is now level {{ character.level }}!</p>
    <p v-if="character.bonus.prof != info.prof_bonus" class="py-1">Proficiency Bonus is now {{ info.prof_bonus }}!</p>

    <p>Attribute Bonuses Available: {{ info.ability_score_bonuses }}</p>
    <div class="d-flex justify-content-around">
      <div v-for="a in attributes" :key="a" class="text-center">
        <p class="text-capitalize">{{ a }}</p>
        <div class="d-flex justify-content-center align-items-center">
          <p>{{ info[a] }}</p>
          <div class="d-flex flex-column">
            <button @click="changeAttribute(a, -1)" type="button" class="btn mdi mdi-chevron-up p-0 border-0 selectable" :disabled="!info.ability_score_bonuses"></button>
            <button @click="changeAttribute(a, 1)" type="button" class="btn mdi mdi-chevron-down p-0 border-0 selectable" :disabled="info[a] == character[a]"></button>
          </div>
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
      <div class="py-1">
        <p class="pb-1">Cantrips Available: {{ info.spellcasting.cantrips_known }}</p>
        <form v-if="info.spellcasting.cantrips_known" @submit.prevent="addMagic('cantrips')">
          <div class="input-group">
            <select v-model="editable.cantrips" id="selectCantrip" class="form-select" aria-label="Select Cantrip" required>
              <option disabled>Add Cantrip</option>
              <option v-for="c in magic.cantrips" :key="c.index" :value="c">
                {{ c.name }}
              </option>
            </select>
            <router-link :to="editable.cantrips.url.replace('api', 'info')" v-if="editable.cantrips?.url" target="_blank" class="mdi mdi-information text-primary selectable input-group-text" title="Learn more"></router-link>
            <button class="mdi mdi-plus input-group-text" type="submit" title="Add Cantrip"></button>
          </div>
        </form>
        <ul>
          <li v-for="(c, index) in info.cantrips" :key="c.index">
            {{ c.name }}
            <router-link :to="c.url.replace('api', 'info')" target="_blank" class="mdi mdi-information text-primary rounded selectable" title="Learn more"></router-link>
            <i @click="removeMagic('cantrips', index)" class="mdi mdi-delete text-danger px-1 fs-5 rounded selectable" title="Remove Spell"></i>
          </li>
        </ul>
      </div>

      <div class="py-1">
        <p class="pb-1">Spells Available: {{ info.spellcasting.spells_known }}</p>
        <form v-if="info.spellcasting.spells_known" @submit.prevent="addMagic('spells')">
          <div class="input-group">
            <select v-model="editable.spells" id="selectSpell" class="form-select" aria-label="Select Spell" required>
              <option disabled>Add Spell</option>
              <option v-for="s in magic.spells" :key="s.index" :value="s">
                {{ s.name }}
              </option>
            </select>
            <router-link :to="editable.spells.url.replace('api', 'info')" v-if="editable.spells?.url" target="_blank" class="mdi mdi-information text-primary selectable input-group-text" title="Learn more"></router-link>
            <button class="mdi mdi-plus input-group-text" type="submit" title="Add Spell"></button>
          </div>
        </form>
        <ul>
          <li v-for="(s, index) in info.spells" :key="s.index">
            {{ s.name }}
            <router-link :to="s.url.replace('api', 'info')" target="_blank" class="mdi mdi-information text-primary rounded selectable" title="Learn more"></router-link>
            <i @click="removeMagic('spells', index, s)" class="mdi mdi-delete text-danger px-1 fs-5 rounded selectable" title="Remove Spell"></i>
          </li>
        </ul>
      </div>

      <p>Spell Slots:</p>
      <ul v-if="info.casting">
        <li v-for="c in Object.entries(info.casting)" :key="c">Level {{ c[0] }}: {{ c[1].max }} spell{{ c[1].max > 1 ? 's' : '' }}</li>
      </ul>
    </div>

    <div class="text-end">
      <button @click="levelUp()" type="button" class="btn btn-success mt-2" :disabled="info.spellcasting?.cantrips_known || info.spellcasting?.spells_known || info.ability_score_bonuses">Level Up!</button>
    </div>
  </div>
</template>

<script>
import { computed, onMounted, ref } from 'vue'
import { AppState } from '../AppState.js'
import { Character } from '../models/Character.js'
import { infosService } from '../services/InfosService.js'
import { charactersService } from '../services/CharactersService.js'
import { Modal } from 'bootstrap'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({})
    const info = ref({})
    const magic = ref({})

    onMounted(() => {
      document.getElementById('level').addEventListener('show.bs.modal', getCharacterLevel)
    })

    async function getCharacterLevel() {
      try {
        const character = AppState.activeCharacter
        info.value = await infosService.getInfoDetails(`api/classes/${character.class.toLowerCase()}/levels/${character.level}`, false)
        info.value.bonus = {
          prof: info.value.prof_bonus,
          ability: info.value.ability_score_bonuses
        }
        info.value.ability_score_bonuses -= character.bonus.ability
        AppState.attributes.forEach(a => {
          info.value[a] = character[a]
        })

        if (info.value.spellcasting) {
          handleMagic(character)
        }
      } catch (error) {
        Pop.error(error.message, '[GETTING CHARACTER LEVEL]')
      }
    }

    async function handleMagic(character) {
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
      info.value.casting = { ...info.value.spellcasting }
      delete info.value.casting.cantrips_known
      delete info.value.casting.spells_known
      info.value.casting = Object.entries(info.value.casting).filter((c, index) => {
        if (c[1] > 0) {
          c[0] = index + 1
          c[1] = { current: 0, max: c[1] }
          return true
        }
        return false
      })

      if (info.value.spellcasting.cantrips_known) {
        magic.value.cantrips = (await infosService.getInfoDetails(`api/classes/${character.class.toLowerCase()}/levels/0/spells`, false)).results
        magic.value.cantrips = magic.value.cantrips
          .filter(cantrip => !character.cantrips.find(c => c.name == cantrip.name))
          .sort((a, b) => a.name > b.name ? 1 : -1)
      }
      info.value.spellcasting.spells_known -= character.spells.length

      if (info.value.spellcasting.spells_known) {
        magic.value.spells = []

        for (let i = 1; i <= info.value.casting.length; i++) {
          const res = await infosService.getInfoDetails(`api/classes/${character.class.toLowerCase()}/levels/${i}/spells`, false)
          magic.value.spells = magic.value.spells.concat(res.results)
        }
        magic.value.spells = magic.value.spells
          .filter(spell => !character.spells.find(s => s.name == spell.name))
          .sort((a, b) => a.name > b.name ? 1 : -1)
      }
      info.value.casting = Object.fromEntries(info.value.casting)
    }

    return {
      editable,
      info,
      magic,
      character: computed(() => AppState.activeCharacter),
      attributes: computed(() => AppState.attributes),

      changeAttribute(type, sign) {
        info.value.ability_score_bonuses += sign
        info.value[type] -= sign
      },

      addMagic(type) {
        if (!info.value[type]) {
          info.value[type] = []
        }
        magic.value[type] = magic.value[type].filter(m => m.name != editable.value[type].name)
        delete editable.value[type].index
        editable.value[type].level = 1
        info.value[type].push(editable.value[type])
        editable.value[type] = {}
        info.value.spellcasting[`${type}_known`]--
      },

      removeMagic(type, index, item) {
        info.value[type].splice(index, 1)
        const foundIndex = magic.value[type].findIndex(m => m.name > item.name)
        magic.value[type].splice(foundIndex, 0, item)
        info.value.spellcasting[`${type}_known`]++
      },

      async levelUp() {
        try {
          Modal.getOrCreateInstance('#level').hide()

          if (info.value.cantrips) {
            info.value.cantrips = info.value.cantrips.concat(this.character.cantrips)
          }

          if (info.value.spells) {
            info.value.spells = info.value.spells.concat(this.character.spells)
          }

          if (info.value.features) {
            info.value.charFeatures = info.value.features.map(f => {
              delete f.index
              return f
            }).concat(this.character.charFeatures)
            delete info.value.features
          }
          info.value.xp = this.character.xp
          info.value.manual = false
          delete info.value.name
          delete info.value.class
          await charactersService.checkLevel(new Character(info.value))
          AppState.equipment = {}
          await infosService.getEquipment()
          info.value = {}
          magic.value = {}
          Pop.success(`${this.character.name} is now level ${this.character.level}!`)
        } catch (error) {
          Pop.error(error.message, '[LEVELING UP]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>