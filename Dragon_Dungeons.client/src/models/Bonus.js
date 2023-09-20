import { RepoItem } from "./RepoItem.js"

export class Bonus extends RepoItem {
  constructor(data) {
    super(data)
    this.str = data?.str
    this.dex = data?.dex
    this.con = data?.con
    this.int = data?.intelligence
    this.wis = data?.wis
    this.cha = data?.cha
    this.characterId = data?.characterId
  }
}