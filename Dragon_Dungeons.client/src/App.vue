<template>
  <header class="container-fluid position-fixed">
    <Navbar />
  </header>
  <main class="container-fluid bg-wheat">
    <router-view />
  </main>

  <ModalComponent id="accountForm">
    <template #title>Edit Account</template>
    <template #body>
      <AccountForm />
    </template>
  </ModalComponent>

  <ModalComponent id="categoryForm">
    <template #title>Add Category</template>
    <template #body>
      <CategoryForm />
    </template>
  </ModalComponent>

  <ModalComponent id="notesForm">
    <template #title>Add Note</template>
    <template #body>
      <NotesForm />
    </template>
  </ModalComponent>

  <ModalComponent id="initiative">
    <template #title>Battle!</template>
    <template #body>
      <InitiativeForm />
    </template>
  </ModalComponent>

  <ModalComponent id="level">
    <template #title>Level Up Available!</template>
    <template #body>
      <LevelForm />
    </template>
  </ModalComponent>

  <ModalComponent id="award">
    <template #title>Award Players!</template>
    <template #body>
      <AwardForm />
    </template>
  </ModalComponent>

  <ModalComponent id="trade" class="modal-lg">
    <template #title>Trading!</template>
    <template #body>
      <TradeForm />
    </template>
  </ModalComponent>

  <FeedbackOffCanvas />
</template>

<script>
import { computed, onMounted } from 'vue'
import { AppState } from './AppState'
import { campaignHub } from './handlers/CampaignHub.js'
import Navbar from './components/Navbar.vue'
import AccountForm from './components/AccountForm.vue'
import CategoryForm from './components/CategoryForm.vue'
import NotesForm from './components/NotesForm.vue'
import InitiativeForm from './components/InitiativeForm.vue'
import LevelForm from './components/LevelForm.vue'
import AwardForm from './components/AwardForm.vue'
import TradeForm from './components/TradeForm.vue'
import FeedbackOffCanvas from './components/FeedbackOffCanvas.vue'

export default {
  setup() {
    onMounted(() => {
      campaignHub.start()
    })

    return {
      appState: computed(() => AppState)
    }
  },
  components: { Navbar, AccountForm, NotesForm, CategoryForm, InitiativeForm, FeedbackOffCanvas, LevelForm, AwardForm, TradeForm }
}
</script>
<style lang="scss">
  @import "./assets/scss/main.scss";

  :root {
    --main-height: calc(100vh - 76px);
    --russian: #06062A;
    --oxford: #09093B;
    --wheat: #EFDBB5;
  }

  // body {
  //   font-family: 'Inknut Antiqua', serif;
  // }

  header {
    z-index: 1000;
  }

  main {
    box-shadow: 0 0 200px rgba(0, 0, 0, 0.9) inset;
    margin-top: 76px;
  }

  p {
    margin: 0;
  }

  .bg-russian {
    background-color: var(--russian);
  }

  .bg-oxford {
    background-color: var(--oxford);
  }

  .bg-wheat {
    background-color: var(--wheat);
  }

  .text-russian {
    color: var(--russian);
  }

  @media screen and (min-width: 768px) {
    .h-md-50 {
      height: 50%;
    }

    .vh-md-25 {
      height: 25vh;
    }
  }
</style>