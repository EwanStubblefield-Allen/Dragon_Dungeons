import { AppState } from "../AppState.js"
import { Character } from "../models/Character.js"
import { saveState } from "../utils/Store.js"
import { api } from "./AxiosService.js"

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

  async createCharacter(characterData) {
    characterData = this.converter(characterData, true)
    const res = await api.post('api/characters', characterData)
    AppState.characters.push(new Character(this.converter(res.data)))
  }

  async updateCharacter(characterData) {
    characterData = this.converter(characterData, true)
    const res = await api.put(`api/characters/${characterData.id}`, characterData)
    res.data = this.converter(res.data)
    const foundIndex = AppState.characters.findIndex(c => c.id == characterData.id)
    AppState.characters.splice(foundIndex, 1, new Character(res.data))
  }

  converter(data, bool = false) {
    if (bool) {
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