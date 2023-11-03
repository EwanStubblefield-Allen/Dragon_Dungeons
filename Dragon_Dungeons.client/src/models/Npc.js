import { RepoItem } from "./RepoItem.js"

export class Npc extends RepoItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.picture = data.picture
    this.class = data.class
    this.race = data.race
    this.characterId = data.characterId
    this.campaignId = data.campaignId
  }
}

export class Player extends Npc {
  constructor(data) {
    super(data)
    this.creatorId = data.creatorId
  }
}