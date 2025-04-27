import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

export function getEnvVariable(env: Record<string, string>, key: string): string {
  const value = env[key];
  if (!value) {
    throw new Error(`Environment variable "${key}" is required but not defined.`);
  }
  return value;
}

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd());

  const API_URL = getEnvVariable(env, 'VITE_API_URL');
  const PORT = parseInt(getEnvVariable(env, 'VITE_FRONTEND_PORT'));
  const HOST = getEnvVariable(env, 'VITE_HOST');

  return {
    plugins: [react()],
    server: {
      port: PORT,
      host: HOST,
      strictPort: true, // Exits when specified port is already in use
      proxy: { // For development - to avoid CORS issues, proxy API requests to the backend server
        '/api': {
          target: API_URL, // Use API_URL from .env or default
          changeOrigin: true, // Changes the origin of the request to match the target
          secure: false, // Disable SSL verification for development
        },
      },
    }
  }
})
