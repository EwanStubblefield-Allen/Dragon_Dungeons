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
        <div class="bg-light mx-0 p-2 rounded elevation-5">
          <div v-for="(e, index) in characterProp.equipment" :key="e.index">
            <div class="d-flex justify-content-between align-items-center">
              <div class="d-flex">
                <p v-if="e.count">{{ e.count }}</p>
                <p v-else>1</p>
                <p class="px-2">{{ e.name }}</p>
                <a :href="`#/info${e.url.replace('/api', '')}`" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></a>
              </div>
              <button @click="equipItem(e, index)" class="btn btn-primary" type="button">Equip/Use</button>
            </div>
            <hr v-if="index != characterProp.equipment.length - 1" class="my-2">
          </div>
        </div>
      </div>

      <div v-else-if="selectable == 'Features and Traits'">
        <section class="row bg-light mx-0 my-2 p-2 rounded elevation-5">
          <div class="col-4 text-center">
            <p>Age:</p>
            <p>{{ characterProp.age }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Height:</p>
            <p>{{ characterProp.feet }}' {{ characterProp.inches }}"</p>
          </div>
          <div class="col-4 text-center">
            <p>Weight:</p>
            <p>{{ characterProp.weight }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Eyes:</p>
            <p class="text-capitalize">{{ characterProp.eyes }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Skin:</p>
            <p class="text-capitalize">{{ characterProp.skin }}</p>
          </div>
          <div class="col-4 text-center">
            <p>Hair:</p>
            <p class="text-capitalize">{{ characterProp.hair }}</p>
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
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

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
      selectable,

      async equipItem(equipment, index) {
        try {
          await charactersService.equipItem(equipment, index)
        } catch (error) {
          Pop.error(error.message, '[GETTING ITEM]')
        }
      }
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