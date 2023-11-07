import { RepoItem } from "./RepoItem.js"
import { Profile } from "./Account.js"

export class Campaign extends RepoItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.description = data.description
    this.privateNotes = data.privateNotes
    this.publicNotes = data.publicNotes
    this.events = data.events
    this.npcs = data.npcs
    this.monsters = data.monsters
    this.players = data.players
    this.comments = data.comments
    this.initiative = data.initiative
    this.creator = new Profile(data.creator)
    this.creatorId = data.creatorId
  }
}