<template>
  <ul class="nav nav-tabs">
    <li @click="selectable = 'Inventory'" :class="{ 'active': selectable == 'Inventory' }" class="nav-item flex-grow-1 text-center p-2 selectable rounded" data-bs-toggle="offcanvas" data-bs-target="#characterOffcanvas" aria-controls="characterOffcanvas">
      <p class="fw-bold">Inventory</p>
    </li>

    <li @click="selectable = 'Features and Traits'" :class="{ 'active': selectable == 'Features and Traits' }" class="nav-item flex-grow-1 text-center p-2 selectable rounded" data-bs-toggle="offcanvas" data-bs-target="#characterOffcanvas" aria-controls="characterOffcanvas">
      <p class="fw-bold">Features and Traits</p>
    </li>

    <li @click="selectable = 'Background'" :class="{ 'active': selectable == 'Background' }" class="nav-item flex-grow-1 text-center p-2 selectable rounded" data-bs-toggle="offcanvas" data-bs-target="#characterOffcanvas" aria-controls="characterOffcanvas">
      <p class="fw-bold">Background</p>
    </li>

    <!-- <li @click="selectable = 'Notes'" :class="{ 'active': selectable == 'Notes' }" class="nav-item flex-grow-1 text-center p-2 selectable rounded" data-bs-toggle="offcanvas" data-bs-target="#characterOffcanvas" aria-controls="characterOffcanvas">
      <p class="fw-bold">Notes</p>
    </li> -->
  </ul>

  <div class="offcanvas offcanvas-start bg-dark" tabindex="-1" id="characterOffcanvas" aria-labelledby="characterOffcanvasLabel">
    <div class="offcanvas-header">
      <h5 class="offcanvas-title" id="characterOffcanvasLabel">{{ selectable }}</h5>
      <i type="button" class="mdi mdi-close text-light fs-5" data-bs-dismiss="offcanvas" aria-label="Close"></i>
    </div>
    <div class="offcanvas-body">

      <div v-if="selectable == 'Inventory'">
        <div class="bg-light mx-0 p-2 rounded elevation-5"></div>
      </div>

      <div v-else-if="selectable == 'Features and Traits'">
        <section class="row bg-light mx-0 my-2 p-2 rounded elevation-5">
          <div class="col-4 text-center">
            <p>Age: {{ characterProp.age }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Height: {{ characterProp.feet }}' {{ characterProp.inches }}"</p>
          </div>
          <div class="col-4 text-center">
            <p>Weight: {{ characterProp.weight }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Eyes: {{ characterProp.eyes }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Skin: {{ characterProp.skin }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Hair: {{ characterProp.hair }}</p>
          </div>
        </section>

        <div class="my-2 px-3 py-1 bg-light rounded elevation-5">
          <p class="fs-4">Features:</p>
          <p class="px-2">{{ characterProp.features }}</p>
        </div>
        <div class="my-2 px-3 py-1 bg-light rounded elevation-5">
          <p class="fs-4">Traits:</p>
          <p class="px-2">{{ characterProp.personalityTraits }}</p>
        </div>
      </div>

      <div v-else-if="selectable == 'Background'">
        <div class="bg-light my-2 p-2 rounded elevation-5">
          <p class="fs-4">Background: {{ characterProp.background }}</p>
          <p>
            <span class="fs-4">Backstory: </span>
            {{ characterProp.backstory }}
          </p>
        </div>

        <div class="my-2 px-3 py-2 bg-light rounded elevation-5">
          <p class="fs-4">Ideal:</p>
          <p class="px-2">{{ characterProp.features }}</p>
        </div>
        <div class="my-2 px-3 py-2 bg-light rounded elevation-5">
          <p class="fs-4">Flaws:</p>
          <p class="px-2">{{ characterProp.personalityTraits }}</p>
        </div>
        <div class="my-2 px-3 py-2 bg-light rounded elevation-5">
          <p class="fs-4">Bonds:</p>
          <p class="px-2">{{ characterProp.personalityTraits }}</p>
        </div>
      </div>

      <div v-else-if="selectable == 'Notes'">
        <div class="bg-light my-2 p-2 rounded elevation-5">
          <p class="fs-4">Background: {{ characterProp.background }}</p>
          <p>
            <span class="fs-4">Backstory: </span>
            {{ characterProp.backstory }}
          </p>
        </div>

        <div class="my-2 px-3 py-2 bg-light rounded elevation-5">
          <p class="fs-4">Ideal:</p>
          <p class="px-2">{{ characterProp.features }}</p>
        </div>
        <div class="my-2 px-3 py-2 bg-light rounded elevation-5">
          <p class="fs-4">Flaws:</p>
          <p class="px-2">{{ characterProp.personalityTraits }}</p>
        </div>
        <div class="my-2 px-3 py-2 bg-light rounded elevation-5">
          <p class="fs-4">Bonds:</p>
          <p class="px-2">{{ characterProp.personalityTraits }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { onMounted, ref } from 'vue'
import { Character } from '../models/Character.js'

export default {
  props: {
    characterProp: {
      type: Character,
      required: true
    }
  },

  setup() {
    const selectable = ref('')

    onMounted(() => {
      document.getElementById('characterOffcanvas').addEventListener('hidden.bs.offcanvas', () => {
        selectable.value = ''
      })
    })

    return {
      selectable
    }
  }
}
</script>

<style lang="scss" scoped>
  .active,
  .nav-item {
    transition: .5s;
  }

  .active,
  .nav-item:hover {
    color: var(--wheat);
    background: linear-gradient(145deg, #000000, #23272b);
    box-shadow: 10px 10px 19px #000000, -10px -10px 19px #262a2e;
  }
</style>