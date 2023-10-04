<template>
  <section class="row">
    <div class="col-12 col-md-3 col-lg-2 overflow-auto infoBar">
      <section class="row p-3">
        <div class="col-12 p-0">
          <p class="fs-3 text-capitalize p-2">Steps:</p>
          <hr class="mt-0">
        </div>
        <div v-for="(l, index) in list" :key="l" class="col-12 col-sm-6 col-md-12">
          <router-link :to="{ name: 'CreateCharacter', params: { characterStep: l.toLowerCase().replaceAll(' ', '-') } }" v-if="charPage >= index">
            <p class="text-light selectable rounded px-2 py-1">{{ l }}</p>
          </router-link>
          <p v-else class="text-secondary rounded px-2 py-1">{{ l }}</p>
          <hr v-if="index != list.length - 1" class="my-2">
        </div>
      </section>
    </div>

    <div class="col-12 col-md-9 col-lg-10 offset-md-3 offset-lg-2">
      <div v-if="route.params.characterStep == 'basics'">
        <BasicsForm />
      </div>

      <div v-else-if="route.params.characterStep == 'features'">
        <FeaturesForm />
      </div>

      <div v-else-if="route.params.characterStep == 'background'">
        <BackgroundForm />
      </div>

      <div v-else-if="route.params.characterStep == 'personality-traits'">
        <PersonalityTraitsForm />
      </div>

      <div v-else-if="route.params.characterStep == 'attributes'">
        <AttributesForm />
      </div>

      <div v-else-if="route.params.characterStep == 'proficiencies'">
        <ProficienciesForm />
      </div>

      <div v-else-if="route.params.characterStep == 'spells'">
        <SpellsForm />
      </div>

      <div v-else-if="route.params.characterStep == 'equipment'">
        <EquipmentForm />
      </div>
    </div>
  </section>
</template>

<script>
import { computed, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import BasicsForm from '../components/BasicsForm.vue'
import FeaturesForm from '../components/FeaturesForm.vue'
import BackgroundForm from '../components/BackgroundForm.vue'
import PersonalityTraitsForm from '../components/PersonalityTraitsForm.vue'
import AttributesForm from '../components/AttributesForm.vue'
import ProficienciesForm from '../components/ProficienciesForm.vue'
import SpellsForm from '../components/SpellsForm.vue'
import EquipmentForm from '../components/EquipmentForm.vue'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    const list = ['Basics', 'Features', 'Background', 'Personality Traits', 'Attributes', 'Proficiencies', 'Spells', 'Equipment']

    watchEffect(() => {
      let charPage = AppState.charPage

      for (let i = list.length - 1; i > charPage; i--) {
        if (list[i].toLowerCase().replace(' ', '-') == route.params.characterStep && AppState.tempCharacter) {
          router.push({ name: 'CreateCharacter', params: { characterStep: list[charPage].toLowerCase().replace(' ', '-') } })
        }
      }
    })

    return {
      route,
      list,
      charPage: computed(() => AppState.charPage)
    }
  },
  components: { BasicsForm, FeaturesForm, BackgroundForm, PersonalityTraitsForm, AttributesForm, ProficienciesForm, SpellsForm, EquipmentForm }
}
</script>

<style lang="scss" scoped>
  .infoBar {
    background-color: var(--oxford);
    color: white;
    height: 25vh;
  }

  @media screen and (min-width: 768px) {
    .infoBar {
      height: var(--main-height);
      position: fixed;
    }
  }
</style>