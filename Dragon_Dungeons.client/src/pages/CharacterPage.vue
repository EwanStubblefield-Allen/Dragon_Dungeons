<template>
  <section v-if="character" class="row py-3">
    <div class="col-12 col-md-6">
      <section class="row">
        <div class="col-12 col-sm-6 col-lg-4">
          <img class="img-fluid h-100 w-100 rounded elevation-5" :src="character.picture.url" :alt="character.name">
        </div>
        <div class="col-12 col-sm-6 col-lg-4">
          <p class="fs-1 fw-bold">{{ character.name }}</p>
          <hr class="my-2">
          <p class="fs-3">Level: {{ character.level }}</p>
          <p class="fs-3">{{ character.race }} {{ character.class }}</p>
          <p class="fs-3">{{ character.alignment }}</p>
        </div>
        <div class="col-12 col-lg-4">
          <section class="row align-item">
            <div class="col-7 p-1">
              <div class="d-flex justify-content-around align-items-center bg-dark rounded elevation-5 h-100">
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
            <div class="col-5 p-1">
              <div class="bg-dark text-center rounded elevation-5 p-2">
                <p>Armor</p>
                <p>{{ Math.floor((character.dex - 10) / 2) }}</p>
                <p>Class</p>
              </div>
            </div>
            <div class="col-4 p-1">
              <div class="bg-dark text-center rounded elevation-5 p-2">
                <p>Prof</p>
                <p>{{ Math.floor((character.dex - 10) / 2) }}</p>
                <p>Bonus</p>
              </div>
            </div>
            <div class="col-4 p-1">
              <div class="bg-dark text-center rounded elevation-5 p-2">
                <p>Walking</p>
                <p>{{ character.speed }} ft.</p>
                <p>Speed</p>
              </div>
            </div>
            <div class="col-4 p-1">
              <div class="bg-dark d-flex flex-column justify-content-center align-items-center rounded elevation-5 h-100 p-1">
                <p>Initiative</p>
                <p>{{ Math.floor((character.dex - 10) / 2) }}</p>
              </div>
            </div>
          </section>
        </div>
      </section>
    </div>

    <div class="col-12 col-md-6">
      <CharacterInfo :characterProp="character" />
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
      character: computed(() => AppState.activeCharacter)
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