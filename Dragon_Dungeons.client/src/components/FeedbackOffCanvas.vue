<template>
  <div class="offcanvas offcanvas-end text-bg-dark" tabindex="-1" id="feedbackOffcanvas" aria-labelledby="feedbackLabel">
    <div class="offcanvas-header">
      <h5 class="offcanvas-title" id="feedbackLabel">Feedback</h5>
      <button type="button" class="btn-close btn-close-white" data-bs-dismiss="offcanvas" aria-label="Close"></button>
    </div>
    <div class="offcanvas-body">
      <form @submit.prevent="sendFeedback()">
        <div class="form-group mb-3">
          <label for="category" class="form-label">What is your feedback related to?</label>
          <select v-model="editable.category" class="form-select" id="category" aria-label="Category select" required>
            <option disabled>Select One</option>
            <option>Bug</option>
            <option>Feature Request</option>
            <option>Other</option>
          </select>
        </div>
        <div class="form-group mb-3">
          <label for="problem" class="form-label">Tell me about what is going on.</label>
          <textarea v-model="editable.problem" class="form-control" id="problem" maxlength="1000" required></textarea>
        </div>
        <div class="form-group mb-3">
          <label for="suggestion" class="form-label">Do you have suggestion to help me improve.</label>
          <textarea v-model="editable.suggestion" class="form-control" id="suggestion" maxlength="1000"></textarea>
        </div>
        <button type="submit" class="btn btn-primary w-100">Submit</button>
      </form>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import emailJS from '@emailjs/browser'
import Pop from '../utils/Pop.js'

export default {
  setup() {
    const editable = ref({})

    return {
      editable,

      sendFeedback() {
        emailJS.send('service_zr4j517', 'template_6jvsb3t', editable.value, 'tUTZA6v53e0QSaO0p')
          .then((result) => {
            Pop.success(result.text, '[EMAIL SENT!!]')
          }, (error) => {
            Pop.error(error.text, '[EMAIL FAILED TO SEND!!]')
          })
        editable.value = {}
      }
    }
  }
}
</script>

<style lang="scss" scoped></style>