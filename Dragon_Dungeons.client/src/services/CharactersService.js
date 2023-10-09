import { AppState } from "../AppState.js"
import { Character } from "../models/Character.js"
import { saveState } from "../utils/Store.js"
import { api } from "./AxiosService.js"
import Pop from "../utils/Pop.js"

const keys = ['picture', 'skills', 'proficiencies', 'cantrips', 'spells', 'equipment']

class CharactersService {
  changeCharPage(current) {
    if (current == AppState.charPage) {
      AppState.charPage++
      saveState('charPage', AppState.charPage)
    }
  }

  saveCharacter(characterData) {
    saveState('tempCharacter', characterData)
    AppState.tempCharacter = new Character(characterData)
  }

  resetCharacter() {
    saveState('tempCharacter', {})
    saveState('tempClass', {})
    saveState('charPage', 0)
    AppState.tempCharacter = {}
    AppState.tempClass = {}
    AppState.charPage = 0
  }

  async getCharacterById(characterId) {
    const res = await api.get(`api/characters/${characterId}`)
    AppState.activeCharacter = new Character(this.converter(res.data))
  }

  async getCharactersByUserId() {
    try {
      const res = await api.get('account/characters')
      AppState.characters = res.data.map(d => {
        d = this.converter(d)
        return new Character(d)
      })
    } catch (error) {
      Pop.error(error.message, '[GETTING USERS CHARACTERS]')
    }
  }

  async createCharacter(characterData) {
    if (characterData.proficiencies) {
      characterData.proficiencies = characterData.proficiencies.concat(AppState.tempClass.proficiencies)
    } else {
      characterData.proficiencies = AppState.tempClass.proficiencies
    }
    characterData.skills = characterData.skills.map(s => s.name.replace('Skill: ', ''))
    characterData = this.converter(characterData, true)
    AppState.attributes.forEach(a => {
      if (characterData.bonus[a]) {
        characterData[a] += characterData.bonus[a]
      }
    })
    characterData.bonus = characterData.bonus.bonus

    switch (characterData.class) {
      case 'Barbarian':
        characterData.maxHp = 13
        break
      case 'Fighter':
      case 'Paladin':
      case 'Ranger':
        characterData.maxHp = 11
        break
      case 'Bard':
      case 'Cleric':
      case 'Druid':
      case 'Monk':
      case 'Rogue':
      case 'Warlock':
        characterData.maxHp = 9
        break
      case 'Sorcerer':
      case 'Wizard':
        characterData.maxHp = 7
        break
      default:
        throw new Error('Unknown Class!')
    }
    characterData.maxHp += Math.floor((characterData.con - 10) / 2)
    characterData.hp = characterData.maxHp
    const res = await api.post('api/characters', characterData)
    AppState.characters.push(new Character(this.converter(res.data)))
    this.resetCharacter()
  }

  async updateCharacter(characterData) {
    characterData = this.converter(characterData, true)
    const res = await api.put(`api/characters/${characterData.id}`, characterData)
    res.data = this.converter(res.data)
    const foundIndex = AppState.characters.findIndex(c => c.id == characterData.id)
    AppState.characters.splice(foundIndex, 1, new Character(res.data))
  }

  converter(data, input = false) {
    if (input) {
      data.intelligence = data.int
      data.bonus.intelligence = data.bonus?.int
    }

    for (let k in keys) {
      let d = data[keys[k]]

      if (d) {
        if (typeof d == 'object') {
          d = JSON.stringify(d)
        } else {
          d = JSON.parse(d)
        }
        data[keys[k]] = d
      }
    }
    return data
  }
}

export const charactersService = new CharactersService()