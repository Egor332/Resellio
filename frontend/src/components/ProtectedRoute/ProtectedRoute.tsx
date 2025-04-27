import { ReactNode } from 'react'
import { Navigate } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '../../store/store'
import { Navigation } from '../../assets/constants/navigation'
import { Role } from '../../store/auth/authSlice'

type ProtectedRouteProps = {
  children: ReactNode
  requiredRole?: Role
}

const ProtectedRoute = ({ children, requiredRole }: ProtectedRouteProps) => {
  const { isAuthenticated, user } = useSelector(
    (state: RootState) => state.auth
  )
  const userRole = user?.role

  if (!isAuthenticated) {
    return <Navigate to={Navigation.LOGIN} replace />
  }

  if (requiredRole && userRole !== requiredRole) {
    return <Navigate to="-1" />
  }

  return <>{children}</>
}

export default ProtectedRoute
