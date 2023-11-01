import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import Pop from "../utils/Pop.js"

class CampaignHub {
  constructor() {
    this.client = new HubConnectionBuilder()
      .withUrl('hubs/campaignHub')
      .configureLogging(LogLevel.Information)
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
}

export const campaignHub = new CampaignHub()