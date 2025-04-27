import OrganisersHeader from '../../components/OrganisersHeader/OrganisersHeader'
import BaseWrapperPage from '../../pages/BaseWrapperPage/BaseWrapperPage'
import Footer from '../../components/Footer/Footer'
import { ROLE_COLORS } from '../../theme/themeConstants'

const OrganisersWrapperPage = () => {
  return (
    <BaseWrapperPage
      Header={OrganisersHeader}
      Footer={() => <Footer backgroundColor={ROLE_COLORS.ORGANISER.primary} />}
    />
  )
}

export default OrganisersWrapperPage
