<template>
  <section v-if="campaign" class="row">
    <div class="col-12 col-md-6 h-md py-2">
      <div class="d-flex flex-column justify-content-between h-md-50">
        <div class="pb-2">
          <div class="d-flex justify-content-between">
            <p class="fs-2 fw-bold text-wrap text-truncate">{{ campaign.name }}</p>
            <button v-if="campaign.initiative.entities" class="btn btn-danger elevation-5" type="button" data-bs-toggle="modal" data-bs-target="#initiative">Battle Details</button>
          </div>
          <hr class="my-2">
          <p class="fs-5 px-2">{{ campaign.description }}</p>
        </div>

        <ul class="nav nav-tabs">
          <li class="nav-item">
            <p @click="selectable = 1" :class="{ 'active elevation-5': selectable == 1 }" class="nav-link selectable text-dark fw-bold">Public</p>
          </li>

          <li v-if="campaign.creatorId == account.id" class="nav-item">
            <p @click="selectable = 2" :class="{ 'active elevation-5': selectable == 2 }" class="nav-link selectable text-dark fw-bold">Private</p>
          </li>

          <li v-if="campaign.creatorId == account.id" class="nav-item">
            <p @click="selectable = 3" :class="{ 'active elevation-5': selectable == 3 }" class="nav-link selectable text-dark fw-bold">Events</p>
          </li>
        </ul>
      </div>

      <div v-if="selectable != 3" class="bg-dark rounded-bottom elevation-5 overflow-auto h-md-50 pb-2">
        <div class="row mx-0 p-2">
          <button v-if="campaign.creatorId == account.id" @click="handleNote(selectable)" class="col-12 btn btn-light" type="button" data-bs-toggle="modal" data-bs-target="#categoryForm">Category +</button>
        </div>
        <div v-for="(p, index) in selectable == 1 ? campaign.publicNotes : campaign.privateNotes" :key="p" class="row mx-0 px-2">
          <div class="col-12 d-flex justify-content-between align-items-center">
            <p class="fs-3 fw-bold">{{ p.category }}</p>
            <i v-if="campaign.creatorId == account.id" type="button" class="rounded selectable no-select mdi mdi-dots-horizontal fs-5" data-bs-toggle="dropdown" aria-expanded="false" title="More Options"></i>

            <div class="dropdown-menu dropdown-menu-end p-0" aria-labelledby="authDropdown">
              <div class="list-group text-center">
                <div @click="handleNote([selectable, index])" class="list-group-item dropdown-item list-group-item-action selectable" data-bs-toggle="modal" data-bs-target="#notesForm">
                  <p class="mdi mdi-plus"> Add Note</p>
                </div>

                <div @click="removeCategory(index)" class="list-group-item dropdown-item list-group-item-action text-danger selectable">
                  <p class="mdi mdi-delete">Delete Category</p>
                </div>
              </div>
            </div>
          </div>
          <hr class="col-12 my-2">
          <div v-for="(n, i) in p.notes" :key="n" class="col-12 col-md-6 py-2">
            <div class="d-flex justify-content-between align-items-center">
              <p class="fs-5 fw-bold">{{ n.name }}</p>
              <i v-if="campaign.creatorId == account.id" @click="removeNote(index, i)" class="mdi mdi-delete text-danger selectable"></i>
            </div>
            <hr class="my-2">
            <p class="px-2">{{ n.description }}</p>
          </div>
        </div>

        <div class="d-flex justify-content-center">
          <p v-if="selectable == 1 ? !campaign.publicNotes.length : !campaign.privateNotes.length" class="fs-3">No Notes Are Available!</p>
        </div>
      </div>

      <div v-else class="bg-dark rounded-bottom elevation-5 overflow-auto h-md-50 pb-2">
        <div class="row mx-0 p-2">
          <button @click="handleNote()" class="col-12 btn btn-light" type="button" data-bs-toggle="modal" data-bs-target="#notesForm">Note +</button>
          <div v-for="(e, index) in campaign.events" :key="e" class="col-12 col-md-6 p-2">
            <div class="d-flex justify-content-between align-items-center">
              <p class="px-2 fs-3 fw-bold">{{ e.name }}</p>
              <i @click="removeNote(index)" class="mdi mdi-delete fs-5 text-danger selectable"></i>
            </div>
            <hr class="my-2">
            <p class="px-2">{{ e.description }}</p>
          </div>
        </div>
      </div>
    </div>

    <div class="col-12 col-md-6 py-2 h-md">
      <CampaignOwner :campaignProp="campaign" />
    </div>
  </section>

  <div v-else>
    <Loader />
  </div>
</template>

<script>
import { computed, onMounted, ref, watchEffect } from 'vue'
import { onBeforeRouteUpdate, useRoute, useRouter } from 'vue-router'
import { AppState } from '../AppState.js'
import { campaignsService } from '../services/CampaignsService.js'
import { campaignHub } from '../handlers/CampaignHub.js'
import CampaignOwner from '../components/CampaignOwner.vue'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    const selectable = ref(1)

    onMounted(() => {
      campaignHub.onCampaign()
    })

    onBeforeRouteUpdate(() => {
      campaignHub.leaveCampaign(route.params.campaignId)
    })

    watchEffect(() => {
      if (route.params.campaignId) {
        getCampaignById()
        campaignHub.enterCampaign(route.params.campaignId)
      }
    })

    async function getCampaignById() {
      try {
        await campaignsService.getCampaignById(route.params.campaignId)
      }
      catch (error) {
        Pop.error(error.message, '[GETTING CAMPAIGN BY ID]')
        router.push('/')
      }
    }

    function getKey() {
      if (selectable.value == 1) {
        return 'publicNotes'
      } else if (selectable.value == 2) {
        return 'privateNotes'
      }
      return 'events'
    }

    return {
      selectable,
      account: computed(() => AppState.account),
      campaign: computed(() => AppState.activeCampaign),

      handleNote(index = null) {
        AppState.notes = index
      },

      async removeCategory(index) {
        try {
          const isSure = await Pop.confirm('Are you sure you want to delete this category?', 'This will delete its corresponding notes!')

          if (!isSure) {
            return
          }
          const campaignData = {}
          const key = getKey()
          campaignData[key] = [...AppState.activeCampaign[key]]
          campaignData[key].splice(index, 1)
          await campaignsService.updateCampaign(campaignData, this.campaign.id)
          Pop.toast('Category was deleted!')
        } catch (error) {
          Pop.error(error.message, '[DELETING CATEGORY]')
        }
      },

      async removeNote(index, i = null) {
        try {
          const isSure = await Pop.confirm('Are you sure you want to delete this note?')

          if (!isSure) {
            return
          }
          const campaignData = {}
          const key = getKey()
          campaignData[key] = [...AppState.activeCampaign[key]]

          if (i == null) {
            campaignData[key].splice(index, 1)
          } else {
            campaignData[key][index].notes.splice(i, 1)
          }
          await campaignsService.updateCampaign(campaignData, this.campaign.id)
          Pop.toast('Note was deleted!')
        } catch (error) {
          Pop.error(error.message, '[DELETING NOTE]')
        }
      }
    }
  },
  components: { CampaignOwner }
}
</script>

<style lang="scss" scoped>
  @media screen and (min-width: 768px) {
    .h-md {
      height: var(--main-height);
    }
  }
</style>