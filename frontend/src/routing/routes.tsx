import { RouteObject, Navigate } from 'react-router-dom'
import { Navigation } from '../assets/constants/navigation'
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
      <ProtectedRoute requiredRole="Customer">
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
    ],
  },

  {
    path: Navigation.ORGANISERS,
    element: (
      <ProtectedRoute requiredRole="Organiser">
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
    ],
  },

  {
    path: '*',
    element: <Navigate to={Navigation.LOGIN} replace />,
  },
]
