<template>
  <div class="m-3">
    <p class="fs-1 fw-bold py-2">Spells</p>

    <ul class="nav nav-tabs">
      <li class="nav-item" v-if="cantrips">
        <p @click="selectable = [0, 'cantrips']" :class="{ 'active elevation-5': !selectable[0] }" class="nav-link selectable text-dark fw-bold">Cantrips</p>
      </li>
      <li class="nav-item" v-if="spells">
        <p @click="selectable = [1, 'spells']" :class="{ 'active elevation-5': selectable[0] }" class="nav-link selectable text-dark fw-bold">Spells</p>
      </li>
    </ul>

    <div class="bg-dark p-3 rounded-bottom elevation-5">
      <form @submit.prevent="selectable = [1, 'spells']" v-if="selectable[0] == 0" class="row g-3">
        <div class="col-12">
          <p class="fs-3 fw-bold">Choose {{ cantrips.count }}
            <router-link :to="{ name: 'Info', params: { infoId: 'spells', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
          </p>
          <section class="row p-2">
            <p @click="addPro(c, cantrips.count)" v-for="c in cantrips.results" :key="c" :class="{ 'bg-light text-dark elevation-5': editable.cantrips?.find(cantrip => cantrip.name == c.name) }" class="col-6 col-sm-4 col-md-3 col-lg-2 p-2 text-center selectable rounded">{{ c.name }}</p>
          </section>
        </div>

        <div class="col-12 text-end">
          <button type="submit" :class="{ 'btn-secondary disabled': editable[selectable[1]]?.length != cantrips.count }" class="btn btn-primary">Save</button>
        </div>
      </form>

      <form @submit.prevent="changeCharPage()" v-else class="row g-3">
        <div class="col-12">
          <p class="fs-3 fw-bold">Choose {{ spells.count }}
            <router-link :to="{ name: 'Info', params: { infoId: 'spells', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
          </p>
          <section class="row align-items-center p-2">
            <p @click="addPro(s, spells.count)" v-for="s in spells.results" :key="s" :class="{ 'bg-light text-dark elevation-5': editable.spells?.find(spell => spell.name == s.name) }" class="col-6 col-sm-4 col-md-3 col-lg-2 p-2 text-center selectable rounded">{{ s.name }}</p>
          </section>
        </div>

        <div class="col-12 text-end">
          <button type="submit" :class="{ 'btn-secondary disabled': editable[selectable[1]]?.length != spells.count }" class="btn btn-primary">Save</button>
        </div>
      </form>
    </div>
  </div>
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
    const selectable = ref([0, 'cantrips'])
    const cantrips = ref({})
    const spells = ref({})

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
      getInfo()
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

    async function getInfo() {
      try {
        const selectedClass = AppState.tempCharacter.class.toLowerCase().replaceAll(' ', '-')
        const data = await infosService.getInfoDetails(`api/classes/${selectedClass}/levels/1`, false)
        editable.value.bonus.bonus = data.prof_bonus
        const casting = data.spellcasting

        if (!casting || !Object.values(casting).find(c => c > 0)) {
          return changeCharPage()
        }
        cantrips.value = await infosService.getInfoDetails('api/spells?level=0', false)
        cantrips.value.count = casting.cantrips_known
        spells.value = await infosService.getInfoDetails(`api/classes/${selectedClass}/levels/1/spells`, false)

        if (casting.spells_known) {
          spells.value.count = casting.spells_known
        } else {
          switch (selectedClass) {
            case 'wizard':
              spells.value.count = Math.floor((AppState.tempCharacter.int - 10) / 2) + 1
              break
            case 'cleric':
            case 'druid':
              spells.value.count = Math.floor((AppState.tempCharacter.wis - 10) / 2) + 1

              break
            default:
              Pop.error('Needs a case for spells')
              break
          }

          if (spells.value.count < 1) {
            spells.value.count = 1
          }
        }
      } catch (error) {
        Pop.error(error.message, '[GETTING SPELLS]')
      }
    }

    function changeCharPage() {
      charactersService.changeCharPage(6)
      router.push({ name: 'CreateCharacter', params: { characterStep: 'equipment' } })
    }

    return {
      editable,
      selectable,
      cantrips,
      spells,
      changeCharPage,

      addPro(item, length) {
        let type = selectable.value[1]

        if (!editable.value[type]) {
          editable.value[type] = []
        }

        if (editable.value[type].includes(item)) {
          return
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