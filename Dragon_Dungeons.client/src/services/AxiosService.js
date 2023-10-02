import Axios from 'axios'
import { baseURL } from '../env'
import { logger } from '../utils/Logger.js'
import { openAiAuth } from '../config.js'

export const api = Axios.create({
  baseURL,
  timeout: 8000
})

export const dndApi = Axios.create({
  baseURL: 'https://www.dnd5eapi.co',
  timeout: 8000
})

export const openApi = Axios.create({
  baseURL: 'https://api.openai.com/v1',
  headers: { Authorization: openAiAuth },
  timeout: 60000
})

export const imgApi = Axios.create({
  baseURL: 'https://api.cloudinary.com/v1_1',
  timeout: 8000
})

api.interceptors.request.use(config => config, handleAxiosError)
api.interceptors.response.use(response => response, handleAxiosError)

function handleAxiosError(error) {
  if (error.response) {
    // The request was made and the server responded with a status code
    // that falls out of the range of 2xx
    logger.warn('[📡 AXIOS_ERROR_RESPONSE_DATA]', error.response.data)
  } else if (error.request) {
    // The request was made but no response was received
    logger.warn('[📡 AXIOS_ERROR_NO_RESPONSE]', error.request)
  } else {
    // Something happened in setting up the request that triggered an Error
    logger.warn('[📡 AXIOS_ERROR_INVALID_REQUEST]', error.message)
  }
  return Promise.reject(error)
}