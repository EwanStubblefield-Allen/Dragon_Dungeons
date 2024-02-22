import { IMAGE_API_SECRET } from '../config.js'
import { api } from './AxiosService.js'

class ImagesService {
  async createImg(imageData) {
    const res = await api.post('api/images', { File: imageData })
    return { url: res.data.secure_url, id: res.data.public_id, sign: res.data.signature }
  }

  async removeImg(imageId) {
    const timestamp = Date.now()
    const sign = await this.sha256(`public_id=${imageId}&timestamp=${timestamp}${IMAGE_API_SECRET}`)
    const imageData = {
      public_id: imageId,
      timestamp: timestamp,
      signature: sign
    }
    await api.post('api/images', imageData)
  }

  async sha256(message) {
    // encode as UTF-8
    const msgBuffer = new TextEncoder().encode(message)

    // hash the message
    const hashBuffer = await crypto.subtle.digest('SHA-256', msgBuffer)

    // convert ArrayBuffer to Array
    const hashArray = Array.from(new Uint8Array(hashBuffer))

    // convert bytes to hex string
    const hashHex = hashArray.map(b => b.toString(16).padStart(2, '0')).join('')
    return hashHex
  }
}

export const imagesService = new ImagesService()
