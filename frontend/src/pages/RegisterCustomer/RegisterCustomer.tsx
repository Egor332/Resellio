import { SyntheticEvent, useState } from 'react'
import {
  CustomerRegisterData,
  registerCustomer,
} from '../../store/auth/authSlice'
import useBanner from '../../hooks/useBanner'
import { useNavigate } from 'react-router-dom'
import { useDispatch } from 'react-redux'
import { AppDispatch } from '../../store/store.ts'
import { Navigation } from '../../assets/constants/navigation'
import { Button, TextField, Typography, Box } from '@mui/material'

function RegisterCustomer() {
  const [customerData, setCustomerData] = useState<CustomerRegisterData>({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
  })
  const [confirmPassword, setConfirmPassword] = useState<string>('')
  const [isLoading, setIsLoading] = useState(false)
  const dispatch = useDispatch<AppDispatch>()
  const navigate = useNavigate()
  const banner = useBanner()

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    if (name === 'confirmPassword') {
      setConfirmPassword(value)
    } else {
      setCustomerData({ ...customerData, [name]: value })
    }
  }

  const handleSubmit = async (event: SyntheticEvent) => {
    event.preventDefault()
    setIsLoading(true)

    if (customerData.password !== confirmPassword) {
      banner.showError('Passwords do not match. Please try again.')
      setIsLoading(false)
      return
    }

    try {
      await dispatch(registerCustomer(customerData)).unwrap()
      banner.showSuccess('Customer registered successfully!')
      navigate(Navigation.LOGIN)
    } catch (err) {
      banner.showError('Customer registration failed. Please try again.')
      console.error('Customer registration failed:', err)
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <Box sx={{ width: '100%', maxWidth: 400, p: 3 }}>
      <Typography variant="h4" component="h1" gutterBottom align="center">
        Register customer
      </Typography>

      <form onSubmit={handleSubmit}>
        <TextField
          fullWidth
          label="First Name"
          name="firstName"
          type="text"
          value={customerData.firstName}
          onChange={handleChange}
          margin="normal"
          required
          autoFocus
        />

        <TextField
          fullWidth
          label="Last Name"
          name="lastName"
          type="text"
          value={customerData.lastName}
          onChange={handleChange}
          margin="normal"
          required
        />

        <TextField
          fullWidth
          label="Email"
          name="email"
          type="email"
          value={customerData.email}
          onChange={handleChange}
          margin="normal"
          required
        />

        <TextField
          fullWidth
          label="Password"
          name="password"
          type="password"
          value={customerData.password}
          onChange={handleChange}
          margin="normal"
          required
        />

        <TextField
          fullWidth
          label="Confirm Password"
          name="confirmPassword"
          type="password"
          value={confirmPassword}
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
          {isLoading ? 'Registering...' : 'Register'}
        </Button>

        <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center' }}>
          <Button
            variant="text"
            color="primary"
            onClick={() => navigate(Navigation.ROOT)}
            disabled={isLoading}
          >
            Already have an account? Log in
          </Button>
        </Box>
      </form>
    </Box>
  )
}

export default RegisterCustomer
