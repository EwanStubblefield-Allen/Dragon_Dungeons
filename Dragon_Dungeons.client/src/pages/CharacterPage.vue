<template>
  <section v-if="character" class="row py-2">
    <div class="col-12 col-md-6">
      <p class="fs-2 fw-bold text-wrap text-truncate">{{ character.name }}</p>
      <hr class="my-2">
      <section class="row">
        <div class="col-12 col-sm-6 col-lg-4">
          <img class="img-fluid w-100 rounded elevation-5" :src="character.picture.url" :alt="character.name">
        </div>
        <div class="col-12 col-sm-6 col-lg-4">
          <p class="fs-5">Level: {{ character.level }}</p>
          <p class="fs-5">{{ character.race }} {{ character.class }}</p>
          <p class="fs-5">{{ character.alignment }}</p>
          <div class="d-flex justify-content-around align-items-center">
            <button class="btn btn-dark d-flex align-items-center m-1">
              <i class="mdi mdi-campfire pe-2"></i>
              <p>Short Rest</p>
            </button>
            <button class="btn btn-dark d-flex align-items-center m-1">
              <i class="mdi mdi-weather-night pe-2"></i>
              <p>Long Rest</p>
            </button>
          </div>
        </div>

        <div class="col-12 col-lg-4 p-2">
          <section class="row bg-dark text-center rounded elevation-5 mx-0 py-2">
            <u class="col-12">Saving Throws</u>
            <div v-for="a in attributes" :key="a" class="col-6 col-md-12 col-xl-6">
              <div class="d-flex justify-content-between align-items-center">
                <i v-if="savingThrows.includes(a)" class="mdi mdi-circle"></i>
                <i v-else class="mdi mdi-circle-outline"></i>
                <p class="text-center text-uppercase">{{ a }}</p>
                <p v-if="savingThrows.includes(a)" title="Modifier" class="text-end">{{ Math.floor((character[a] - 10) / 2) + character.bonus }}</p>
                <p v-else title="Modifier" class="text-end">{{ Math.floor((character[a] - 10) / 2) + character.bonus }}</p>
              </div>
            </div>
          </section>

          <section class="row mx-0 py-2">
            <div class="col-6 col-lg-12 col-xl-6 p-1">
              <div class="bg-dark text-center rounded elevation-5 p-2">
                <p>Armor</p>
                <p>{{ armorClass }}</p>
                <p>Class</p>
              </div>
            </div>
            <div class="col-6 col-lg-12 col-xl-6 p-1">
              <div class="bg-dark d-flex flex-column justify-content-center align-items-center rounded elevation-5 h-100 p-1">
                <p class="text-break">Initiative</p>
                <p>{{ Math.floor((character.dex - 10) / 2) }}</p>
              </div>
            </div>
          </section>
        </div>
      </section>

      <CharacterEquipment :characterProp="character" />
    </div>

    <div class="col-12 col-md-6">
      <CharacterInfo :characterProp="character" />

      <section class="row p-2">
        <div class="col-12 col-lg-6 p-1">
          <div v-if="character.hp > 0" class="d-flex justify-content-around align-items-center bg-dark rounded elevation-5 p-2 h-100">
            <div class="text-center">
              <p>Hp:</p>
              <p>{{ character.hp }} / {{ character.maxHp }}</p>
            </div>
            <div class="text-center">
              <p>Temp:</p>
              <p>{{ character.tempHp }}</p>
            </div>
          </div>
          <div v-else class="bg-dark text-center rounded elevation-5 p-2">
            <section v-for="d in Object.keys(deathSaves)" :key="d" class="row mx-0">
              <p class="col-7 px-0">{{ d }}</p>
              <div class="col-5 d-flex justify-content-center px-0">
                <div v-for="i in 3" :key="i" class="d-flex align-items-center">
                  <i :class="{ 'mdi-circle': deathSaves[d] >= i }" class="mdi mdi-circle-outline"></i>
                  <p v-if="i < 3" class="pb-1">-</p>
                </div>
              </div>
            </section>
            <u>Death Saves</u>
          </div>
        </div>
        <div class="col-6 col-lg-3 p-1">
          <div class="bg-dark text-center rounded elevation-5 p-2">
            <p>Prof.</p>
            <p>{{ character.bonus }}</p>
            <p>Bonus</p>
          </div>
        </div>
        <div class="col-6 col-lg-3 p-1">
          <div class="bg-dark text-center rounded elevation-5 p-2">
            <p>Walk</p>
            <p>{{ character.speed }} ft.</p>
            <p>Speed</p>
          </div>
        </div>
      </section>

      <CharacterSkills :characterProp="character" />
    </div>
  </section>
</template>

<script>
import { useRoute, useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { computed, onUnmounted, ref, watchEffect } from 'vue'
import { charactersService } from '../services/CharactersService.js'
import CharacterInfo from '../components/CharacterInfo.vue'
import CharacterSkills from '../components/CharacterSkills.vue'
import CharacterEquipment from '../components/CharacterEquipment.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    const savingThrows = ref([])
    const deathSaves = ref({ Success: 0, Failure: 0 })

    onUnmounted(() => {
      AppState.activeCharacter = null
    })

    watchEffect(() => {
      if (route.params.characterId) {
        getCharacterById()
      }
    })

    async function getCharacterById() {
      try {
        await charactersService.getCharacterById(route.params.characterId)
        const prof = AppState.activeCharacter.proficiencies

        for (let i = prof.length - 1; i > 0; i--) {
          const p = prof[i].name

          if (!p.startsWith('Saving Throw:')) {
            return
          }
          savingThrows.value.push(p.replace('Saving Throw: ', '').toLowerCase())
        }
      }
      catch (error) {
        Pop.error(error.message, '[GETTING CHARACTER BY ID]')
        router.push('/')
      }
    }
    return {
      character: computed(() => AppState.activeCharacter),
      attributes: computed(() => AppState.attributes),
      armorClass: computed(() => {
        const character = AppState.activeCharacter
        const armor = AppState.equipment.armor
        const dex = Math.floor((character.dex - 10) / 2)
        let armorClass = 0

        if (!armor || armor.armor_class.dex_bonus) {
          armorClass += dex
        }

        if (!armor) {
          return armorClass += 10
        }
        return armorClass += armor.armor_class.base
      }),
      savingThrows,
      deathSaves
    }
  },
  components: { CharacterInfo, CharacterSkills, CharacterEquipment }
}
</script>

<style lang="scss" scoped>
  img {
    object-fit: cover;
    object-position: top;
  }
</style>