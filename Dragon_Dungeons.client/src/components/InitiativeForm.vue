<template>
  <div v-if="initiative">
    <div v-if="initiative.intCount == initiative.intTotal">
    </div>

    <div v-else-if="!initiative.entities?.find(e => e.creatorId == account.id && !e.initiative)">
      <p class="fs-2 fw-bold text-center">Waiting on others to roll for initiative!</p>
    </div>

    <form v-else @submit.prevent="updateInitiative()" class="text-wrap">
      <div v-for="(e, index) in initiative.entities" :key="e.id">
        <div v-if="e.creatorId == account.id" class="form-group py-2">
          <label :for="`initiative${index}`">{{ e.name }}'s Initiative</label>
          <input v-model="editable[index]" type="number" class="form-control" :id="`initiative${index}`" min="1" max="20" required>
        </div>
      </div>
      <div class="text-end">
        <button class="btn btn-success mt-2" type="submit">Set Initiative</button>
      </div>
    </form>
  </div>
</template>

<script>
import { computed, ref } from 'vue'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref([])

    return {
      editable,
      account: computed(() => AppState.account),
      initiative: computed(() => AppState.activeCampaign?.initiative),

      async updateInitiative() {
        try {
          this.initiative.entities = this.initiative.entities.map((m, index) => {
            if (editable.value[index]) {
              m.initiative = editable.value[index]
              this.initiative.intTotal++
            }
            return m
          })
          await campaignsService.updateCampaign({ initiative: this.initiative }, AppState.activeCampaign.id)
        } catch (error) {
          Pop.error(error.message, '[UPDATING INITIATIVE]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>