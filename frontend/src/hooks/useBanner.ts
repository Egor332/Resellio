import { useDispatch } from 'react-redux'
import { showBanner } from '../store/banner/bannerSlice'

const useBanner = () => {
  const dispatch = useDispatch()

  return {
    showSuccess: (message: string, autoHideDuration?: number) => {
      dispatch(showBanner({ message, severity: 'success', autoHideDuration }))
    },
    showError: (message: string, autoHideDuration?: number) => {
      dispatch(showBanner({ message, severity: 'error', autoHideDuration }))
    },
    showInfo: (message: string, autoHideDuration?: number) => {
      dispatch(showBanner({ message, severity: 'info', autoHideDuration }))
    },
    showWarning: (message: string, autoHideDuration?: number) => {
      dispatch(showBanner({ message, severity: 'warning', autoHideDuration }))
    },
  }
}

export default useBanner
