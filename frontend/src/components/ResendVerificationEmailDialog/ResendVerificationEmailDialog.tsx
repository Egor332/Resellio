import { useState } from 'react'
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Box,
} from '@mui/material'
import useBanner from '../../hooks/useBanner'
import { apiRequest } from '../../services/httpClient'
import { API_ENDPOINTS, getApiEndpoint } from '../../assets/constants/api'

interface ResendVerificationEmailDialogProps {
  open: boolean
  onClose: () => void
}

function ResendVerificationEmailDialog({
  open,
  onClose,
}: ResendVerificationEmailDialogProps) {
  const [email, setEmail] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const banner = useBanner()

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)
    
    try {
      await apiRequest(getApiEndpoint(API_ENDPOINTS.RESENT_VERIFICATION_EMAIL), { email: email })
      
      banner.showSuccess('Verification email sent successfully')
      setEmail('')
      onClose()
    } catch (error: any) {
      banner.showError(error.message || 'Failed to resend verification email')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
      <DialogTitle sx={{ textAlign: 'left' }}>Resend Verification Email</DialogTitle>
      <form onSubmit={handleSubmit}>
        <DialogContent>
          <Box sx={{ my: 2 }}>
            <TextField
              autoFocus
              margin="dense"
              id="email"
              label="Email Address"
              type="email"
              fullWidth
              variant="outlined"
              value={email}
              onChange={handleEmailChange}
              required
            />
          </Box>
        </DialogContent>
        <DialogActions sx={{ px: 3, pb: 3 }}>
          <Button onClick={onClose} disabled={isLoading}>
            Cancel
          </Button>
          <Button type="submit" variant="contained" disabled={isLoading}>
            {isLoading ? 'Sending...' : 'Send'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  )
}

export default ResendVerificationEmailDialog
