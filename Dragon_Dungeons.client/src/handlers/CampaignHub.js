import { AppState } from "../AppState.js"
import { baseURL } from "../env.js"
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import Pop from "../utils/Pop.js"
import { router } from "../router.js"

class CampaignHub {
  constructor() {
    this.client = new HubConnectionBuilder()
      .withUrl(baseURL + '/hubs/campaignHub')
      .configureLogging(LogLevel.Trace)
      .withAutomaticReconnect()
      .build()
  }

  async start() {
    try {
      await this.client.start()
    } catch (error) {
      Pop.error(error.message, '[STARTING SIGNAL R]')
      setTimeout(this.start, 5000)
    }
  }

  enterCampaign(campaignId) {
    this.client.invoke('EnterCampaign', campaignId)
  }

  leaveCampaign(campaignId) {
    this.client.invoke('LeaveCampaign', campaignId)
  }

  onCampaign() {
    this.client.on('PlayerJoinedCampaign', (playerData) => {
      AppState.activeCampaign.players.push(playerData)
      Pop.success(`${playerData.name} joined the campaign!`)
    })
    this.client.on('PlayerLeftCampaign', (playerData) => {
      if (playerData.creatorId == AppState.account.id) {
        AppState.campaigns = AppState.campaigns.filter(c => c.id != playerData.campaignId)
        router.push({ name: 'Account' })
      }
      AppState.activeCampaign.players = AppState.activeCampaign.players.filter(p => p.id != playerData.id)
      Pop.toast(`${playerData.name} left the campaign!`)
    })
  }

  offCampaign() {
    this.client.off('PlayerJoinedCampaign')
    this.client.off('PlayerLeftCampaign')
  }
}

export const campaignHub = new CampaignHub()