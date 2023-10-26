<template>
  <form @submit.prevent="changeCharPage()" class="row g-3">
    <div class="col-12 fs-3 fw-bold text-end">
      <button @click="randomizeAtt()" type="button" class="btn btn-primary">Randomize</button>
    </div>

    <div class="col-12">
      <draggable v-model="editable" class="row dragArea g-3" :force-fallback="true" animation="300" easing="cubic-bezier(1, 0, 0, 1)">
        <div class="col-12 col-sm-6 col-md-3 col-lg-2 draggable" v-for="(a, i) in attributes" :key="a">
          <div class="d-flex justify-content-center align-items-center">
            <p class="fs-5 px-2">{{ a.toUpperCase() }}</p>
            <router-link :to="{ name: 'Info', params: { infoId: 'ability-scores', infoDetails: a } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
          </div>

          <div class="d-flex justify-content-around align-items-center p-2">
            <label :for="a"></label>
            <input v-model="editable[i]" type="number" :id="a" class="form-control fs-4" style="width: 66.66% !important;" min="1" max="20" required>
            <p class="fs-3 text-center">{{ Math.floor((editable[i] - 10) / 2) }}</p>
          </div>

          <div class="d-flex justify-content-around align-items-center px-2 fs-5">
            <p class="pe-2">Racial Bonus:</p>
            <p v-if="bonus[a]">+{{ bonus[a] }}</p>
            <p v-else>+0</p>
          </div>
        </div>
      </draggable>
    </div>

    <div class="col-12 text-end">
      <button type="submit" class="btn btn-primary">Save</button>
    </div>
  </form>
</template>

<script>
import { computed, defineComponent, onBeforeUnmount, onMounted, ref } from 'vue'
import { VueDraggableNext } from 'vue-draggable-next'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { charactersService } from '../services/CharactersService.js'

export default defineComponent({
  data() {
    const router = useRouter()
    const editable = ref([])
    const attributes = AppState.attributes

    onMounted(() => {
      let temp = AppState.tempCharacter
      editable.value.id = temp?.id

      for (let a in attributes) {
        let att = attributes[a]

        if (temp ? !temp[att] : true) {
          editable.value[a] = 8
        } else {
          editable.value[a] = temp[att]
        }
      }
    })

    onBeforeUnmount(() => {
      let temp = AppState.tempCharacter

      if (!temp.manual) {
        return
      }

      for (let a in attributes) {
        temp[attributes[a]] = editable.value[a]
      }
      temp.manual = true

      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else {
        saveCharacter(temp)
      }
    })

    function saveCharacter(temp) {
      charactersService.saveCharacter(temp)
    }

    return {
      editable,
      attributes,
      enabled: true,
      dragging: false,
      bonus: computed(() => AppState.tempCharacter.bonus),

      randomizeAtt() {
        for (let e in editable.value) {
          editable.value[e] = Math.ceil(Math.random() * 20)
        }
      },

      changeCharPage() {
        charactersService.changeCharPage(4)
        router.push({ name: 'CreateCharacter', params: { characterStep: 'proficiencies' } })
      }
    }
  },
  components: {
    draggable: VueDraggableNext
  }
})
</script>

<style lang="scss" scoped>
  .draggable {
    cursor: grab;
    border: 2px dashed white;
  }
</style>