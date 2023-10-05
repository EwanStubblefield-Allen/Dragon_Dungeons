import { AppState } from "../AppState.js"
import { openApi } from "./AxiosService.js"
import Compressor from "compressorjs"

class OpenService {
  async generateImg(prompt) {
    const res = await openApi.post('images/generations', prompt)
    return res.data.data[0].url
  }

  async compress(file, compress) {
    await new Promise((resolve) => {
      new Compressor(file, {
        quality: compress,
        success(result) {
          resolve(AppState.tempCharacter.picture = result)
        }
      })
    })

    if (AppState.tempCharacter.picture.size > 1000000) {
      if (file.size == AppState.tempCharacter.picture.size) {
        throw new Error('Cannot compress image!!')
      }
      throw new Error('Image is too large!!')
    }
  }
}

export const openService = new OpenService()