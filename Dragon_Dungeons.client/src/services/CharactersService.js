import { AppState } from "../AppState.js"
import { Character } from "../models/Character.js"
import { api } from "./AxiosService.js"

class CharactersService {
  changeCharPage(current) {
    if (current == AppState.charPage) {
      AppState.charPage++
    }
  }

  async createTempCharacter(characterData) {
    // const res = await api.post('api/character', characterData)
    // AppState.tempCharacter = new Character(res.data)
    // FIXME Temp
    characterData.intelligence = characterData.int
    AppState.tempCharacter = new Character(characterData)
  }

  async updateTempCharacter(characterData) {
    const res = await api.put(`api/character/${characterData.id}`, characterData)
    AppState.tempCharacter = new Character(res.data)
  }
}

export const charactersService = new CharactersService()