<template>
  <section class="row">
    <div class="col-12 col-md-12 p-0 position-relative">
      <img class="cover-image" :src="profileProp.coverImg" :alt="`${profileProp.name} CoverImg`">

      <div class="d-md-flex justify-content-between align-items-end position">
        <img class="account-picture" :src="profileProp.picture" :alt="profileProp.name">
      </div>
    </div>
  </section>

  <section class="row justify-content-between">
    <div class="offset-md-2 col-10 col-md-8 order-2 order-md-1 d-flex justify-content-md-center align-items-center pt-3">
      <div class="fs-1 fw-bold text-center text-break text-uppercase">{{ profileProp.name }}</div>
    </div>

    <div v-if="account.id == profileProp.id" class="col-12 col-md-2 order-1 order-md-2 text-end">
      <button class="btn btn-lg btn-block mdi mdi-pencil fs-3" data-bs-toggle="modal" data-bs-target="#accountForm" title="Edit Account"></button>
    </div>
    <div v-else class="col-12 col-md-2 order-1 order-md-2 text-end py-4"></div>
  </section>

  <section class="row justify-content-center">
    <div class="col-12 col-md-9 py-3 fs-5 text-break">
      {{ profileProp.bio }}
    </div>
  </section>
</template>

<script>
import { computed } from 'vue'
import { AppState } from '../AppState.js'

export default {
  props: {
    profileProp: {
      type: Object,
      required: true
    }
  },

  setup() {
    return {
      account: computed(() => AppState.account)
    }
  }
}
</script>

<style lang="scss" scoped>
  .account-picture {
    width: 20vh;
    height: 20vh;
    object-fit: cover;
    object-position: center;
    border-radius: 50%;
  }

  .position {
    position: absolute;
    bottom: -10vh;
    left: 5vw;
  }

  .cover-image {
    object-fit: cover;
    object-position: center;
    width: 100%;
    height: 50vh;
  }

  .text-stroke {
    height: 8vh;
    -webkit-text-stroke-width: 2px;
    -webkit-text-stroke-color: black;
  }
</style>