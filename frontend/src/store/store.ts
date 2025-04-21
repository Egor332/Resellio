import { configureStore } from '@reduxjs/toolkit'
import authReducer, { logout } from './auth/authSlice'
import bannerReducer from './banner/bannerSlice'

const authMiddleware = (store: any) => {
  let listenerAdded = false

  return (next: any) => (action: any) => {
    if (!listenerAdded) {
      const handleAuthError = (event: CustomEvent) => {
        if (event.detail?.status === 401) {
          store.dispatch(logout())
        }
      }

      window.addEventListener('auth_error', handleAuthError as EventListener)
      listenerAdded = true
    }

    return next(action)
  }
}

const store = configureStore({
  reducer: {
    auth: authReducer,
    banner: bannerReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(authMiddleware),
  devTools: process.env.NODE_ENV !== 'production',
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export default store
