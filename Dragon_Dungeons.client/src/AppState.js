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
  xpLevels: [0, 300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000, 120000, 140000, 165000, 195000, 225000, 265000, 305000, 355000],
  notes: null,
  initiative: null,
  trade: {
    offer: { equipment: [], currency: [] },
    want: { equipment: [], currency: [] }
  }
})