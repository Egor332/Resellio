import { SyntheticEvent, useState } from 'react'
import { Link } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css'
import formStyles from '../../styles/FormStyles.module.css'

function LoginForm() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')

  const [errorMsg] = useState<string>('')

  const handleSubmit = (event: SyntheticEvent) => {
    event.preventDefault()
  }

  return (
    <form onSubmit={handleSubmit} className={`text-start ${formStyles['form']}`}>
      <div className={formStyles['form-row']}>
        <label className="form-label fw-bold">Email:</label>
        <input
          type="text"
          className={`form-control ${formStyles['form-input']}`}
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </div>
      <div className={formStyles['form-row']}>
        <label className="form-label fw-bold">Password:</label>
        <input
          type="password"
          className={`form-control ${formStyles['form-input']}`}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>
      {errorMsg && <div className="alert alert-danger">{errorMsg}</div>}
      <div className={formStyles['btn-row']}>
        <button type="submit" className={`btn btn-primary ${formStyles['btn-frst']}`}>
          Login
        </button>
        <Link to="customers/register" className={`btn btn-secondary ${formStyles['btn-scnd']}`}>
          Register
        </Link>
      </div>
      <div className="d-flex justify-content-between">
        <Link to="/reset-password" className="btn btn-link p-0">
          Reset Password
        </Link>
      </div>
    </form>
  )
}

export default LoginForm
