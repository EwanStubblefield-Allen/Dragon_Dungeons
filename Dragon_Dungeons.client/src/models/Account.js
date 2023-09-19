import { RepoItem } from "./RepoItem.js"

export class Profile extends RepoItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.picture = data.picture
  }
}

export class Account extends Profile {
  constructor(data) {
    super(data)
    this.email = data.email
  }
}