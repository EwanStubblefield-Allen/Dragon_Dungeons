<template>
  <p class="fs-5">{{ level[0] }}{{ level[0] == 1 ? 'st' : level[0] == 2 ? 'nd' : level[0] == 3 ? 'rd' : 'th' }} Level:</p>
  <hr class="my-2">

  <draggable @change="findPlacement" :list="spells" class="dragArea list-group w-full" :force-fallback="true" :group="{ name: 'spells', pull: true, put: true }" :sort="true" animation="300" easing="cubic-bezier(1, 0, 0, 1)">
    <div class="row d-flex justify-content-around align-items-center list-group-item m-1" v-for="s in spells" :key="s.name">
      <CharacterMagic :magicProp="s" />
    </div>
  </draggable>
</template>

<script>
import { computed, defineComponent } from 'vue'
import { VueDraggableNext } from 'vue-draggable-next'
import { AppState } from '../AppState.js'
import CharacterMagic from './CharacterMagic.vue'

export default defineComponent({
  props: {
    level: {
      type: Array,
      required: true
    }
  },

  data(props) {
    return {
      enabled: true,
      dragging: false,
      spells: computed(() => AppState.equipment.spells[props.level[0]]),

      findPlacement(event) {
        const { added } = event

        if (added) {
          const foundIndex = Object.entries(AppState.equipment.spells).findIndex(s => s[1][added.newIndex] == added.element)
          AppState.equipment.spells[foundIndex + 1][added.newIndex].level = foundIndex + 1
          const newIndex = AppState.activeCharacter.spells.findIndex(s => s.name == added.element.name)
          AppState.activeCharacter.spells[newIndex].level = foundIndex + 1
        }
      }
    }
  },
  components: {
    draggable: VueDraggableNext,
    CharacterMagic
  }
})
</script>

<style lang="scss" scoped></style>