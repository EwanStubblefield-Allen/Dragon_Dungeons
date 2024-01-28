import { AppState } from "../AppState.js"
import { Campaign } from "../models/Campaign.js"
import { saveState } from "../utils/Store.js"
import { api } from "./AxiosService.js"
import Pop from "../utils/Pop.js"

const keys = ['privateNotes', 'publicNotes', 'events', 'monsters', 'initiative']

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
      const campaigns = res.data.map(d => {
        d = this.converter(d)
        return new Campaign(d)
      })
      AppState.campaigns = AppState.campaigns.concat(campaigns)
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

  async updateCampaign(campaignData, campaignId) {
    campaignData = this.converter(campaignData)
    const res = await api.put(`api/campaigns/${campaignId}`, campaignData)
    const campaign = new Campaign(this.converter(res.data))
    const foundIndex = AppState.campaigns.findIndex(c => c.id == campaign.id)
    AppState.campaigns.splice(foundIndex, 1, campaign)
    AppState.activeCampaign = campaign
  }

  async removeCampaign(campaignId) {
    const res = await api.delete(`api/campaigns/${campaignId}`)
    AppState.campaigns = AppState.campaigns.filter(c => c.id != campaignId)
    return res.data
  }

  converter(data) {
    for (let k in keys) {
      let d = data[keys[k]]

      if (!d) {
        d = []
      } else {
        if (typeof d == 'object') {
          d = JSON.stringify(d)
        } else {
          d = JSON.parse(d)
        }
      }
      data[keys[k]] = d
    }
    return data
  }
}

export const campaignsService = new CampaignsService()