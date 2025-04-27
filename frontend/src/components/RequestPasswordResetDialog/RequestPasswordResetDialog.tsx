import { useState } from 'react'
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
} from '@mui/material'
import authService from '../../services/authService'
import useBanner from '../../hooks/useBanner'

interface RequestPasswordResetDialogProps {
  open: boolean
  onClose: () => void
}

function RequestPasswordResetDialog({
  open,
  onClose,
}: RequestPasswordResetDialogProps) {
  const [email, setEmail] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const banner = useBanner()

  const handleRequestReset = async () => {
    setIsLoading(true)

    try {
      await authService.requestPasswordReset(email)
      banner.showSuccess('Reset email sent. Please check your inbox.')
      onClose()
    } catch (err: any) {
      banner.showError(
        err.message || 'Failed to send reset email. Please try again.'
      )
    } finally {
      setIsLoading(false)
    }
  }

  const handleClose = () => {
    setEmail('')
    onClose()
  }

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Reset Password</DialogTitle>
      <DialogContent>
        <TextField
          fullWidth
          label="Email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          margin="normal"
          required
          autoFocus
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isLoading}>
          Cancel
        </Button>
        <Button
          onClick={handleRequestReset}
          variant="contained"
          disabled={!email || isLoading}
        >
          {isLoading ? 'Sending...' : 'Send Reset Link'}
        </Button>
      </DialogActions>
    </Dialog>
  )
}

export default RequestPasswordResetDialog
