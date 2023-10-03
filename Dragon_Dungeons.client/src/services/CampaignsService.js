import { AppState } from "../AppState.js"
import { Campaign } from "../models/Campaign.js"
import { saveState } from "../utils/Store.js"
import { api } from "./AxiosService.js"

const keys = ['privateNote', 'publicNote', 'events', 'monsters']

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

  resetCampaign() {
    saveState('tempCampaign', {})
    saveState('camPage', 0)
    AppState.tempCampaign = {}
  }

  async createCampaign(campaignData) {
    campaignData = this.converter(campaignData)
    const res = await api.post('api/campaigns', campaignData)
    AppState.campaigns.push(new Campaign(this.converter(res.data)))
    this.resetCampaign()
  }

  converter(data) {
    for (let k in keys) {
      let d = data[keys[k]]

      if (d) {
        if (typeof d == 'object') {
          d = JSON.stringify(d)
        } else {
          d = JSON.parse(d)
        }
        data[keys[k]] = d
      }
    }
    return data
  }
}

export const campaignsService = new CampaignsService()