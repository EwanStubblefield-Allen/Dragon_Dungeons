import { AppState } from "../AppState.js"
import { logger } from "../utils/Logger.js"
import { dndApi } from "./AxiosService.js"

class InfosService {
  async getInfo() {
    AppState.info = (await dndApi.get('api')).data
  }

  async getInfoById(infoId) {
    let url = AppState.info[infoId.replace(' ', '-').toLowerCase()]
    AppState.infoArr = (await dndApi.get(url)).data.results
  }

  async getInfoDetails(i) {
    let res = await dndApi.get(i)

    if (res.data.reference) {
      res = await dndApi.get(res.data.reference.url)
    }
    delete res.data.url
    delete res.data.index
    logger.log(res.data)
    const infoDetails = Object.entries(res.data).map(d => {
      if (typeof d[1] == 'string' && this.checkIfLink({ url: d[1] })) {
        return { name: d[0], url: d[1] }
      }
      return d
    })
    AppState.infoDetails = this.handleObj(infoDetails).filter(i => i)
    logger.log(AppState.infoDetails)
    AppState.infoHtml = this.handleHtml(AppState.infoDetails)
  }

  handleObj(arr) {
    const newArr = arr.map(d => {
      if (Object.getPrototypeOf(d) == Object.prototype) {
        delete d.index
        logger.log('1', d)

        if (!this.checkIfLink(d)) {
          d = this.handleObj(Object.entries(d))
        } else {
          logger.log('what')
        }
      } else if (d[0] == 'from') {
        logger.log('4', d)

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
          logger.log('splicing empty array', d)
          return
        } else if (d[1].length == 1) {
          delete d[1][0].index
          logger.log('2', d)

          if (!this.checkIfLink(d[1][0]) && typeof d[1][0] != 'string') {
            d[1] = this.handleObj(Object.entries(d[1][0]))
          } else {
            d[1] = d[1][0]
          }
        } else if (Object.getPrototypeOf(d[1]) == Object.prototype) {
          delete d[1].index
          logger.log('3', d)

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

  handleHtml(arr, size = 1, list = 0) {
    let template = ''
    logger.log(arr)
    arr.forEach((a, index) => {
      if (typeof a == 'object') {
        if (Object.getPrototypeOf(a) == Object.prototype) {
          logger.log('should be object')

          if (list > 1) {
            if (index == 0) {
              template += /*HTML*/`<ul>`
            }
            logger.log('1')
            template += /*HTML*/`
              <li>
                <a href="#/info/${a.url.replace('/api/', '')}" class="selectable px-2">${a.name}</a>
              </li>`

            if (index == arr.length - 1) {
              template += /*HTML*/`</ul>`
              list = 0
            }
          } else {
            logger.log('2')

            template += /*HTML*/`
              <a href="#/info/${a.url.replace('/api/', '')}" class="selectable px-2">${a.name}</a>`
          }
        } else {
          logger.log('should be array', a)
          // debugger

          if (typeof a[0] == 'string') {
            a[0] += ':'
          }

          if (list > 1) {
            if (!Array.isArray(a[0])) {
              template += this.handleHtml(a, size + 1, list)
            } else {
              a.forEach((item, i) => {
                // debugger

                if (typeof item[0] != 'string') {
                  template += this.handleHtml(item, size + 1, false)
                } else {
                  item[0] = item[0].replaceAll(/[_\-$]/g, ' ').replaceAll(/(?<=[-\s]).|^./gm, String.call.bind(item[0].toUpperCase))

                  // if (i == 0) {
                  //   template += /*HTML*/`<ul>`
                  // }

                  if (typeof item[1] == 'object') {
                    logger.log('3')

                    if (Array.isArray(item[1])) {
                      template += this.handleHtml(item, size + 1, list)
                    } else {
                      template += /*HTML*/`
                      <li>
                        <p>${item[0]}:</p>
                        <a href="#/info/${item[1].url.replace('/api/', '')}" class="selectable px-2">${item[1].name}</a>
                      </li>`
                    }
                  } else {
                    item[1] = item[1].toString()
                    logger.log('4')

                    template += /*HTML*/`
                  <li>
                    <p>${item[0]}: ${item[1].replace(/^./gm, String.call.bind(item[1].toUpperCase))}</p>
                  </li>`
                  }

                  // if (i == arr.length - 1) {
                  //   template += /*HTML*/`</ul>`
                  //   list = 0
                  // }
                }
              })
            }
          } else {
            let itemType

            if (typeof a[0] == 'object') {
              itemType = 2
            } else if (Array.isArray(a[1])) {
              itemType = typeof a[1][0]

              for (let i = 1; i < a[1].length; i++) {
                if (itemType != typeof a[1][i]) {
                  itemType == 0
                }
              }

              if (itemType) {
                itemType = 1
              }
            }

            if (!itemType) {
              itemType = list
            }
            template += this.handleHtml(a, size + 1, itemType)
          }
        }
      } else {
        a = a.toString()

        if (list > 1) {
          if (index == 0) {
            template += /*HTML*/`<ul>`

            if (Object.getPrototypeOf(arr[1]) == Object.prototype) {
              a += ':'
            }
          }
          template += /*HTML*/`
              <li>
                <p>${a.replaceAll(/[_\-$]/g, ' ').replaceAll(/(?<=[-\s]).|^./gm, String.call.bind(a.toUpperCase))}</p>
              </li>`

          if (index == arr.length - 1) {
            template += /*HTML*/`</ul>`
            list = 0
          }
        } else if (index == 0) {
          logger.log('5')

          a = a.replace('desc', 'Description').replaceAll(/[_\-$]/g, ' ').replaceAll(/(?<=[-\s]).|^./gm, String.call.bind(a.toUpperCase))
          template += /*HTML*/`
            <div class="px-${size - 1}">
              <p class="fs-${size} fw-bold">${a}</p>`
        } else {
          logger.log('6')

          template += /*HTML*/`
              <p class="px-2">${a.replaceAll(/ \(.\)/g, '<br>-').replace(/\(.\)/g, '-').replaceAll(/(?<=- )./g, String.call.bind(a.toUpperCase))}</p>
            </div>`
        }
        logger.log('should be string', a)
      }

      if (list > 0) {
        list++
      }
    })
    template = template.trim()

    if (template.startsWith('<div') && !template.endsWith('</div>')) {
      template += '</div>'
    }
    return template
  }
}

export const infosService = new InfosService()