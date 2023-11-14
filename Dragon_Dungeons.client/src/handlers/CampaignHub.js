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
      this.client.onreconnected(() => {
        if (AppState.activeCampaign.id) {
          this.enterGroup(AppState.activeCampaign.id)
        }
        this.enterGroup(AppState.account.id)
      })
    } catch (error) {
      Pop.error(error.message, '[STARTING SIGNALR]')
      setTimeout(this.start, 5000)
    }
  }

  enterGroup(groupId) {
    this.client.invoke('EnterGroup', groupId)
  }

  leaveGroup(groupId) {
    this.client.invoke('LeaveGroup', groupId)
  }

  awardXp(campaignId, xp) {
    this.client.invoke('AwardXp', campaignId, xp)
  }

  awardPlayers(campaignId, award) {
    this.client.invoke('AwardPlayers', campaignId, JSON.stringify(award))
  }

  trade(location, contents) {
    this.client.invoke('Trade', location, contents)
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
        const character = AppState.activeCharacter
        await charactersService.checkLevel({ level: character.level, xp: character.xp, manual: character.manual }, xp)
        Pop.success(`You were awarded ${xp}xp`)
      } catch (error) {
        Pop.error(error.message, '[AWARDING XP]')
      }
    })

    this.client.on('AwardPlayers', async (award) => {
      try {
        if (!AppState.activeCharacter) {
          await charactersService.getCharacterById(AppState.activeCampaign.players[0].characterId)
        }
        const character = AppState.activeCharacter
        award = JSON.parse(award)
        const contents = {
          xp: award.xp,
          currency: award.currency.length
        }

        if (award.xp) {
          award.xp += character.xp
        }
        award.currency = character.currency.map((c, index) => {
          if (award.currency[index]) {
            c[1] += award.currency[index]
          }
          return c
        })
        await charactersService.updateCharacter(award)
        let string = ''

        if (contents.xp) {
          string += 'xp'

          if (contents.currency) {
            string += ' and currency'
          }
        } else if (contents.currency) {
          string += 'currency'
        } else {
          return
        }

        Pop.success(`You were awarded ${string}!`)
      } catch (error) {
        Pop.error(error.message, '[AWARDING PLAYERS]')
      }
    })

    this.client.on('Trade', async (contents) => {
      try {
        [contents.offer, contents.want] = [contents.want, contents.offer]
        await charactersService.getCharacterById(contents.id)

        if (contents.dm && contents.player) {
          return await charactersService.handleTrade(contents)
        }
        contents.equal = true
        AppState.trade = contents
        Modal.getOrCreateInstance('#trade').show()
      } catch (error) {
        Pop.error(error.message, '[TRADING]')
      }
    })
  }
}

export const campaignHub = new CampaignHub()