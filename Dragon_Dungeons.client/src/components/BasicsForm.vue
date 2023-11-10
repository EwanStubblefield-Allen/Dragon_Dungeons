<template>
  <p class="fs-1 fw-bold m-3 py-2">Basics</p>

  <form @submit.prevent="changeCharPage()" class="row g-3 bg-dark m-3 p-3 rounded elevation-5">
    <div class="col-md-12 form-floating">
      <input v-model="editable.name" type="text" class="form-control" id="name" minlength="3" maxlength="100" placeholder="Name..." required>
      <label for="name" class="text-dark">Character Name:</label>
    </div>

    <div class="col-md-4 form-group">
      <label for="race">
        Race:
        <router-link :to="{ name: 'Info', params: { infoId: 'races', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select @change="areYouSure('race')" v-model="editable.race" id="race" class="form-select" required>
        <option v-for="r in race" :key="r">{{ r }}</option>
      </select>
    </div>

    <div class="col-md-4 form-group">
      <label for="class">
        Class:
        <router-link :to="{ name: 'Info', params: { infoId: 'classes', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select @change="areYouSure('class')" v-model="editable.class" id="class" class="form-select" required>
        <option v-for="c in dndClass" :key="c">{{ c }}</option>
      </select>
    </div>

    <div class="col-md-4 form-group">
      <label for="alignment">
        Alignment:
        <router-link :to="{ name: 'Info', params: { infoId: 'alignments', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select v-model="editable.alignment" id="alignment" class="form-select" required>
        <option v-for="a in alignment" :key="a">{{ a }}</option>
      </select>
    </div>

    <div class="col-12 col-md-6">
      <div class="form-check">
        <input v-model="imageType" class="form-check-input" type="radio" name="generateImg" id="generateImg" value="generateImg">
        <label class="form-check-label" for="generateImg">
          Generate Image
        </label>
      </div>
      <div class="form-check">
        <input v-model="imageType" class="form-check-input" type="radio" name="importImg" id="importImg" value="importImg">
        <label class="form-check-label" for="importImg">
          Import Image
        </label>
      </div>
    </div>

    <form v-if="imageType == 'generateImg' && editable.race && editable.class" @submit.prevent="generateImg()" class="col-12 col-md-6 form-group">
      <label for="picture">Generate Character Picture:</label>
      <div class="input-group">
        <input v-model="description.prompt" type="text" class="form-control" id="picture" minlength="3" maxlength="100" placeholder="Description..." required>
        <button v-if="!loading" type="submit" class="mdi mdi-plus input-group-text" title="Generate Image"></button>
      </div>
    </form>

    <div v-else-if="imageType == 'importImg'" class="col-12 col-md-6 form-group">
      <label for="picture" class="text-dark">Character Picture:</label>
      <input @change="handleFile" type="file" class="form-control" id="picture" accept=".jpg, .jpeg, .png" required>
    </div>

    <div class="col-12">
      <div class="row align-items-end">
        <div v-if="loading">
          <Loader />
        </div>
        <div v-else-if="editable.picture" class="col-12 col-md-6 position-relative">
          <div class="d-flex icon-position">
            <i @click="removeImg()" class="mdi mdi-delete text-danger px-1 fs-5 rounded selectable" type="button" title="Delete Image"></i>
          </div>
          <img class="img-fluid w-100 rounded elevation-5" :src="editable.picture" alt="Character Image">
        </div>
        <div :class="{ 'col-md-6': editable.picture }" class="col-12 text-end pt-3">
          <button type="submit" :class="{ 'btn-secondary': !editable.picture }" class="btn btn-primary" :disabled="!editable.picture">Save</button>
        </div>
      </div>
    </div>
  </form>
</template>

