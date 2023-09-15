<template>
  <section class="row">
    <div class="col-12 col-md-3 col-lg-2 overflow-auto infoBar">
      <section class="row p-3">
        <div class="col-12 p-0">
          <p class="fs-3 text-capitalize p-2">Steps:</p>
          <hr class="mt-0">
        </div>
        <div v-for="(l, index) in list" :key="l" class="col-12 col-sm-6 col-md-12">
          <router-link :to="{ name: 'Character', params: { characterId: l.toLowerCase().replaceAll(' ', '-') } }" class="text-light selectable rounded p-2">{{ l }}</router-link>
          <hr v-if="index != list.length - 1" class="my-2">
        </div>
      </section>
    </div>

    <div v-if="route.params.characterId == 'basics'" class="col-12 col-md-9 col-lg-10">
      <BasicsForm />
    </div>

    <div v-if="route.params.characterId == 'features'" class="col-12 col-md-9 col-lg-10">
      <FeaturesForm />
    </div>

    <div v-if="route.params.characterId == 'background'" class="col-12 col-md-9 col-lg-10">
      <BackgroundForm />
    </div>

    <div v-if="route.params.characterId == 'personality-traits'" class="col-12 col-md-9 col-lg-10">
      <PersonalityTraitsForm />
    </div>

    <div v-if="route.params.characterId == 'attributes'" class="col-12 col-md-9 col-lg-10">
      <AttributesForm />
    </div>
  </section>
</template>

<script>
import { useRoute } from 'vue-router'
import BasicsForm from '../components/BasicsForm.vue'
import FeaturesForm from '../components/FeaturesForm.vue'
import BackgroundForm from '../components/BackgroundForm.vue'
import PersonalityTraitsForm from '../components/PersonalityTraitsForm.vue'
import AttributesForm from '../components/AttributesForm.vue'

export default {
  setup() {
    const route = useRoute()
    const list = ['Basics', 'Features', 'Background', 'Personality Traits', 'Attributes', 'Proficiencies', 'Saving Throws', 'Skills', 'Levels', 'Attacks', 'Spells', 'Equipment']

    return {
      route,
      list
    }
  },
  components: { BasicsForm, FeaturesForm, BackgroundForm, PersonalityTraitsForm, AttributesForm }
}
</script>

<style lang="scss" scoped>
  .infoBar {
    background-color: var(--oxford);
    color: white;
    height: var(--main-height);
  }

  @media screen and (max-width: 768px) {
    .infoBar {
      height: 25vh;
    }
  }
</style>