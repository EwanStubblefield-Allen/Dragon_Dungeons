<template>
  <div class="m-3">
    <p class="fs-1 fw-bold py-2">Attributes</p>

    <ul class="nav nav-tabs">
      <li class="nav-item">
        <p @click="changeType(false)" :class="{ 'active elevation-5': !editable.manual }" class="nav-link selectable text-dark fw-bold">Points</p>
      </li>

      <li class="nav-item">
        <p @click="changeType(true)" :class="{ 'active elevation-5': editable.manual }" class="nav-link selectable text-dark fw-bold">Manual</p>
      </li>
    </ul>

    <div class="bg-dark p-3 rounded-bottom elevation-5">
      <div v-if="!editable.manual">
        <PointsCard />
      </div>

      <div v-else>
        <DragAndDrop />
      </div>
    </div>
  </div>
</template>

<script>
import { ref, watchEffect } from 'vue'
import { AppState } from '../AppState.js'
import PointsCard from './PointsCard.vue'
import DragAndDrop from './DragAndDrop.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({})

    watchEffect(() => {
      editable.value = { ...AppState.tempCharacter }
    })

    return {
      editable,

      async changeType(bool) {
        if (editable.value.manual == bool) {
          return
        }
        const isSure = await Pop.confirm('This will reset your attributes! Continue?')

        if (!isSure) {
          return
        }
        let temp = AppState.tempCharacter
        temp.manual = bool
        temp.str = temp.dex = temp.con = temp.int = temp.wis = temp.cha = 8
      }
    }
  },
  components: { PointsCard, DragAndDrop }
}
</script>

<style lang="scss" scoped></style>