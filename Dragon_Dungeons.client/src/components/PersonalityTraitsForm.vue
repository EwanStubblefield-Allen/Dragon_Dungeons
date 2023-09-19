<template>
  <p class="fs-1 fw-bold m-3 py-2">Personality Traits</p>

  <form @submit.prevent="changeCharPage()" class="row g-3 bg-dark m-3 p-3 rounded elevation-5">
    <div class="col-6 form-group">
      <label for="personalityTraits">Personality Traits:</label>
      <textarea v-model="editable.personalityTraits" class="form-control" id="personalityTraits" rows="5" minlength="1" maxlength="500" placeholder="Personality Traits..." required></textarea>
    </div>

    <div class="col-6 form-group">
      <label for="ideals">Ideals:</label>
      <textarea v-model="editable.ideals" class="form-control" id="ideals" rows="5" minlength="1" maxlength="500" placeholder="Ideals..." required></textarea>
    </div>

    <div class="col-6 form-group">
      <label for="bonds">Bonds:</label>
      <textarea v-model="editable.bonds" class="form-control" id="bonds" rows="5" minlength="1" maxlength="500" placeholder="Bonds..." required></textarea>
    </div>

    <div class="col-6 form-group">
      <label for="flaws">Flaws:</label>
      <textarea v-model="editable.flaws" class="form-control" id="flaws" rows="5" minlength="1" maxlength="500" placeholder="Flaws..." required></textarea>
    </div>

    <div class="col-12 text-end">
      <button type="submit" class="btn btn-primary">Save</button>
    </div>
  </form>
</template>

<script>
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { charactersService } from '../services/CharactersService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})

    onBeforeUnmount(() => {
      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else if (editable.value.id) {
        updateCharacter()
      } else {
        createCharacter()
      }
    })

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
      let textElement = document.getElementById('personalityTraits')
      textElement.focus()
      textElement.setSelectionRange(textElement.value.length, textElement.value.length)
    })

    function createCharacter() {
      try {
        charactersService.createTempCharacter(editable.value)
      } catch (error) {
        Pop.error(error.message, '[CREATING CHARACTER]')
      }
    }

    function updateCharacter() {
      try {
        charactersService.updateTempCharacter(editable.value)
      } catch (error) {
        Pop.error(error.message, '[UPDATING CHARACTER]')
      }
    }

    return {
      editable,

      changeCharPage() {
        charactersService.changeCharPage(3)
        router.push({ name: 'Character', params: { characterId: 'attributes' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>