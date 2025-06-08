import { TApiEndpoint } from '../assets/constants/api'
import getEnvVariable from '../utils/envUtils'

const DEFAULT_TIMEOUT = parseInt(
  getEnvVariable(import.meta.env, 'VITE_DEFAULT_HTTP_TIMEOUT')
)

/**
 * A generic HTTP client function to make API requests.
 * @param endpoint - The API endpoint object containing URL, method, and auth requirement.
 * @param data - Optional data to be sent with the request.
 * @returns A promise that resolves to the response data.
 */
export const apiRequest = async (
  endpoint: TApiEndpoint,
  data?: Record<string, any> | FormData,
  isFormData?: boolean,
  timeout?: number
): Promise<any> => {
  try {
    const token = endpoint.isAuthRequired
      ? localStorage.getItem('auth_token')
      : null
    const controller = new AbortController()
    const timeoutId = setTimeout(
      () => controller.abort(),
      timeout ?? DEFAULT_TIMEOUT
    )

    const response = await fetch(endpoint.url, {
      method: endpoint.method,
      headers: {
        ...(data && !isFormData ? { 'Content-Type': 'application/json' } : {}),
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
      },
      body: data
        ? isFormData
          ? (data as FormData)
          : JSON.stringify(data)
        : undefined,
      signal: controller.signal,
    })

    clearTimeout(timeoutId)

    if (response.status === 401 && endpoint.isAuthRequired) {
      window.dispatchEvent(
        new CustomEvent('auth_error', {
          detail: { status: 401 },
          bubbles: true,
          cancelable: false,
        })
      )

      throw new Error('Authentication failed. Please log in again.')
    }

    if (!response.ok) {
      let errorMessage = `HTTP ${response.status} errror!`

      try {
        const errorData = await response.text();
        if (errorData) {
          errorMessage = errorData
        }
      } catch (parseError) {
        console.warn('Could not parse error response as text', parseError);
      }

      throw new Error(errorMessage)
    }

    try {
      return await response.json()
    } catch (error) {
      console.warn(
        'Response could not be parsed as JSON, returning empty object'
      )
      return {}
    }
  } catch (error: any) {
    const isTimeout = error.name === 'AbortError'

    throw new Error(
      isTimeout
        ? 'The request timed out. Please try again.'
        : `${error.message}`
    )
  }
}
