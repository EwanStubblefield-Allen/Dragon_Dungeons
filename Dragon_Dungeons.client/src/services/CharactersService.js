import { AppState } from "../AppState.js"
import { Character } from "../models/Character.js"
import { saveState } from "../utils/Store.js"
import { api } from "./AxiosService.js"
import Pop from "../utils/Pop.js"

const keys = ['skills', 'proficiencies', 'cantrips', 'spells', 'equipment']

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
    characterData = this.converter(characterData, true)
    const res = await api.post('api/characters', characterData)
    AppState.characters.push(new Character(this.converter(res.data)))
    saveState('charPage', 0)
    saveState('tempCharacter', {})
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