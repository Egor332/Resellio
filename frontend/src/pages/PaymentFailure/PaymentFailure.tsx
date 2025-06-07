import ErrorIcon from '@mui/icons-material/Error'
import { Navigation } from '../../assets/constants/navigation'
import PaymentResultPage from '../../components/PaymentResultPage/PaymentResultPage'

function PaymentFailure() {
  return (
    <PaymentResultPage
      icon={
        <ErrorIcon 
          sx={{ 
            fontSize: 80, 
            color: 'error.main', 
            mb: 2 
          }} 
        />
      }
      title="Payment Failed"
      message="Unfortunately, your payment could not be processed. Please check your payment details and try again."
      titleColor="error.main"
      primaryButton={{
        text: "Return to Cart",
        path: Navigation.CUSTOMERS_CART
      }}
      secondaryButton={{
        text: "Back to Home",
        path: Navigation.CUSTOMERS
      }}
    />
  )
}

export default PaymentFailure
