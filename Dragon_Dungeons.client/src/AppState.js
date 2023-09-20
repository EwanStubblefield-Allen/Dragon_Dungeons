import { reactive } from 'vue'
import { loadState } from './utils/Store.js'
import { Character } from './models/Character.js'

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
  tempCharacter: loadState('tempCharacter', Character) ?? {},
  charPage: loadState('charPage') ?? 0
})