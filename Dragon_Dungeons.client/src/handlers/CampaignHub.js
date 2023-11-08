import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import { baseURL } from "../env.js"
import { router } from "../router.js"
import { AppState } from "../AppState.js"
import { charactersService } from "../services/CharactersService.js"
import { Modal } from "bootstrap"
import Pop from "../utils/Pop.js"

class CampaignHub {
  constructor() {
    this.client = new HubConnectionBuilder()
      .withUrl(baseURL + '/hubs/campaignHub')
      .configureLogging(LogLevel.Error)
      .withAutomaticReconnect()
      .build()
  }

  async start() {
    try {
      await this.client.start()
    } catch (error) {
      Pop.error(error.message, '[STARTING SIGNALR]')
      setTimeout(this.start, 5000)
    }
  }

  enterCampaign(campaignId) {
    this.client.invoke('EnterCampaign', campaignId)
  }

  leaveCampaign(campaignId) {
    this.client.invoke('LeaveCampaign', campaignId)
  }

  awardXp(campaignId, xp) {
    this.client.invoke('AwardXp', campaignId, xp)
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

    this.client.on('CampaignNotes', (publicNotes) => {
      AppState.activeCampaign.publicNotes = JSON.parse(publicNotes)
      Pop.success('Public notes was updated!')
    })

    this.client.on('AddComment', (commentData) => {
      AppState.activeCampaign.comments.push(commentData)
      Pop.success('New comment added!')
    })

    this.client.on('UpdateComment', (commentData) => {
      const foundIndex = AppState.activeCampaign.comments.findIndex(c => c.id == commentData.id)
      AppState.activeCampaign.comments.splice(foundIndex, 1, commentData)
    })

    this.client.on('RemoveComment', (commentId) => {
      AppState.activeCampaign.comments = AppState.activeCampaign.comments.filter(c => c.id != commentId)
    })

    this.client.on('InitiateBattle', (initiative) => {
      initiative = JSON.parse(initiative)
      AppState.activeCampaign.initiative = initiative

      if (!initiative.entities) {
        return Modal.getOrCreateInstance('#initiative').hide()
      }

      if (!initiative.intTotal) {
        return Modal.getOrCreateInstance('#initiative').show()
      }
      AppState.activeCampaign.initiative.entities.sort((a, b) => b.initiative - a.initiative)
    })

    this.client.on('AwardXp', async (xp) => {
      try {
        if (!AppState.activeCharacter) {
          await charactersService.getCharacterById(AppState.activeCampaign.players[0].characterId)
        }
        charactersService.checkLevel(AppState.activeCharacter, xp)
        Pop.success(`You were awarded ${xp}xp`)
      } catch (error) {
        Pop.error(error.message, '[UPDATING XP]')
      }
    })
  }
}

export const campaignHub = new CampaignHub()