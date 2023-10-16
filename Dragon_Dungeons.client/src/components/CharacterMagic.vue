<template>
  <hr v-if="magicProp.level == 0" class="my-2">
  <div class="col-6 col-xl-4 d-flex align-items-center">
    <abbr v-if="magicProp.concentration" class="mdi mdi-meditation fs-6 pe-1" title="Concentration"></abbr>
    <abbr v-if="magicProp.ritual" class="mdi mdi-candelabra-fire fs-6 pe-1" title="Ritual"></abbr>
    <abbr v-if="magicProp.area_of_effect" class="mdi mdi-axis-arrow fs-6 pe-1" :title="`AOE: ${magicProp.area_of_effect.size}ft. ${magicProp.area_of_effect.type}`"></abbr>
    <p>{{ magicProp.name }}</p>
  </div>
  <div class="col-3 col-xl-2">
    <abbr class="no-select" :title="magicProp.casting_time">{{ magicProp.casting_time.replace(/ .*/g, (cast) => cast.charAt(1).toUpperCase()) }}</abbr>
  </div>
  <div class="col-3 col-xl-1">
    <abbr v-if="magicProp.damage" class="no-select" :title="magicProp.damage.damage_type.name">{{ handleDamage(magicProp.damage) }}</abbr>
    <p v-else>--</p>
  </div>
  <div class="col-4 col-xl-2 text-center text-xl-start">
    <p>{{ magicProp.range.replace(' feet', 'ft.') }}</p>
  </div>
  <div class="col-6 col-xl-3 d-flex justify-content-center justify-content-xl-start">
    <abbr v-for="c in magicProp.components" :key="c" class="pe-1 no-select" :title="c == 'V' ? 'Verbal' : c == 'S' ? 'Somatic' : 'Material'">{{ c }},</abbr>
    <abbr v-if="magicProp.duration" class="no-select" :title="magicProp.duration">Dur: {{ magicProp.duration.replace('Instantaneous', 'Inst').replace('Up to ', '').replace(/ .*/g, (dur) => dur.charAt(1).toUpperCase()) }}</abbr>
  </div>
</template>

<script>
import { AppState } from '../AppState.js'

export default {
  props: {
    magicProp: {
      type: Object,
      required: true
    }
  },

  setup(props) {
    return {
      handleDamage(damage) {
        if (damage.damage_at_slot_level) {
          return damage.damage_at_slot_level[props.spellLevel]
        }
        let tempLevel = AppState.activeCharacter.level

        while (!damage.damage_at_character_level[tempLevel]) {
          tempLevel--
        }
        return damage.damage_at_character_level[tempLevel]
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>