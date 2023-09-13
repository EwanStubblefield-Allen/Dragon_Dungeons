<template>
  <section class="row">
    <div class="col-12 col-md-3 col-lg-2 overflow-auto infoBar">
      <section class="row p-3">
        <p class="fs-3 text-capitalize pb-2">Steps:</p>
        <hr>
        <div class="col-12 col-sm-6 col-md-12">
          <p @click="editable == 1" class="text-light selectable rounded p-2">Race</p>
          <hr class="my-2">
        </div>
      </section>
    </div>
  </section>
</template>

<script>
import { AppState } from '../AppState.js'
import { computed, onMounted, ref } from 'vue'

export default {
  setup() {
    const editable = ref()

    onMounted(() => {

    })

    return {
      editable
    }
  }
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