import { createSlice, PayloadAction } from '@reduxjs/toolkit'

type BannerSeverity = 'error' | 'warning' | 'info' | 'success'

interface BannerState {
  message: string
  open: boolean
  severity: BannerSeverity
  autoHideDuration: number
}

const initialState: BannerState = {
  message: '',
  open: false,
  severity: 'info',
  autoHideDuration: 5000,
}

const bannerSlice = createSlice({
  name: 'banner',
  initialState,
  reducers: {
    showBanner: (
      state,
      action: PayloadAction<{
        message: string
        severity?: BannerSeverity
        autoHideDuration?: number
      }>
    ) => {
      state.message = action.payload.message
      state.severity = action.payload.severity || 'info'
      state.autoHideDuration = action.payload.autoHideDuration || 5000
      state.open = true
    },
    hideBanner: (state) => {
      state.open = false
    },
  },
})

export const { showBanner, hideBanner } = bannerSlice.actions
export default bannerSlice.reducer
