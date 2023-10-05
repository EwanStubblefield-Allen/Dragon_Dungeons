import { AppState } from "../AppState.js"
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

    if (!arr) {
      return res.data
    }

    if (res.data.reference) {
      res = await dndApi.get(res.data.reference.url)
    }

    if (Object.getPrototypeOf(res.data) == Object.prototype) {
      delete res.data.url
      delete res.data.index
      res.data = Object.entries(res.data).map(d => {
        if (typeof d[1] == 'string' && this.checkIfLink({ url: d[1] })) {
          return { name: d[0], url: d[1] }
        }
        return d
      })
    }
    AppState.infoDetails = this.handleObj(res.data).filter(i => i)
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
          if (d[1].options[0].choice) {
            let template = []
            d[1].options.forEach(option => {
              template = template.concat(option.choice.from.options)
            })
            d = template
          } else {
            d = d[1].options
          }
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

          if (option.index) {
            delete option.index
            return option
          }
          return this.handleObj(Object.entries(option))
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

  handleHtml(arr, size = 0) {
    let template = ''
    template += `<div class="ps-2">`
    arr.forEach((a, index) => {
      if (typeof a == 'object') {
        if (Array.isArray(a)) {
          template += this.arrHtml(a, size)
        } else {
          template += this.objHtml(arr, a, size + 1)
        }
      } else if (a == null) {
        return
      } else {
        template += this.strHtml(arr, a, index, size + 1)
      }
    })
    template += '</div>'
    return template
  }

  arrHtml(a, size) {
    let template = ''

    if (typeof a[0] == 'string' && a.length == 2 && 2 < a[0].length && a[0].length < 30) {
      a[0] += ':'
    }

    template += this.handleHtml(a, size)
    return template
  }

  objHtml(arr, a, size) {
    let template = ''

    if (a.name == 'image') {
      template += /*HTML*/`
      <div class="text-center text-lg-start">
        <img class="mt-2 img-fluid rounded elevation-5" src="http://www.dnd5eapi.co${a.url}" alt="${arr[0][1]}Image" style="max-height: 75vh;"/>
      </div>`
    } else if (arr.find(n => n[0] == 'name:')) {
      template += /*HTML*/`
        <a href="#/info/${a.url.replace('/api/', '')}" class="fs-${size} fw-bold text-decoration text-capitalize text-dark"><u>${a.name.replaceAll(/[_\-$]/g, ' ')}</u></a>`
    } else {
      template += /*HTML*/`
        <a href="#/info/${a.url.replace('/api/', '')}" class="fs-${size + 3} text-decoration text-dark ps-2"><u>${a.name}</u></a>`
    }
    return template
  }

  strHtml(arr, a, index, size) {
    let template = ''
    a = a.toString()

    if (index == 0 && arr.length == 2 && 2 < a.length && a.length < 30) {
      template += /*HTML*/`
        <p class="fs-${size} fw-bold text-capitalize">${a.replace(/desc(?!r)/g, 'Description').replaceAll(/[_\-$]/g, ' ')}</p>`
    } else {
      template += /*HTML*/`
        <p class="fs-${size + 3} ps-2">${a.replaceAll(/ \(.\)/g, '<br>-').replace(/\(.\)/g, '-').replaceAll(/(?<=- ).|^./g, String.call.bind(a.toUpperCase))}</p>`
    }
    return template
  }
}

export const infosService = new InfosService()