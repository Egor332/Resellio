import { apiRequest } from './httpClient'
import { API_ENDPOINTS, getApiEndpoint } from '../assets/constants/api'
import { Customer, Organiser } from '../store/auth/authSlice'
import {
  LoginCredentials,
  CustomerRegisterData,
  OrganiserRegisterData,
  Role,
} from '../store/auth/authSlice'

interface LoginResponse {
  token: string
  userRole: Role
  message: string
}

interface RegistersResponse {
  id: number
  message: string
}

const authService = {
  login: async (credentials: LoginCredentials): Promise<LoginResponse> => {
    try {
      return await apiRequest(getApiEndpoint(API_ENDPOINTS.LOGIN), credentials)
    } catch (error: any) {
      throw new Error(error.message || 'Login failed')
    }
  },

  registerCustomer: async (
    customerData: CustomerRegisterData
  ): Promise<RegistersResponse> => {
    try {
      return await apiRequest(
        getApiEndpoint(API_ENDPOINTS.CUSTOMERS_REGISTER),
        customerData
      )
    } catch (error: any) {
      throw new Error(error.message || 'Registration failed')
    }
  },

  registerOrganiser: async (
    organiserData: OrganiserRegisterData
  ): Promise<RegistersResponse> => {
    try {
      return await apiRequest(
        getApiEndpoint(API_ENDPOINTS.ORGANISERS_REGISTER),
        organiserData
      )
    } catch (error: any) {
      throw new Error(error.message || 'Organiser registration failed')
    }
  },

  getCurrentUser: async (): Promise<Customer | Organiser> => {
    try {
      // TODO: add full support
      return await apiRequest(getApiEndpoint(API_ENDPOINTS.CURRENT_USER))
    } catch (error: any) {
      throw new Error(error.message || 'Failed to get user info')
    }
  },

  requestPasswordReset: async (email: string): Promise<{ message: string }> => {
    try {
      return await apiRequest(
        getApiEndpoint(API_ENDPOINTS.REQUEST_PASSWORD_RESET),
        { email }
      )
    } catch (error: any) {
      throw new Error(error.message || 'Failed to request password reset')
    }
  },

  resetPassword: async (
    token: string,
    newPassword: string
  ): Promise<{ message: string }> => {
    try {
      return await apiRequest(getApiEndpoint(API_ENDPOINTS.RESET_PASSWORD), {
        token,
        newPassword,
      })
    } catch (error: any) {
      throw new Error(error.message || 'Failed to reset password')
    }
  },
}

export default authService
