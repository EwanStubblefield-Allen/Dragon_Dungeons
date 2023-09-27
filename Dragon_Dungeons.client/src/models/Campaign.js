import { RepoItem } from "./RepoItem.js"

export class Campaign extends RepoItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.privateNote = data.privateNote
    this.publicNote = data.publicNote
    this.events = data.events
    this.npcs = data.npcs
  }
}