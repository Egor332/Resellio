import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd());

  const API_URL = `${env.VITE_API_URL ?? 'http://localhost:80'}`;
  console.log('API_URL in vite.config.ts:', API_URL); // Log the API URL for debugging
  const PORT = parseInt(env.VITE_FRONTEND_PORT ?? '5173');
  const HOST = `${env.VITE_HOST ?? 'localhost'}`;

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
