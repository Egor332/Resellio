import getEnvVariable from '../../utils/envUtils'

export interface TApiEndpoint {
  url: string
  method: 'GET' | 'POST' | 'PUT' | 'DELETE'
  isAuthRequired: boolean
}

export type TApiEndpoints = Record<string, TApiEndpoint>

export const apiEndpoints: TApiEndpoints = {
  CUSTOMERS_REGISTER: {
    url: '/api/Customers/register',
    method: 'POST',
    isAuthRequired: false,
  },
  ORGANISERS_REGISTER: {
    url: '/api/Organisers/register',
    method: 'POST',
    isAuthRequired: false,
  },
  LOGIN: {
    url: '/api/Authentication/login',
    method: 'POST',
    isAuthRequired: false,
  },
  REQUEST_PASSWORD_RESET: {
    url: '/api/ResetPassword/request-password-reset',
    method: 'POST',
    isAuthRequired: false,
  },
  RESET_PASSWORD: {
    url: '/api/ResetPassword/reset-password',
    method: 'POST',
    isAuthRequired: false,
  },
  USER_INFO: {
    url: '/api/Users/user-info',
    method: 'GET',
    isAuthRequired: true,
  },
}

const apiUrl = getEnvVariable(import.meta.env, 'VITE_API_URL')

export const getApiEndpoint = (endpoint: TApiEndpoint): TApiEndpoint => {
  // Use relative paths in development to leverage the proxy
  if (import.meta.env.MODE === 'development') {
    return endpoint // Relative path (e.g., "/api/customers/register")
  }

  // Use full URL in production
  return {
    ...endpoint,
    url: `${apiUrl}${endpoint.url}`,
  }
}
