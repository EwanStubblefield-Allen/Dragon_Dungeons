import { RepoItem } from "./RepoItem.js"
import { Profile } from "./Account.js"

export class Campaign extends RepoItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.description = data.description
    this.privateNote = data.privateNote
    this.publicNote = data.publicNote
    this.events = data.events
    this.npcs = data.npcs
    this.monsters = data.monsters
    this.players = data.players
    this.creator = new Profile(data.creator)
    this.creatorId = data.creatorId
  }
}