<template>
  <section v-if="character" class="row py-1">
    <div class="col-12 col-md-6">
      <div>
        <p class="fs-2 fw-bold text-wrap text-truncate">{{ character.name }}</p>
        <hr class="my-2">
      </div>

      <section class="row">
        <div class="col-12 col-sm-6 col-lg-4">
          <img class="img-fluid w-100 rounded elevation-5" :src="character.picture.url" :alt="character.name">
        </div>
        <div class="col-12 col-sm-6 col-lg-4">
          <div class="d-flex justify-content-between align-items-center">
            <p class="fs-5">Level: {{ character.level }}</p>
            <button v-if="character.creatorId == account.id && character.manual" type="button" class="btn btn-primary mdi mdi-plus px-1 py-0 fs-5" data-bs-toggle="modal" data-bs-target="#level"></button>
          </div>
          <abbr class="progress" role="progressbar" aria-label="Animated striped example" :aria-valuenow="level" aria-valuemin="0" aria-valuemax="100" :title="level">
            <div class="progress-bar progress-bar-striped progress-bar-animated"></div>
          </abbr>
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
                <p v-if="savingThrows.includes(a)" title="Modifier" class="text-end no-select">{{ Math.floor((character[a] - 10) / 2) + character.bonus.prof }}</p>
                <p v-else title="Modifier" class="text-end">{{ Math.floor((character[a] - 10) / 2) }}</p>
              </div>
            </div>
          </section>

          <section class="row mx-0 py-2">
            <div class="col-6 col-lg-12 col-xl-6 p-1">
              <div class="bg-dark text-center rounded elevation-5 p-2">
                <p>Armor</p>
                <p>{{ character.armorClass }}</p>
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

      <section class="row px-2">
        <div class="col-12 col-lg-6 p-1">
          <div v-if="character.hp > 0" class="d-flex justify-content-around align-items-center bg-dark rounded elevation-5 p-2 h-100">
            <div class="text-center">
              <p>Hp:</p>
              <p>{{ character.hp }} / {{ character.maxHp }}</p>
            </div>
            <div v-if="character.creatorId == account.id" class="d-flex align-items-center w-50">
              <i @click="addHp('hp')" class="mdi mdi-menu-left fs-5 selectable"></i>
              <input v-model="editable" type="number" class="form-control" id="hp" min="-2000" max="2000" required>
              <i @click="addHp('tempHp')" class="mdi mdi-menu-right fs-5 selectable"></i>
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
                  <i v-if="deathSaves[d] >= i" @click="updateDeathSaves(d, -1)" class="mdi mdi-circle selectable rounded"></i>
                  <i v-else @click="updateDeathSaves(d, +1)" class="mdi mdi-circle-outline selectable rounded"></i>
                  <p v-if="i < 3" class="pb-1 no-select">-</p>
                </div>
              </div>
            </section>
            <u>Death Saves</u>
          </div>
        </div>
        <div class="col-6 col-lg-3 p-1">
          <div class="bg-dark text-center rounded elevation-5 p-2">
            <p>Prof.</p>
            <p>{{ character.bonus.prof }}</p>
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

  <div v-else>
    <Loader />
  </div>
</template>

<script>
import { computed, onUnmounted, ref, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { charactersService } from '../services/CharactersService.js'
import { infosService } from '../services/InfosService.js'
import CharacterInfo from '../components/CharacterInfo.vue'
import CharacterSkills from '../components/CharacterSkills.vue'
import CharacterEquipment from '../components/CharacterEquipment.vue'
import Loader from '../components/Loader.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    const editable = ref(0)
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

          if (p.startsWith('Saving Throw:')) {
            savingThrows.value.push(p.replace('Saving Throw: ', '').toLowerCase())
          }
        }
        infosService.getEquipment()
      }
      catch (error) {
        Pop.error(error.message, '[GETTING CHARACTER BY ID]')
        router.push('/')
      }
    }

    return {
      editable,
      savingThrows,
      deathSaves,
      account: computed(() => AppState.account),
      character: computed(() => AppState.activeCharacter),
      level: computed(() => {
        const character = AppState.activeCharacter
        return Math.floor(character?.xp / AppState.xpLevels[character?.level] * 10000) / 100 + '%'
      }),
      attributes: computed(() => AppState.attributes),

      async addHp(option) {
        try {
          const temp = this.character[option]

          if (this.character[option] + editable.value < 0) {
            this.character[option] = 0
          } else if (option == 'hp' && this.character[option] + editable.value > this.character.maxHp) {
            this.character[option] = this.character.maxHp
          }
          else {
            this.character[option] += editable.value
          }

          if (temp == this.character[option]) {
            return editable.value = 0
          }
          await charactersService.updateCharacter({ hp: this.character.hp, tempHp: this.character.tempHp })
          Pop.success('Hp updated!')
          editable.value = 0
        } catch (error) {
          Pop.error(error.message, '[UPDATING HP]')
        }
      },

      updateDeathSaves(type, num) {
        deathSaves.value[type] += num

        if (deathSaves.value[type] == 3) {
          if (type == 'Success') {
            editable.value = 1
            this.addHp('hp')
          } else {
            Pop.toast('Your Character Died!')
          }
          deathSaves.value = { Success: 0, Failure: 0 }
        }
      }
    }
  },
  components: { CharacterInfo, CharacterSkills, CharacterEquipment, Loader }
}
</script>

<style lang="scss" scoped>
  img {
    object-fit: cover;
    object-position: top;
  }

  .progress-bar {
    width: v-bind(level);
  }
</style>