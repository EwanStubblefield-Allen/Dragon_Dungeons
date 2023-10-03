import { reactive } from 'vue'
import { loadState } from './utils/Store.js'
import { Character } from './models/Character.js'
import { Campaign } from './models/Campaign.js'

// NOTE AppState is a reactive object to contain app level data
export const AppState = reactive({
  user: {},
  /** @type {import('./models/Account.js').Account} */
  account: {},
  info: {},
  infoArr: [],
  infoDetails: {},
  infoHtml: '',
  /** @type {import('./models/Character.js').Character.js[]} */
  characters: [],
  /** @type {import('./models/Character.js').Character.js} */
  tempCharacter: loadState('tempCharacter', Character),
  /** @type {import('./models/Campaign.js').Campaign.js[]} */
  campaigns: [],
  /** @type {import('./models/Campaign.js').Campaign.js} */
  tempCampaign: loadState('tempCampaign', Campaign),
  charPage: loadState('charPage') ?? 0,
  camPage: loadState('camPage') ?? 0
})