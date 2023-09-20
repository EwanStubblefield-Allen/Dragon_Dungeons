<template>
  <p class="fs-1 fw-bold m-3 py-2">Equipment</p>

  <form @submit.prevent="finishCreation()" class="row g-3 bg-dark text-dark m-3 p-3 rounded elevation-5">
    <div v-for="(start, index) in starting" :key="start" class="col-12 form-group">
      <label class="text-light text-capitalize" :for="`equipment${index}`">
        {{ start.desc.replaceAll(/\(.\)/g, '') }}
        <router-link :to="{ name: 'Info', params: { infoId: 'equipment', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <div>
        <select v-model="editable.equipment[index]" @change="checkOptions(index)" :id="`equipment${index}`" class="form-select text-capitalize" required>
          <option v-for="s in start.from" :key="s.index" :value="s">
            <p v-if="Array.isArray(s)">
              <span v-for="equip in s" :key="equip.index">
                {{ equip.choose ?? equip.count }} {{ `${equip.name} ` }}
              </span>
            </p>
            <p v-else>{{ s.count ?? s.choose }} {{ s.name }}</p>
          </option>
        </select>
        <div v-if="list[index]?.length">
          <section v-if="!Array.isArray(editable.equipment[index])" class="row pt-3">
            <p @click="addPro(sel, editable.equipment[index].choose, index)" v-for="sel in list[index]" :key="sel.index" :class="{ 'bg-light text-dark elevation-5': selectable[index]?.find(s => s.name == sel.name) }" class="col-6 col-sm-4 col-md-3 col-lg-2 p-2 text-light text-center selectable rounded">{{ sel.name }}</p>
          </section>
          <section v-else class="row pt-3">
            <p @click="addPro(sel, false, index)" v-for="sel in list[index]" :key="sel.index" :class="{ 'bg-light text-dark elevation-5': selectable[index]?.find(s => s.name == sel.name) }" class="col-6 col-sm-4 col-md-3 col-lg-2 p-2 text-light text-center selectable rounded">{{ sel.name }}</p>
          </section>
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

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const selectable = ref([])
    const starting = ref([])
    const list = ref([])
    let isFinished = false

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
      selectable.value.forEach((select, index) => {
        if (select.length) {
          if (Array.isArray(editable.value.equipment[index])) {
            editable.value.equipment[index] = editable.value.equipment[index].filter(e => !e.choose)
            editable.value.equipment[index].push(select)
          } else {
            editable.value.equipment[index] = select
          }
        }
      })

      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else if (isFinished) {
        createCharacter()
      } else {
        saveCharacter()
      }
    })

    async function createCharacter() {
      try {
        charactersService.createCharacter(editable.value)
      } catch (error) {
        Pop.error(error.message, '[CREATING CHARACTER]')
      }
    }

    function saveCharacter() {
      charactersService.saveCharacter(editable.value)
    }

    async function getInfo() {
      try {
        let selectedClass = await infosService.getInfoDetails(`api/classes/${AppState.tempCharacter.class.toLowerCase()}`, false)
        starting.value = await Promise.all(selectedClass.starting_equipment_options.map(async s => {
          if (!Array.isArray(s.from.options)) {
            s.from = (await infosService.getInfoDetails(s.from.equipment_category.url, false)).equipment
            s.from.forEach(o => o.count ? o.count : o.count = 1)
          } else {
            s.from = s.from.options.map(option => {
              if (option.choice) {
                option.choice.from.equipment_category.choose = option.choice.choose
                option = option.choice.from.equipment_category
              } else {
                if (option.items) {
                  option = option.items.map(item => {
                    if (item.choice) {
                      item.choice.from.equipment_category.choose = item.choice.choose
                      return item.choice.from.equipment_category
                    } else {
                      item.of.count = item.count
                      return item.of
                    }
                  })
                } else if (option.of) {
                  option.of.count = option.count
                  option = option.of
                }
              }
              return option
            })
          }
          return s
        }))
        editable.value.equipment.forEach(async (equip, i) => {
          if (Array.isArray(equip)) {
            const foundIndex = starting.value[i].from.findIndex(fro => {
              if (Array.isArray(fro)) {
                let isSomewhatEqual = fro.findIndex(f => equip.find(e => JSON.stringify(e) == JSON.stringify(f)))

                if (isSomewhatEqual >= 0) {
                  equip = equip[isSomewhatEqual]
                  return true
                }
              } else {
                if (fro.choose == equip.length) {
                  return true
                }
              }
            })

            if (foundIndex == -1) {
              return
            }
            editable.value.equipment[i] = starting.value[i].from[foundIndex]
            await checkOptions(i)
            selectable.value[i] = equip
          }
        })
      } catch (error) {
        Pop.error(error.message, '[GETTING SPELLS]')
      }
    }

    async function checkOptions(index) {
      let option = editable.value.equipment[index]

      if (Array.isArray(option)) {
        option.forEach(o => {
          getOptions(o, index)
        })
      } else {
        getOptions(option, index)
      }
    }

    async function getOptions(option, index) {
      if (!option.choose) {
        selectable.value[index] = []
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

      addPro(item, length, index) {
        if (!length) {
          length = 0
          editable.value.equipment[index].forEach(e => {
            if (e.choose) {
              length += e.choose
            }
          })
        }

        if (!selectable.value[index]) {
          selectable.value[index] = []
        }

        if (selectable.value[index].includes(item)) {
          return
        }
        selectable.value[index].push(item)

        if (selectable.value[index].length > length) {
          selectable.value[index].shift()
        }
      },

      async finishCreation() {
        const isSure = await Pop.confirm('Are you sure you are finished creating this character?')

        if (!isSure) {
          return
        }
        isFinished = true
        router.push({ name: 'Home' })
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>