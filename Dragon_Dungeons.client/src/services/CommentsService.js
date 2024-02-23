import { AppState } from '../AppState.js'
import { api } from './AxiosService.js'

class CommentsService {
  async createComment(commentData) {
    await api.post(`api/campaigns/${AppState.activeCampaign.id}/comments`, commentData)
  }

  async updateComment(commentData) {
    await api.put(
      `api/campaigns/${AppState.activeCampaign.id}/comments/${commentData.id}`,
      commentData
    )
  }

  async removeComment(commentId) {
    await api.delete(`api/campaigns/${AppState.activeCampaign?.id}/comments/${commentId}`)
  }
}

export const commentsService = new CommentsService()
