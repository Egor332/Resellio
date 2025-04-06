import { TApiEndpoint } from "../assets/constants/api";

const DEFAULT_TIMEOUT = parseInt(import.meta.env.VITE_HTTP_TIMEOUT) || 10000;

/**
 * A generic HTTP client function to make API requests.
 * @param endpoint - The API endpoint object containing URL, method, and auth requirement.
 * @param data - Optional data to be sent with the request.
 * @returns A promise that resolves to the response data.
 */
export const apiRequest = async (endpoint: TApiEndpoint, data?: Record<string, any>, timeout?: number): Promise<any> => {
    try {
        console.log('Requesting:', endpoint.url, data); // TODO: Remove in production
        const response = await fetch(endpoint.url, {
            method: endpoint.method,
            headers: {
                ...(data ? { 'Content-Type': 'application/json' } : {}),
            },
            body: data ? JSON.stringify(data) : undefined,
            signal: AbortSignal.timeout(timeout ?? DEFAULT_TIMEOUT),
        });

        if (!response.ok) {
            throw new Error(
                `HTTP error! Status: ${response.status} ${response.statusText}`
            );
        }

        return await response.json().catch(() => {
            throw new Error('Failed to parse response JSON.');
        });
    } catch (error: any) {
        const isTimeout = error.name === 'AbortError';
        const isNetworkError = error instanceof TypeError;

        throw new Error(
            isTimeout
                ? 'The request timed out. Please try again.'
                : isNetworkError
                ? 'A network error occurred. Please check your connection.'
                : `${error.message}`
        );
    }
};