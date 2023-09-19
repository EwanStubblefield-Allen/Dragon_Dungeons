<template>
  <p class="fs-1 fw-bold m-3 py-2">Equipment</p>

  <form @submit.prevent="finishCreation()" class="row g-3 bg-dark text-dark m-3 p-3 rounded elevation-5">
    <div v-for="(start, index) in starting" :key="start" class="col-12 form-group">
      <label class="text-light text-capitalize" for="equipment">
        {{ start.desc.replaceAll(/\(.\)/g, '') }}
        <router-link :to="{ name: 'Info', params: { infoId: 'equipment', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <div class="input-group">
        <select v-model="editable.equipment[index]" @change="checkOptions(index)" id="equipment" class="form-select text-capitalize">
          <option v-for="s in start.from" :key="s" :value="s">{{ s.count ?? s.choose }} {{ s.name }}</option>
        </select>
        <div v-if="list[index]?.length" class="input-group">
          <select v-for="i in editable.equipment[index].choose" :key="i" v-model="selectable[index]" id="class1" class="form-select" required>
            <option v-for="sel in list[index]" :key="sel" :value="sel">{{ sel.name }}</option>
          </select>
        </div>
      </div>
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
import { infosService } from '../services/InfosService.js'
import Pop from '../utils/Pop.js'
import { logger } from '../utils/Logger.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const selectable = ref([])
    const starting = ref([])
    const list = ref([])

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }

      if (!editable.value.equipment) {
        editable.value.equipment = []
      }

      if (!AppState.tempCharacter.class) {
        return
      }
      getInfo()
    })

    onBeforeUnmount(() => {
      if (selectable.value.length) {
        selectable.value.forEach((s, i) => {
          if (s.name) {
            s.choose = 1
            editable.value.equipment[i] = s
          }
        })
      }

      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else if (editable.value.id) {
        updateCharacter()
      }
      else {
        createCharacter()
      }
    })

    function createCharacter() {
      try {
        charactersService.createTempCharacter(editable.value)
      }
      catch (error) {
        Pop.error(error.message, '[CREATING CHARACTER]')
      }
    }

    function updateCharacter() {
      try {
        charactersService.updateTempCharacter(editable.value)
      }
      catch (error) {
        Pop.error(error.message, '[UPDATING CHARACTER]')
      }
    }

    async function getInfo() {
      try {
        // FIXME Fighter and other classes with multiple options
        let selectedClass = await infosService.getInfoDetails(`api/classes/${AppState.tempCharacter.class.toLowerCase()}`, false)
        starting.value = await Promise.all(selectedClass.starting_equipment_options.map(async s => {
          if (!Array.isArray(s.from.options)) {
            s.from = (await infosService.getInfoDetails(s.from.equipment_category.url, false)).equipment
            s.from.forEach(o => o.count ? o.count : o.count = 1)
          } else {
            s.from = s.from.options.map(o => {
              if (o.choice) {
                o.choice.from.equipment_category.choose = o.choice.choose
                o = o.choice.from.equipment_category
              } else {
                if (o.items) {
                  o = o.items[0]
                }

                if (o.of) {
                  o.of.count = o?.count
                  o = o.of
                }
              }
              return o
            })
          }
          return s
        }))
        logger.log(starting.value)
        editable.value.equipment.forEach(async (e, i) => {
          if (e.choose) {
            editable.value.equipment[i] = starting.value[i].from[starting.value[i].from.length - 1]
            await checkOptions(i)
            delete e.choose
            selectable.value[i] = e
          }
        })
      } catch (error) {
        Pop.error(error.message, '[GETTING SPELLS]')
      }
    }

    async function checkOptions(index) {
      let option = editable.value.equipment[index]

      if (!option.choose) {
        return list.value[index] = []
      }
      let choose = await infosService.getInfoDetails(option.url, false)
      list.value[index] = choose.equipment
    }

    return {
      editable,
      selectable,
      starting,
      list,
      checkOptions,

      async finishCreation() {
        const isSure = await Pop.confirm('Are you sure you are finished creating this character?')

        if (!isSure) {
          return
        }
        router.push({ name: 'Home' })
        charactersService.changeCharPage(0)
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>