import Compressor from 'compressorjs'
import { AppState } from '../AppState.js'
import { api } from './AxiosService.js'

class ImagesService {
  async generateImg(prompt) {
    const res = await api.post('api/images/generations', prompt)
    return res.data.data[0].url
  }

  async createImg(imageData) {
    const res = await api.post('api/images/creation', { File: imageData }, { timeout: 60000 })
    return { url: res.data.secure_url, id: res.data.public_id, sign: res.data.signature }
  }

  async removeImg(imageId) {
    await api.post('api/images/deletion', { public_id: imageId })
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

  async compress(file, compress) {
    await new Promise(resolve => {
      new Compressor(file, {
        quality: compress,
        convertTypes: 'image/webp',
        success(result) {
          resolve((AppState.tempCharacter.picture = result))
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

export const imagesService = new ImagesService()
