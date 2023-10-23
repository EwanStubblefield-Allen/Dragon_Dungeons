import { RepoItem } from "./RepoItem.js"

export class Player extends RepoItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.picture = data.picture
    this.level = data.level
    this.class = data.class
    this.race = data.race
    this.characterId = data.characterId
    this.campaignId = data.campaignId
  }
}