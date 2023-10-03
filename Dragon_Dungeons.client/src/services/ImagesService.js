import { imageAuth } from "../config.js"
import { imgApi } from "./AxiosService.js"

class ImagesService {
  async createImg(image) {
    const imageData = {
      file: image,
      upload_preset: 'ml_default',
      api_key: imageAuth
    }
    const res = await imgApi.post('dtcatqouc/image/upload', imageData)
    return { url: res.data.secure_url, id: res.data.public_id }
  }
}

export const imagesService = new ImagesService()