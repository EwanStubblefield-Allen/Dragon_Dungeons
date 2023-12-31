<template>
  <div class="m-3">
    <p class="fs-1 fw-bold py-2">Proficiencies</p>

    <ul v-if="options[1]" class="nav nav-tabs">
      <li class="nav-item">
        <p @click="selectable = [0, 'skills']" :class="{ 'active elevation-5': !selectable[0] }" class="nav-link selectable text-dark fw-bold">Skills</p>
      </li>
      <li class="nav-item">
        <p @click="selectable = [1, 'proficiencies']" :class="{ 'active elevation-5': selectable[0] }" class="nav-link selectable text-dark fw-bold">Other</p>
      </li>
    </ul>

    <div class="bg-dark p-3 rounded-bottom elevation-5">
      <div v-for="(opt, index) in options" :key="opt">
        <form v-if="selectable[0] == index" @submit.prevent="!options[1] || selectable[0] ? changeCharPage() : selectable = [1, 'proficiencies']" class="row g-3">
          <div class="col-12">
            <p class="fs-3 fw-bold">Choose {{ opt.choose }} {{ selectable[1] }}
              <router-link :to="{ name: 'Info', params: { infoId: selectable[1], infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
            </p>
            <section class="row align-items-center p-2">
              <p @click="addPro(o.item, opt.choose)" v-for="o in opt.from.options" :key="o.item.name" :class="{ 'bg-light text-dark elevation-5': editable[selectable[1]]?.includes(o.item) }" class="col-6 col-sm-4 col-md-3 col-lg-2 my-1 p-2 text-center text-break selectable rounded">{{ o.item.name.replace('Skill: ', '') }}</p>
            </section>
          </div>

          <div class="col-12 text-end">
            <button type="submit" class="btn btn-primary" :class="{ 'btn-secondary disabled': editable[selectable[1]]?.length != opt.choose }">Save</button>
          </div>
        </form>
      </div>
    </div>
  </div>
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
    const selectable = ref([0, 'skills'])
    const options = ref([])

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
      getOptions()
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

    async function getOptions() {
      try {
        const selectedClass = AppState.tempClass
        selectedClass.proficiency_choices.forEach(choice => {
          if (!choice.from.options[0].item) {
            let template = []
            choice.from.options.forEach(option => {
              template = template.concat(option.choice.from.options)
            })
            choice.from.options = template
          }
        })
        options.value = selectedClass.proficiency_choices
      } catch (error) {
        Pop.error(error.message, '[GETTING OPTIONS]')
      }
    }

    return {
      editable,
      selectable,
      options,

      changeCharPage() {
        charactersService.changeCharPage(5)
        router.push({ name: 'CreateCharacter', params: { characterStep: 'spells' } })
      },

      addPro(item, length) {
        delete item.index
        let type = selectable.value[1]

        if (!editable.value[type]) {
          editable.value[type] = []
        }
        const foundIndex = editable.value[type].findIndex(e => e == item)

        if (foundIndex > -1) {
          return editable.value[type].splice(foundIndex, 1)
        }
        editable.value[type].push(item)

        if (editable.value[type].length > length) {
          editable.value[type].shift()
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>