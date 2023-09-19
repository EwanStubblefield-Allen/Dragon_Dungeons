<template>
  <div>
    hi
  </div>
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

<style scoped lang="scss"></style>