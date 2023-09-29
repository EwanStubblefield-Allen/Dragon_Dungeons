import { openApi } from "./AxiosService.js"

class OpenService {
  async createImg(prompt) {
    const res = await openApi.post('generations', prompt)
    return res.data.data[0].url
  }
}

export const openService = new OpenService()