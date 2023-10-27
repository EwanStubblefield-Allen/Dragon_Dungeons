import { SocketHandler } from "../utils/SocketHandler.js"

class CampaignHandler extends SocketHandler {
  constructor() {
    super(true)
  }

  enterCampaign(campaignId) {
    this.emit('ENTER_CAMPAIGN', campaignId)
  }

  leaveCampaign(campaignId) {
    this.emit('LEAVE_CAMPAIGN', campaignId)
  }
}

export const campaignHandler = new CampaignHandler()