<template>
  <section class="row justify-content-center">
    <div class="col-10 py-3">
      <section class="masonry">
        <router-link :to="{ name: 'Character', params: { characterId: 'basics' } }" class="masonry-item border border-dark elevation-5">
          <p class="fs-1 fw-bold text-russian text-center">Build Character</p>
        </router-link>
        <router-link :to="{ name: 'Account' }" class="masonry-item border border-dark elevation-5">
          <p class="fs-1 fw-bold text-russian text-center">Account</p>
        </router-link>
        <button class="masonry-item border border-dark elevation-5" data-bs-toggle="collapse" data-bs-target="#infoCollapse" aria-expanded="false" aria-controls="infoCollapse">
          <p class="fs-1 fw-bold text-russian text-center">Info</p>
        </button>
        <router-link :to="{ name: 'Campaign', params: { campaignId: 'basics' } }" class="masonry-item border border-dark elevation-5">
          <p class="fs-1 fw-bold text-russian text-center">Create Campaign</p>
        </router-link>
      </section>
    </div>
  </section>
</template>

<script>
import { onMounted } from 'vue'
import { AppState } from '../AppState.js'

export default {
  setup() {
    onMounted(() => {
      AppState.charPage = 0
      AppState.tempCharacter = {}
    })

    return {}
  }
}
</script>

<style scoped lang="scss">
  .masonry {
    columns: 2 200px;
    column-gap: 1rem;

    a,
    button {
      display: inline-flex;
      justify-content: center;
      align-items: center;
      width: 100%;
    }

    .masonry-item:nth-child(1) {
      height: calc(var(--main-height)*.5);
      background-image: url(../assets/img/Character.png);
      background-position: center;
      background-size: cover;
      background-repeat: no-repeat;
      border-radius: 50px 5px 5px 5px;
      margin-bottom: 1rem;
    }

    .masonry-item:nth-child(2) {
      height: calc(var(--main-height)*.4);
      background-image: url(../assets/img/Dark.jpg);
      background-position: center;
      background-size: cover;
      background-repeat: no-repeat;
      border-radius: 5px 5px 5px 50px;
    }

    .masonry-item:nth-child(3) {
      height: calc(var(--main-height)*.4);
      background-image: url(../assets/img/Water.jpg);
      background-position: center;
      background-size: cover;
      background-repeat: no-repeat;
      border-radius: 5px 50px 5px 5px;
      margin-bottom: 1rem;
    }

    .masonry-item:nth-child(4) {
      height: calc(var(--main-height)*.5);
      background-image: url(../assets/img/Bridge.jpg);
      background-position: center;
      background-size: cover;
      background-repeat: no-repeat;
      border-radius: 5px 5px 50px 5px;
    }
  }

  .text-russian {
    text-shadow: 0px 3px 5px var(--wheat);
  }

  @media screen and (max-width: 543px) {
    .masonry-item {
      border-radius: 5px !important;
      margin-bottom: 1rem;
    }
  }
</style>