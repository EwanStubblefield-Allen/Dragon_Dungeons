import { AppState } from "../AppState.js"
import { dndApi } from "./AxiosService.js"

class InfosService {
  async getInfo() {
    const res = await dndApi.get('api')
    delete res.data.backgrounds
    AppState.info = res.data
  }

  async getInfoById(infoId) {
    let url = AppState.info[infoId.replace(' ', '-').toLowerCase()]
    AppState.infoArr = (await dndApi.get(url)).data.results
  }

  async getInfoDetails(i, arr = true) {
    let res = await dndApi.get(i)

    if (!arr) {
      return res.data
    }

    if (res.data.reference) {
      res = await dndApi.get(res.data.reference.url)
    }
    delete res.data.url
    delete res.data.index
    const infoDetails = Object.entries(res.data).map(d => {
      if (typeof d[1] == 'string' && this.checkIfLink({ url: d[1] })) {
        return { name: d[0], url: d[1] }
      }
      return d
    })
    AppState.infoDetails = this.handleObj(infoDetails).filter(i => i)
    AppState.infoHtml = this.handleHtml(AppState.infoDetails)
  }

  handleObj(arr) {
    const newArr = arr.map(d => {
      if (Object.getPrototypeOf(d) == Object.prototype) {
        delete d.index

        if (!this.checkIfLink(d)) {
          d = this.handleObj(Object.entries(d))
        }
      } else if (d[0] == 'from') {
        if (d[1].options) {
          d = d[1].options
        } else {
          d = Object.entries(d[1])
        }
        d = d.map(option => {
          let goDeeper = true
          let foundObj

          while (goDeeper) {
            foundObj = Object.values(option).find(o => typeof o == 'object')

            if (!foundObj) {
              goDeeper = false
            } else {
              option = foundObj
            }
          }
          delete option.index
          return option
        })
      } else if (typeof d[1] == 'object') {
        if (Array.isArray(d[1]) && !d[1].length) {
          return
        } else if (d[1].length == 1) {
          delete d[1][0].index

          if (!this.checkIfLink(d[1][0]) && typeof d[1][0] != 'string') {
            d[1] = this.handleObj(Object.entries(d[1][0]))
          } else {
            d[1] = d[1][0]
          }
        } else if (Object.getPrototypeOf(d[1]) == Object.prototype) {
          delete d[1].index

          if (!this.checkIfLink(d[1])) {
            d[1] = this.handleObj(Object.entries(d[1]))
          }
        } else {
          d[1] = this.handleObj(d[1])
        }
      }
      return d
    })
    return newArr
  }

  checkIfLink(obj) {
    if (typeof obj == 'string') {
      return false
    }
    return Object.values(obj).find(o => {
      if (typeof o == 'string') {
        return o.includes('api/')
      }
      return false
    })
  }

  handleHtml(arr, size = 0) {
    let template = ''
    template += `<div class="px-${size}">`
    arr.forEach((a, index) => {
      if (typeof a == 'object') {
        if (Array.isArray(a)) {
          template += this.arrHtml(a, size)
        } else {
          size++
          template += this.objHtml(arr, a, size)
        }
      } else if (!a) {
        return
      } else {
        size++
        template += this.strHtml(a, index, size)
      }
    })
    template += '</div>'

    return template
  }

  arrHtml(a, size) {
    let template = ''

    if (typeof a[0] == 'string' && a.length == 2 && a[0].length < 30) {
      a[0] += ':'
    }

    template += this.handleHtml(a, size)
    return template
  }

  objHtml(arr, a, size) {
    let template = ''

    if (arr.find(n => n[0] == 'name:')) {
      template += /*HTML*/`
        <a href="#/info/${a.url.replace('/api/', '')}" class="fs-${size} fw-bold text-decoration text-capitalize text-dark"><u>${a.name.replaceAll(/[_\-$]/g, ' ')}</u></a>`
    } else {
      template += /*HTML*/`
        <a href="#/info/${a.url.replace('/api/', '')}" class="text-decoration text-dark px-2"><u>${a.name}</u></a>`
    }
    return template
  }

  strHtml(a, index, size) {
    let template = ''
    a = a.toString()

    if (index == 0 && a.length < 30) {
      template += /*HTML*/`
        <p class="fs-${size} fw-bold text-capitalize">${a.replace(/desc(?!r)/g, 'Description').replaceAll(/[_\-$]/g, ' ')}</p>`
    } else {
      template += /*HTML*/`
        <p class="px-2">${a.replaceAll(/ \(.\)/g, '<br>-').replace(/\(.\)/g, '-').replaceAll(/(?<=- )./g, String.call.bind(a.toUpperCase))}</p>`
    }
    return template
  }
}

export const infosService = new InfosService()