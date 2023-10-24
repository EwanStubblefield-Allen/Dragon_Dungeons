import { AppState } from "../AppState.js"
import { Player } from "../models/Npc.js"
import { api } from "./AxiosService.js"

class PlayersService {
  async createPlayer(playerData) {
    const player = { ...playerData.character }
    player.characterId = player.id
    player.campaignId = playerData.campaignId
    player.picture = player.picture.url
    delete player.id
    delete player.createdAt
    delete player.updatedAt
    await api.post('api/players', new Player(player))
  }

  async removePlayer(playerData) {
    const res = await api.delete(`api/players/${playerData.id}`)
    AppState.activeCampaign.players = AppState.activeCampaign.players.filter(p => p.id != res.data.id)
    return res.data
  }
}

export const playersService = new PlayersService()