<script>
import { onBeforeUnmount, onMounted, ref, watchEffect } from 'vue'
import { useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import { charactersService } from '../services/CharactersService.js'
import { openService } from '../services/OpenService.js'
import { saveState } from '../utils/Store.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const router = useRouter()
    const editable = ref({})
    const imageType = ref('generateImg')
    const description = ref({})
    const loading = ref(false)
    const dndClass = ['Barbarian', 'Bard', 'Cleric', 'Druid', 'Fighter', 'Monk', 'Paladin', 'Ranger', 'Rogue', 'Sorcerer', 'Warlock', 'Wizard']
    const race = ['Dragonborn', 'Dwarf', 'Elf', 'Gnome', 'Half-Elf', 'Half-Orc', 'Halfling', 'Human', 'Tiefling']
    const alignment = ['Chaotic Evil', 'Chaotic Good', 'Chaotic Neutral', 'Lawful Evil', 'Lawful Good', 'Lawful Neutral', 'Neutral', 'Neutral Evil', 'Neutral Good']

    onMounted(() => {
      editable.value = { ...AppState.tempCharacter }
    })

    onBeforeUnmount(() => {
      handleSave()
    })

    watchEffect(() => {
      description.value.prompt = `${editable.value.race} ${editable.value.class}`
    })

    async function handleSave() {
      if (editable.value.race && !editable.value.bonus) {
        await getRace()
      }

      if (editable.value.class && !AppState.tempClass.index) {
        await getClass()
      }

      if (JSON.stringify(editable.value) == '{}' || editable.value == AppState.tempCharacter) {
        return
      } else {
        saveCharacter()
      }
    }

    async function getRace() {
      try {
        const race = await infosService.getInfoDetails(`api/races/${editable.value.race.toLowerCase().replaceAll(' ', '-')}`, false)
        editable.value.speed = race.speed
        editable.value.bonus = {}
        race.ability_bonuses.forEach(b => editable.value.bonus[b.ability_score.index] = b.bonus)
      } catch (error) {
        Pop.error(error.message, '[GETTING RACE]')
      }
    }

    async function getClass() {
      try {
        const selectedClass = await infosService.getInfoDetails(`api/classes/${editable.value.class.toLowerCase()}`, false)
        selectedClass.proficiencies.forEach(p => delete p.index)
        editable.value.hitDie = selectedClass.hit_die
        AppState.tempClass = selectedClass
        saveState('tempClass', selectedClass)
      } catch (error) {
        Pop.error(error.message, '[GETTING CLASS]')
      }
    }

    function saveCharacter() {
      editable.value.creator = AppState.account
      charactersService.saveCharacter(editable.value)
    }

    return {
      editable,
      imageType,
      description,
      loading,
      dndClass,
      race,
      alignment,

      async areYouSure(type) {
        if (!AppState.tempCharacter[type]) {
          return
        }
        const isSure = await Pop.confirm('If you change this all progress will be reset!! Continue?')

        if (!isSure) {
          editable.value[type] = AppState.tempCharacter[type]
          return
        }
        const temp = editable.value[type]
        editable.value = {}
        charactersService.resetCharacter()
        editable.value[type] = temp
      },

      async handleFile(event, compress = .6) {
        try {
          editable.value.picture = ''
          const file = event.target.files[0]

          if (!file) {
            return
          }
          await openService.compress(file, compress)
          const picture = AppState.tempCharacter.picture
          let reader = new FileReader()

          reader.onload = (e) => {
            editable.value.picture = e.target.result
          }
          reader.readAsDataURL(picture)
        } catch (error) {
          Pop.error(error.message, '[HANDLING FILE]')

          if (error.message == 'Image is too large!!') {
            const compressMore = await Pop.confirm('Image is too large compress image more?')

            if (compressMore) {
              this.handleFile(event, compress - .2)
            }
          }
        }
      },

      async generateImg() {
        try {
          editable.value.picture = ''
          loading.value = true
          editable.value.picture = await openService.generateImg(description.value)
        } catch (error) {
          Pop.error(error.message, '[GENERATING IMAGE]')
        } finally {
          loading.value = false
        }
      },

      async removeImg() {
        const isSure = await Pop.confirm('Are you sure you want to delete this image')

        if (!isSure) {
          return
        }
        editable.value.picture = ''
      },

      changeCharPage() {
        charactersService.changeCharPage(0)
        router.push({ name: 'CreateCharacter', params: { characterStep: 'features' } })
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  img {
    object-fit: cover;
    object-position: center;
  }

  .icon-position {
    position: absolute;
    top: 5px;
    right: 20px;
    text-shadow: 0px 3px 5px var(--russian);
  }
</style>