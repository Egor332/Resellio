import BaseHeader from '../BaseHeader/BaseHeader'
import { ROLE_COLORS } from '../../theme/themeConstants'
import { Navigation } from '../../assets/constants/navigation'

function GuestHeader() {
  return (
    <BaseHeader
      title="Resellio Guest"
      backgroundColor={ROLE_COLORS.GUEST.primary}
      linkPath={Navigation.ROOT}
    />
  )
}

export default GuestHeader
