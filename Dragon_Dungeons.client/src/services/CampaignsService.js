import { AppState } from "../AppState.js"
import { Campaign } from "../models/Campaign.js"
import { saveState } from "../utils/Store.js"
import { api } from "./AxiosService.js"
import Pop from "../utils/Pop.js"

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

  async getCampaignById(campaignId) {
    const res = await api.get(`api/campaigns/${campaignId}`)
    AppState.activeCampaign = new Campaign(this.converter(res.data))
    return AppState.activeCampaign
  }

  async getCampaignsByUserId() {
    try {
      const res = await api.get('account/campaigns')
      AppState.campaigns = res.data.map(d => {
        d = this.converter(d)
        return new Campaign(d)
      })
    } catch (error) {
      Pop.error(error.message, '[GETTING USERS CAMPAIGNS]')
    }
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