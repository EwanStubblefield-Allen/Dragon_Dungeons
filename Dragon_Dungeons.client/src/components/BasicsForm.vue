<template>
  <p class="fs-1 fw-bold m-3 py-2">Basics</p>

  <form @submit.prevent="" class="row g-3 bg-dark text-dark m-3 p-3 rounded elevation-5">
    <div class="col-md-12 form-floating">
      <input type="text" class="form-control" id="name" minlength="3" maxlength="100" placeholder="Name..." required>
      <label for="name">Character Name:</label>
    </div>
    <div class="col-md-4 form-group">
      <label class="text-light" for="class">
        Class:
        <router-link :to="{ name: 'Info', params: { infoId: 'classes', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select id="class" class="form-select" placeholder="Class..." required>
        <option v-for="c in dndClass" :key="c">{{ c }}</option>
      </select>
    </div>
    <div class="col-md-4 form-group">
      <label class="text-light" for="race">
        Race:
        <router-link :to="{ name: 'Info', params: { infoId: 'races', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select id="race" class="form-select" placeholder="Race..." required>
        <option v-for="r in race" :key="r">{{ r }}</option>
      </select>
    </div>
    <div class="col-md-4 form-group">
      <label class="text-light" for="alignment">
        Alignment:
        <router-link :to="{ name: 'Info', params: { infoId: 'alignments', infoDetails: 'search' } }" target="_blank" class="mdi mdi-information text-primary selectable" title="Learn more"></router-link>
      </label>
      <select id="alignment" class="form-select" placeholder="Alignment..." required>
        <option v-for="a in alignment" :key="a">{{ a }}</option>
      </select>
    </div>
    <div class="col-12 text-end">
      <router-link :to="{ name: 'Character', params: { characterId: 'features' } }">
        <button type="submit" class="btn btn-primary">Save</button>
      </router-link>
    </div>
  </form>
</template>

<script>

export default {
  setup() {
    const dndClass = ['Barbarian', 'Bard', 'Cleric', 'Druid', 'Fighter', 'Monk', 'Paladin', 'Ranger', 'Rogue', 'Sorcerer', 'Warlock', 'Wizard']
    const race = ['Dragonborn', 'Dwarf', 'Elf', 'Gnome', 'Half-Elf', 'Half-Orc', 'Halfling', 'Human', 'Tiefling']
    const alignment = ['Chaotic Evil', 'Chaotic Good', 'Chaotic Neutral', 'Lawful Evil', 'Lawful Good', 'Lawful Neutral', 'Neutral', 'Neutral Evil', 'Neutral Good']

    return {
      dndClass,
      race,
      alignment
    }
  }
}
</script>

<style lang="scss" scoped></style>