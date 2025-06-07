import { RouteObject, Navigate } from 'react-router-dom'
import { Navigation } from '../assets/constants/navigation'
import { Role } from '../store/auth/authSlice'
import GuestWrapperPage from '../pages/GuestWrapperPage/GuestWrapperPage'
import CustomersWrapperPage from '../pages/CustomersWrapperPage/CustomersWrapperPage'
import OrganisersWrapperPage from '../pages/OrganisersWrapperPage/OrganisersWrapperPage'
import Login from '../pages/Login/Login'
import RegisterCustomer from '../pages/RegisterCustomer/RegisterCustomer'
import RegisterOrganiser from '../pages/RegisterOrganiser/RegisterOrganiser'
import ResetPassword from '../pages/ResetPassword/ResetPassword'
import ProtectedRoute from '../components/ProtectedRoute/ProtectedRoute'
import CustomersHome from '../pages/CustomersHome/CustomersHome'
import CustomersCart from '../pages/CustomersCart/CustomersCart'
import OrganisersHome from '../pages/OrganisersHome/OrganisersHome'
import CustomersProfile from '../pages/CustomersProfile/CustomersProfile'
import OrganisersProfile from '../pages/OrganisersProfile/OrganisersProfile'
import OrganisersAddEvent from '../pages/OrganisersAddEvent/OrganisersAddEvent'
import CustomersMyTickets from '../pages/CustomersMyTickets/CustomersMyTickets'
import PaymentSuccess from '../pages/PaymentSuccess/PaymentSuccess'
import PaymentFailure from '../pages/PaymentFailure/PaymentFailure'

export const routes: RouteObject[] = [
  {
    path: Navigation.ROOT,
    element: <GuestWrapperPage />,
    children: [
      {
        index: true,
        element: <Navigate to={Navigation.LOGIN} replace />,
      },
      {
        path: Navigation.LOGIN,
        element: <Login />,
      },
      {
        path: Navigation.REGISTER_CUSTOMER,
        element: <RegisterCustomer />,
      },
      {
        path: Navigation.REGISTER_ORGANISER,
        element: <RegisterOrganiser />,
      },
      {
        path: `${Navigation.RESET_PASSWORD}/:token`,
        element: <ResetPassword />,
      },
    ],
  },

  {
    path: Navigation.CUSTOMERS,
    element: (
      <ProtectedRoute requiredRole={Role.Customer}>
        <CustomersWrapperPage />
      </ProtectedRoute>
    ),
    children: [
      {
        index: true,
        element: <CustomersHome />,
      },
      {
        path: Navigation.CUSTOMERS_CART,
        element: <CustomersCart />,
      },
      {
        path: Navigation.CUSTOMERS_PROFILE,
        element: <CustomersProfile />,
      },
      {
        path: Navigation.CUSTOMERS_MY_TICKETS,
        element: <CustomersMyTickets />,
      },
      {
        path: Navigation.CUSTOMERS_PAYMENT_SUCCESS,
        element: <PaymentSuccess />,
      },
      {
        path: Navigation.CUSTOMERS_PAYMENT_FAILURE,
        element: <PaymentFailure />,
      },
    ],
  },

  {
    path: Navigation.ORGANISERS,
    element: (
      <ProtectedRoute requiredRole={Role.Organiser}>
        <OrganisersWrapperPage />
      </ProtectedRoute>
    ),
    children: [
      {
        index: true,
        element: <OrganisersHome />,
      },
      {
        path: Navigation.ORGANISERS_PROFILE,
        element: <OrganisersProfile />,
      },
      {
        path: Navigation.ORGANISERS_ADD_EVENT,
        element: <OrganisersAddEvent />,
      },
    ],
  },

  {
    path: '*',
    element: <Navigate to={Navigation.LOGIN} replace />,
  },
]
