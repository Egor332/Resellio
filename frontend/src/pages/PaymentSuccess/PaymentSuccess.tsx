import CheckCircleIcon from '@mui/icons-material/CheckCircle'
import { Navigation } from '../../assets/constants/navigation'
import PaymentResultPage from '../../components/PaymentResultPage/PaymentResultPage'

function PaymentSuccess() {
  return (
    <PaymentResultPage
      icon={
        <CheckCircleIcon 
          sx={{ 
            fontSize: 80, 
            color: 'success.main', 
            mb: 2 
          }} 
        />
      }
      title="Payment Successful!"
      message="Your payment has been processed successfully. You can now view your tickets in the 'My Tickets' section."
      titleColor="success.main"
      primaryButton={{
        text: "View My Tickets",
        path: Navigation.CUSTOMERS_MY_TICKETS
      }}
      secondaryButton={{
        text: "Back to Home",
        path: Navigation.CUSTOMERS
      }}
    />
  )
}

export default PaymentSuccess
