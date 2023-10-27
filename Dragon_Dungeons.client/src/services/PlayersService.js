import { AppState } from "../AppState.js"
import { Player } from "../models/Npc.js"
import { Campaign } from "../models/Campaign.js"
import { api } from "./AxiosService.js"
import Pop from "../utils/Pop.js"

class PlayersService {
  async getPlayersByUserId() {
    try {
      const res = await api.get('account/players')
      const campaignList = res.data.map(d => new Campaign(d))
      AppState.campaigns = AppState.campaigns.concat(campaignList)
    } catch (error) {
      Pop.error(error.message, '[GETTING PLAYERS BY USER ID]')
    }
  }

  async createPlayer(playerData) {
    const player = { ...playerData.character }
    player.characterId = player.id
    player.picture = player.picture.url
    delete player.id
    delete player.createdAt
    delete player.updatedAt
    const res = await api.post(`api/campaigns/${playerData.campaignId}/players`, new Player(player))
    AppState.campaigns.push(new Campaign(res.data))
  }

  async removePlayer(playerData) {
    const res = await api.delete(`api/campaigns/${playerData.campaignId}/players/${playerData.id}`)

    if (res.data.creatorId == AppState.account.id) {
      AppState.campaigns = AppState.campaigns.filter(c => c.id != res.data.campaignId)
    }
    AppState.activeCampaign.players = AppState.activeCampaign.players.filter(p => p.id != res.data.id)
    return res.data
  }
}

export const playersService = new PlayersService()