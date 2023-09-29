import { AppState } from "../AppState.js"
import { logger } from "../utils/Logger.js"
import { openApi } from "./AxiosService.js"
import Compressor from "compressorjs"

class OpenService {
  async createImg(prompt) {
    // TODO Turn image url to blob
    const res = await openApi.post('generations', prompt)
    fetch(res.data.data[0].url, {
      headers: { 'Content-type': 'application/json' }
    })
    // await this.compress(file, 0)
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
  }
}

export const openService = new OpenService()