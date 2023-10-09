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

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
    })

    onBeforeUnmount(() => {
      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else {
        saveCharacter()
      }
    })

    function saveCharacter() {
      charactersService.saveCharacter(editable.value)
    }

    return {
      editable,

      changeCharPage() {
        charactersService.changeCharPage(2)
        router.push({ name: 'CreateCharacter', params: { characterStep: 'personality-traits' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>