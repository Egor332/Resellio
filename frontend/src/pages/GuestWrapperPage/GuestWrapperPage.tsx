import Footer from '../../components/Footer/Footer'
import GuestHeader from '../../components/GuestHeader/GuestHeader'
import BaseWrapperPage from '../../pages/BaseWrapperPage/BaseWrapperPage'
import { ROLE_COLORS } from '../../theme/themeConstants'

const GuestWrapperPage = () => {
  return (
    <BaseWrapperPage
      Header={GuestHeader}
      Footer={() => <Footer backgroundColor={ROLE_COLORS.GUEST.primary} />}
    />
  )
}

export default GuestWrapperPage
