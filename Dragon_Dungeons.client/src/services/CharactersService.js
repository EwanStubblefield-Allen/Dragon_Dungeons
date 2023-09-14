import { AppState } from "../AppState.js"

class CharactersService {
  changeCharacterForm(num) {
    AppState.characterForm = num
  }
}

export const charactersService = new CharactersService()