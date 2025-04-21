import CustomersHeader from '../../components/CustomersHeader/CustomersHeader'
import BaseWrapperPage from '../../pages/BaseWrapperPage/BaseWrapperPage'
import Footer from '../../components/Footer/Footer'
import { ROLE_COLORS } from '../../theme/themeConstants'

const CustomersWrapperPage = () => {
  return (
    <BaseWrapperPage
      Header={CustomersHeader}
      Footer={() => <Footer backgroundColor={ROLE_COLORS.CUSTOMER.primary} />}
    />
  )
}

export default CustomersWrapperPage
