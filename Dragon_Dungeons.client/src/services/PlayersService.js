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
}

export const playersService = new PlayersService()