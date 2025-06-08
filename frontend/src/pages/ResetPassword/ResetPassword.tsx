import { useState } from 'react'
import { useNavigate, useParams, Navigate } from 'react-router-dom'
import { Button, TextField, Typography, Box, Paper } from '@mui/material'
import { Navigation } from '../../assets/constants/navigation'
import authService from '../../services/authService'
import useBanner from '../../hooks/useBanner'

function ResetPassword() {
  const [newPassword, setNewPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const navigate = useNavigate()
  const banner = useBanner()
  const { token: tokenFromUrl } = useParams<{ token: string }>()

  const handleResetPassword = async () => {
    if (newPassword !== confirmPassword) {
      banner.showError('Passwords do not match')
      return
    }

    setIsLoading(true)

    try {
      await authService.resetPassword(tokenFromUrl!, newPassword)
      banner.showSuccess('Password reset successful')
      navigate(Navigation.LOGIN)
    } catch (err: any) {
      banner.showError(err.message || 'Failed to reset password')
    } finally {
      setIsLoading(false)
    }
  }

  // If no token is provided in the URL, redirect to login
  if (!tokenFromUrl) {
    return <Navigate to={Navigation.LOGIN} />
  }

  return (
    <Box sx={{ width: '100%', maxWidth: 450, p: 3 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom align="center">
          Reset Your Password
        </Typography>

        <Typography variant="body1" sx={{ mb: 3 }}>
          Please enter your new password.
        </Typography>
        <TextField
          fullWidth
          label="New Password"
          type="password"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          margin="normal"
          required
          autoFocus
        />
        <TextField
          fullWidth
          label="Confirm New Password"
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          margin="normal"
          required
        />
        <Button
          variant="contained"
          fullWidth
          onClick={handleResetPassword}
          disabled={!newPassword || !confirmPassword || isLoading}
          sx={{ mt: 3 }}
        >
          {isLoading ? 'Resetting...' : 'Reset Password'}
        </Button>

        <Box sx={{ mt: 3, textAlign: 'center' }}>
          <Button
            variant="text"
            onClick={() => navigate(Navigation.LOGIN)}
            disabled={isLoading}
          >
            Back to Login
          </Button>
        </Box>
      </Paper>
    </Box>
  )
}

export default ResetPassword
