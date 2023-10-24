import { AppState } from "../AppState.js"
import { Npc } from "../models/Npc.js"
import { api } from "./AxiosService.js"

class NpcsService {
  async createNpc(npcData, campaignId) {
    npcData.characterId = npcData.id
    npcData.picture = npcData.picture.url
    npcData = new Npc(npcData)
    const res = await api.post(`api/campaigns/${campaignId}/npcs`, npcData)
    AppState.activeCampaign.npcs.push(res.data)
  }
}

export const npcsService = new NpcsService()