import BaseHeader, { NavItem } from '../BaseHeader/BaseHeader'
import { ROLE_COLORS } from '../../theme/themeConstants'
import { Navigation } from '../../assets/constants/navigation'
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart'
import PersonIcon from '@mui/icons-material/Person'
import { useDispatch } from 'react-redux'
import { AppDispatch } from '../../store/store'
import { logout } from '../../store/auth/authSlice'
import { useNavigate } from 'react-router-dom'

function CustomerHeader() {
  const dispatch = useDispatch<AppDispatch>()
  const navigate = useNavigate()

  const customerNavItems: NavItem[] = [
    {
      icon: <ShoppingCartIcon />,
      path: Navigation.CART, // Assuming this path exists in Navigation
      tooltip: 'Shopping Cart',
    },
    {
      icon: <PersonIcon />,
      tooltip: 'Account',
      menuItems: [
        { label: 'My Tickets', path: Navigation.MY_TICKETS }, // Assuming these paths exist
        { label: 'Profile', path: Navigation.PROFILE },
        {
          label: 'Logout',
          onClick: () => {
            dispatch(logout())
            navigate(Navigation.LOGIN)
          },
        },
      ],
    },
  ]

  return (
    <BaseHeader
      title="Resellio Customer"
      backgroundColor={ROLE_COLORS.CUSTOMER.primary}
      linkPath={Navigation.CUSTOMERS}
      navItems={customerNavItems}
    />
  )
}

export default CustomerHeader
