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
</template>

<script>
import { computed } from 'vue'
import { AppState } from './AppState'
import Navbar from './components/Navbar.vue'
import AccountForm from './components/AccountForm.vue'
import CategoryForm from './components/CategoryForm.vue'
import NotesForm from './components/NotesForm.vue'

export default {
  setup() {
    return {
      appState: computed(() => AppState)
    }
  },
  components: { Navbar, AccountForm, NotesForm, CategoryForm }
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