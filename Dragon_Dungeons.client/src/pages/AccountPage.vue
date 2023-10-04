<template>
  <section class="row justify-content-center">
    <div class="col-12">
      <ProfileDetails :profileProp="account" />
    </div>

    <div class="col-10">
      <p class="fs-2 fw-bold">My Characters:</p>
      <section class="row py-3">
        <div v-for="c in characters" :key="c.id" class="col-12 col-sm-6 col-md-4 col-lg-3">
          <CharacterCard :characterProp="c" />
        </div>
      </section>
    </div>
  </section>
</template>

<script>
import { computed } from 'vue'
import { AppState } from '../AppState'
import ProfileDetails from '../components/ProfileDetails.vue'
import CharacterCard from '../components/CharacterCard.vue'

export default {
  setup() {
    return {
      account: computed(() => AppState.account),
      characters: computed(() => AppState.characters)
    }
  },
  components: { ProfileDetails, CharacterCard }
}
</script>

<style scoped></style>