import { ReactNode, useEffect, useState } from 'react'
import { Navigate } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../../store/store'
import { Navigation } from '../../assets/constants/navigation'
import { Role } from '../../store/auth/authSlice'
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Button,
} from '@mui/material'

type ProtectedRouteProps = {
  children: ReactNode
  requiredRole?: Role
}

const ProtectedRoute = ({ children, requiredRole }: ProtectedRouteProps) => {
  const { isAuthenticated, user } = useSelector(
    (state: RootState) => state.auth
  )
  const userRole = user?.role

  const [openDialog, setOpenDialog] = useState(false)
  const [errorMessage, setErrorMessage] = useState('')
  const [redirectTo, setRedirectTo] = useState<string | null>(null)
  const [shouldRedirect, setShouldRedirect] = useState(false)

  useEffect(() => {
    if (!isAuthenticated) {
      setOpenDialog(true)
      setErrorMessage('You must be logged in to access this page.')
      setRedirectTo(Navigation.LOGIN)
      setShouldRedirect(true)
    } else if (requiredRole && userRole !== requiredRole) {
      setOpenDialog(true)
      setErrorMessage(
        `You need ${requiredRole} privileges to access this page.`
      )
      setRedirectTo(null)
      setShouldRedirect(true)
    } else {
      setOpenDialog(false)
      setShouldRedirect(false)
    }
  }, [isAuthenticated, userRole, requiredRole])

  const handleClose = () => {
    setOpenDialog(false)
  }

  if (!openDialog) {
    if (shouldRedirect) {
      return redirectTo ? (
        <Navigate to={redirectTo} replace />
      ) : (
        <Navigate to="-1" />
      )
    }
    return <>{children}</>
  }

  return (
    <Dialog
      open={openDialog}
      onClose={handleClose}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle id="alert-dialog-title">{'Access Error'}</DialogTitle>
      <DialogContent>
        <DialogContentText id="alert-dialog-description">
          {errorMessage}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="primary" autoFocus>
          OK
        </Button>
      </DialogActions>
    </Dialog>
  )
}

export default ProtectedRoute
