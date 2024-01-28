import { AppState } from '../AppState'
import { campaignHub } from '../handlers/CampaignHub.js'
import { Account } from '../models/Account.js'
import { router } from '../router.js'
import { logger } from '../utils/Logger'
import { api } from './AxiosService'

class AccountService {
  async getAccount() {
    try {
      const res = await api.get('account')
      AppState.account = new Account(res.data)
      campaignHub.enterGroup(AppState.account.id)
    } catch (err) {
      logger.error('HAVE YOU STARTED YOUR SERVER YET???', err)
      router.push('/')
    }
  }

  async updateAccount(formData) {
    const res = await api.put('account', formData)
    AppState.account = new Account(res.data)
  }
}

export const accountService = new AccountService()