export interface TApiEndpoint {
    url: string; 
    method: 'GET' | 'POST' | 'PUT' | 'DELETE'; 
    isAuthRequired: boolean; 
}

export type TApiEndpoints = Record<string, TApiEndpoint>;

export const apiEndpoints: TApiEndpoints = {
    CUSTOMERS_REGISTER: {
        url: '/api/Customers/register',
        method: 'POST',
        isAuthRequired: false,
    },
    LOGIN: {
        url: '/api/Authentication/login',
        method: 'POST',
        isAuthRequired: false,
    }
};

const apiUrl = import.meta.env.VITE_API_URL ?? 'http://localhost:80';

export const getApiEndpoint = (endpoint: TApiEndpoint): TApiEndpoint => {
    // Use relative paths in development to leverage the proxy
    if (import.meta.env.MODE === 'development') {
        return endpoint; // Relative path (e.g., "/api/customers/register")
    }

    // Use full URL in production
    return {
        ...endpoint,
        url: `${apiUrl}${endpoint.url}`,
    };
};