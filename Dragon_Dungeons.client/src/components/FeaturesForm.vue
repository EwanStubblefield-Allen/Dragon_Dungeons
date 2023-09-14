<template>
  <div class="m-3">
    <p class="fs-1 fw-bold py-2">Features</p>

    <ul class="nav nav-tabs">
      <li class="nav-item">
        <p @click="editable = 1" :class="{ 'active elevation-5': editable == 1 }" class="nav-link selectable text-dark fw-bold">Physical</p>
      </li>

      <li class="nav-item">
        <p @click="editable = 2" :class="{ 'active elevation-5': editable == 2 }" class="nav-link selectable text-dark fw-bold">Race</p>
      </li>

      <li class="nav-item">
        <p @click="editable = 3" :class="{ 'active elevation-5': editable == 3 }" class="nav-link selectable text-dark fw-bold">Alignment</p>
      </li>
    </ul>

    <div v-if="editable == 1" class="bg-dark p-3 rounded-bottom elevation-5">
      <form @submit.prevent="" class="row g-3">
        <div class="col-md-4 form-group">
          <label for="age">Age:</label>
          <input type="number" class="form-control" id="age" min="1" placeholder="Age..." required>
        </div>
        <div class="col-md-4 form-group">
          <label for="height">Height:</label>
          <div class="input-group">
            <input type="number" class="form-control" id="height" min="1" max="10" placeholder="Feet..." required>
            <input type="number" class="form-control" id="height" min="0" max="11" placeholder="Inches..." required>
          </div>
        </div>
        <div class="col-md-4 form-group">
          <label for="weight">Weight:</label>
          <div class="input-group">
            <input type="number" class="form-control" id="weight" min="1" max="500" placeholder="Weight..." required>
            <div class="input-group-text">.lb</div>
          </div>
        </div>
        <div class="col-md-4 form-group">
          <label for="eyes">Eyes:</label>
          <input type="text" class="form-control" id="eyes" minlength="3" maxlength="30" placeholder="Eyes..." required>
        </div>
        <div class="col-md-4 form-group">
          <label for="skin">Skin:</label>
          <input type="text" class="form-control" id="skin" minlength="3" maxlength="30" placeholder="Skin..." required>
        </div>
        <div class="col-md-4 form-group">
          <label for="hair">Hair:</label>
          <input type="text" class="form-control" id="hair" minlength="3" maxlength="30" placeholder="Hair..." required>
        </div>
        <div class="col-12 text-end">
          <button @click="editable++" type="submit" class="btn btn-primary">Save</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'

export default {
  setup() {
    const editable = ref(1)

    return {
      editable
    }
  }
}
</script>

<style lang="scss" scoped></style>