import { SyntheticEvent, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useDispatch } from 'react-redux'
import 'bootstrap/dist/css/bootstrap.min.css'
import { Navigation } from '../../assets/constants/navigation'
import { LoginCredentials, loginUser } from '../../store/auth/authSlice.ts'
import { AppDispatch } from '../../store/store.ts'
import { Button, TextField, Typography, Box } from '@mui/material'
import RegistrationDialog from '../../components/RegistrationDialog/RegistrationDialog'
import RequestPasswordResetDialog from '../../components/RequestPasswordResetDialog/RequestPasswordResetDialog'
import ResendVerificationEmailDialog from '../../components/ResendVerificationEmailDialog/ResendVerificationEmailDialog'
import useBanner from '../../hooks/useBanner'

function Login() {
  const [loginCredentials, setLoginCredentials] = useState<LoginCredentials>({
    email: '',
    password: '',
  })
  const [isLoading, setIsLoading] = useState(false)
  const [isRegisterDialogOpen, setIsRegisterDialogOpen] = useState(false)
  const [isResetDialogOpen, setIsResetDialogOpen] = useState(false)
  const [isResendVerificationDialogOpen, setIsResendVerificationDialogOpen] = useState(false)
  const dispatch = useDispatch<AppDispatch>()
  const navigate = useNavigate()
  const banner = useBanner()

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    setLoginCredentials({ ...loginCredentials, [name]: value })
  }

  const handleSubmit = async (event: SyntheticEvent) => {
    event.preventDefault()
    setIsLoading(true)

    try {
      const loginResult = await dispatch(loginUser(loginCredentials)).unwrap()

      const role = loginResult.user.role
      if (role === 'Customer') {
        navigate(Navigation.CUSTOMERS, { replace: true })
      }
      if (role === 'Organiser') {
        navigate(Navigation.ORGANISERS, { replace: true })
      }
    } catch (err: any) {
      banner.showError(
        err.message ||
          err.error ||
          'Login failed. Please check your credentials.'
      ) // TODO: get rid of any + ensure proper message
    } finally {
      setIsLoading(false)
    }
  }

  const openRegisterDialog = () => {
    setIsRegisterDialogOpen(true)
  }

  const closeRegisterDialog = () => {
    setIsRegisterDialogOpen(false)
  }

  const openResetDialog = () => {
    setIsResetDialogOpen(true)
  }

  const closeResetDialog = () => {
    setIsResetDialogOpen(false)
  }

  const openResendVerificationDialog = () => {
    setIsResendVerificationDialogOpen(true)
  }

  const closeResendVerificationDialog = () => {
    setIsResendVerificationDialogOpen(false)
  }

  return (
    <Box sx={{ width: '100%', maxWidth: 400, p: 3 }}>
      <Typography variant="h4" component="h1" gutterBottom align="center">
        Log in
      </Typography>

      <form onSubmit={handleSubmit}>
        <TextField
          fullWidth
          label="Email"
          name="email"
          type="email"
          value={loginCredentials.email}
          onChange={handleChange}
          margin="normal"
          required
          autoFocus
        />

        <TextField
          fullWidth
          label="Password"
          name="password"
          type="password"
          value={loginCredentials.password}
          onChange={handleChange}
          margin="normal"
          required
        />

        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
          sx={{ mt: 2 }}
          disabled={isLoading}
        >
          {isLoading ? 'Logging in...' : 'Log In'}
        </Button>

        <Box sx={{ mt: 2, display: 'flex', justifyContent: 'space-between' }}>
          <Button
            variant="text"
            color="primary"
            onClick={openResetDialog}
            disabled={isLoading}
          >
            Forgot Password?
          </Button>
          <Button
            variant="text"
            color="primary"
            onClick={openRegisterDialog}
            disabled={isLoading}
          >
            Register
          </Button>
        </Box>
        
        <Box sx={{ mt: 1, display: 'flex', justifyContent: 'flex-start' }}>
          <Button
            variant="text"
            color="inherit"
            onClick={openResendVerificationDialog}
            disabled={isLoading}
            sx={{ 
              opacity: 0.75,
            }}
          >
            Resend verification email
          </Button>
        </Box>
      </form>

      <RegistrationDialog
        open={isRegisterDialogOpen}
        onClose={closeRegisterDialog}
      />
      <RequestPasswordResetDialog
        open={isResetDialogOpen}
        onClose={closeResetDialog}
      />
      <ResendVerificationEmailDialog
        open={isResendVerificationDialogOpen}
        onClose={closeResendVerificationDialog}
      />
    </Box>
  )
}

export default Login
