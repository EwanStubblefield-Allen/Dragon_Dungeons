<template>
  <header class="container-fluid">
    <Navbar />
  </header>
  <main class="container-fluid bg-wheat">
    <router-view />
  </main>
</template>

<script>
import { computed } from 'vue'
import { AppState } from './AppState'
import Navbar from './components/Navbar.vue'

export default {
  setup() {
    return {
      appState: computed(() => AppState)
    }
  },
  components: { Navbar }
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

  main {
    box-shadow: 0 0 200px rgba(0, 0, 0, 0.9) inset;
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
</style>