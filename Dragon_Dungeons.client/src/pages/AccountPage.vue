<template>
  <section class="row">
    <div class="col-12">
      <ProfileDetails :profileProp="account" />
    </div>
  </section>
</template>

<script>
import { computed } from 'vue'
import { AppState } from '../AppState'
import ProfileDetails from '../components/ProfileDetails.vue'

export default {
  setup() {
    return {
      account: computed(() => AppState.account)
    }
  },
  components: { ProfileDetails }
}
</script>

<style scoped></style>