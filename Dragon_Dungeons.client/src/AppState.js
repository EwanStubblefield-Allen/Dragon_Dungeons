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
  /** @type {import('./models/Character.js').Character[]} */
  characters: [],
  /** @type {import('./models/Character.js').Character | null} */
  activeCharacter: null,
  /** @type {import('./models/Character.js').Character} */
  tempCharacter: loadState('tempCharacter', Character),
  /** @type {import('./models/Campaign.js').Campaign[]} */
  campaigns: [],
  /** @type {import('./models/Campaign.js').Campaign | null} */
  activeCampaign: null,
  /** @type {import('./models/Campaign.js').Campaign} */
  tempCampaign: loadState('tempCampaign', Campaign),
  tempClass: loadState('tempClass', Object),
  charPage: loadState('charPage') ?? 0,
  camPage: loadState('camPage') ?? 0,
  equipment: loadState('equipment', Object),
  attributes: ['str', 'dex', 'con', 'int', 'wis', 'cha'],
  notes: null,
  initiative: null
})