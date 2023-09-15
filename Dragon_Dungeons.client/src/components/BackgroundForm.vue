<template>
  <p class="fs-1 fw-bold m-3 py-2">Background</p>

  <form @submit.prevent="changeCharPage()" class="row g-3 bg-dark m-3 p-3 rounded elevation-5">
    <div class="col-md-12 form-floating">
      <input v-model="editable.background" type="text" class="form-control" id="background" minlength="3" maxlength="100" placeholder="Background..." required>
      <label class="text-dark" for="background">Background:</label>
    </div>
    <div class="col-12 form-group">
      <label for="backstory">Backstory:</label>
      <textarea v-model="editable.backstory" class="form-control" id="backstory" rows="10" minlength="1" maxlength="1000" placeholder="Backstory..." required></textarea>
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
        charactersService.changeCharPage(2)
        router.push({ name: 'Character', params: { characterId: 'personality-traits' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>