import { ReactNode, useEffect } from 'react'
import { Navigate } from 'react-router-dom'
import { useSelector, useDispatch } from 'react-redux'
import { RootState, AppDispatch } from '../../store/store'
import { Navigation } from '../../assets/constants/navigation'
import { Role } from '../../store/auth/authSlice'

import { fetchCurrentUser } from '../../store/auth/authSlice'

type ProtectedRouteProps = {
  children: ReactNode
  requiredRole?: Role
}

const ProtectedRoute = ({ children, requiredRole }: ProtectedRouteProps) => {
  const dispatch: AppDispatch = useDispatch()
  const { isAuthenticated, user, loading } = useSelector(
    (state: RootState) => state.auth
  )

  if (user === null && !loading && localStorage.getItem('auth_token') !== null) {
    dispatch(fetchCurrentUser())
  }

  if (!isAuthenticated) {
    return <Navigate to={Navigation.LOGIN} replace />
  }

  if (requiredRole && user && user.role !== requiredRole) {
    return <Navigate to="-1" />
  }

  return <>{children}</>
}

export default ProtectedRoute
