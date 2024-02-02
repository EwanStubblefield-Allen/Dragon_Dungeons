import { AppState } from "../AppState.js"
import { saveState } from "../utils/Store.js"
import { dndApi } from "./AxiosService.js"

class InfosService {
  async getInfo() {
    const res = await dndApi.get('api')
    delete res.data.backgrounds
    AppState.info = res.data
  }

  async getInfoById(infoId, arr = true) {
    let url = AppState.info[infoId.replace(' ', '-').toLowerCase()]
    const res = await dndApi.get(url)

    if (!arr) {
      return res.data.results
    }
    AppState.infoArr = res.data.results
  }

  async getInfoDetails(i, arr = true) {
    let res = await dndApi.get(i)
    let data = res.data

    if (!arr) {
      return data
    }

    // If data is an object convert to array and remove unnecessary keys
    if (Object.getPrototypeOf(data) == Object.prototype) {
      delete data.url
      delete data.index
      data = Object.entries(data).map(d => {
        if (typeof d[1] == 'string' && this.isLink({ url: d[1] })) {
          return { name: d[0], url: d[1] }
        }
        return d
      })
    }
    AppState.infoHtml = this.convertToHtml(data)
  }

  convertToHtml(arr, size = 0) {
    let template = /*HTML*/`<div class="ps-2">`
    arr.forEach((item, index) => {
      if (typeof item == 'object') {
        if (!Array.isArray(item)) {
          delete item.index

          if (!this.isLink(item)) {
            template += this.convertToHtml(Object.entries(item), size)
          } else {
            template += this.objToHtml(arr, item, size + 1)
          }
        } else {
          template += this.handleArr(item, size)
        }
      } else {
        template += this.strToHtml(arr, item, index, size + 1)
      }
    })
    template += '</div>'
    return template
  }

  isLink(obj) {
    if (typeof obj == 'string') {
      return false
    }

    return Object.values(obj).find(o => {
      if (typeof o == 'string' && o.includes('api/')) {
        if (!obj.name && obj.level) {
          obj.name = `Level ${obj.level}`
        }
        return true
      } else {
        return false
      }
    })
  }

  handleArr(arr, size) {
    // If 'from' is present indicates that arr needs to formatted into a multiple choice
    if (arr[0] == 'from') {
      // Checks to see how multiple choice is formatted
      if (arr[1].options) {
        if (arr[1].options[0].choice) {
          let tempArr = []
          arr[1].options.forEach(option => {
            tempArr = tempArr.concat(option.choice.from.options)
          })
          arr = tempArr
        } else {
          arr = arr[1].options
        }
      } else {
        arr = Object.entries(arr[1])
      }

      // Finds the arr tail to the options of the multiple choice
      arr = arr.map(option => {
        let goDeeper = true

        while (goDeeper) {
          let foundObj = Object.values(option).find(o => typeof o == 'object')

          if (!foundObj) {
            goDeeper = false
          } else {
            option = foundObj
          }
        }

        if (!option.index) {
          return ''
        }
        delete option.index
        return option
      })
    } else if (Array.isArray(arr[1])) {
      if (!arr[1].length) {
        return ''
      } else if (arr[1].length == 1) {
        if (this.isLink(arr[1][0]) || typeof arr[1][0] == 'string') {
          arr[1] = arr[1][0]
        } else {
          delete arr[1][0].index
          arr[1] = this.convertToHtml(Object.entries(arr[1][0]))
        }
      }
    } else if (Object.getPrototypeOf(arr[1]) == Object.prototype) {
      delete arr[1].index

      if (!this.isLink(arr[1])) {
        arr[1] = this.convertToHtml(Object.entries(arr[1]))
      }
    }
    return this.convertToHtml(arr, size)
  }

  objToHtml(arr, a, size) {
    if (a.name == 'image') {
      return /*HTML*/`
        <div class="text-center text-lg-start">
          <img class="mt-2 img-fluid rounded elevation-5" src="http://www.dnd5eapi.co${a.url}" alt="${arr[0][1]}Image" style="max-height: 75vh;"/>
        </div>`
    } else if (arr.find(n => n[0] == 'name')) {
      return /*HTML*/`
        <a href="#/info/${a.url.replace('/api/', '')}" class="fs-${size} fw-bold text-decoration text-capitalize text-dark"><u>${a.name.replaceAll(/[_\-$]/g, ' ')}</u></a>`
    } else {
      return /*HTML*/`
        <a href="#/info/${a.url.replace('/api/', '')}" class="fs-${size + 3} text-decoration text-dark ps-2"><u>${a.name}</u></a>`
    }
  }

  strToHtml(arr, a, index, size) {
    a = a.toString()

    if (index == 0 && arr.length == 2 && 2 < a.length && a.length < 30) {
      return /*HTML*/`
        <p class="fs-${size} fw-bold text-capitalize">
          ${
            // Replaces desc with Description
            a.replace(/desc(?!r)/g, 'Description')
            // Replaces _, -, and $ with a space
            .replaceAll(/[_\-$]/g, ' ')}:
        </p>`
    } else {
      return /*HTML*/`
        <p class="fs-${size + 3} ps-2">
          ${
            // Replace ' '(a through d) with <br>-
            a.replaceAll(/ \([a-d]\)/g, '<br>-')
            // Replace (a through d) with -
            .replace(/\([a-d]\)/g, '-')
            // Replace - var or \n var with an uppercase of var
            .replaceAll(/(?<=- ).|^./g, String.call.bind(a.toUpperCase))}
        </p>`
    }
  }

  async getEquipment() {
    const character = AppState.activeCharacter

    if (character.id == AppState.equipment.id) {
      return
    }
    AppState.equipment = { id: character.id, weapons: [], cantrips: [], spells: {} }

    if (character.armor?.index) {
      AppState.equipment.armor = await infosService.getInfoDetails(character.armor.url, false)
    }
    AppState.equipment.weapons = await Promise.all(character.weapons.map(async (w) => {
      return await infosService.getInfoDetails(w.url, false)
    }))

    if (character.cantrips) {
      AppState.equipment.cantrips = await Promise.all(character.cantrips.map(async (c) => {
        return await infosService.getInfoDetails(c.url, false)
      }))
    }

    if (character.spells) {
      Object.keys(character.casting).forEach(c => AppState.equipment.spells[c] = [])

      for (let s of character.spells) {
        const spell = await infosService.getInfoDetails(s.url, false)
        AppState.equipment.spells[s.level].push(spell)
      }
    }
    saveState('equipment', AppState.equipment)
  }
}

export const infosService = new InfosService()