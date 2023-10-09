import { Profile } from "./Account.js"
import { RepoItem } from "./RepoItem.js"

export class Character extends RepoItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.picture = data.picture
    this.hp = data.hp
    this.maxHp = data.maxHp
    this.tempHp = data.tempHp
    this.speed = data.speed
    this.hitDie = data.hitDie
    this.level = data.level
    this.class = data.class
    this.race = data.race
    this.alignment = data.alignment
    this.age = data.age
    this.feet = data.feet
    this.inches = data.inches
    this.weight = data.weight
    this.eyes = data.eyes
    this.skin = data.skin
    this.hair = data.hair
    this.features = data.features
    this.background = data.background
    this.backstory = data.backstory
    this.personalityTraits = data.personalityTraits
    this.ideals = data.ideals
    this.bonds = data.bonds
    this.flaws = data.flaws
    this.manual = data.manual
    this.str = data.str
    this.dex = data.dex
    this.con = data.con
    this.int = data.intelligence || data.int
    this.wis = data.wis
    this.cha = data.cha
    this.bonus = data.bonus
    this.skills = data.skills
    this.proficiencies = data.proficiencies
    this.cantrips = data.cantrips
    this.spells = data.spells
    this.equipment = data.equipment
    this.armor = data.armor
    this.weapons = data.weapons || []
    this.creator = new Profile(data.creator)
    this.creatorId = data.creatorId
  }
}