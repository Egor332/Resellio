import { Alert, Snackbar } from '@mui/material'
import { useSelector, useDispatch } from 'react-redux'
import { hideBanner } from '../../store/banner/bannerSlice'

const Banner: React.FC = () => {
  const dispatch = useDispatch()
  const { message, open, severity, autoHideDuration } = useSelector(
    (state: any) => state.banner
  )

  const handleClose = () => {
    dispatch(hideBanner())
  }

  return (
    <Snackbar
      open={open}
      autoHideDuration={autoHideDuration}
      onClose={handleClose}
      anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
    >
      <Alert
        onClose={handleClose}
        severity={severity}
        variant="filled"
        sx={{ width: '100%' }}
      >
        {message}
      </Alert>
    </Snackbar>
  )
}

export default Banner
