import getEnvVariable from '../../utils/envUtils'

export enum BodyTypes {
  JSON = 'application/json',
  FORM_URLENCODED = 'application/x-www-form-urlencoded',
  MULTIPART_FORM_DATA = 'multipart/form-data',
}

export interface TApiEndpoint {
  url: string
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH'
  isAuthRequired: boolean
}

export type TApiEndpoints = Record<string, TApiEndpoint>

export const API_ENDPOINTS: TApiEndpoints = {
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
  CREATE_EVENT: {
    url: '/api/Events/event',
    method: 'POST',
    isAuthRequired: true,
  },
  USER_INFO: {
    url: '/api/Users/user-info',
    method: 'GET',
    isAuthRequired: true,
  },
  GET_EVENTS: {
    url: `/api/Events/events`,
    method: 'GET',
    isAuthRequired: true,
  },
  GET_TICKET_TYPES: {
    url: '/api/TicketTypes/ticket-types-of-event',
    method: 'GET',
    isAuthRequired: true,
  },
  GET_TICKETS: {
    url: '/api/Tickets/available-tickets-of-selected-type',
    method: 'GET',
    isAuthRequired: true,
  },
  LOCK_TICKET: {
    url: '/api/ShoppingCart/lock-ticket',
    method: 'POST',
    isAuthRequired: true,
  },
  UNLOCK_TICKET: {
    url: '/api/ShoppingCart/unlock-ticket',
    method: 'POST',
    isAuthRequired: true,
  },
  CART_INFO: {
    url: '/api/ShoppingCart/cart-info',
    method: 'GET',
    isAuthRequired: true,
  },
  MY_TICKETS: {
    url: '/api/Tickets/my-tickets',
    method: 'GET',
    isAuthRequired: true,
  },
  RESELL_TICKET: {
    url: '/api/Tickets/resell',
    method: 'PATCH',
    isAuthRequired: true,
  },
  STOP_RESELLING: {
    url: '/api/Tickets/stop-selling',
    method: 'PATCH',
    isAuthRequired: true,
  },
}

const apiUrl = getEnvVariable(import.meta.env, 'VITE_API_URL')

export const getApiEndpoint = (endpoint: TApiEndpoint, params?: URLSearchParams): TApiEndpoint => {
  const urlSufix = params ? `${endpoint.url}?${params}` : endpoint.url

  // Use relative paths in development to leverage the proxy
  if (import.meta.env.MODE === 'development') {
    return {
      ...endpoint,
      url: urlSufix
    }
  }

  // Use full URL in production
  return {
    ...endpoint,
    url: `${apiUrl}${urlSufix}`,
  }
}
