<template>
  <form @submit.prevent="sendTrade()" @change="editable.equal = false" class="row">
    <div class="col-lg-6">
      <p class="fs-3 fw-bold">Offers</p>
      <form @submit.prevent="addEquipment('offer')" class="d-flex justify-content-end">
        <div class="input-group">
          <select v-model="temp.offer" id="offerSelectMonster" class="form-select" aria-label="Select Equipment" required>
            <option disabled>Add Equipment</option>
            <option v-for="e in account.id == character?.creatorId ? character?.equipment : equipment" :key="e.index" :value="e">
              {{ e.name }}
            </option>
          </select>
          <router-link :to="temp.offer.url.replace('api', 'info')" v-if="temp.offer" target="_blank" class="mdi mdi-information text-primary selectable input-group-text" title="Learn more"></router-link>
          <button class="mdi mdi-plus input-group-text" type="submit" title="Add Equipment"></button>
        </div>
      </form>
      <section class="row py-2">
        <div v-for="(o, index) in editable.offer.equipment" :key="o.index" class="col-12 col-sm-6 d-flex">
          <router-link :to="o.url.replace('api', 'info')" target="_blank" class="fs-5 fw-bold text-dark">
            <p class="px-2 text-decoration-underline">{{ o.count }} {{ o.name }}</p>
          </router-link>
          <div>
            <i @click="removeEquipment('offer', index)" class="mdi mdi-delete fs-5 text-danger selectable"></i>
          </div>
        </div>
      </section>

      <section class="row justify-content-around py-2">
        <div class="col-2 form-group text-center">
          <label for="offerCp">Cp</label>
          <input v-model="editable.offer.currency[0]" type="number" class="form-control" id="offerCp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="offerSp">Sp</label>
          <input v-model="editable.offer.currency[1]" type="number" class="form-control" id="offerSp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="offerEp">Ep</label>
          <input v-model="editable.offer.currency[2]" type="number" class="form-control" id="offerEp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="offerGp">Gp</label>
          <input v-model="editable.offer.currency[3]" type="number" class="form-control" id="offerGp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="offerPp">Pp</label>
          <input v-model="editable.offer.currency[4]" type="number" class="form-control" id="offerPp" min="0" max="5000">
        </div>
      </section>
    </div>

    <div class="col-lg-6 text-lg-end">
      <p class="fs-3 fw-bold">Wants</p>
      <form @submit.prevent="addEquipment('want')" class="d-flex justify-content-end">
        <div class="input-group">
          <select v-model="temp.want" id="wantSelectMonster" class="form-select" aria-label="Select Equipment" required>
            <option disabled>Add Equipment</option>
            <option v-for="e in account.id == character?.creatorId ? equipment : character?.equipment" :key="e.index" :value="e">
              {{ e.name }}
            </option>
          </select>
          <router-link :to="temp.want.url.replace('api', 'info')" v-if="temp.want" target="_blank" class="mdi mdi-information text-primary selectable input-group-text" title="Learn more"></router-link>
          <button class="mdi mdi-plus input-group-text" type="submit" title="Add Equipment"></button>
        </div>
      </form>
      <section class="row py-2">
        <div v-for="(w, index) in editable.want.equipment" :key="w.index" class="col-12 col-sm-6 d-flex">
          <router-link :to="w.url.replace('api', 'info')" target="_blank" class="fs-5 fw-bold text-dark">
            <p class="px-2 text-decoration-underline">{{ w.count }} {{ w.name }}</p>
          </router-link>
          <div>
            <i @click="removeEquipment('want', index)" class="mdi mdi-delete fs-5 text-danger selectable"></i>
          </div>
        </div>
      </section>

      <section class="row justify-content-around py-2">
        <div class="col-2 form-group text-center">
          <label for="wantCp">Cp</label>
          <input v-model="editable.want.currency[0]" type="number" class="form-control" id="wantCp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="wantSp">Sp</label>
          <input v-model="editable.want.currency[1]" type="number" class="form-control" id="wantSp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="wantEp">Ep</label>
          <input v-model="editable.want.currency[2]" type="number" class="form-control" id="wantEp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="wantGp">Gp</label>
          <input v-model="editable.want.currency[3]" type="number" class="form-control" id="wantGp" min="0" max="5000">
        </div>
        <div class="col-2 form-group text-center">
          <label for="wantPp">Pp</label>
          <input v-model="editable.want.currency[4]" type="number" class="form-control" id="wantPp" min="0" max="5000">
        </div>
      </section>
    </div>
    <div class="text-end pt-2">
      <button v-if="editable.equal" class="btn btn-success" type="submit">Accept</button>
      <button v-else class="btn btn-success" type="submit">Trade</button>
    </div>
  </form>
</template>

<script>
import { computed, onMounted, ref, watchEffect } from 'vue'
import { AppState } from '../AppState.js'
import { infosService } from '../services/InfosService.js'
import { charactersService } from '../services/CharactersService.js'
import { campaignHub } from '../handlers/CampaignHub.js'
import { Modal } from 'bootstrap'

export default {
  setup() {
    const editable = ref({})
    const temp = ref({})
    const equipment = ref([])

    onMounted(() => {
      document.getElementById('trade').addEventListener('show.bs.modal', async () => {
        if (!equipment.value.length) {
          equipment.value = (await infosService.getInfoDetails('api/equipment', false)).results
        }
      })
    })

    watchEffect(() => {
      editable.value = { ...AppState.trade }
    })

    return {
      editable,
      temp,
      equipment,
      account: computed(() => AppState.account),
      character: computed(() => AppState.activeCharacter),

      sendTrade() {
        editable.value.id = this.character.id
        let location

        if (this.character.creatorId == AppState.account.id) {
          editable.value.player = true

          if (!editable.value.equal) {
            editable.value.dm = false
          }
          location = AppState.activeCampaign.creatorId
        } else {
          editable.value.dm = true

          if (!editable.value.equal) {
            editable.value.player = false
          }
          location = this.character.creatorId
        }

        if (editable.value.dm && editable.value.player) {
          charactersService.handleTrade(editable.value)
        }
        campaignHub.trade(location, editable.value)
        Modal.getOrCreateInstance('#trade').hide()
      },

      addEquipment(type) {
        const foundIndex = editable.value[type].equipment.findIndex(o => o.index == temp.value[type].index)

        if (foundIndex > -1) {
          editable.value[type].equipment[foundIndex].count++
        } else {
          temp.value[type].count = 1
          editable.value[type].equipment.push(temp.value[type])
        }
        temp.value[type] = null
        editable.value.equal = false
      },

      removeEquipment(type, index) {
        if (editable.value[type].equipment[index].count > 1) {
          editable.value[type].equipment[index].count--
        } else {
          editable.value[type].equipment.splice(index, 1)
        }
        editable.value.equal = false
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>