import BaseHeader, { NavItem } from '../BaseHeader/BaseHeader'
import { ROLE_COLORS } from '../../theme/themeConstants'
import { Navigation } from '../../assets/constants/navigation'
import PersonIcon from '@mui/icons-material/Person'
import { useDispatch } from 'react-redux'
import { AppDispatch } from '../../store/store'
import { logout } from '../../store/auth/authSlice'
import { useNavigate } from 'react-router-dom'

function OrganiserHeader() {
  const dispatch = useDispatch<AppDispatch>()
  const navigate = useNavigate()

  const organiserNavItems: NavItem[] = [
    {
      icon: <PersonIcon />,
      tooltip: 'Account',
      menuItems: [
        { label: 'My Events', path: Navigation.MY_EVENTS },
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
      title="Resellio Organiser"
      backgroundColor={ROLE_COLORS.ORGANISER.primary}
      linkPath={Navigation.ORGANISERS}
      navItems={organiserNavItems}
    />
  )
}

export default OrganiserHeader
