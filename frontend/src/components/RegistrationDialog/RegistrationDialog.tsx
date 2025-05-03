import {
  Dialog,
  DialogTitle,
  DialogContent,
  Button,
  Box,
  Typography,
} from '@mui/material'
import { useNavigate } from 'react-router-dom'
import { Navigation } from '../../assets/constants/navigation'
import { ROLE_COLORS } from '../../theme/themeConstants'

interface RegistrationDialogProps {
  open: boolean
  onClose: () => void
}

interface RegistrationOptionProps {
  title: string
  description: string
  color: {
    primary: string
    secondary: string
  }
  onClick: () => void
}

const RegistrationOption = ({
  title,
  description,
  color,
  onClick,
}: RegistrationOptionProps) => (
  <Box
    sx={{
      flex: 1,
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      p: 3,
      border: '1px solid #e0e0e0',
      borderRadius: 2,
      '&:hover': {
        boxShadow: '0 4px 8px rgba(0,0,0,0.1)',
        cursor: 'pointer',
      },
    }}
    onClick={onClick}
  >
    <Typography variant="h6" sx={{ color: color.primary, mb: 1 }}>
      {title}
    </Typography>
    <Typography variant="body2" align="center">
      {description}
    </Typography>
    <Button
      variant="contained"
      sx={{
        mt: 2,
        bgcolor: color.primary,
        '&:hover': {
          bgcolor: color.secondary,
        },
      }}
      onClick={onClick}
    >
      Register as {title}
    </Button>
  </Box>
)

const RegistrationDialog = ({ open, onClose }: RegistrationDialogProps) => {
  const navigate = useNavigate()

  const handleCustomerRegistration = () => {
    navigate(Navigation.REGISTER_CUSTOMER)
    onClose()
  }

  const handleOrganiserRegistration = () => {
    navigate(Navigation.REGISTER_ORGANISER)
    onClose()
  }

  const registrationOptions = [
    {
      title: 'Customer',
      description:
        'Register as a customer to browse and purchase tickets for events',
      color: ROLE_COLORS.CUSTOMER,
      onClick: handleCustomerRegistration,
    },
    {
      title: 'Organiser',
      description:
        'Register as an organiser to create and manage your own events',
      color: ROLE_COLORS.ORGANISER,
      onClick: handleOrganiserRegistration,
    },
  ]

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle align="center">Choose Registration Type</DialogTitle>
      <DialogContent>
        <Box
          sx={{
            display: 'flex',
            flexDirection: { xs: 'column', sm: 'row' },
            gap: 2,
            justifyContent: 'space-between',
            my: 2,
          }}
        >
          {registrationOptions.map((option, index) => (
            <RegistrationOption key={index} {...option} />
          ))}
        </Box>
      </DialogContent>
    </Dialog>
  )
}

export default RegistrationDialog
