import { AppState } from "../AppState.js"
import { Campaign } from "../models/Campaign.js"
import { saveState } from "../utils/Store.js"

class CampaignsService {
  changeCamPage(current) {
    if (current == AppState.camPage) {
      AppState.camPage++
      saveState('camPage', AppState.camPage)
    }
  }

  saveCampaign(campaignData) {
    saveState('tempCampaign', campaignData)
    AppState.tempCampaign = new Campaign(campaignData)
  }
}

export const campaignsService = new CampaignsService()