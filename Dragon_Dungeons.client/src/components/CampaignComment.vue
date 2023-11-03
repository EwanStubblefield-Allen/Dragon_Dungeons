<template>
  <div class="h-100 overflow-auto">
    <div v-for="c in campaignProp.comments" :key="c.id" class="card m-2">
      <div class="d-flex justify-content-between">
        <div class="d-flex align-items-center">
          <img class="profile-pic m-2" :src="c.creator.picture" :alt="c.creator.name">

          <div>
            <div class="d-flex">
              <p class="fw-bold pb-2">{{ c.creator.name }}</p>
              <p v-if="c.updatedAt != c.createdAt" class="px-3">edited</p>
            </div>

            <div class="text-secondary">
              <p>{{ new Date(c.updatedAt).toLocaleString() }}</p>
            </div>
          </div>
        </div>

        <div v-if="account.id == c.creatorId" class="dropdown p-2">
          <div type="button" class="selectable no-select" data-bs-toggle="dropdown" aria-expanded="false">
            <i class="mdi mdi-dots-horizontal fs-5"></i>
          </div>
          <div class="dropdown-menu dropdown-menu-end p-0" aria-labelledby="authDropdown">
            <div class="list-group">
              <div @click="editComment(c)" class="list-group-item dropdown-item list-group-item-action selectable">
                <i class="mdi mdi-pencil blue"></i>
                Edit
              </div>
              <div @click="removeComment(c.id)" class="list-group-item dropdown-item list-group-item-action text-danger selectable">
                <i class="mdi mdi-delete"></i>
                Delete
              </div>
            </div>
          </div>
        </div>
        <div v-else-if="account.id == campaignProp.creatorId" class="p-2">
          <i @click="removeComment(c.id)" class="mdi mdi-delete text-danger selectable fs-5" title="Delete Comment"></i>
        </div>
      </div>
      <div class="d-flex justify-content-between align-items-end px-3 pb-2 fs-5">
        <p class="text-break pe-3">{{ c.description }}</p>
      </div>
    </div>
  </div>

  <form @submit.prevent="handleComment()" class="px-2 w-100">
    <div class="input-group">
      <input v-model="editable.description" id="comment" class="form-control" type="text" minlength="2" maxlength="100" placeholder="Leave your comment..." required>
      <button class="mdi mdi-plus input-group-text" type="submit" title="Post Comment"></button>
    </div>
  </form>
</template>

<script>
import { computed, ref } from 'vue'
import { AppState } from '../AppState.js'
import { Campaign } from '../models/Campaign.js'
import { commentsService } from '../services/CommentsService.js'
import Pop from '../utils/Pop.js'

export default {
  props: {
    campaignProp: {
      type: Campaign,
      required: true
    }
  },
  setup() {
    const editable = ref({})

    async function createComment() {
      try {
        await commentsService.createComment(editable.value)
        Pop.success('Comment was posted!')
      } catch (error) {
        Pop.error(error.message, '[CREATING COMMENT]')
      }
    }

    async function updateComment() {
      try {
        await commentsService.updateComment(editable.value)
        Pop.success('Comment was updated!')
      } catch (error) {
        Pop.error(error.message, '[UPDATING COMMENT]')
      }
    }

    return {
      editable,
      account: computed(() => AppState.account),

      editComment(commentData) {
        editable.value = { ...commentData }
        const commentElement = document.getElementById('comment')
        commentElement.focus()
        commentElement.setSelectionRange(commentElement.value.length, commentElement.value.length)
      },

      handleComment() {
        if (editable.value.id) {
          updateComment()
        } else {
          createComment()
        }
        editable.value = {}
      },

      async removeComment(commentId) {
        try {
          const isSure = await Pop.confirm('Are you sure you want to delete this comment?')

          if (!isSure) {
            return
          }
          await commentsService.removeComment(commentId)
          Pop.toast('Comment was deleted!')
        } catch (error) {
          Pop.error(error.message, '[DELETING COMMENT]')
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .profile-pic {
    height: 10vh;
    width: 10vh;
    border-radius: 50%;
    object-fit: cover;
    object-position: center;
  }

  .h-100 {
    max-height: 50vh;
  }
</style>