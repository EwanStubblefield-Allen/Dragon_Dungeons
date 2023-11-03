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

  async removeNpc(npcData) {
    const res = await api.delete(`api/campaigns/${npcData.campaignId}/npcs/${npcData.id}`)
    AppState.activeCampaign.npcs = AppState.activeCampaign.npcs.filter(n => n.id != res.data.id)
    return res.data
  }
}

export const npcsService = new NpcsService()