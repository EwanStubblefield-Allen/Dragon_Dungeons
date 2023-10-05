<template>
  <section v-if="character" class="row py-2">
    <div class="col-12 col-md-6">
      <section class="row">
        <div class="col-12 col-sm-6 col-lg-4">
          <img class="img-fluid w-100 rounded elevation-5" :src="character.picture.url" :alt="character.name">
        </div>
        <div class="col-12 col-sm-6 col-lg-4">
          <p class="fs-2 fw-bold text-truncate">{{ character.name }}</p>
          <hr class="my-2">
          <p class="fs-5">Level: {{ character.level }}</p>
          <p class="fs-5">{{ character.race }} {{ character.class }}</p>
          <p class="fs-5">{{ character.alignment }}</p>
        </div>

        <div class="col-12 col-lg-4">
          <div class="d-flex">
            <button class="btn btn-dark mdi mdi-campfire m-1">Short Rest</button>
            <button class="btn btn-dark mdi mdi-weather-night m-1"> Long Rest</button>
          </div>
          <section class="row bg-dark text-center rounded elevation-5 p-2">
            <div v-for="a in attributes" :key="a" class="col-6">
              <div class="d-flex justify-content-between align-items-center">
                <i class="mdi mdi-circle"></i>
                <p class="text-center text-uppercase">{{ a }}</p>
                <p title="Modifier" class="text-end">{{ Math.floor((character[a] - 10) / 2) + character.bonus }}</p>
              </div>
            </div>
          </section>
        </div>
      </section>
    </div>

    <div class="col-12 col-md-6">
      <CharacterInfo :characterProp="character" />

      <section class="row p-2 align-item">
        <div class="col-12 col-sm-8 col-lg-4 p-1">
          <div class="d-flex justify-content-around align-items-center bg-dark rounded elevation-5 p-2 h-100">
            <div class="text-center">
              <p>Hp:</p>
              <p>{{ character.hp }} / {{ character.maxHp }}</p>
            </div>
            <div class="text-center">
              <p>Temp:</p>
              <p>{{ character.tempHp }}</p>
            </div>
          </div>
        </div>
        <div class="col-6 col-sm-4 col-lg-2 p-1">
          <div class="bg-dark text-center rounded elevation-5 p-2">
            <p>Armor</p>
            <p>{{ Math.floor((character.dex - 10) / 2) }}</p>
            <p>Class</p>
          </div>
        </div>
        <div class="col-6 col-sm-4 col-lg-2 p-1">
          <div class="bg-dark text-center rounded elevation-5 p-2">
            <p>Prof.</p>
            <p>{{ Math.floor((character.dex - 10) / 2) }}</p>
            <p>Bonus</p>
          </div>
        </div>
        <div class="col-6 col-sm-4 col-lg-2 p-1">
          <div class="bg-dark text-center rounded elevation-5 p-2">
            <p>Walk</p>
            <p>{{ character.speed }} ft.</p>
            <p>Speed</p>
          </div>
        </div>
        <div class="col-6 col-sm-4 col-lg-2 p-1">
          <div class="bg-dark d-flex flex-column justify-content-center align-items-center rounded elevation-5 h-100 p-1">
            <p>Initiative</p>
            <p>{{ Math.floor((character.dex - 10) / 2) }}</p>
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
import { computed, watchEffect } from 'vue'
import { charactersService } from '../services/CharactersService.js'
import CharacterInfo from '../components/CharacterInfo.vue'
import CharacterSkills from '../components/CharacterSkills.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    watchEffect(() => {
      if (route.params.characterId) {
        getCharacterById()
      }
    })

    async function getCharacterById() {
      try {
        await charactersService.getCharacterById(route.params.characterId)
      }
      catch (error) {
        Pop.error(error.message, '[GETTING CHARACTER BY ID]')
        router.push('/')
      }
    }
    return {
      character: computed(() => AppState.activeCharacter),
      attributes: computed(() => AppState.attributes)
    }
  },
  components: { CharacterInfo, CharacterSkills }
}
</script>

<style lang="scss" scoped>
  img {
    object-fit: cover;
    object-position: top;
  }
</style>